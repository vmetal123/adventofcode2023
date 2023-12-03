namespace adventofcode2023;

public class DayTwo
{
    public void SolvePartOne()
    {
        var lines = File.ReadAllLines("day2.txt");
        int sum = 0;
        foreach (var line in lines)
        {
            sum += GetIndexFromGame(line);
        }

        Console.WriteLine($"Sum: {sum}");
    }

    public void SolvePartTwo()
    {
        var lines = File.ReadAllLines("day2.txt");
        int sum = 0;
        foreach (var line in lines)
        {
            sum += GetPowerOfGame(line);
        }

        Console.WriteLine($"Sum: {sum}");
    }

    private int GetPowerOfGame(string line)
    {
        var gameInfo = line.Split(":");
        var cubes = gameInfo[1].Split(";", StringSplitOptions.TrimEntries);
        int maxRed = 0, maxGreen = 0, maxBlue = 0;
        foreach (var cube in cubes)
        {
            var subsets = cube.Split(",");
            foreach (var subset in subsets)
            {
                var cubeInfo = subset.Split(" ", StringSplitOptions.TrimEntries);
                int totalColor = Convert.ToInt32(cubeInfo[0]);

                switch (cubeInfo[1])
                {
                    case "red":
                        maxRed = totalColor > maxRed ? totalColor : maxRed;
                        break;
                    case "green":
                        maxGreen = totalColor > maxGreen ? totalColor : maxGreen;
                        break;
                    case "blue":
                        maxBlue = totalColor > maxBlue ? totalColor : maxBlue;
                        break;
                }

            }
        }

        return maxRed * maxGreen * maxBlue;
    }

    private int GetIndexFromGame(string line)
    {
        int maxRedCubes = 12, maxGreenCubes = 13, maxBlueCubes = 14;
        var gameInfo = line.Split(":");

        int gameIndex = Convert.ToInt32(gameInfo[0].Trim().Split(" ")[1]);

        var cubes = gameInfo[1].Trim().Split(";");

        bool isGameValid = true;
        foreach (var cube in cubes)
        {
            var subsets = cube.Split(",");
            foreach (var subset in subsets)
            {
                var cubeInfo = subset.Trim().Split(" ");
                int totalColor = Convert.ToInt32(cubeInfo[0]);

                switch (cubeInfo[1])
                {
                    case "red":
                        if (totalColor > maxRedCubes)
                        {
                            isGameValid = false;
                        }
                        break;
                    case "green":
                        if (totalColor > maxGreenCubes)
                        {
                            isGameValid = false;
                        }
                        break;
                    case "blue":
                        if (totalColor > maxBlueCubes)
                        {
                            isGameValid = false;
                        }
                        break;
                }

                if (!isGameValid)
                {
                    return 0;
                }
            }
        }

        return gameIndex;
    }
}