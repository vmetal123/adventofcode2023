namespace adventofcode2023;

public class DayFour
{
    public void SolvePartOne()
    {
        var lines = File.ReadAllLines("day4.txt");
        int sum = 0;
        int baseNumber = 2;
        var index =1;

        foreach (var line in lines)
        {
            var game = line.Split("|", StringSplitOptions.TrimEntries);
            var winnerSide = game[0].Split(":", StringSplitOptions.TrimEntries);
            var winners = winnerSide[1].Split(" ", StringSplitOptions.TrimEntries);
            var numbers = game[1].Split(" ", StringSplitOptions.TrimEntries).Where(x => x != "").Distinct().ToArray();

            var totalWinners = numbers.Count(x => winners.Any(y => y == x));

            Console.WriteLine($"Total winners at index {index}: {totalWinners}");

            int totalPoints = Convert.ToInt32(Math.Pow(baseNumber, (totalWinners - 1)));

            Console.WriteLine($"Total points: {totalPoints}");

            sum += totalPoints;
            index++;
        }

        Console.WriteLine($"Sum: {sum}");
    }
}
