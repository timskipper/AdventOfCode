namespace AdventOfCode;

public class DaySixteen
{
    private readonly Dictionary<string, int> flow_rates;
    private readonly Dictionary<string, List<string>> connections;
    private Dictionary<(string, List<string>, int), int> cache;

    public DaySixteen(string dataFile)
    {
        cache = new Dictionary<(string, List<string>, int), int>();
        flow_rates = new Dictionary<string, int>();
        connections = new Dictionary<string, List<string>>();

        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                line = line.Replace("Valve ", "");
                line = line.Replace(" has flow rate=", ",");
                line = line.Replace(" tunnel leads to valve ", "");
                line = line.Replace(" tunnels lead to valves ", "");
                line = line.Replace(", ", ",");

                var l = line.Split(';');
                var left = l[0].Split(",");
                var right = l[1].Split(",");

                var flow = int.Parse(left[1]);
                connections.Add(left[0], right.ToList());
                flow_rates.Add(left[0], flow);
            });

        var co = new List<string>();
        //Part1Answer = FindMaxPressure("AA", co, 30).Result;
        Part2Answer = Part2().Result;
    }

    public long Part1Answer { get; set; }

    public long Part2Answer { get; set; }

    private async Task<int> Part2()
    {
        cache = new Dictionary<(string, List<string>, int), int>();
        var humans = await FindMaxPressure("AA", new List<string>(), 26);
        var elephants = await FindMaxPressure("AA", new List<string>(), 26);
        return humans + elephants;
    }

    private async Task<int> FindMaxPressure(string current, List<string> opened, int minutes_remaining)
    {
        if (cache.TryGetValue((current, opened, minutes_remaining), out var value))
        {
            return value;
        }

        var best = 0;
        var rate = (minutes_remaining - 1) * flow_rates[current];

        var current_opened = new List<string>();
        current_opened.AddRange(opened);
        current_opened.Add(current);

        foreach (var adjacent in connections[current])
        {
            if (!opened.Contains(current) && rate != 0)
            {
                if (minutes_remaining <= -1) return best;
                best = Utils.Max(best, rate + await FindMaxPressure(adjacent, current_opened, minutes_remaining - 2));
            }
            if (minutes_remaining <= 0) return best;
            best = Utils.Max(best, await FindMaxPressure(adjacent, opened, minutes_remaining - 1));
        }

        cache.TryAdd((current, opened, minutes_remaining), best);
        return best;
    }

    
}
