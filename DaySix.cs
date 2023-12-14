using System.Text;

namespace adventofcode2023;

public class DaySix
{
    public void SolvePartOne()
    {
        var lines = File.ReadAllLines("day6.txt");

        int result = 1;
        foreach (var (time, distance) in GetBoatMoves(lines[0], lines[1]))
        {
            int total = 0;
            for (int i = 0; i < time; i++)
            {
                var rest = time - i;
                var count = rest * i;
                if (count > distance)
                {
                    total++;
                }
            }
            result *= total;
        }

        Console.WriteLine($"Result: {result}");
    }

    public void SolvePartTwo()
    {
        var lines = File.ReadAllLines("day6.txt");

        long time = LineToInt(lines[0]);
        long distance = LineToInt(lines[1]);
        long total = 0;
        for (long i = 0; i < time; i++)
        {
            var rest = time - i;
            var count = rest * i;
            if (count > distance)
            {
                total++;
            }
        }

        Console.WriteLine($"Result: {total}");
    }

    IEnumerable<BoatMove> GetBoatMoves(string timeLine, string distanceLine)
    {
        var times = LineToIntList(timeLine);
        var distance = LineToIntList(distanceLine);

        for (int i = 0; i < times.Count; i++)
        {
            yield return new BoatMove(times[i], distance[i]);
        }

    }

    private List<int> LineToIntList(string line)
    {
        var numbersLine = line.Split(':', StringSplitOptions.TrimEntries)[1];
        var numbers = new List<int>();
        var current = new StringBuilder();
        for (int i = 0; i < numbersLine.Length; i++)
        {
            if (char.IsDigit(numbersLine[i]))
            {
                current.Append(numbersLine[i]);
                if (numbersLine.Length - i == 1)
                {
                    numbers.Add(int.Parse(current.ToString()));
                }
                continue;
            }

            if (current.Length > 0)
            {
                numbers.Add(int.Parse(current.ToString()));
                current.Clear();
            }

        }

        return numbers;
    }

    private long LineToInt(string line)
    {
        var numbersLine = line.Split(':', StringSplitOptions.TrimEntries)[1];
        var numberAsString = numbersLine.Replace(" ", string.Empty);
        return long.Parse(numberAsString);
    }
}

record BoatMove(int Time, int Distance);