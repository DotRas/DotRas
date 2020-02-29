using System.Threading.Tasks;

namespace DotRas
{
    /// <summary>
    /// Contains extensions for the <see cref="Task"/> class.
    /// </summary>
    internal static class TaskExtensions
    {
        /// <summary>
        /// Gets the result of the operation synchronously.
        /// </summary>
        /// <param name="task">The task to run synchronously.</param>
        public static void GetResultSynchronously(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the result of the operation synchronously.
        /// </summary>
        /// <typeparam name="TResult">The type of result.</typeparam>
        /// <param name="task">The task to run synchronously.</param>
        /// <returns>The result of the operation.</returns>
        public static TResult GetResultSynchronously<TResult>(this Task<TResult> task)
        {
            return task.GetAwaiter().GetResult();
        }
    }
}