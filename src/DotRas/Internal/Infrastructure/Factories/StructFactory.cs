using System;
using System.Linq;
using System.Reflection;
using DotRas.Internal.Abstractions.Factories;
using DotRas.Internal.Abstractions.Services;
using DotRas.Internal.Interop;

namespace DotRas.Internal.Infrastructure.Factories
{
    internal class StructFactory : IStructFactory, IStructArrayFactory
    {
        private readonly IMarshaller marshaller;

        public StructFactory(IMarshaller marshaller)
        {
            this.marshaller = marshaller ?? throw new ArgumentNullException(nameof(marshaller));
        }

        public T Create<T>() where T : new()
        {
            return Create<T>(out var _);
        }

        public T Create<T>(out int size) where T : new()
        {
            object result = new T();
            size = marshaller.SizeOf<T>();

            var field = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public).SingleOrDefault(o => o.GetCustomAttributes<SizeOfAttribute>().Any());
            if (field == null)
            {
                throw new NotSupportedException($"The type '{typeof(T)}' does not contain a field with a '{nameof(SizeOfAttribute)}' attribute.");
            }

            field.SetValue(result, size);
            return (T)result;
        }

        public T[] CreateArray<T>(int count, out int size) where T : new()
        {
            if (count <= 0)
            {
                throw new ArgumentException("The count must be greater than zero.");
            }

            var result = new T[count];
            result[0] = Create<T>(out var structSize);

            size = structSize * count;

            return result;
        }
    }
}