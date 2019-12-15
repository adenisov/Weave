using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Weave.Messaging.Core
{
    public static class TaskExtensions
    {
        private const bool DefaultContinueOnCapturedContext = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public static ConfiguredTaskAwaitable DropContext(this Task task) => task.ConfigureAwait(DefaultContinueOnCapturedContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ConfiguredTaskAwaitable<T> DropContext<T>(this Task<T> task) => task.ConfigureAwait(DefaultContinueOnCapturedContext);
    }
}
