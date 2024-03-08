using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Hathora.Core.Scripts.Runtime.Common.Extensions
{
    /// <summary>Extension methods for Process (terminal cmds).</summary>
    public static class ProcessExtensions
    {
        /// <summary>
        /// Later versions of .NET have the Async ver of WaitForExit, but Unity doesn't.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="cancellationToken"></param>
        public static async Task WaitForExitAsync(
            this Process process, 
            CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<object>();

            void Process_Exited(object sender, EventArgs e)
            {
                tcs.TrySetResult(null);
            }

            process.EnableRaisingEvents = true;
            process.Exited += Process_Exited;

            try
            {
                if (process.HasExited)
                    return;

                await using (cancellationToken.Register(() => tcs.TrySetCanceled()))
                    await tcs.Task;
            }
            finally
            {
                process.Exited -= Process_Exited;
            }
        }
    }
}
