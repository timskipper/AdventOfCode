namespace AdventOfCode;

public class DayEleven
{
    protected class Monkey
    {
        public Monkey()
        {
            Items = new List<long>();
        }

        public List<long> Items { get; set; }
        public string Operation { get; set; }
        public bool UseOldValue { get; set; }
        public long RightSide { get; set; }
        public long DivisibleBy { get; set; }
        public ValueTuple<int, int> ThrowTo { get; set; }
        public long ItemsInspected { get; set; }

        public override string ToString()
        {
            return $"Items: {Items.Count} Inspected: {ItemsInspected}";
        }
    }

    public DayEleven(string dataFile)
    {
        var input = File.ReadAllLines(dataFile);
        var monkeys = ParseNotes(input);
        Monkey[] topTwoMonkeys;

        monkeys = DoMonkeys(monkeys, 20, 3);
        topTwoMonkeys = monkeys.OrderByDescending(x => x.ItemsInspected).Take(2).ToArray();
        Part1Answer = topTwoMonkeys[0].ItemsInspected * topTwoMonkeys[1].ItemsInspected;

        monkeys = ParseNotes(input);
        monkeys = DoMonkeys(monkeys, 10000, 1);
        topTwoMonkeys = monkeys.OrderByDescending(x => x.ItemsInspected).Take(2).ToArray();
        Part2Answer = topTwoMonkeys[0].ItemsInspected * topTwoMonkeys[1].ItemsInspected;
    }

    public long Part1Answer { get; }
    public long Part2Answer { get; }

    private List<Monkey> ParseNotes(string[] input)
    {
        var monkeys = new List<Monkey> { new Monkey() };
        int monkeyIndex = 0;
        var throwLogic = (0, 0);

        foreach (var l in input)
        {
            var line = l.Trim();

            if (line.StartsWith("Monkey "))
            {
                monkeyIndex = int.Parse(line.Replace("Monkey ", "").Split(':')[0]);

                if (monkeyIndex > 0)
                {
                    // Not my first monkey
                    monkeys[monkeyIndex - 1].ThrowTo = throwLogic;
                    monkeys.Add(new Monkey());
                }
                throwLogic = monkeys[monkeyIndex].ThrowTo;
            }

            if (line.StartsWith("Starting items:"))
            {
                var items = line.Replace("Starting items: ", "").Split(',');
                foreach (var item in items)
                {
                    monkeys[monkeyIndex].Items.Add(int.Parse(item));
                }
            }

            if (line.StartsWith("Operation:"))
            {
                var command = line.Replace("Operation: new = ", "").Split(' ');
                monkeys[monkeyIndex].Operation = command[1];
                monkeys[monkeyIndex].UseOldValue = command[2] == "old";
                if (int.TryParse(command[2], out var value))
                {
                    monkeys[monkeyIndex].RightSide = value;
                }
            }

            if (line.StartsWith("Test:"))
            {
                monkeys[monkeyIndex].DivisibleBy = int.Parse(line.Replace("Test: divisible by ", ""));
            }

            if (line.StartsWith("If true:"))
            {
                throwLogic.Item1 = int.Parse(line.Replace("If true: throw to monkey ", ""));
            }

            if (line.StartsWith("If false:"))
            {
                throwLogic.Item2 = int.Parse(line.Replace("If false: throw to monkey ", ""));
            }
        }
        monkeys[monkeyIndex].ThrowTo = throwLogic;

        return monkeys;
    }

    private List<Monkey> DoMonkeys(List<Monkey> monkeys, int rounds, long worryLevel)
    {
        for (var round = 0; round < rounds; round++)
        {
            for (var m = 0; m < monkeys.Count; m++)
            {
                var count = monkeys[m].Items.Count;
                monkeys[m].ItemsInspected += count;

                for (long i = 0; i < count; i++)
                {
                    var newWorry = Worry(monkeys[m].Items[0], monkeys[m].Operation, monkeys[m].UseOldValue ? monkeys[m].Items[0] : monkeys[m].RightSide);
                    if (worryLevel == 1)
                    {
                        var result = monkeys.Aggregate<Monkey, long>(1, (current, monkey) => current * monkey.DivisibleBy);
                        newWorry %= result;
                    }

                    monkeys[m].Items[0] = newWorry;
                    monkeys[m].Items[0] /= worryLevel;

                    if (monkeys[m].Items[0] % monkeys[m].DivisibleBy == 0)
                    {
                        monkeys[monkeys[m].ThrowTo.Item1].Items.Add(monkeys[m].Items[0]);
                    }
                    else
                    {
                        monkeys[monkeys[m].ThrowTo.Item2].Items.Add(monkeys[m].Items[0]);
                    }
                    monkeys[m].Items.RemoveAt(0);
                }
            }
        }

        return monkeys;
    }

    private long Worry(long left, string operation, long right)
    {
        if (operation == "*")
        {
            return left *= right;
        }

        if (operation == "+")
        {
            return left += right;
        }

        return 0;
    }
}