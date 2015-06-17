using System;
using System.Threading;
using ScalableItemizer;
using ScalableItemizer.Mod;

namespace ScalableItemizerConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Program1();
        }

        private static void Program1()
        {
            var itemizer = Itemizer.Supplied();
            int test1 = 0;
            int test2 = 0;
            int test3 = 0;
            int test4 = 0;
            int test5 = 0;

            itemizer.Add(0.5, ItemizerOptions.InheritItems, (i) =>
            {
                Interlocked.Increment(ref test1);
                Console.WriteLine("test 1");
            });

            itemizer.Add(0.5, ItemizerOptions.InheritItems, (i) =>
            {
                Interlocked.Increment(ref test5);
                Console.WriteLine("test 5");
            });

            itemizer.Add(1, ItemizerOptions.InheritItems, (i) =>
            {
                Interlocked.Increment(ref test2);
                Console.WriteLine("test 2");
            });

            itemizer.Add(2, ItemizerOptions.InheritItems, (i) =>
            {
                Interlocked.Increment(ref test3);
                Console.WriteLine("test 3");
            });

            itemizer.Add(2, ItemizerOptions.InheritItems, (i) =>
            {
                Interlocked.Increment(ref test4);
                Console.WriteLine("test 4");
            });

            itemizer.Start();
            Console.ReadLine();
            itemizer.Supply(100000);
            Console.ReadLine();
            itemizer.Dispose();
            Console.WriteLine("test1: {0}, test2: {1}, test3: {2}, test4: {3}, test5: {4}", test1, test2, test3, test4, test5);
            Console.ReadLine();
        }
    }
}