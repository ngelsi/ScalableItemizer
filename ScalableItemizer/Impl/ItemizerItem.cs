using ScalableItemizer.Intf;
using ScalableItemizer.Mod;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ScalableItemizer.Impl
{
    internal class ItemizerItem : ItemizerBase, IItemizerItem
    {
        private ManualResetEventSlim resetEvent;
        private Action action;

        public double ItemsPerIteration { get; private set; }

        public string Identifier { get; private set; }

        public ItemizerOptions Options { get; private set; }

        public ItemizerItem(double itemsPerIteration, Action action)
        {
            ItemsPerIteration = itemsPerIteration;
            Identifier = Guid.NewGuid().ToString("D");

            this.action = action;
            this.resetEvent = new ManualResetEventSlim(false);
        }

        public ItemizerItem(double itemsPerIteration, ItemizerOptions options, Action action)
            : this(itemsPerIteration, action)
        {
            Options = options;
        }

        public override void Start()
        {
            base.Start();
            resetEvent.Set();
        }

        public void AddItems(double itemsToAdd)
        {
            lock (lockObject)
            {
                if (Options.HasFlag(ItemizerOptions.InheritItems))
                {
                    items += itemsToAdd;
                }
                else
                {
                    if ((itemsToAdd < 1.0 && items < 1.0) || (itemsToAdd < ItemsPerIteration))
                    {
                        items += itemsToAdd;
                    }
                    else
                    {
                        items = itemsToAdd;
                    }
                }
            }
        }

        protected override void InternalStart()
        {
            while (Interlocked.Exchange(ref disposed, 0) == 0)
            {
                resetEvent.Wait();

                bool continueRunning;
                lock (lockObject)
                {
                    continueRunning = Running && 0 < (int)items;
                }

                if (continueRunning)
                {
                    do
                    {
                        lock (lockObject)
                        {
                            if (!(0 <= items - 1.0))
                            {
                                continue;
                            }
                            else
                            {
                                items -= 1.0;
                            }
                        }

                        OnExecuting();

                        if (Options.HasFlag(ItemizerOptions.SeparateThread))
                        {
                            Task.Factory.StartNew(action);
                        }
                        else
                        {
                            action();
                        }

                        lock (lockObject)
                        {
                            continueRunning = Running && 0 < (int)items;
                        }
                    }
                    while (continueRunning);
                }

                resetEvent.Reset();
            }
        }
    }
}