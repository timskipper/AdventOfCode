using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace AdventOfCode;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class Benchmarks
{
    //[Benchmark]
    //public void Run_DayOne()
    //{
    //    var dayEight = new DayOne(@"C:\Users\Tim\source\repos\AdventOfCode\day1input.txt");
    //}

    //[Benchmark]
    //public void Run_DayTwo()
    //{
    //    var dayEight = new DayTwo(@"C:\Users\Tim\source\repos\AdventOfCode\day2input.txt");
    //}

    //[Benchmark]
    //public void Run_DayThree()
    //{
    //    var x = new DayThree(@"C:\Users\Tim\source\repos\AdventOfCode\day3input.txt");
    //}

    //[Benchmark]
    //public void Run_DayFour()
    //{
    //    var x = new DayFour(@"C:\Users\Tim\source\repos\AdventOfCode\day4input.txt");
    //}

    //[Benchmark]
    //public void Run_DayFive()
    //{
    //    var x = new DayFive(@"C:\Users\Tim\source\repos\AdventOfCode\day5input.txt");
    //}

    //[Benchmark]
    //public void Run_DaySix()
    //{
    //    var x = new DaySix(@"C:\Users\Tim\source\repos\AdventOfCode\day6input.txt");
    //}

    //[Benchmark]
    //public void Run_DaySeven()
    //{
    //    var x = new DaySeven(@"C:\Users\Tim\source\repos\AdventOfCode\day7input.txt");
    //}

    //[Benchmark]
    //public void Run_DayEight()
    //{
    //    var x = new DayEight(@"C:\Users\Tim\source\repos\AdventOfCode\day8input.txt");
    //}

    //[Benchmark]
    //public void Run_DayNine()
    //{
    //    var x = new DayNine(@"C:\Users\Tim\source\repos\AdventOfCode\day9input.txt");
    //}

    //[Benchmark]
    //public void Run_DayTen()
    //{
    //    var x = new DayTen(@"C:\Users\Tim\source\repos\AdventOfCode\day10input.txt");
    //}

    //[Benchmark]
    //public void Run_DayEleven()
    //{
    //    var x = new DayEleven(@"C:\Users\Tim\source\repos\AdventOfCode\day11input.txt");
    //}
}