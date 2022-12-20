namespace AdventOfCode;

public class DaySeventeen
{
    internal enum RockTypes
    {
        Line,
        Cross,
        L,
        Bar,
        Square
    }

    internal enum WhichWayTypes
    {
        Left = -1,
        Right = 1
    }

    internal class Rock
    {
        internal Rock(RockTypes type)
        {
            Type = type;
            Shape = new List<(long, long)>();
            switch (type)
            {
                case RockTypes.Line:
                    Shape = new List<(long, long)> { (0, 0), (1, 0), (2, 0), (3, 0) };
                    break;
                case RockTypes.Cross:
                    Shape = new List<(long, long)> { (1, 0), (0, -1), (1, -1), (2, -1), (1, -2) };
                    break;
                case RockTypes.L:
                    Shape = new List<(long, long)> { (2, 0), (2, -1), (0, -2), (1, -2), (2, -2) };
                    break;
                case RockTypes.Bar:
                    Shape = new List<(long, long)> { (0, 0), (0, -1), (0, -2), (0, -3) };
                    break;
                case RockTypes.Square:
                    Shape = new List<(long, long)> { (0, 0), (1, 0), (0, -1), (1, -1) };
                    break;
            }
        }

        public RockTypes Type { get; set; }

        public List<ValueTuple<long, long>> Shape { get; set; }

        public ValueTuple<long, long> Position { get; set; }

        public List<ValueTuple<long, long>> Coordinates => Shape.Select(v => (Position.Item1 + v.Item1, Position.Item2 + v.Item2)).ToList();

        public long MaxY => Position.Item2 + Shape.Max(x => x.Item2);

        public long Height => Shape.Max(x => x.Item2) - Shape.Min(x => x.Item2);
    }

    private readonly List<Rock> rocks;

    public DaySeventeen(string dataFile)
    {
        var jetData = File.ReadAllText(dataFile);

        var jetStream = new LinkedList<WhichWayTypes>();
        foreach (var j in jetData)
        {
            jetStream.AddLast(j == '<' ? WhichWayTypes.Left : WhichWayTypes.Right);
        }

        var rockSequence = new LinkedList<RockTypes>();
        rockSequence.AddLast(RockTypes.Line);
        rockSequence.AddLast(RockTypes.Cross);
        rockSequence.AddLast(RockTypes.L);
        rockSequence.AddLast(RockTypes.Bar);
        rockSequence.AddLast(RockTypes.Square);

        rocks = new List<Rock>();

        var whichWay = jetStream.First;
        var whichRock = rockSequence.First;
        var rockCount = 0L;

        var theRock = PlaceRock(whichRock!.Value, 3);

        while (true)
        {
            var beforeRock = theRock.Position;
            theRock = Push(theRock, whichWay!.Value);
            if (DetectCollision(theRock))
            {
                theRock.Position = beforeRock;
            }

            beforeRock = theRock.Position;
            theRock = Drop(theRock);
            if (DetectCollision(theRock))
            {
                theRock.Position = beforeRock;
                rocks.Add(theRock);
                rockCount++;
                if (rockCount == 2022)
                {
                    break;
                }
                if (rockCount % 50 == 0)
                {
                    rocks = rocks.TakeLast(50).ToList();
                }

                whichRock = whichRock.NextOrFirst();
                theRock = new Rock(whichRock.Value);
                theRock = PlaceRock(whichRock.Value, FindFloor() + theRock.Height);
            }
            whichWay = whichWay.NextOrFirst();
        }

        //PaintMap();
        Part1Answer = rocks.Max(x => x.Position.Item2) + 1;
    }

    public long Part1Answer { get; set; }

    public long Part2Answer { get; set; }

    private static Rock PlaceRock(RockTypes type, long y)
    {
        const long edgeDelta = 2;
        var rock = new Rock(type) { Position = (edgeDelta, y) };
        return rock;
    }

    private bool DetectCollision(Rock rock)
    {
        if (rock.Coordinates.Any(x => x.Item1 is < 0 or > 6))
        {
            return true;   // Hit the wall
        }
        if (rock.Coordinates.Any(x => x.Item2 < 0))
        {
            return true;   // Hit the floor
        }
        if (!rocks.Any())
        {
            return false;
        }

        return rocks.Any(placedRock => rock.Coordinates.Any(xy => placedRock.Coordinates.Contains(xy)));
    }

    private static Rock Push(Rock rock, WhichWayTypes whichWay)
    {
        var pos = rock.Position;
        pos.Item1 += whichWay == WhichWayTypes.Left ? -1 : 1;
        rock.Position = pos;
        return rock;
    }

    private static Rock Drop(Rock rock)
    {
        var pos = rock.Position;
        pos.Item2 -= 1;
        rock.Position = pos;
        return rock;
    }

    private long FindFloor()
    {
        const long floorDelta = 4;
        if (!rocks.Any())
        {
            return 0;
        }
        return rocks.Aggregate(0L, (current, rock) => Utils.Max(current, rock.MaxY)) + floorDelta;
    }

    private void PaintMap()
    {
        if (!rocks.Any()) return;
        Console.Clear();

        var rows = rocks.SelectMany(x => x.Coordinates).OrderByDescending(x => x.Item2).ThenBy(x => x.Item1).ToList();
        var maxY = rows.Max(x => x.Item2);
        for (var y = maxY; y >= 0; y--)
        {
            if (rows.All(x => x.Item2 != y))
            {
                continue;
            }
            for (var x = 0; x < 7; x++)
            {
                var isRock = rows.Any(r => r.Item1 == x && r.Item2 == y);
                Console.Write(isRock ? "#" : ' ');
            }
            Console.WriteLine();
        }
    }
}