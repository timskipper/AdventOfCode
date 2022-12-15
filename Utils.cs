namespace AdventOfCode;

public static class Utils
{
    public static IEnumerable<(long, long)> CircleScan(ValueTuple<long, long> centre, long radius, long ty)
    {
        const float a = 0.707107F;
        var result = new HashSet<ValueTuple<long, long>>();

        var rotations = new HashSet<ValueTuple<ValueTuple<float, float>, ValueTuple<float, float>>>
        {
            new((1, 0), (0, 1)),
            new((a, a), (-a, a)),
            new((0, 1), (-1, 0)),
            new((-a, a), (-a, -a)),
            new((-1, 0), (0, -1)),
            new((-a, -a), (a, -a)),
            new((0, -1), (1, 0)),
            new((a, -a), (a, a))
        };

        for (var distance = 1; distance < radius + 1; distance++)
        {
            foreach (var angle in rotations)
            {
                var x = 0;
                var y = distance;
                var d = 1 - distance;

                while (x < y)
                {
                    var xr = angle.Item1.Item1 * x + angle.Item1.Item2 * y;
                    var yr = angle.Item2.Item1 * x + angle.Item2.Item2 * y;

                    xr = centre.Item1 + xr;
                    yr = centre.Item2 + yr;

                    var point = (Math.Round(xr), Math.Round(yr));

                    if ((long)yr == ty)
                    {
                        result.Add(((long)point.Item1, (long)point.Item2));
                    }

                    if (d < 0)
                    {
                        d += 3 + 2 * x;
                    }
                    else
                    {
                        d += 5 - 2 * (y - x);
                        y -= 1;
                    }

                    x += 1;
                }
            }
        }

        return result;
    }
}