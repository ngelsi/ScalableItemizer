using ScalableItemizer.Intf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScalableItemizer.Impl
{
    internal abstract class ItemizerBase : IItemizerBase
    {
        public event EventHandler Started;

        public event EventHandler Stopped;

        public event EventHandler Executing;

        public bool Running { get; protected set; }

        protected double items;
        protected int started;
        protected int disposed;
        protected object lockObject;

        protected abstract void InternalStart();

        protected ItemizerBase()
        {
            lockObject = new object();
        }

        public virtual void Start()
        {
            if (Interlocked.Exchange(ref started, 1) == 0)
            {
                Task.Factory.StartNew(InternalStart);
            }

            lock (lockObject)
            {
                if (Running == false)
                {
                    OnStarted();
                    Running = true;
                }
            }
        }

        public virtual void Stop()
        {
            lock (lockObject)
            {
                if (Running)
                {
                    OnStopped();
                    Running = false;
                }
            }
        }

        public virtual void Dispose()
        {
            Stop();
            Interlocked.Exchange(ref disposed, 1);
        }

        protected virtual void OnStarted()
        {
            var handler = Started;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnStopped()
        {
            var handler = Stopped;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnExecuting()
        {
            var handler = Executing;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}