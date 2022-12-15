using System.Text.RegularExpressions;

namespace AdventOfCode;

public class DayFourteen
{
    const char rockChar = '#';
    const char airChar = '.';
    const char sandChar = 'o';
    const char startChar = '+';

    private Dictionary<ValueTuple<long, long>, char> map;
    private ValueTuple<long, long> minMap = (int.MaxValue, int.MaxValue);
    private ValueTuple<long, long> maxMap = (0, 0);

    public DayFourteen(string dataFile)
    {
        map = new Dictionary<ValueTuple<long, long>, char>();
        var rockPaths = new Dictionary<int, List<ValueTuple<long, long>>>();

        var r = new Regex(@"((\d+,\d+)+)");
        var i = 0;

        var start = (500, 0);

        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                var rock = new List<ValueTuple<long, long>>();
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

        DrawRocks(rockPaths);
        DrawMap(start);
        PaintMap();
        DropSand(start);
        PaintMap();

        map = new Dictionary<(long, long), char>();
        DrawRocks(rockPaths);
        DrawMap(start);
        PaintMap();
        DropSandPart2(start);
        PaintMap();
    }

    public int Part1Answer { get; set; }

    public int Part2Answer { get; set; }

    private void DrawRocks(Dictionary<int, List<ValueTuple<long, long>>> rockPaths)
    {
        foreach (var rock in rockPaths)
        {
            var previousNode = rock.Value[0];
            map[previousNode] = rockChar;

            for (var r = 0; r < rock.Value.Count; r++)
            {
                var node = rock.Value[r];
                long xDelta = node.Item1 - previousNode.Item1;
                long yDelta = node.Item2 - previousNode.Item2;

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

    private void DrawMap(ValueTuple<long, long> start)
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

    private void DropSand(ValueTuple<long, long> start)
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
                    nextPos = (pos.Item1 + 1, pos.Item2 + 1);
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

    private void DropSandPart2(ValueTuple<long, long> start)
    {
        var grains = (from position
                    in map
                      where position.Value == airChar
                      select Solve_BFS(map, start, position.Key)
                into steps
                      where steps != -1
                      select steps)
            .ToList();

        Part2Answer = grains.Sum();
    }

    private static int Solve_BFS(Dictionary<ValueTuple<long, long>, char> map, ValueTuple<long, long> start, ValueTuple<long, long> finish)
    {
        var queue = new Queue<ValueTuple<long, long>>();
        var visited = new List<(long, long)>();

        var steps = 0;
        var nodesLeft = 1;
        var nodeCount = 0;
        var solved = false;

        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var position = queue.Dequeue();
            if (position == finish)
            {
                solved = true;
                break;
            }
            var validMoves = GetValidMoves(map, visited, position);
            foreach (var move in validMoves)
            {
                queue.Enqueue(move);
                visited.Add(move);
                nodeCount++;
            }

            nodesLeft--;
            if (nodesLeft == 0)
            {
                nodesLeft = nodeCount;
                nodeCount = 0;
                steps++;
            }
        }

        return solved ? steps : -1;
    }

    private static List<ValueTuple<long, long>> GetValidMoves(Dictionary<ValueTuple<long, long>, char> map, ICollection<(long, long)> visited, ValueTuple<long, long> position)
    {
        var testPositions = new List<ValueTuple<long, long>>
        {
            (position.Item1 - 1, position.Item2 - 1),
            (position.Item1 + 1, position.Item2 - 1),
            (position.Item1, position.Item2 - 1),
        };

        return testPositions.Where(test => map.ContainsKey(test) && map[test] <= map[position] + 1 && !visited.Contains(test)).ToList();
    }

    private void PaintMap()
    {
        Console.WriteLine();

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