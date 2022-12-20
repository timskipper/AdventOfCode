using System.Text.RegularExpressions;

namespace AdventOfCode;

enum RobotTypes
{
    Ore,
    Clay,
    Obsidian,
    Geode
}

public class DayNineteen
{
    public DayNineteen(string dataFile)
    {
        var r = new Regex(@"(\b\d+)");

        var blueprints = new Dictionary<int, Blueprint>();

        File.ReadAllLines(dataFile)
            .ToList()
            .ForEach(line =>
            {
                var vals = r.Matches(line).Cast<Match>().ToArray();

                var i = int.Parse(vals[0].Value);
                var oreRobotOreCost = int.Parse(vals[1].Value);
                var clayRobotOreCost = int.Parse(vals[2].Value);
                var obsidianRobotOreCost = int.Parse(vals[3].Value);
                var obsidianRobotClayCost = int.Parse(vals[4].Value);
                var geodeRobotOreCost = int.Parse(vals[5].Value);
                var geodeRobotObsidianCost = int.Parse(vals[6].Value);

                var blueprint = new Blueprint(i,
                    new Robot(RobotTypes.Ore, oreRobotOreCost, 0, 0),
                    new Robot(RobotTypes.Clay, clayRobotOreCost, 0, 0),
                    new Robot(RobotTypes.Obsidian, obsidianRobotOreCost, obsidianRobotClayCost, 0),
                    new Robot(RobotTypes.Geode, geodeRobotOreCost, 0, geodeRobotObsidianCost));

                blueprints.Add(i, blueprint);
            });

        foreach (var blueprint in blueprints)
        {
            var ql = blueprint.Value.QualityLevel(24);
        }
    }

    public int Part1Answer { get; set; }

    public int Part2Answer { get; set; }

    internal class Blueprint
    {
        private int oreRobotCount;
        private int clayRobotCount;
        private int obsidianRobotCount;
        private int geodeRobotCount;

        private int OreResources;
        private int ClayResources;
        private int ObsidianResources;
        private int GeodeResources;

        public Blueprint(int id, Robot ore, Robot clay, Robot obsidian, Robot geode)
        {
            oreRobotCount = 1;
            clayRobotCount = 0;
            obsidianRobotCount = 0;
            geodeRobotCount = 0;

            OreResources = 0;
            ClayResources = 0;
            ObsidianResources = 0;

            ID = id;
            OreRobot = ore;
            ClayRobot = clay;
            ObsidianRobot = obsidian;
            GeodeRobot = geode;
        }

        public int ID { get; set; }
        public Robot OreRobot { get; set; }
        public Robot ClayRobot { get; set; }
        public Robot ObsidianRobot { get; set; }
        public Robot GeodeRobot { get; set; }

        public int QualityLevel(int budgetMins)
        {
            var oreRobots = new List<Robot> { OreRobot };
            var clayRobots = new List<Robot>();
            var obsidianRobots = new List<Robot>();

            var geodes = 0;

            for (var m = budgetMins; m > 0; m--)
            {
                foreach (var oreRobot in oreRobots)
                {

                }
            }

            return 0;
        }
    }

    internal class Robot
    {
        public Robot(RobotTypes type, int ore, int clay, int obsidian)
        {
            Type = type;
            OreCost = ore;
            ClayCost = clay;
            ObsidianCost = obsidian;
        }

        public RobotTypes Type { get; set; }
        public int OreCost { get; set; }
        public int ClayCost { get; set; }
        public int ObsidianCost { get; set; }

        public int ConstructionTime => OreCost + ClayCost + ObsidianCost + 1;

        public (bool success, int quantityMade, int timeTaken, int oreUsed, int clayUsed, int obsidianUsed) Construct(int quantity, int ore, int clay, int obsidian)
        {
            var success = true;
            var quantityMade = 0;
            var timeTaken = 0;
            var oreUsed = 0;
            var clayUsed = 0;
            var obsidianUsed = 0;

            while (quantityMade < quantity)
            {
                oreUsed += OreCost;
                clayUsed += ClayCost;
                obsidianUsed += ObsidianCost;

                if (oreUsed <= ore && clayUsed <= clay && obsidianUsed <= obsidian)
                {
                    timeTaken++;
                    quantityMade++;
                }
                else
                {
                    success = false;
                    break;
                }
            }

            return (success, quantityMade, timeTaken, oreUsed, clayUsed, obsidianUsed);
        }
    }
}
