﻿using AdventOfCode;

var dayOne = new DayOne("../../../day1input.txt");
Console.WriteLine($"Day 1 Part 1: {dayOne.MostCalories}");
Console.WriteLine($"Day 1 Part 2: {dayOne.TopThreeCalories}");

var dayTwo = new DayTwo("../../../day2input.txt");
Console.WriteLine($"Day 2 Part 1: {dayTwo.Part1Score}");
Console.WriteLine($"Day 2 Part 2: {dayTwo.Part2Score}");

var dayThree = new DayThree("../../../day3input.txt");
Console.WriteLine($"Day 3 Part 1: {dayThree.Answer}");