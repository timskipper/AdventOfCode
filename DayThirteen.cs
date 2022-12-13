using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode;

public class DayThirteen
{
    public DayThirteen(string dataFile)
    {
        var correctOrder = new List<int>();

        var i = 0;
        File.ReadAllLines(dataFile)
            .Chunk(3).ToList()
            .ForEach(line =>
            {
                Console.WriteLine(line[0]);
                Console.WriteLine(line[1]);

                i++;
                var s1 = (JArray)JsonConvert.DeserializeObject(line[0])!;
                var s2 = (JArray)JsonConvert.DeserializeObject(line[1])!;

                var discontinue = false;
                if (CheckArray(ref s1, ref s2, ref discontinue))
                {
                    correctOrder.Add(i);
                    Console.WriteLine($"{i} is Correct");
                }
                else
                {
                    Console.WriteLine($"{i} is Incorrect");
                }

                Console.WriteLine();
            });

        Console.WriteLine($"{correctOrder.Count} packets are in the correct order");
        Part1Answer = correctOrder.Sum(x => x);
    }

    public int Part1Answer { get; }

    public int Part2Answer { get; }

    private bool CheckArray(ref JArray left, ref JArray right, ref bool discontinue)
    {
        var i = 0;
        var ok = false;

        if (discontinue) { return false; }
        if (!left.Any()) { return true; }

        foreach (var item in left)
        {
            if (i > right.Count - 1)
            {
                return false;
            }

            var nodeL = item;
            var nodeR = right[i];

            if (!discontinue && nodeL.Type == JTokenType.Array && nodeR.Type == JTokenType.Array)
            {
                var p1 = (JArray)nodeL;
                var p2 = (JArray)nodeR;
                ok = CheckArray(ref p1, ref p2, ref discontinue);
            }
            else if (!discontinue && nodeL.Type == JTokenType.Integer && nodeR.Type == JTokenType.Array)
            {
                nodeL = JArray.Parse($"[{nodeL.Value<int>()}]");
                var p1 = (JArray)nodeL;
                var p2 = (JArray)nodeR;
                left = p1;

                ok = CheckArray(ref p1, ref p2, ref discontinue);
            }
            else if (!discontinue && nodeL.Type == JTokenType.Array && nodeR.Type == JTokenType.Integer)
            {
                var p1 = (JArray)nodeL;
                var p2 = JArray.Parse($"[{nodeR.Value<int>()}]");
                right = p2;

                ok = CheckArray(ref p1, ref p2, ref discontinue);
            }
            else if (!discontinue && nodeL.Type == JTokenType.Integer && nodeR.Type == JTokenType.Integer)
            {
                ok = nodeL.Value<int>() <= nodeR.Value<int>();
                discontinue = ok;
            }

            i++;
        }

        if (i > left.Count && i <= right.Count)
        {
            return true;
        }

        return ok;
    }
}