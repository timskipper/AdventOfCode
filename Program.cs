using AdventOfCode;

var dayOne = new DayOne("../../../day1input.txt");
Console.WriteLine($"Elf with {dayOne.MostCalories} calories has the most.");
Console.WriteLine($"Total of top 3 calories is {dayOne.TopThreeCalories}");

var dayTwo = new DayTwo("../../../day2input.txt");
Console.WriteLine($"Total score is {dayTwo.Score}");