using Microsoft.Diagnostics.Runtime.Utilities;
using Perfolizer.Mathematics.QuantileEstimators;

namespace AdventOfCode;

public class DayTwenty
{
    public DayTwenty(string dataFile)
    {
        var sequence = new List<(long val, int i)>();
        var queue = new List<(long val, int i)>();

        var i = 0;
        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                var v = long.Parse(line);
                queue.Add((v, i));
                sequence.Add((v, i));
                i++;
            });

        queue = Decrypt(sequence, queue, 1);

        var zeroIndex = queue.IndexOf(queue.Single(x => x.val == 0));
        var p1 = GetQueuePos(queue, 1000 + zeroIndex);
        var p2 = GetQueuePos(queue, 2000 + zeroIndex);
        var p3 = GetQueuePos(queue, 3000 + zeroIndex);

        Part1Answer = p1 + p2 + p3;

        /////////////////////////////////////////////////////////////////////////// 

        sequence = new List<(long val, int i)>();
        queue = new List<(long val, int i)>();

        long decryptionKey = 811589153;
        i = 0;
        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                var v = long.Parse(line) * decryptionKey;
                queue.Add((v, i));
                sequence.Add((v, i));
                i++;
            });

        queue = Decrypt(sequence, queue, 10);

        zeroIndex = queue.IndexOf(queue.Find(y => y.val == 0));
        p1 = GetQueuePos(queue, 1000 + zeroIndex);
        p2 = GetQueuePos(queue, 2000 + zeroIndex);
        p3 = GetQueuePos(queue, 3000 + zeroIndex);

        Part2Answer = p1 + p2 + p3;

        var d20 = new Day20(dataFile);
        Console.WriteLine(d20.Part1());
        Console.WriteLine(d20.Part2());
    }

    public long Part1Answer { get; set; }

    public long Part2Answer { get; set; }

    private List<(long val, int i)> Decrypt(List<(long val, int i)> sequence, List<(long val, int i)> queue, int iterations)
    {
        DisplayQueue(queue);

        var queueCount = queue.Count;
        for (var i = 0; i < iterations; i++)
        {
            foreach (var number in sequence)
            {
                var oldIndex = queue.IndexOf(number);
                var newIndex = (int)(oldIndex + number.val) % (queueCount - 1);
                if (newIndex <= 0 && oldIndex + number.val != 0)
                {
                    newIndex = queueCount - 1 + newIndex;
                }

                queue.RemoveAt(oldIndex);
                queue.Insert(newIndex, number);

                DisplayQueue(queue);
            }
        }

        return queue;
    }

    private long GetQueuePos(List<(long, int)> queue, int pos)
    {
        return queue[pos % queue.Count].Item1;
    }

    private void DisplayQueue(List<(long, int)> queue)
    {
        return;
        foreach (var n in queue)
        {
            Console.Write($"{n.Item1} ");
        }
        Console.WriteLine();
    }

    class Day20
    {
        private List<string> puzzleInputLines;

        public Day20(string inputPath)
        {
            puzzleInputLines = new List<string>();
            File.ReadAllLines(inputPath)
                .ToList()
                .ForEach(line =>
                {
                    puzzleInputLines.Add(line);
                });
        }

        public string Part1()
        {
            List<(long val, int salt)> numbers = puzzleInputLines.Select((x, i) => (long.Parse(x), i)).ToList();
            numbers = Mix(numbers);
            int zeroIndex = numbers.IndexOf(numbers.Find(x => x.val == 0));
            return (
                GetAtCircular(numbers, 1000 + zeroIndex) +
                GetAtCircular(numbers, 2000 + zeroIndex) +
                GetAtCircular(numbers, 3000 + zeroIndex)
            ).ToString();
        }

        public string Part2()
        {
            List<(long val, int salt)> numbers = puzzleInputLines.Select((x, i) => (long.Parse(x) * 811589153, i)).ToList();
            List<(long val, int salt)> mixedNumbers = new(numbers);
            for (int i = 0; i < 10; i++)
            {
                mixedNumbers = Mix(numbers, mixedNumbers);
            }
            int zeroIndex = mixedNumbers.IndexOf(mixedNumbers.Find(x => x.val == 0));
            return (
                GetAtCircular(mixedNumbers, 1000 + zeroIndex) +
                GetAtCircular(mixedNumbers, 2000 + zeroIndex) +
                GetAtCircular(mixedNumbers, 3000 + zeroIndex)
            ).ToString();
        }

        protected List<(long val, int salt)> Mix(List<(long val, int i)> startNumbers, List<(long val, int i)>? result = null)
        {
            result = result == null ? new(startNumbers) : result;
            foreach (var number in startNumbers)
            {
                var oldIndex = result.IndexOf(number);
                var newIndex = (int)((oldIndex + number.val) % (result.Count - 1));
                if (newIndex <= 0 && oldIndex + number.val != 0)
                {
                    newIndex = result.Count - 1 + newIndex;
                }

                result.RemoveAt(oldIndex);
                result.Insert(newIndex, number);
            }
            return result;
        }

        protected long GetAtCircular(List<(long val, int salt)> list, int index)
        {
            return list[index % list.Count].val;
        }
    }
}