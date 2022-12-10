#include <iostream>
#include "DayEight.h"
#include "Timer.h"

int main()
{
	timer timer;

	DayEight dayEight(R"(C:\Users\Tim\source\repos\AdventOfCode\day8input.txt)");
	timer.stop();
	std::cout << "Day 8 Part 1: " << dayEight.p1_answer << std::endl;
	std::cout << "Day 8 Part 2: " << dayEight.p2_answer << std::endl;
}
