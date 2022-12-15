namespace AdventOfCode;

public class DayFifteen
{
    public DayFifteen(string dataFile)
    {
        var nearestBeacons = new Dictionary<ValueTuple<long, long>, ValueTuple<long, long>>();
        var distances = new Dictionary<ValueTuple<long, long>, long>();

        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                line = line.Replace("Sensor at x=", "");
                line = line.Replace(", y=", ",");
                line = line.Replace("closest beacon is at x=", "");

                var l = line.Split(':');
                var sxy = l[0].Split(",");
                var bxy = l[1].Split(",");

                var sxy0 = long.Parse(sxy[0]);
                var sxy1 = long.Parse(sxy[1]);
                var bxy0 = long.Parse(bxy[0]);
                var bxy1 = long.Parse(bxy[1]);

                nearestBeacons.Add((sxy0, sxy1), (bxy0, bxy1));
                distances.Add((sxy0, sxy1), ManhattanDistance((sxy0, sxy1), (bxy0, bxy1)));
            });

        var ty = 2000000;

        var targetRow = FindNoBeacons(nearestBeacons, ty).Where(x => x.Item2 == ty).ToHashSet();
        Part1Answer = targetRow.Count;

        var a_line = new List<long>();
        var b_line = new List<long>();
        foreach (var sensor in distances)
        {
            a_line.Add(sensor.Key.Item2 - sensor.Key.Item1 + sensor.Value + 1);
            a_line.Add(sensor.Key.Item2 - sensor.Key.Item1 - sensor.Value - 1);
            b_line.Add(sensor.Key.Item1 + sensor.Key.Item2 + sensor.Value + 1);
            b_line.Add(sensor.Key.Item1 + sensor.Key.Item2 - sensor.Value - 1);
        }

        const long max = 4000000;
        const long tf = 4000000;
        var frequency = new HashSet<long>();

        foreach (var a in a_line)
        {
            foreach (var b in b_line)
            {
                var intersection = ((b - a) / 2, (a + b) / 2);
                if (intersection.Item1 is > 0 and < max && intersection.Item2 is > 0 and < max)
                {
                    if (distances.Keys.All(x => ManhattanDistance(intersection, x) > distances[x]))
                    {
                        frequency.Add(tf * intersection.Item1 + intersection.Item2);
                    }
                }
            }
        }

        Part2Answer = frequency.Single();
    }

    public long Part1Answer { get; set; }

    public long Part2Answer { get; set; }

    private long ManhattanDistance((long, long) x, (long, long) y)
    {
        return Math.Abs(x.Item1 - y.Item1) + Math.Abs(x.Item2 - y.Item2);
    }

    private HashSet<ValueTuple<long, long>> FindNoBeacons(Dictionary<ValueTuple<long, long>, ValueTuple<long, long>> nearestBeacons, long ty)
    {
        var nobeacons = new HashSet<ValueTuple<long, long>>();

        foreach (var sensor in nearestBeacons)
        {
            var d = ManhattanDistance(sensor.Key, sensor.Value);
            if (ty > sensor.Key.Item2 + d && ty < sensor.Key.Item2 - d) continue;

            var dtc = (d - Math.Abs(ty - sensor.Key.Item2));

            for (var i = sensor.Key.Item1 - dtc; i < sensor.Key.Item1 + dtc; i++)
            {
                nobeacons.Add((i, ty));
            }
        }

        return nobeacons;
    }
}