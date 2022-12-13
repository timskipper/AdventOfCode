namespace AdventOfCode;

public class DayTwelve
{
    public DayTwelve(string dataFile)
    {
        var start = (0, 0);
        var finish = (0, 0);

        var map = new Dictionary<(int, int), int>();
        var input = File.ReadAllLines(dataFile);

        var y = 0;
        foreach (var line in input)
        {
            var x = 0;
            foreach (var c in line)
            {
                switch (c)
                {
                    case 'S':
                        start = (x, y);
                        map.Add((x, y), 'a');
                        break;
                    case 'E':
                        finish = (x, y);
                        map.Add((x, y), 'z');
                        break;
                    default:
                        map.Add((x, y), c);
                        break;
                }
                x++;
            }
            y++;
        }

        DisplayTrack(map);

        Part1Answer = Solve_BFS(map, start, finish);

        const char startingElevation = 'a';
        var minSteps = (from position
                in map
                        where position.Value == startingElevation
                        select Solve_BFS(map, position.Key, finish)
                into steps
                        where steps != -1
                        select steps)
            .ToList();

        Part2Answer = minSteps.Min();
    }

    public int Part1Answer { get; }

    public int Part2Answer { get; }

    public void DisplayTrack(Dictionary<ValueTuple<int, int>, int> map)
    {
        var maxSize = map.Max(x => x.Key);
        for (var y = 0; y < maxSize.Item2; y++)
        {
            for (var x = 0; x < maxSize.Item1; x++)
            {
                Console.Write((char)map[(x, y)]);
            }
            Console.WriteLine();
        }
    }

    private static int Solve_BFS(Dictionary<ValueTuple<int, int>, int> map, ValueTuple<int, int> start, ValueTuple<int, int> finish)
    {
        var queue = new Queue<ValueTuple<int, int>>();
        var visited = new List<(int, int)>();

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

    private static List<ValueTuple<int, int>> GetValidMoves(Dictionary<ValueTuple<int, int>, int> map, ICollection<(int, int)> visited, ValueTuple<int, int> position)
    {
        var testPositions = new List<ValueTuple<int, int>>
        {
            (position.Item1, position.Item2 - 1),
            (position.Item1 - 1, position.Item2),
            (position.Item1 + 1, position.Item2),
            (position.Item1, position.Item2 + 1),
        };

        return testPositions.Where(test => map.ContainsKey(test) && map[test] <= map[position] + 1 && !visited.Contains(test)).ToList();
    }

    private static Dictionary<(int, int), int> BuildTrack(Dictionary<ValueTuple<int, int>, int> map, List<ValueTuple<int, int>> track)
    {
        var result = new Dictionary<ValueTuple<int, int>, int>();

        foreach (var node in map)
        {
            if (track.Contains(node.Key))
            {
                result.Add(node.Key, (char)'@');
            }
            else
            {
                result.Add(node.Key, node.Value);
            }
        }

        return result;
    }
}