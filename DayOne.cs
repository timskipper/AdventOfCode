public class DayOne
{
    private readonly List<Elf> data;

    public DayOne(string dataFile)
    {
        this.data = new List<Elf>();

        var input = File.ReadAllLines(dataFile);

        var elfId = 1;
        foreach (var line in input)
        {
            if (line == string.Empty)
            {
                elfId++;
                continue;
            }

            data.Add(new Elf(elfId, int.Parse(line)));
        }
    }

    public int MostCalories => data
        .GroupBy(
            elf => elf.Id,
            elf => elf.Calories,
            (elf, elves) => new
            {
                Id = elf,
                Calories = elves.Sum()
            }
        )
        .OrderByDescending(x => x.Calories)
        .First()
        .Calories;
}

public class Elf
{
    public Elf(int id, int calories)
    {
        Id = id;
        Calories = calories;
    }

    public int Id { get; set; }
    public int Calories { get; set; }
}