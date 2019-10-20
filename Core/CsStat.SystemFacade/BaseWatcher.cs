using System;
using System.Threading;

namespace CsStat.SystemFacade
{
    public abstract class BaseWatcher
    {
        private Timer _timer;
        private static long _timerInterval;
        private static readonly object _locker = new object();

        public void Start(long timerInterval)
        {
            _timerInterval = timerInterval;
            _timer = new Timer(Callback, null, 0, timerInterval);
        }

        public void Stop()
        {
            _timer.Dispose();
        }

        private void Callback(object state)
        {
            var hasLock = false;
            try
            {
                Monitor.TryEnter(_locker, ref hasLock);
                if (!hasLock)
                {
                    return;
                }

                _timer.Change(Timeout.Infinite, Timeout.Infinite);

                try
                {
                    ReadFile();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            finally
            {
                if (hasLock)
                {
                    Monitor.Exit(_locker);
                    _timer.Change(_timerInterval, _timerInterval);
                }
            }
        }

        protected abstract void ReadFile();
    }
}