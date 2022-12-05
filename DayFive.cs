namespace AdventOfCode
{
    public class DayFive
    {
        private string finalTopStack;
        private string finalTopStackV2;
        private Dictionary<int, Stack<string>> stacks;

        public DayFive(string dataFile)
        {
            var input = File.ReadAllLines(dataFile);

            // Part 1
            InitialiseStacks();

            foreach (var line in input)
            {
                if (!line.StartsWith("move"))
                {
                    continue;
                }
                (var quantity, var from, var to) = ParseLine(line);
                MoveCrates(quantity, from, to);
            }

            finalTopStack = GetTopRow(stacks);

            // Part 2
            InitialiseStacks();

            foreach (var line in input)
            {
                if (!line.StartsWith("move"))
                {
                    continue;
                }
                (var quantity, var from, var to) = ParseLine(line);
                MoveCratesV2(quantity, from, to);
            }

            finalTopStackV2 = GetTopRow(stacks);
        }

        public string Part1Answer => finalTopStack;
        public string Part2Answer => finalTopStackV2;

        private void InitialiseStacks()
        {
            stacks = new Dictionary<int, Stack<string>>
            {
                { 1, new Stack<string>(new string[] {"H","B","V","W","N","M","L","P"})},
                { 2, new Stack<string>(new string[] {"M","Q","H"})},
                { 3, new Stack<string>(new string[] {"N","D","B","G","F","Q","M","L"})},
                { 4, new Stack<string>(new string[] {"Z","T","F","Q","M","W","G"})},
                { 5, new Stack<string>(new string[] {"M","T","H","P"})},
                { 6, new Stack<string>(new string[] {"C","B","M","J","D","H","G","T"})},
                { 7, new Stack<string>(new string[] {"M","N","B","F","V","R"})},
                { 8, new Stack<string>(new string[] {"P","L","H","M","R","G","S"})},
                { 9, new Stack<string>(new string[] {"P","D","B","C","N"})}
            };
        }

        private (int quantity, int from, int to) ParseLine(string line)
        {
            var l = line.Split(" ");
            return (quantity: int.Parse(l[1]), from: int.Parse(l[3]), to: int.Parse(l[5]));
        }

        private void MoveCrates(int quantity, int from, int to)
        {
            for (int i = 0; i < quantity; i++)
            {
                var crate = stacks[from].Pop();
                stacks[to].Push(crate);
            }
        }

        private void MoveCratesV2(int quantity, int from, int to)
        {
            var temp = new List<string>();

            for (int i = 0; i < quantity; i++)
            {
                var crate = stacks[from].Pop();
                temp.Add(crate);
            }

            for (int i = quantity - 1; i >= 0; i--)
            {
                stacks[to].Push(temp[i]);
            }
        }

        private string GetTopRow(Dictionary<int, Stack<string>> stacks)
        {
            var row = string.Empty;
            for (int i = 1; i <= 9; i++)
            {
                row += stacks[i].Peek();
            }
            return row;
        }
    }
}
