namespace AdventOfCode;

public class DayTwentyOne
{
    private readonly string dataFile;
    private Dictionary<string, long> yellers;
    private Dictionary<string, (string left, string operation, string right)> operations;

    public DayTwentyOne(string dataFile)
    {
        this.dataFile = dataFile;

        ParseInput();

        long? answer = null;
        while (answer is null)
        {
            var calculations = JibberJabber(yellers, operations, false);
            if (!calculations.Any())
            {
                continue;
            }
            if (calculations.ContainsKey("root"))
            {
                answer = calculations["root"];
            }
            else
            {
                foreach (var c in calculations.Where(c => !yellers.ContainsKey(c.Key)))
                {
                    yellers.Add(c.Key, c.Value);
                    operations.Remove(c.Key);
                }
            }
        }

        Part1Answer = (long)answer;

        //////////////////////////////////////////////////////////////////////////////////////////////////////////

        var allYellers = new Dictionary<string, long>(yellers);

        ParseInput();

        answer = null;
        yellers = new Dictionary<string, long>(allYellers);
        var originalOps = new Dictionary<string, (string left, string operation, string right)>(operations);

        long breakAt = 302;//long.MaxValue;
        long processed = 0;
        //Parallel.For(301, breakAt, (humanNumber, state) =>
        for (var humanNumber = 300; humanNumber < breakAt; humanNumber++)
        {
            processed++;
            if (processed % 92233720368547700 == 0)
            {
                Console.WriteLine($"Processing... {processed} {(processed / long.MaxValue) * 100}");
            }

            //if (humanNumber == breakAt)
            //{
            //    state.Break();
            //}

            yellers["humn"] = humanNumber;

            while (answer is null)
            {
                var calculations = JibberJabber(yellers, operations, true);
                if (!calculations.Any())
                {
                    continue;
                }

                if (calculations.ContainsKey("root"))
                {
                    if (calculations["root"] == -1)
                    {
                        answer = yellers["humn"];
                        Part2Answer = (long)answer;
                        //state.Break();
                        return;
                    }

                    operations = new Dictionary<string, (string left, string operation, string right)>(originalOps);
                    break;
                }

                foreach (var c in calculations.Where(c => !yellers.ContainsKey(c.Key)))
                {
                    yellers.Add(c.Key, c.Value);
                    operations.Remove(c.Key);
                }
            }
        }
        //});
    }

    public long Part1Answer { get; set; }
    public long Part2Answer { get; set; }

    private Dictionary<string, long> JibberJabber(Dictionary<string, long> yelled,
        Dictionary<string, (string left, string operation, string right)> operations,
        bool isPart2)
    {
        var calcs = new Dictionary<string, long>();
        foreach (var op in operations)
        {
            if (!yelled.ContainsKey(op.Value.left) || !yelled.ContainsKey(op.Value.right)) continue;

            var left = yelled[op.Value.left];
            var right = yelled[op.Value.right];
            var operation = op.Value.operation;
            calcs.Add(op.Key, calculate(left, operation, right));
        }

        if (isPart2)
        {
            var rootOp = operations.SingleOrDefault(x => x.Key == "root");
            if (yelled[rootOp.Value.left] == yelled[rootOp.Value.right])
            {
                calcs.Add(rootOp.Key, -1);
                return calcs;
            }
        }

        return calcs;
    }

    private long calculate(long left, string operation, long right)
    {
        switch (operation)
        {
            case "+":
                return left + right;
            case "-":
                return left - right;
            case "*":
                return left * right;
            case "/":
                return left / right;
            default:
                throw new ArgumentException("monkey talk!");
        }
    }

    private void ParseInput()
    {
        yellers = new Dictionary<string, long>();
        operations = new Dictionary<string, (string left, string operation, string right)>();

        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                var monkey = line.Substring(0, 4);
                if (int.TryParse(line.Substring(6, line.Length - 6), out var number))
                {
                    yellers.Add(monkey, number);
                }
                else
                {
                    var left = line.Substring(6, 4);
                    var op = line.Substring(11, 1);
                    var right = line.Substring(13, 4);
                    operations.Add(monkey, (left, op, right));
                }
            });
    }
}