namespace AdventOfCode
{
    public class DayTwo
    {
        private int totalScore1;
        private int totalScore2;

        public DayTwo(string dataFile)
        {
            totalScore1 = 0;
            totalScore2 = 0;
            var input = File.ReadAllLines(dataFile);

            foreach (var line in input)
            {
                var game = line.Split(' ');
                totalScore1 += CalculateScore(game[0], game[1]);
                totalScore2 += CalculateResponse(game[0], game[1]);
            }
        }

        public int Part1Score => totalScore1;
        public int Part2Score => totalScore2;

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

        public int CalculateResponse(string play, string result)
        {
            switch (play)
            {
                case "A": // Rock
                    switch (result)
                    {
                        case "X": // Lose
                            return 0 + 3;
                        case "Y": // Draw
                            return 3 + 1;
                        case "Z": // Win
                            return 6 + 2;
                    }
                    break;
                case "B": // Paper
                    switch (result)
                    {
                        case "X": // Lose
                            return 0 + 1;
                        case "Y": // Draw
                            return 3 + 2;
                        case "Z": // Win
                            return 6 + 3;
                    }
                    break;
                case "C": // Scissors
                    switch (result)
                    {
                        case "X": // Lose
                            return 0 + 2;
                        case "Y": // Draw
                            return 3 + 3;
                        case "Z": // Win
                            return 6 + 1;
                    }
                    break;
            }
            return 0;
        }
    }
}
