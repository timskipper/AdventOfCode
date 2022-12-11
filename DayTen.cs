namespace AdventOfCode
{
    public class DayTen
    {
        private readonly Dictionary<int, int> signals;

        public DayTen(string dataFile)
        {
            var input = File.ReadAllLines(dataFile);

            var cycle = 0;
            var x = 1;
            signals = new Dictionary<int, int>();

            foreach (var line in input)
            {
                var command = line.Split(' ');
                var instruction = command[0];

                int signalStrength;
                if (instruction == "noop")
                {
                    cycle++;
                    signalStrength = cycle * x;
                    RecordSignalStrength(cycle, signalStrength);
                    DrawPixel(cycle, x);
                }

                if (instruction == "addx")
                {
                    var value = int.Parse(command[1]);

                    for (var c = 0; c < 2; c++)
                    {
                        cycle++;
                        signalStrength = cycle * x;
                        RecordSignalStrength(cycle, signalStrength);
                        DrawPixel(cycle, x);
                    }
                    x += value;
                }
            }

            Part1Answer = signals[20] + signals[60] + signals[100] + signals[140] + signals[180] + signals[220];
        }

        public int Part1Answer { get; }

        private void RecordSignalStrength(int cycle, int ss)
        {
            if (cycle % 20 == 0)
            {
                signals.Add(cycle, ss);
            }
        }

        private void DrawPixel(int cycle, int x)
        {
            var xPos = (cycle % 40) - 1;
            if (xPos == x - 1 || xPos == x || xPos == x + 1)
            {
                Console.Write("#");
            }
            else
            {
                Console.Write(" ");
            }

            if (cycle % 40 == 0)
            {
                Console.WriteLine();
            }
        }
    }
}
