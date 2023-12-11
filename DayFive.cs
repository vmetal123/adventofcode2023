using System.Runtime.InteropServices;
using System.Security.AccessControl;

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

    public void SolvePartTwo()
    {
        var lines = File.ReadAllLines("day5test.txt");

        List<SeedRange> seeds = GetSeedsList(lines[0]);
        long lenght = lines.LongLength;
        var mappings = new List<Mapping>();

        long index = 1;
        while (index < lenght)
        {
            if (lines[index].Contains("-to-"))
            {
                var mapping = GetMappingInfo(lines, ref index);
                mappings.Add(mapping);
                continue;
            }
            index++;
        }

        List<SeedRange> newSeeds = seeds;
        foreach (var mapping in mappings)
        {
            List<SeedRange> newSeedsList = new();
            foreach (var seed in newSeeds)
            {
                newSeedsList.AddRange(mapping.Map(seed));
            }

            newSeeds = newSeedsList;
        }
        Console.WriteLine($"Lowes location: {newSeeds.Select(x => x.Start).Min()}");
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

    private List<SeedRange> GetSeedsList(string line)
    {
        var seeds = line
            .Split(':', StringSplitOptions.TrimEntries)[1]
            .Split(' ', StringSplitOptions.TrimEntries)
            .Select(x => Convert.ToInt64(x)).ToArray();

        var seedList = new List<SeedRange>();

        for (int i = 0; i < seeds.Length; i += 2)
        {
            seedList.Add(new SeedRange(seeds[i], seeds[i + 1]));
        }

        return seedList;
    }

    private (long index, long seed) ProcessMapBySeed(string[] lines, long seed, long index, long lenght)
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

        for (int i = 0; i < rows.Count; i++)
        {
            if (seed >= rows[i].Item1 && seed < rows[i].Item2)
            {
                seed = (seed - rows[i].Item1) + rows[i].Item3;
                break;
            }
        }

        return (index, seed);
    }

    private Mapping GetMappingInfo(string[] lines, ref long index)
    {
        index++;
        var mappings = new List<MappingRange>();
        while (index < lines.LongLength && lines[index] != string.Empty)
        {
            var mappingInfo = lines[index]
                .Split(' ')
                .Select(x => Convert.ToInt64(x))
                .ToArray();

            var mappingRange = new MappingRange(mappingInfo[0], mappingInfo[1], mappingInfo[2]);
            mappings.Add(mappingRange);
            index++;
        }
        return new Mapping(mappings.OrderBy(x => x.SourceStart).ToList());
    }
}

internal record SeedRange(long Start, long Length);

internal record Mapping(List<MappingRange> Ranges)
{
    public List<SeedRange> Map(SeedRange seedRange)
    {
        List<SeedRange> newSeeds = new();
        var remaining = seedRange;

        foreach (var mapping in Ranges)
        {
            if (remaining.Start < mapping.SourceStart)
            {
                var cutOffLength = Math.Min(remaining.Length, mapping.SourceStart - remaining.Start);
                var cuttOff = new SeedRange(remaining.Start, cutOffLength);
                newSeeds.Add(cuttOff);

                remaining = new SeedRange(remaining.Start + cutOffLength, remaining.Length - cutOffLength);
            }

            if (remaining.Length <= 0)
            {
                break;
            }

            if (remaining.Start >= mapping.SourceStart && remaining.Start < (mapping.SourceStart + mapping.Lenght))
            {
                var intersectionLength = Math.Min(remaining.Length, (mapping.SourceStart + mapping.Lenght) - remaining.Start);
                var intersection = new SeedRange(remaining.Start, intersectionLength);
                var transformed = mapping.Transform(intersection);
                newSeeds.Add(transformed);

                remaining = new SeedRange(remaining.Start + intersectionLength, remaining.Length - intersectionLength);
            }

            if (remaining.Length <= 0)
            {
                break;
            }
        }

        if (remaining.Length > 0)
        {
            newSeeds.Add(remaining);
        }

        return newSeeds;
    }
}

internal record MappingRange(long DestStart, long SourceStart, long Lenght)
{
    private long MapSource(long value) => DestStart + (value - SourceStart);

    internal SeedRange Transform(SeedRange intersection) =>
    new SeedRange(MapSource(intersection.Start), intersection.Length);
}