using ScalableItemizer.Intf;
using ScalableItemizer.Mod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ScalableItemizer.Impl
{
    internal abstract class ScalableItemizer : ItemizerBase, IScalableItemizer
    {
        private List<ItemizerItem> itemizers;
        private int currentItemPosition;

        protected readonly ManualResetEventSlim ResetEvent;

        protected ScalableItemizer()
        {
            itemizers = new List<ItemizerItem>();
            ResetEvent = new ManualResetEventSlim(false);
        }

        public IItemizerItem Add(double itemsPerIteration, Action action)
        {
            return Add(new ItemizerItem(itemsPerIteration, action));
        }

        public IItemizerItem Add(double itemsPerIteration, ItemizerOptions options, Action action)
        {
            return Add(new ItemizerItem(itemsPerIteration, options, action));
        }

        public void Remove(string identifier)
        {
            lock (lockObject)
            {
                var itemizer = itemizers.FirstOrDefault(f => f.Identifier == identifier);
                Remove(itemizer);
            }
        }

        public void Remove(IItemizerItem item)
        {
            lock (lockObject)
            {
                var itemizer = item as ItemizerItem;

                itemizer.Dispose();
                itemizers.Remove(itemizer);
            }
        }

        protected override void InternalStart()
        {
            while (Interlocked.Exchange(ref disposed, 0) == 0)
            {
                ResetEvent.Wait();

                bool continueRunning;
                lock (lockObject)
                {
                    continueRunning = Running && 0 < items;
                }

                if (continueRunning)
                {
                    OnExecuting();

                    do
                    {
                        if (itemizers.Count <= currentItemPosition)
                        {
                            Interlocked.Exchange(ref currentItemPosition, 0);
                        }

                        lock (lockObject)
                        {
                            var itemizer = itemizers.ElementAtOrDefault(currentItemPosition);
                            if (itemizer == null)
                            {
                                continue;
                            }

                            var itemizerSideItems = itemizer.ItemsPerIteration;
                            if (!(0 <= items - itemizerSideItems))
                            {
                                continueRunning = Running && 0 < (int)items;
                                if (continueRunning)
                                {
                                    Interlocked.Increment(ref currentItemPosition);
                                }

                                continue;
                            }

                            items -= itemizer.ItemsPerIteration;

                            itemizer.AddItems(itemizer.ItemsPerIteration);
                            itemizer.Start();

                            Interlocked.Increment(ref currentItemPosition);
                            continueRunning = Running && 0 < items;
                        }
                    }
                    while (continueRunning);
                }

                ResetEvent.Reset();
            }
        }

        protected virtual IItemizerItem Add(ItemizerItem item)
        {
            lock (lockObject)
            {
                itemizers.Add(item);
            }

            return item;
        }

        public override void Dispose()
        {
            base.Dispose();

            foreach (var item in itemizers)
            {
                item.Dispose();
            }

            itemizers.Clear();
        }
    }
}