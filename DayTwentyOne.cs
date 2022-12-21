namespace AdventOfCode;

public class DayTwentyOne
{
    private readonly string dataFile;
    private Queue<(string monkey, long number)> yellers;
    private Dictionary<string, long> yelled;
    private Dictionary<string, (string left, string operation, string right)> operations;

    public DayTwentyOne(string dataFile)
    {
        this.dataFile = dataFile;
    }

    public long Part1Answer
    {
        get
        {
            ParseInput();

            long? answer = null;
            while (answer is null)
            {
                if (yellers.Any())
                {
                    var y = yellers.Dequeue();
                    yelled.Add(y.monkey, y.number);
                }

                if (yelled.Count < 2) continue;

                var calculations = JibberJabber(yelled, operations, false);
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
                    foreach (var c in calculations.Where(c => !yelled.ContainsKey(c.Key)))
                    {
                        yelled.Add(c.Key, c.Value);
                        operations.Remove(c.Key);
                    }
                }
            }

            return (long)answer;
        }
    }

    public long Part2Answer
    {
        get
        {
            return 0L;
        }
    }

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

            if (isPart2 && op.Key == "root" && left == right)
            {
                calcs.Add(op.Key, -1);
                return calcs;
            }
            calcs.Add(op.Key, calculate(left, operation, right));
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
        yellers = new Queue<(string monkey, long number)>();
        yelled = new Dictionary<string, long>();
        operations = new Dictionary<string, (string left, string operation, string right)>();

        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                var monkey = line.Substring(0, 4);
                if (int.TryParse(line.Substring(6, line.Length - 6), out var number))
                {
                    yellers.Enqueue((monkey, number));
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