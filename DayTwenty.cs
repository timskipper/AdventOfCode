namespace AdventOfCode;

public class DayTwenty
{
    public DayTwenty(string dataFile)
    {
        var queue = new LinkedList<int>();
        var sequence = new List<int>();

        var i = 0;
        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                var v = int.Parse(line);
                queue.AddLast(v);
                sequence.Add(v);
                i++;
            });

        //DisplayQueue(queue);

        foreach (var moves in sequence)
        {
            //Console.WriteLine($"Move {moves}");
            var current = queue.First;
            while (current != null)
            {
                if (current.Value != moves)
                {
                    current = current.Next;
                    continue;
                }

                if (moves < 0)
                {
                    var previous = current.PreviousOrLast();
                    for (var x = 0; x < Math.Abs(moves); x++)
                    {
                        previous = previous.PreviousOrLast();
                    }
                    queue.Remove(current);
                    queue.AddAfter(previous, current);
                }
                else if (moves > 0)
                {
                    var next = current.NextOrFirst();
                    for (var x = 0; x < Math.Abs(moves); x++)
                    {
                        next = next.NextOrFirst();
                    }
                    queue.Remove(current);
                    queue.AddBefore(next, current);
                }

                //DisplayQueue(queue);
                current = current.Next;
            }
        }

        Part1Answer = GetQueuePos(queue, 1000, 0) + GetQueuePos(queue, 2000, 0) + GetQueuePos(queue, 3000, 0);
    }

    public int Part1Answer { get; set; }

    public int Part2Answer { get; set; }

    private int GetQueuePos(LinkedList<int> queue, int pos, int afterValue)
    {
        var startCounting = false;
        var i = 0;

        var current = queue.First;
        while (true)
        {
            if (current.Value == afterValue)
            {
                startCounting = true;
            }
            if (startCounting)
            {
                i++;
            }
            if (i == pos + 1)
            {
                return current.Value;
            }
            current = current.NextOrFirst();
        }
    }

    private void DisplayQueue(LinkedList<int> queue)
    {
        foreach (var n in queue)
        {
            Console.Write($"{n} ");
        }
        Console.WriteLine();
    }
}