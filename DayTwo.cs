namespace AdventOfCode
{
    public class DayTwo
    {
        private int totalScore;

        public DayTwo(string dataFile)
        {
            totalScore = 0;
            var input = File.ReadAllLines(dataFile);

            foreach (var line in input)
            {
                var game = line.Split(' ');
                totalScore += CalculateScore(game[0], game[1]);
            }
        }

        public int Score => totalScore;

        public int CalculateScore(string play, string response)
        {
            switch (play)
            {
                case "A": // Rock
                    switch (response)
                    {
                        case "X": // Rock
                            return 3 + 1;
                        case "Y": // Paper
                            return 6 + 2;
                        case "Z": // Scissors
                            return 0 + 3;
                    }
                    break;
                case "B": // Paper
                    switch (response)
                    {
                        case "X": // Rock
                            return 0 + 1;
                        case "Y": // Paper
                            return 3 + 2;
                        case "Z": // Scissors
                            return 6 + 3;
                    }
                    break;
                case "C": // Scissors
                    switch (response)
                    {
                        case "X": // Rock
                            return 6 + 1;
                        case "Y": // Paper
                            return 0 + 2;
                        case "Z": // Scissors
                            return 3 + 3;
                    }
                    break;
            }
            return 0;
        }
    }
}
