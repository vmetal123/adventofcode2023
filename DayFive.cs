namespace adventofcode2023;

public class DayFive
{
    public void SolvePartOne()
    {
        var lines = File.ReadAllLines("day5.txt");
        long lenght = lines.Length;
        long[] seeds = GetSeeds(lines[0]);
        long index = 1;
        while (index < lenght)
        {
            if (lines[index].Contains("seed-to-soil"))
            {
                index = ProcessMap(lines, seeds, index, lenght);
                continue;
            }

            if (lines[index].Contains("soil-to-fertilizer"))
            {
                index = ProcessMap(lines, seeds, index, lenght);
                continue;
            }

            if (lines[index].Contains("fertilizer-to-water"))
            {
                index = ProcessMap(lines, seeds, index, lenght);
                continue;
            }

            if (lines[index].Contains("water-to-light"))
            {
                index = ProcessMap(lines, seeds, index, lenght);
                continue;
            }

            if (lines[index].Contains("light-to-temperature"))
            {
                index = ProcessMap(lines, seeds, index, lenght);
                continue;
            }

            if (lines[index].Contains("temperature-to-humidity"))
            {
                index = ProcessMap(lines, seeds, index, lenght);
                continue;
            }

            if (lines[index].Contains("humidity-to-location"))
            {
                index = ProcessMap(lines, seeds, index, lenght);
                continue;
            }

            index++;
        }

        Console.WriteLine($"Lowes location: {seeds.Min()}");
    }

    private static long ProcessMap(string[] lines, long[] seeds, long index, long lenght)
    {
        var rows = new List<(long, long, long, long)>();
        index++;
        while (lines[index] != string.Empty)
        {
            var info = lines[index]
                .Split(' ', StringSplitOptions.TrimEntries)
                .Select(x => Convert.ToInt64(x)).ToArray();

            var initialSource = info[1];
            var finalSource = initialSource + info[2];
            var initialDest = info[0];
            var finalDest = initialDest + info[2];

            rows.Add((initialSource, finalSource, initialDest, finalDest));

            index++;

            if (index == lenght)
            {
                break;
            }
            continue;
        }

        for (int i = 0; i < seeds.Length; i++)
        {
            for (int j = 0; j < rows.Count; j++)
            {
                if (seeds[i] >= rows[j].Item1 && seeds[i] < rows[j].Item2)
                {
                    seeds[i] = (seeds[i] - rows[j].Item1) + rows[j].Item3;
                    break;
                }
            }
        }

        return index;
    }

    private long[] GetSeeds(string line)
    {
        return line
            .Split(':', StringSplitOptions.TrimEntries)[1]
            .Split(' ', StringSplitOptions.TrimEntries)
            .Select(x => Convert.ToInt64(x)).ToArray();
    }
}

internal record Map(List<int> Destination, List<int> Source, List<int> Length);