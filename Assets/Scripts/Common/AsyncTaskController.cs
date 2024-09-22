using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interfaces;

namespace Common
{
    public class AsyncTaskController : IDisposable
    {
        public AsyncTaskController()
        {
            _isStarted = true;

            Task.Run(async () =>
            {
                while (IsStarted)
                {
                    if (_tasks.Count > 0)
                        await _tasks.Dequeue();

                    await Task.Delay(10);
                }
            });
        }

        private readonly Queue<Task> _tasks = new();

        private bool _isStarted;

        private bool IsStarted
        {
            get
            {
                lock (this)
                {
                    return _isStarted;
                }
            }
        }

        public void Dispose()
        {
            lock (this)
            {
                _isStarted = false;
            }

            _tasks.Clear();
        }

        public void Wait<T>(Task<T> task, Action<T> onComplete, Action<string> onError)
        {
            _tasks.Enqueue(TaskRunner(task, onComplete, onError));
        }

        private static async Task TaskRunner<T>(Task<T> task, Action<T> onComplete, Action<string> onError)
        {
            try
            {
                var result = await task;
                onComplete(result);
            }
            catch (Exception ex)
            {
                onError(ex.ToString());
            }
        }
    }
}
