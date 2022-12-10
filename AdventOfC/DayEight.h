#pragma once

#include <algorithm>
#include <string>
#include <vector>
#include <ranges>
#include <ostream>

#include "Utils.h"

class DayEight
{
	std::vector<std::vector<int>> forest;
	std::vector<std::vector<bool>> visibility;
	std::vector<std::vector<int>> score;

public:
	int p1_answer;
	int p2_answer;

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

		p1_answer = part1_answer();
		p2_answer = part2_answer();
	}

private:
	int part1_answer()
	{
		const auto visible = std::views::join(visibility);
		const auto count = std::ranges::count(visible.begin(), visible.end(), true);
		return count;
	}

	int part2_answer()
	{
		const auto max_value = std::ranges::max(score | std::views::join);
		return max_value;
	}


	void check_visibility()
	{
		for (int y = 0; y < forest.size() - 1; y++)
		{
			for (int x = 1; x < forest[y].size() - 1; x++)
			{
				const int height = forest[y][x];

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
				const auto height = forest[y][x];

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
