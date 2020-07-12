using System;
using System.Diagnostics;
using System.Reflection;
using DotRas.Diagnostics;
using DotRas.Diagnostics.Events;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;

namespace DotRas.Internal.Infrastructure.Advice
{
    internal class MarshallerLoggingAdvice : LoggingAdvice<IMarshaller>, IMarshaller
    {
        public MarshallerLoggingAdvice(IMarshaller attachedObject, IEventLoggingPolicy eventLoggingPolicy)
            : base(attachedObject, eventLoggingPolicy)
        {
        }

        public int SizeOf<T>()
        {
            return AttachedObject.SizeOf<T>();
        }

        public IntPtr AllocHGlobal(int size)
        {
            return AttachedObject.AllocHGlobal(size);
        }

        public bool FreeHGlobalIfNeeded(IntPtr ptr)
        {
            return AttachedObject.FreeHGlobalIfNeeded(ptr);
        }

        public string PtrToUnicodeString(IntPtr ptr, int length)
        {
            return AttachedObject.PtrToUnicodeString(ptr, length);
        }

        public void StructureToPtr<T>(T structure, IntPtr ptr)
        {
            var stopwatch = Stopwatch.StartNew();
            AttachedObject.StructureToPtr(structure, ptr);
            stopwatch.Stop();

            var structureType = typeof(T);

            var callTrace = new StructMarshalledToPtrTraceEvent
            {
                StructureType = structureType,
                Duration = stopwatch.Elapsed,
                Result = ptr
            };

            AddFields(null, structure, structureType, callTrace);            
            LogVerbose(callTrace);
        }

        private void AddFields(string parent, object instance, Type type, StructMarshalledToPtrTraceEvent eventData)
        {
            foreach (var field in type.GetFields())
            {
                var value = GetValue(instance, field);

                if (field.FieldType.IsNested && !field.FieldType.IsEnum)
                {
                    AddFields(field.Name, value, field.FieldType, eventData);
                }
                else
                {
                    eventData.Fields.Add(string.IsNullOrEmpty(parent) ? field.Name : $"{parent}.{field.Name}", value);
                }
            }
        }

        private static object GetValue(object instance, FieldInfo field)
        {
            var value = field.GetValue(instance);
            if (field.GetCustomAttribute<MaskedValueAttribute>() != null && value != null)
            {
                value = "********";
            }

            return value;
        }

        public byte[] PtrToByteArray(IntPtr ptr, int length)
        {
            return AttachedObject.PtrToByteArray(ptr, length);
        }

        public IntPtr ByteArrayToPtr(byte[] bytes)
        {
            return AttachedObject.ByteArrayToPtr(bytes);
        }
    }
}