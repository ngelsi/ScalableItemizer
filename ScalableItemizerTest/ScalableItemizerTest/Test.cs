using System;
using System.Security.Policy;
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

            itemizer.Add(1, ItemizerOptions.InheritItems, (i) =>
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

            itemizer.Add(0.5, ItemizerOptions.InheritItems, (i) =>
            {
                Interlocked.Increment(ref item1);
            });

            itemizer.Add(1, ItemizerOptions.InheritItems, (i) =>
            {
                Interlocked.Increment(ref item2);
            });

            itemizer.Add(2, ItemizerOptions.InheritItems, (i) =>
            {
                Interlocked.Increment(ref item3);
                if (item3 == 56)
                {
                    handle.Set();
                }
            });

            itemizer.Start();
            itemizer.Supply(100);

            handle.Wait();

            itemizer.Dispose();           

            Assert.AreEqual(Math.Round(item2 / (double)item1), 2);
            Assert.AreEqual(Math.Round(item3 / (double)item2), 2);
            Assert.AreEqual(Math.Round(item3 / (double)item1), 4);
        }

        [TestMethod]
        public void AutomatedTest1()
        {
            var itemizer = Itemizer.Automated(100, 1);
            var handle = new ManualResetEventSlim(false);

            int item1 = 0;

            itemizer.Add(1, (i) =>
            {
                Interlocked.Increment(ref item1);
                if (item1 == 10)
                {
                    handle.Set();
                }
            });

            itemizer.Start();

            handle.Wait();

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

            itemizer.Add(1, (i) =>
            {
                Interlocked.Increment(ref item1);
            });
            itemizer.Add(1, (i) =>
            {
                Interlocked.Increment(ref item2);
            });
            itemizer.Add(1, (i) =>
            {
                Interlocked.Increment(ref item3);
                if (item3 == 20)
                {
                    handle.Set();
                }
            });

            itemizer.Start();

            handle.Wait();

            itemizer.Dispose();

            Assert.AreEqual(Math.Round(item2 / (double)item1), 1);
            Assert.AreEqual(Math.Round(item3 / (double)item2), 1);
            Assert.AreEqual(Math.Round(item3 / (double)item1), 1);
        }
    }
}