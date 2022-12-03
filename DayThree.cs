using System.Text;

namespace AdventOfCode
{
    public class DayThree
    {
        private int answer = 0;

        public DayThree(string dataFile)
        {
            answer = 0;
            var input = File.ReadAllLines(dataFile);

            foreach (var line in input)
            {
                var bytes1 = Encoding.ASCII.GetBytes(line.Substring(0, line.Length / 2));
                var bytes2 = Encoding.ASCII.GetBytes(line.Substring(line.Length / 2));
                var error = bytes1.Where(x => bytes2.Contains(x)).Distinct().Single();
                answer += GetPriority(error);
            }
        }

        public int Answer => answer;

        private int GetPriority(int c)
        {
            var result = c >= 97 ? c - 96 : c - 38;
            return result;
        }
    }
}
