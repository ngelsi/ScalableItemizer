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
        private Action<IItemizerItem> action;
        private Func<double> itemsPerIterationFunc;
        private double itemsPerIteration;

        public double ItemsPerIteration
        {
            get
            {
                return itemsPerIterationFunc != null ? itemsPerIterationFunc() : itemsPerIteration;
            }
        }

        public string Identifier { get; private set; }

        public ItemizerOptions Options { get; private set; }

        private ItemizerItem()
        {
            this.resetEvent = new ManualResetEventSlim(false);
            Identifier = Guid.NewGuid().ToString("D");
        }

        public ItemizerItem(Func<double> itemsPerIterationFunc, Action<IItemizerItem> action)
            : this()
        {
            this.action = action;
            this.itemsPerIterationFunc = itemsPerIterationFunc;
        }

        public ItemizerItem(Func<double> itemsPerIterationFunc, ItemizerOptions options, Action<IItemizerItem> action)
            : this(itemsPerIterationFunc, action)
        {
            Options = options;
        }

        public ItemizerItem(double itemsPerIteration, Action<IItemizerItem> action)
            : this()
        {
            this.itemsPerIteration = itemsPerIteration;
            this.action = action;
        }

        public ItemizerItem(double itemsPerIteration, ItemizerOptions options, Action<IItemizerItem> action)
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
                    if ((!(itemsToAdd < 1.0) || !(items < 1.0)) && (!(itemsToAdd < ItemsPerIteration)))
                    {
                        items = itemsToAdd;
                    }
                    else
                    {
                        items += itemsToAdd;
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
                            Task.Factory.StartNew(() => action(this));
                        }
                        else
                        {
                            action(this);
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