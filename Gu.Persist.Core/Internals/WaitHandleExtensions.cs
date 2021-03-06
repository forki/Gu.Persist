﻿// ReSharper disable UnusedMember.Global
#pragma warning disable UseAsyncSuffix // Use Async suffix
namespace Gu.Persist.Core
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// http://stackoverflow.com/a/18766131/1069200
    /// </summary>
    public static class WaitHandleExtensions
    {
                                      /// <summary>
                                      /// Turn <paramref name="handle"/> into an awaitable <see cref="Task"/>
                                      /// </summary>
        public static Task AsTask(this WaitHandle handle)
        {
            return AsTask(handle, Timeout.InfiniteTimeSpan);
        }

        /// <summary>
        /// Turn <paramref name="handle"/> into an awaitable <see cref="Task"/>
        /// </summary>
        public static Task AsTask(this WaitHandle handle, TimeSpan timeout)
        {
            var tcs = new TaskCompletionSource<object>();
            var registration = ThreadPool.RegisterWaitForSingleObject(
                handle,
                (state, timedOut) =>
                    {
                        var localTcs = (TaskCompletionSource<object>)state;
                        if (timedOut)
                        {
                            localTcs.TrySetCanceled();
                        }
                        else
                        {
                            localTcs.TrySetResult(null);
                        }
                    },
                tcs,
                timeout,
                executeOnlyOnce: true);
            tcs.Task.ContinueWith((_, state) => ((RegisteredWaitHandle)state).Unregister(null), registration, TaskScheduler.Default);
            return tcs.Task;
        }
    }
}
