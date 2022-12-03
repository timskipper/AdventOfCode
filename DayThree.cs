using System;
using System.Text;

namespace AdventOfCode
{
    public class DayThree
    {
        private int part1Answer = 0;
        private int part2Answer = 0;
        private List<List<string>> groups;

        public DayThree(string dataFile)
        {
            part1Answer = 0;
            var input = File.ReadAllLines(dataFile);

            foreach (var line in input)
            {
                var bytes1 = Encoding.ASCII.GetBytes(line.Substring(0, line.Length / 2));
                var bytes2 = Encoding.ASCII.GetBytes(line.Substring(line.Length / 2));
                var error = bytes1.Intersect(bytes2).Distinct().Single();
                part1Answer += GetPriority(error);
            }

            part2Answer = 0;
            var groups = Split(input);
            foreach (var group in groups)
            {
                var bytes1 = Encoding.ASCII.GetBytes(group[0]);
                var bytes2 = Encoding.ASCII.GetBytes(group[1]);
                var bytes3 = Encoding.ASCII.GetBytes(group[2]);
                var badge = bytes1.Intersect(bytes2).Intersect(bytes3).Distinct().Single();
                part2Answer += GetPriority(badge);
            }
        }

        public int Part1Answer => part1Answer;
        public int Part2Answer => part2Answer;

        private int GetPriority(int c)
        {
            var result = c >= 97 ? c - 96 : c - 38;
            return result;
        }

        public static List<List<T>> Split<T>(IList<T> source)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 3)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
    }
}
