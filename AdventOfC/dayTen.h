#pragma once

#include <iostream>
#include <map>
#include <string>
#include <vector>

#include "Utils.h"

class DayTen
{
	std::map<int, int> signals;

public:
	size_t p1_answer;

	explicit DayTen(const std::string& data_file)
	{
		auto cycle = 0;
		auto x = 1;

		const std::vector<std::string> lines = read_all_lines(data_file);

		for (auto& line : lines)
		{
			auto commands = split(line, ' ');
			auto instruction = commands.at(0);

			int signal_strength;
			if (instruction == "noop")
			{
				cycle++;
				signal_strength = cycle * x;
				record_signal_strength(cycle, signal_strength);
				draw_pixel(cycle, x);
			}

			if (instruction == "addx")
			{
				const int value = std::stoi(commands.at(1));

				for (auto c = 0; c < 2; c++)
				{
					cycle++;
					signal_strength = cycle * x;
					record_signal_strength(cycle, signal_strength);
					draw_pixel(cycle, x);
				}
				x += value;
			}
		}

		p1_answer = signals[20] + signals[60] + signals[100] + signals[140] + signals[180] + signals[220];
	}

private:
	void record_signal_strength(const int cycle, const int ss)
	{
		if (cycle % 20 == 0)
		{
			signals.insert(std::pair<int, int>(cycle, ss));
		}
	}

	void draw_pixel(const int cycle, const int x) const
	{
		const auto x_pos = (cycle % 40) - 1;
		if (x_pos == x - 1 || x_pos == x || x_pos == x + 1)
		{
			std::cout << "#";
		}
		else
		{
			std::cout << " ";
		}

		if (cycle % 40 == 0)
		{
			std::cout << std::endl;
		}
	}
};