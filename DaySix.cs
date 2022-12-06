namespace AdventOfCode
{
    public class DaySix
    {
        private int packetCharsProcessed;
        private int messageCharsProcessed;

        public DaySix(string dataFile)
        {
            var input = File.ReadAllText(dataFile);
            packetCharsProcessed = Process(input, 4);
            messageCharsProcessed = Process(input, 14);
        }

        public int Part1Answer => packetCharsProcessed;
        public int Part2Answer => messageCharsProcessed;

        public int Process(string input, int pos)
        {
            var buffer = new List<char>();
            int count = 0;
            foreach (var c in input)
            {
                buffer.Add(c);
                count++;
                if (buffer.Count < pos)
                {
                    continue;
                }
                if (buffer.Distinct().Count() == pos)
                {
                    break;
                }
                buffer.RemoveAt(0);
            }

            return count;
        }
    }
}
