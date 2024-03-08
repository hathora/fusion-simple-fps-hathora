// Created by dylan@hathora.dev

using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Hathora.Core.Scripts.Runtime.Common.Extensions
{
    /// <summary>
    /// Extension methods for UnityWebRequests.
    /// </summary>
    public static class UnityWebRequestAsyncOperationExtensions
    {
        /// <summary>
        /// Allow an awaitable Task. Normally, this is a coroutine.
        /// </summary>
        /// <param name="asyncOperation"></param>
        /// <returns></returns>
        public static Task<UnityWebRequest> AsTask(this UnityWebRequestAsyncOperation asyncOperation)
        {
            var completionSource = new TaskCompletionSource<UnityWebRequest>();
            asyncOperation.completed += operation => completionSource.SetResult(asyncOperation.webRequest);
            return completionSource.Task;
        }
    }
}
