using ScalableItemizer.Intf;
using System;
using System.Timers;

namespace ScalableItemizer.Impl.Itemizers
{
    internal class AutomatedItemizer : ScalableItemizer, IAutomatedItemizer
    {
        private Timer internalTimer;

        public double Interval { get; private set; }

        public double ItemsPerInterval { get; private set; }

        public AutomatedItemizer(double interval, double itemsPerInterval)
            : base()
        {
            Interval = interval;
            ItemsPerInterval = itemsPerInterval;

            internalTimer = new Timer(Interval);
            internalTimer.Elapsed += InternalTimerOnElapsed;
        }

        public override void Start()
        {
            internalTimer.Start();
            ResetEvent.Set();

            base.Start();
        }

        public override void Stop()
        {
            internalTimer.Stop();
            base.Stop();
        }

        protected override IItemizerItem Add(ItemizerItem item)
        {
            if (ItemsPerInterval < item.ItemsPerIteration)
            {
                throw new ArgumentException("The items/iteration value can not be bigger than the items/interval value");
            }

            return base.Add(item);
        }

        private void InternalTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            lock (lockObject)
            {
                items = ItemsPerInterval;
            }

            ResetEvent.Set();
        }
    }
}