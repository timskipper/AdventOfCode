using System.Numerics;

namespace AdventOfCode;
public class DayEighteen
{
    public DayEighteen(string dataFile)
    {
        var cubes = new Dictionary<Vector3, int>();

        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                var input = line.Split(',');
                var x = int.Parse(input[0]);
                var y = int.Parse(input[1]);
                var z = int.Parse(input[2]);

                var vector = new Vector3(x, y, z);
                cubes.Add(vector, 6);
            });

        foreach (var c in cubes)
        {
            var sides = ExposedSides(c.Key, cubes);
            cubes[c.Key] = sides;
        }

        Part1Answer = cubes.Sum(x => x.Value);

        var noSides = cubes.Count(x => x.Value == 0);
        Part2Answer = Part1Answer - (noSides * 6);
    }

    public long Part1Answer { get; set; }

    public long Part2Answer { get; set; }

    public int ExposedSides(Vector3 cube, Dictionary<Vector3, int> cubes)
    {
        var sides = 6;

        var vectors = new[]
        {
            cube with { X = cube.X - 1 },
            cube with { Y = cube.Y + 1 },
            cube with { X = cube.X + 1 },
            cube with { Y = cube.Y - 1 },
            cube with { Z = cube.Z - 1 },
            cube with { Z = cube.Z + 1 }
        };

        foreach (var v in vectors)
        {
            if (cubes.ContainsKey(v))
            {
                sides--;
            }
        }

        return sides;
    }
}