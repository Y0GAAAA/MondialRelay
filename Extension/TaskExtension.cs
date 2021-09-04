using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MondialRelay.Extension
{

    public static class TaskExtension
    {

        /// <summary>
        /// Integer precision millisecond time of day.
        /// </summary>
        private static uint Milliseconds => (uint)DateTime.Now.TimeOfDay.TotalMilliseconds;

        public async static Task<(T, uint)> GetExecutionTime<T>(this Task<T> task)
        {
            var start = Milliseconds;
            T result = await task;

            return (result, Milliseconds - start);
        }

    }

}
