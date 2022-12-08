using System.Transactions;

namespace AdventOfCode
{
    public class DayEight
    {
        private readonly List<List<int>> forest;
        private readonly List<List<bool>> visibility;
        private readonly List<List<int>> score;

        public DayEight(string dataFile)
        {
            var input = File.ReadAllLines(dataFile);

            forest = new List<List<int>>();
            visibility = new List<List<bool>>();
            score = new List<List<int>>();

            foreach (var line in input)
            {
                var row = new List<int>();
                var vrow = new List<bool>();
                var srow = new List<int>();
                foreach (var tree in line)
                {
                    row.Add(int.Parse(tree.ToString()));
                    vrow.Add(true);
                    srow.Add(0);
                }

                forest.Add(row);
                visibility.Add(vrow);
                score.Add(srow);
            }

            CheckVisibility();
            CheckScore();
        }

        public int Part1Answer => visibility.Sum(row => row.Count(col => col));
        public int Part2Answer => score.Aggregate(0, (current, row) => row.Prepend(current).Max());

        private void CheckVisibility()
        {
            for (var y = 1; y < forest.Count - 1; y++)
            {
                for (var x = 1; x < forest[y].Count - 1; x++)
                {
                    var height = forest[y][x];

                    var visibleFromLeft = true;
                    for (var i = 0; i < x; i++)
                    {
                        if (forest[y][i] >= height)
                        {
                            visibleFromLeft = false;
                            break;
                        }
                    }

                    var visibleFromRight = true;
                    for (var i = x + 1; i < forest[y].Count; i++)
                    {
                        if (forest[y][i] >= height)
                        {
                            visibleFromRight = false;
                            break;
                        }
                    }

                    var visibleFromTop = true;
                    for (var i = 0; i < y; i++)
                    {
                        if (forest[i][x] >= height)
                        {
                            visibleFromTop = false;
                            break;
                        }
                    }

                    var visibleFromBottom = true;
                    for (var i = y + 1; i < forest[x].Count; i++)
                    {
                        if (forest[i][x] >= height)
                        {
                            visibleFromBottom = false;
                            break;
                        }
                    }

                    visibility[y][x] = visibleFromLeft || visibleFromRight || visibleFromTop || visibleFromBottom;
                }
            }
        }

        private void CheckScore()
        {
            for (var y = 1; y < forest.Count - 1; y++)
            {
                for (var x = 1; x < forest[y].Count - 1; x++)
                {
                    var height = forest[y][x];

                    var scoreToLeft = 0;
                    for (var i = x - 1; i >= 0; i--)
                    {
                        if (forest[y][i] < height)
                        {
                            scoreToLeft++;
                        }
                        if (forest[y][i] >= height)
                        {
                            scoreToLeft++;
                            break;
                        }
                    }

                    var scoreToRight = 0;
                    for (var i = x + 1; i < forest[y].Count; i++)
                    {
                        if (forest[y][i] < height)
                        {
                            scoreToRight++;
                        }
                        if (forest[y][i] >= height)
                        {
                            scoreToRight++;
                            break;
                        }
                    }

                    var scoreToTop = 0;
                    for (var i = y - 1; i >= 0; i--)
                    {
                        if (forest[i][x] < height)
                        {
                            scoreToTop++;
                        }
                        if (forest[i][x] >= height)
                        {
                            scoreToTop++;
                            break;
                        }
                    }

                    var scoreToBottom = 0;
                    for (var i = y + 1; i < forest[x].Count; i++)
                    {
                        if (forest[i][x] < height)
                        {
                            scoreToBottom++;
                        }
                        if (forest[i][x] >= height)
                        {
                            scoreToBottom++;
                            break;
                        }
                    }

                    score[y][x] = scoreToLeft * scoreToRight * scoreToTop * scoreToBottom;
                }
            }
        }
    }
}
