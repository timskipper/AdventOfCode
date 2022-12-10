#pragma once

#include <algorithm>
#include <string>
#include <vector>

#include "Utils.h"

class DayNine
{
	std::vector<std::tuple<int, int>> tail_visits;
	std::vector<std::tuple<int, int>> last_tail_visits;
	std::tuple<int, int> tails[9];

public:
	size_t p1_answer;
	size_t p2_answer;

	explicit DayNine(const std::string& data_file)
	{
		tail_visits.push_back(std::make_tuple<int, int>(0, 0));
		last_tail_visits.push_back(std::make_tuple<int, int>(0, 0));

		std::tuple<int, int> head;

		const std::vector<std::string> lines = read_all_lines(data_file);

		for (auto& line : lines)
		{
			auto commands = split(line, ' ');
			auto direction = commands.at(0);
			int distance = std::stoi(commands.at(1));

			if (direction == "L" || direction == "D")
			{
				distance = -distance;
			}
			move(direction, distance, head, 0);
		}

		std::ranges::sort(tail_visits);
		const auto unique_tail_visits = std::ranges::unique(tail_visits).begin();
		tail_visits.resize(std::distance(tail_visits.begin(), unique_tail_visits));
		p1_answer = tail_visits.size();

		std::ranges::sort(last_tail_visits);
		const auto unique_last_tail_visits = std::ranges::unique(last_tail_visits).begin();
		last_tail_visits.resize(std::distance(last_tail_visits.begin(), unique_last_tail_visits));
		p2_answer = last_tail_visits.size();
	}

private:
	void move(const std::string& direction, const int distance, std::tuple<int, int>& head_pos, int tail)
	{
		for (auto i = 0; i < std::abs(distance); i++)
		{
			if (direction == "L" || direction == "R")
			{
				if (distance < 0) std::get<0>(head_pos)--;
				if (distance > 0) std::get<0>(head_pos)++;
			}
			else if (direction == "U" || direction == "D")
			{
				if (distance > 0) std::get<1>(head_pos)++;
				if (distance < 0) std::get<1>(head_pos)--;
			}

			auto new_tail = move_tail(head_pos, tail);
			tail_visits.push_back(new_tail);

			for (auto j = 0; j < 8; j++)
			{
				move_tail(tails[j], j + 1);
			}
			last_tail_visits.push_back(tails[8]);
		}
	}

	std::tuple<int, int> move_tail(const std::tuple<int, int>& head_pos, const int tail)
	{
		const auto x_delta = std::get<0>(head_pos) - std::get<0>(tails[tail]);
		const auto y_delta = std::get<1>(head_pos) - std::get<1>(tails[tail]);

		if (is_touching(head_pos, tails[tail]))
		{
			return tails[tail];
		}

		if (x_delta != 0 && y_delta == 0)
		{
			// Moved along x-axis
			std::get<0>(tails[tail]) += x_delta > 0 ? 1 : -1;
		}
		if (x_delta == 0 && y_delta != 0)
		{
			// Moved along y-axis
			std::get<1>(tails[tail]) += y_delta > 0 ? 1 : -1;
		}
		if (x_delta != 0 && y_delta != 0)
		{
			// Moved diagonally
			std::get<0>(tails[tail]) += x_delta > 0 ? 1 : -1;
			std::get<1>(tails[tail]) += y_delta > 0 ? 1 : -1;
		}

		return tails[tail];
	}

	bool is_touching(std::tuple<int, int> pos1, const std::tuple<int, int>& pos2) const
	{
		std::tuple<int, int> adjacent[9];
		adjacent[0] = std::make_tuple(std::get<0>(pos1) - 1, std::get<1>(pos1) + 1);
		adjacent[1] = std::make_tuple(std::get<0>(pos1), std::get<1>(pos1) + 1);
		adjacent[2] = std::make_tuple(std::get<0>(pos1) + 1, std::get<1>(pos1) + 1);
		adjacent[3] = std::make_tuple(std::get<0>(pos1) - 1, std::get<1>(pos1));
		adjacent[4] = std::make_tuple(std::get<0>(pos1), std::get<1>(pos1));
		adjacent[5] = std::make_tuple(std::get<0>(pos1) + 1, std::get<1>(pos1));
		adjacent[6] = std::make_tuple(std::get<0>(pos1) - 1, std::get<1>(pos1) - 1);
		adjacent[7] = std::make_tuple(std::get<0>(pos1), std::get<1>(pos1) - 1);
		adjacent[8] = std::make_tuple(std::get<0>(pos1) + 1, std::get<1>(pos1) - 1);

		for (auto& p1 : adjacent)
		{
			if (p1 == pos2)
			{
				return true;
			}
		}

		return false;
	}
};