namespace AdventOfCode
{
    public class DayNine
    {
        private readonly HashSet<ValueTuple<int, int>> tailVisits;
        private readonly HashSet<ValueTuple<int, int>> lastTailVisits;

        private readonly ValueTuple<int, int>[] Tails;

        public DayNine(string dataFile)
        {
            tailVisits = new HashSet<ValueTuple<int, int>> { new ValueTuple<int, int>(0, 0) };
            lastTailVisits = new HashSet<ValueTuple<int, int>> { new ValueTuple<int, int>(0, 0) };
            Tails = new (int, int)[9];

            var input = File.ReadAllLines(dataFile);

            var head = new ValueTuple<int, int>(0, 0);

            foreach (var line in input)
            {
                var command = line.Split(' ');
                var direction = command[0];
                var distance = int.Parse(command[1]);

                if (direction == "L" || direction == "D")
                {
                    distance = -distance;
                }
                Move(direction, distance, ref head, 0);
            }
        }

        public int Part1Answer => tailVisits.Distinct().Count();
        public int Part2Answer => lastTailVisits.Distinct().Count();

        private void Move(string direction, int distance, ref ValueTuple<int, int> headPos, int tail)
        {
            for (var i = 0; i < Math.Abs(distance); i++)
            {
                if (direction is "L" or "R")
                {
                    if (distance < 0) headPos.Item1--;
                    if (distance > 0) headPos.Item1++;
                }
                else if (direction is "U" or "D")
                {
                    if (distance > 0) headPos.Item2++;
                    if (distance < 0) headPos.Item2--;
                }

                var newTail = MoveTail(ref headPos, tail);
                tailVisits.Add(newTail);

                for (var j = 0; j < 8; j++)
                {
                    MoveTail(ref Tails[j], j + 1);
                }
                lastTailVisits.Add(Tails[8]);
            }
        }

        private ValueTuple<int, int> MoveTail(ref ValueTuple<int, int> headPos, int tail)
        {
            var xDelta = headPos.Item1 - Tails[tail].Item1;
            var yDelta = headPos.Item2 - Tails[tail].Item2;

            if (IsTouching(headPos, Tails[tail]))
            {
                return Tails[tail];
            }

            if (xDelta != 0 && yDelta == 0)
            {
                // Moved along x-axis
                Tails[tail].Item1 += xDelta > 0 ? 1 : -1;
            }
            if (xDelta == 0 && yDelta != 0)
            {
                // Moved along y-axis
                Tails[tail].Item2 += yDelta > 0 ? 1 : -1;
            }
            if (xDelta != 0 & yDelta != 0)
            {
                // Moved diagonally
                Tails[tail].Item1 += xDelta > 0 ? 1 : -1;
                Tails[tail].Item2 += yDelta > 0 ? 1 : -1;
            }

            return Tails[tail];
        }

        private bool IsTouching(ValueTuple<int, int> pos1, ValueTuple<int, int> pos2)
        {
            var adjacent = new List<ValueTuple<int, int>>
            {
                new(pos1.Item1 - 1, pos1.Item2 + 1),
                new(pos1.Item1, pos1.Item2 + 1),
                new(pos1.Item1 + 1, pos1.Item2 + 1),
                new(pos1.Item1 - 1, pos1.Item2 ),
                new(pos1.Item1, pos1.Item2 ),
                new(pos1.Item1 + 1, pos1.Item2 ),
                new(pos1.Item1 - 1, pos1.Item2 - 1),
                new(pos1.Item1, pos1.Item2 - 1),
                new(pos1.Item1 + 1, pos1.Item2 - 1)
            };

            return adjacent.Contains(pos2);
        }
    }
}
