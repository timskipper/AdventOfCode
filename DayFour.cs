namespace AdventOfCode
{
    public class DayFour
    {
        private int totalOverlaps;

        public DayFour(string dataFile)
        {
            var input = File.ReadAllLines(dataFile);

            foreach (var line in input)
            {
                (var r1, var r2) = GetPairRanges(line);
                var overlap = r1.Intersect(r2);
                if (overlap.Count() == r1.Count() || overlap.Count() == r2.Count())
                {
                    totalOverlaps++;
                }
            }
        }

        public int Part1Answer => totalOverlaps;

        private (List<int> r1, List<int> r2) GetPairRanges(string line)
        {
            var pairs = line.Split(",");
            var p1Range = pairs[0].Split("-");
            var p2Range = pairs[1].Split("-");

            return (PopulateRange(p1Range), PopulateRange(p2Range));
        }

        private List<int> PopulateRange(string[] range)
        {
            var start = int.Parse(range[0]);
            var end = int.Parse(range[1]);

            var sections = new List<int>();
            for (int i = start; i <= end; i++)
            {
                sections.Add(i);
            }

            return sections;
        }
    }
}
