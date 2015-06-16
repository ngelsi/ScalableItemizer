using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScalableItemizer;
using ScalableItemizer.Mod;
using System.Threading;

namespace ScalableItemizerTest
{
    [TestClass]
    public class Test
    {
        [TestMethod]
        public void SuppliedTest1()
        {
            var itemizer = Itemizer.Supplied();
            var handle = new ManualResetEventSlim(false);

            int item1 = 0;

            itemizer.Add(1, ItemizerOptions.InheritItems, () =>
            {
                Interlocked.Increment(ref item1);

                if (item1 == 100)
                {
                    handle.Set();
                }
            });

            itemizer.Start();
            itemizer.Supply(100);

            handle.Wait();

            itemizer.Dispose();

            Assert.AreEqual(item1, 100);
        }

        [TestMethod]
        public void SuppliedTest2()
        {
            var itemizer = Itemizer.Supplied();
            var handle = new ManualResetEventSlim(false);

            int item1 = 0;
            int item2 = 0;
            int item3 = 0;

            itemizer.Add(0.5, ItemizerOptions.InheritItems, () =>
            {
                Interlocked.Increment(ref item1);

                if (item1 == 14)
                {
                    handle.Set();
                }
            });

            itemizer.Add(1, ItemizerOptions.InheritItems, () =>
            {
                Interlocked.Increment(ref item2);
            });

            itemizer.Add(2, ItemizerOptions.InheritItems, () =>
            {
                Interlocked.Increment(ref item3);
            });

            itemizer.Start();
            itemizer.Supply(100);

            handle.Wait();

            itemizer.Dispose();

            Assert.AreEqual(item1, 14);
            Assert.AreEqual(item2, 29);
            Assert.AreEqual(item3, 56);
        }

        [TestMethod]
        public void AutomatedTest1()
        {
            var itemizer = Itemizer.Automated(100, 1);
            var handle = new ManualResetEventSlim(false);

            int item1 = 0;

            itemizer.Add(1, () =>
            {
                Interlocked.Increment(ref item1);
            });

            itemizer.Start();

            handle.Wait(1100); // +100ms first tick of Elapsed

            itemizer.Dispose();

            Assert.AreEqual(item1, 10);
        }

        [TestMethod]
        public void AutomatedTest2()
        {
            var itemizer = Itemizer.Automated(100, 2);
            var handle = new ManualResetEventSlim(false);

            int item1 = 0;
            int item2 = 0;
            int item3 = 0;

            itemizer.Add(0.5, () =>
            {
                Interlocked.Increment(ref item1);
            });
            itemizer.Add(1, () =>
            {
                Interlocked.Increment(ref item2);
            });
            itemizer.Add(2, () =>
            {
                Interlocked.Increment(ref item3);
            });

            itemizer.Start();

            handle.Wait(2000);

            itemizer.Dispose();

            Assert.AreEqual(item1, 2);
            Assert.AreEqual(item2, 5);
            Assert.AreEqual(item3, 10);
        }
    }
}