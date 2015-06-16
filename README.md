#Scalable Itemizer

The scalable itemizer is a timer implementation aiming to give a solution to manage multiple actions using a shared resource. With scalable itemizers, you can add multiple actions to a single timer and specify how many iterations of the action should be executed in each interval. The scalable itemizer itself has a limited iteration capacity which lets you prioritize the actions bound to the itemizer. It is also possible to spread one action execution across multiple intervals.

#Automated itemizers

Automated itemizers create a pre-defined number of slots in every interval, which is specified in milliseconds. The bound actions must use these slots to be able to execute themselves. One execution takes away one slot from the pool.

Here is a simple example of an automated itemizer with 2 bound actions:

```c#
//Create 2 slots per 1 second
var itemizer = Itemizer.Automated(1000, 2);

//Bind an action which should be executed once per interval
itemizer.Add(1, () =>
{
    Console.WriteLine("Action 1 executing");
});

//Bind another action which should be executed once per interval
itemizer.Add(1, () =>
{
    Console.WriteLine("Action 2 executing");
});

itemizer.Start();
```

In this case, the two actions will be executed once every second.
However, if we change the second action binding to the following:

```C#
itemizer.Add(0.5, () =>
{
    Console.WriteLine("Action 2 executing");
});
```

The second action will be only executed once every two intervals. Once the action executes, it will take away one slot from the available pool. This means that Action 1 will be executed twice as much as Action 2.

Finally, if we look at the following console application, we can see a more complicated prioritization between bound actions:

```c#
private static void Main(string[] args)
{
    //Create 2 slots per 100 milliseconds
    var itemizer = Itemizer.Automated(100, 2);

    int action1 = 0;
    int action2 = 0;
    int action3 = 0;

    itemizer.Add(0.5, () =>
    {
        Interlocked.Increment(ref action1);
        Console.WriteLine("Action 1 executing");
    });

    itemizer.Add(1, () =>
    {
        Interlocked.Increment(ref action2);
        Console.WriteLine("Action 2 executing");
    });

    itemizer.Add(2, () =>
    {
        Interlocked.Increment(ref action3);
        Console.WriteLine("Action 3 executing");
    });

    itemizer.Start();
    Console.ReadLine();

    itemizer.Dispose();
    Console.WriteLine("Action 1: {0}\nAction 2: {1}\nAction 3:{2}\n", action1, action2, action3);
    Console.ReadLine();
}
```

Action 1 should be executed once every two intervals. Action 2 should be executed once every interval. Action 3 should be executed twice every interval. However, only 2 slots are available in every interval, therefore there is no possibility of executing all actions in one interval. A prioritization must take effect, which is based on the iteration number of the action. The higher the iteration number, the higher it will be prioritized. 

If we execute this console application and let it run a while, we can see the following pattern in the output:

```
Action 1: 25
Action 2: 50
Action 3: 98
```

Action 3 (2 intervals) is executed twice as much as Action 2 (1 interval), and Action 2 is executed twice as much as Action 1 (0.5 interval). Action 3 is executed four times more than Action 1.

One action cannot be executed more times than the slot count of the itemizer. If the itemizer gets 2 slots every round, actions can only be executed 2 times at maximum, and that means that the action consumes a whole round of slots when executed.

#Supplied itemizers

Supplied itemizers, unlike Automated itemizers, do not generate slots on their own. They must be "supplied" with a given number of slots, and the itemizer will share the slots between the actions according to the same prioritization pattern as in the Automated itemizers.

Here is a small example of a supplied itemizer in action:

```c#
private static void Main(string[] args)
{
    var itemizer = Itemizer.Supplied();
    int test1 = 0;
    int test2 = 0;
    int test3 = 0;
    int test4 = 0;
    int test5 = 0;

    itemizer.Add(0.5, ItemizerOptions.InheritItems, () =>
    {
        Interlocked.Increment(ref test1);
        Console.WriteLine("test 1");
    });

    itemizer.Add(1, ItemizerOptions.InheritItems, () =>
    {
        Interlocked.Increment(ref test2);
        Console.WriteLine("test 2");
    });

    itemizer.Add(2, ItemizerOptions.InheritItems, () =>
    {
        Interlocked.Increment(ref test3);
        Console.WriteLine("test 3");
    });

    itemizer.Add(2, ItemizerOptions.InheritItems, () =>
    {
        Interlocked.Increment(ref test4);
        Console.WriteLine("test 4");
    });
    
    itemizer.Add(0.5, ItemizerOptions.InheritItems, () =>
    {
        Interlocked.Increment(ref test5);
        Console.WriteLine("test 5");
    });

    itemizer.Start();
    Console.ReadLine();
    itemizer.Supply(100);
    Console.ReadLine();
    itemizer.Dispose();
    Console.WriteLine("test1: {0}, test2: {1}, test3: {2}, test4: {3}, test5: {4}", test1, test2, test3, test4, test5);
    Console.ReadLine();
}
```

If we execute this console application, we will see the following output:
```
test1: 8, test2: 17, test3: 34, test4: 33, test5: 8
```

In the output, we can see the same prioritization pattern. Every time the itemizer gets supplied with slots using the
    
    itemizer.Supply();
    
method, the slots gets distributed and if there is no slot left, the itemizer waits until it gets supplied again.

#Itemizer options

The **ItemizerOptions** enum is used to modify the behaviour of the itemization for the given action. The ItemizerOptions enum is a flag based enum which lets you specify multiple enum values using 'OR' switches.

You may have noticed the 

    ItemizerOptions.InheritItems
    
option passed when an action was bound in the last example. 

At default, if an action gets to be executed because it is given slots by the itemizer, if there were existing slots present at the action, they get replaced by the new slot count. If you would like to preserve the existing slots, you have to pass the 

    ItemizerOptions.InheritItems

flag with the action. This will tell the itemizer to append the new slots instead of overwriting the existing ones. It is advised to use this option when using Supplied itemizers.

Another itemizer option available is the
    
    ItemizerOptions.SeparateThread
    
which tells the itemizer to execute the actions in a separate thread. Notice that this could cause performance problems if there are multiple actions executing in a really fast pace.
    
    
    