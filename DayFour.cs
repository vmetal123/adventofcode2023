namespace adventofcode2023;

public class DayFour
{
    public void SolvePartOne()
    {
        var lines = File.ReadAllLines("day4.txt");
        int sum = 0;
        int baseNumber = 2;
        var index = 1;

        foreach (var line in lines)
        {
            var game = line.Split("|", StringSplitOptions.TrimEntries);
            var winnerSide = game[0].Split(":", StringSplitOptions.TrimEntries);
            var winners = winnerSide[1].Split(" ", StringSplitOptions.TrimEntries);
            var numbers = game[1].Split(" ", StringSplitOptions.TrimEntries).Where(x => x != "").Distinct().ToArray();

            var totalWinners = numbers.Count(x => winners.Any(y => y == x));

            int totalPoints = Convert.ToInt32(Math.Pow(baseNumber, (totalWinners - 1)));

            sum += totalPoints;
            index++;
        }

        Console.WriteLine($"Sum: {sum}");
    }

    public void SolvePartTwo()
    {

        var lines = File.ReadAllLines("day4.txt");

        var copies = new Dictionary<int, int>();

        int totalLines = lines.Length;

        foreach (var line in lines)
        {
            var game = line.Split("|", StringSplitOptions.TrimEntries);
            var winnerSide = game[0].Split(":", StringSplitOptions.TrimEntries);
            var winnerSideNumber = winnerSide[0].Split(" ", StringSplitOptions.TrimEntries).Where(x=> x != "").ToArray()[1];
            var winners = winnerSide[1].Split(" ", StringSplitOptions.TrimEntries);
            var numbers = game[1].Split(" ", StringSplitOptions.TrimEntries).Where(x => x != "").Distinct().ToArray();

            var totalWinners = numbers.Count(x => winners.Any(y => y == x));

            int cardNumber = Convert.ToInt32(winnerSideNumber);

            if (!copies.Keys.Contains(cardNumber))
            {
                copies.Add(cardNumber, 1);
            }
            else
            {
                copies[cardNumber] += 1;
            }

            int aditionalCard = 1;
            if(copies[cardNumber] > 1) {
                aditionalCard = copies[cardNumber];
            }

            for (int i = 0; i < totalWinners; i++)
            {
                cardNumber += 1;
                if (cardNumber > totalLines)
                {
                    break;
                }
                if (copies.Keys.Contains(cardNumber))
                {
                    copies[cardNumber] += aditionalCard;
                }
                else
                {
                    copies.Add(cardNumber, aditionalCard);
                }
            }
        }

        int sum = copies.Values.Sum();

        Console.WriteLine($"Sum: {sum}");
    }
}
