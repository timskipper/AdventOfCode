#include <iostream>

#include "DayEight.h"

int main()
{
	DayEight dayEight(R"(C:\Users\Tim\source\repos\AdventOfCode\day8input.txt)");
	std::cout << "Day 8 Part 1: " << dayEight.part1_answer() << std::endl;
	std::cout << "Day 8 Part 2: " << dayEight.part2_answer() << std::endl;
}
