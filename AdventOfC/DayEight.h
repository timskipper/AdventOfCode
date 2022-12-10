#pragma once

#include <algorithm>
#include <string>
#include <vector>
#include <fstream>
#include <ostream>

class DayEight
{
	std::vector<std::vector<int>> forest;
	std::vector<std::vector<bool>> visibility;
	std::vector<std::vector<int>> score;

public:
	explicit DayEight(const std::string& data_file)
	{
		const std::vector<std::string> lines = read_all_lines(data_file);

		for (const auto& line : lines)
		{
			std::vector<int> row;
			std::vector<bool> vrow;
			std::vector<int> srow;

			for (auto tree : line)
			{
				row.push_back(std::atoi(&tree));
				vrow.push_back(true);
				srow.push_back(0);
			}

			forest.push_back(row);
			visibility.push_back(vrow);
			score.push_back(srow);
		}

		check_visibility();
		check_score();
	}

	int part1_answer()
	{
		auto result = 0;
		for (auto row = visibility.begin(); row != visibility.end(); ++row)
		{
			for (auto col = row->begin(); col != row->end(); ++col)
			{
				if (*col)
				{
					result++;
				}
			}
		}
		return result;
	}

	int part2_answer()
	{
		std::vector<int> row_maximums;

		for (auto row = score.begin(); row != score.end(); ++row)
		{
			row_maximums.push_back(*std::max_element(row->begin(), row->end()));
		}
		return *std::max_element(row_maximums.begin(), row_maximums.end());
	}

private:
	std::vector<std::string> read_all_lines(const std::string& filename)
	{
		std::vector<std::string> input;

		std::ifstream file;
		file.open(filename);

		while (!file.eof())
		{
			std::string line;
			getline(file, line);
			input.push_back(line);
		}

		file.close();

		return input;
	}

	void check_visibility()
	{
		for (int y = 0; y < forest.size() - 1; y++)
		{
			for (int x = 1; x < forest[y].size() - 1; x++)
			{
				int height = forest[y][x];

				bool visible_from_left = true;
				for (int i = 0; i < x; i++)
				{
					if (forest[y][i] >= height)
					{
						visible_from_left = false;
						break;
					}
				}

				bool visible_from_right = true;
				for (int i = x + 1; i < forest[y].size(); i++)
				{
					if (forest[y][i] >= height)
					{
						visible_from_right = false;
						break;
					}
				}

				bool visible_from_top = true;
				for (int i = 0; i < y; i++)
				{
					if (forest[i][x] >= height)
					{
						visible_from_top = false;
						break;
					}
				}

				bool visible_from_bottom = true;

				for (int i = y + 1; i < forest[x].size(); i++)
				{
					if (forest[i][x] >= height)
					{
						visible_from_bottom = false;
						break;
					}
				}

				visibility[y][x] = visible_from_left || visible_from_right || visible_from_top || visible_from_bottom;
			}
		}
	}

	void check_score()
	{
		for (auto y = 1; y < forest.size() - 1; y++)
		{
			for (auto x = 1; x < forest[y].size() - 1; x++)
			{
				auto height = forest[y][x];

				auto scoreToLeft = 0;
				for (auto i = x - 1; i >= 0; i--)
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

				auto scoreToRight = 0;
				for (auto i = x + 1; i < forest[y].size(); i++)
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

				auto scoreToTop = 0;
				for (auto i = y - 1; i >= 0; i--)
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

				auto scoreToBottom = 0;
				for (auto i = y + 1; i < forest[x].size(); i++)
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
};
