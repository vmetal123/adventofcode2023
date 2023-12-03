using System.Text;

namespace adventofcode2023;

public class DayOne
{
    private readonly Dictionary<string, int> digitsInLetters = new Dictionary<string, int>
    {
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9}
    };

    private readonly int[] ranges = new int[] { 3, 4, 5 };

    public void SolvePartOne()
    {
        var lines = File.ReadAllLines("day1part1.txt");

        int sum = 0;

        foreach (string line in lines)
        {
            sum += ReadNumberPartOne(line);
        }

        Console.WriteLine($"Sum: {sum}");
    }

    public void SolvePartTwo()
    {
        var lines = File.ReadAllLines("day1part1.txt");
        int sum = 0;

        foreach (string line in lines)
        {
            sum += ReadNumberPartTwo(line.Trim());
        }

        Console.WriteLine($"Sum: {sum}");
    }

    private int ReadNumberPartOne(string line)
    {
        var numbers = line.Where(Char.IsDigit).Select(x => x).ToArray();

        var firstNumber = numbers[0];
        var lastNumber = numbers[^1];

        return Convert.ToInt32($"{firstNumber}{lastNumber}");
    }

    private int ReadNumberPartTwo(string line)
    {
        var stringBuilder = new StringBuilder();
        for (int i = 0; i < line.Length; i++)
        {
            if (char.IsDigit(line[i]))
            {
                stringBuilder.Append(line[i]);
                continue;
            }

            foreach (var range in ranges)
            {
                int endIndex = i + range;
                int finalIndex = line.Length;
                if(endIndex > finalIndex)
                {
                    continue;
                }

                string digitLetter = line.Substring(i, range);

                string? digitKey = digitsInLetters.Keys.FirstOrDefault(x => x == digitLetter);


                if(string.IsNullOrEmpty(digitKey)) {
                    continue;
                }

                stringBuilder.Append(digitsInLetters[digitKey]);
            }
        }

        char[] numbers = stringBuilder.ToString().ToArray();
        var firstNumber = numbers[0];
        var lastNumber = numbers[^1];

        return Convert.ToInt32($"{firstNumber}{lastNumber}");
    }
}
