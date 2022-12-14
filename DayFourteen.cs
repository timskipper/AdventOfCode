using System.Text.RegularExpressions;

namespace AdventOfCode;

public class DayFourteen
{
    const char rockChar = '#';
    const char airChar = '.';
    const char sandChar = 'o';
    const char startChar = '+';

    private Dictionary<ValueTuple<int, int>, char> map;
    private ValueTuple<int, int> minMap = (int.MaxValue, int.MaxValue);
    private ValueTuple<int, int> maxMap = (0, 0);

    public DayFourteen(string dataFile)
    {
        map = new Dictionary<ValueTuple<int, int>, char>();
        var rockPaths = new Dictionary<int, List<ValueTuple<int, int>>>();

        var r = new Regex(@"((\d+,\d+)+)");
        var i = 0;

        var start = (500, 0);

        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                var rock = new List<ValueTuple<int, int>>();
                foreach (var pair in r.Matches(line).Cast<Match>())
                {
                    var p = pair.ToString().Split(',');
                    var x = int.Parse(p[0]);
                    var y = int.Parse(p[1]);
                    rock.Add((x, y));

                    minMap.Item1 = x < minMap.Item1 ? x : minMap.Item1;
                    minMap.Item2 = y < minMap.Item2 ? y : minMap.Item2;
                    maxMap.Item1 = x > maxMap.Item1 ? x : maxMap.Item1;
                    maxMap.Item2 = y > maxMap.Item2 ? y : maxMap.Item2;
                }

                rockPaths.Add(i, rock);
                i++;
            });

        //Console.ReadLine();
        DrawRocks(rockPaths);
        DrawMap(start);
        PaintMap();
        DropSand(start);
        PaintMap();
    }

    public int Part1Answer { get; set; }

    public int Part2Answer { get; }

    private void DrawRocks(Dictionary<int, List<ValueTuple<int, int>>> rockPaths)
    {
        foreach (var rock in rockPaths)
        {
            var previousNode = rock.Value[0];
            map[previousNode] = rockChar;

            for (var r = 0; r < rock.Value.Count; r++)
            {
                var node = rock.Value[r];
                var xDelta = node.Item1 - previousNode.Item1;
                var yDelta = node.Item2 - previousNode.Item2;

                if (xDelta != 0 && yDelta == 0)
                {
                    if (xDelta > 0)
                    {
                        for (var i = 0; i <= xDelta; i++)
                        {
                            map[(previousNode.Item1 + i, previousNode.Item2)] = rockChar;
                        }
                    }
                    if (xDelta < 0)
                    {
                        for (var i = xDelta; i <= 0; i++)
                        {
                            map[(previousNode.Item1 + i, previousNode.Item2)] = rockChar;
                        }
                    }
                }

                if (xDelta == 0 && yDelta != 0)
                {
                    if (yDelta > 0)
                    {
                        for (var i = 0; i <= yDelta; i++)
                        {
                            map[(previousNode.Item1, previousNode.Item2 + i)] = rockChar;
                        }
                    }
                    if (yDelta < 0)
                    {
                        for (var i = yDelta; i <= 0; i++)
                        {
                            map[(previousNode.Item1, previousNode.Item2 + i)] = rockChar;
                        }
                    }
                }

                previousNode = node;
            }
        }
    }

    private void DrawMap(ValueTuple<int, int> start)
    {
        map[start] = startChar;
        minMap.Item1 = start.Item1 < minMap.Item1 ? start.Item1 : minMap.Item1;
        minMap.Item2 = start.Item2 < minMap.Item2 ? start.Item2 : minMap.Item2;
        maxMap.Item1 = start.Item1 > maxMap.Item1 ? start.Item1 : maxMap.Item1;
        maxMap.Item2 = start.Item2 > maxMap.Item2 ? start.Item2 : maxMap.Item2;

        for (var y = minMap.Item2; y < maxMap.Item2 + 1; y++)
        {
            for (var x = minMap.Item1; x < maxMap.Item1 + 1; x++)
            {
                if (!map.ContainsKey((x, y)))
                {
                    map[(x, y)] = airChar;
                }
            }
        }
    }

    private void DropSand(ValueTuple<int, int> start)
    {
        var grains = 0;
        var pos = start;
        var nextPos = start;

        while (pos.Item1 >= minMap.Item1 && pos.Item1 <= maxMap.Item1 && pos.Item2 <= maxMap.Item2)
        {
            if (map.ContainsKey((pos.Item1, pos.Item2 + 1)) && map[(pos.Item1, pos.Item2 + 1)] is rockChar or sandChar)
            {
                // Can't go down
                if (map.ContainsKey((pos.Item1 - 1, pos.Item2 + 1)) && map[(pos.Item1 - 1, pos.Item2 + 1)] is rockChar or sandChar)
                {
                    // Can't go down/left
                    if (map.ContainsKey((pos.Item1 + 1, pos.Item2 + 1)) && map[(pos.Item1 + 1, pos.Item2 + 1)] is rockChar or sandChar)
                    {
                        // Can't go down/right
                        // Stop here
                        map[nextPos] = sandChar;
                        grains++;
                        pos = start;
                        continue;
                    }
                    else
                    {
                        nextPos = (pos.Item1 + 1, pos.Item2 + 1);
                    }
                }
                else
                {
                    nextPos = (pos.Item1 - 1, pos.Item2 + 1);
                }
            }
            else
            {
                nextPos = (pos.Item1, pos.Item2 + 1);
            }

            pos = nextPos;
        }

        Part1Answer = grains;
    }

    private void PaintMap()
    {
        Console.Clear();

        for (var y = minMap.Item2; y < maxMap.Item2 + 1; y++)
        {
            for (var x = minMap.Item1; x < maxMap.Item1 + 1; x++)
            {
                Console.Write(map[(x, y)]);
            }
            Console.WriteLine();
        }
    }
}