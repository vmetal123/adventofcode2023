namespace adventofcode2023;

public class DayThree
{
    private readonly List<(int, int)> steps = new List<(int, int)> {
            (0,1),
            (1,0),
            (1,1),
            (0,-1),
            (-1,0),
            (-1,-1),
            (-1,1),
            (1,-1)
        };

    public void SolvePartOne()
    {
        var lines = File.ReadAllLines("day3.txt");

        long sum = 0;
        bool isPartNumber = false;

        int ROWS = lines.Length;
        int COLS = lines[0].Length;

        for (int row = 0; row < ROWS; row++)
        {
            int number = 0;
            for (int col = 0; col < COLS; col++)
            {
                if (!char.IsDigit(lines[row][col]))
                {
                    if (number > 0 && isPartNumber)
                    {
                        Console.WriteLine($"Number found: {number}");
                        sum += number;
                    }
                    number = 0;
                    isPartNumber = false;
                    continue;
                }

                var value = lines[row][col] - '0';
                number = number * 10 + value;

                foreach ((int, int) step in steps)
                {
                    int rowNeighbor = row + step.Item1;
                    int colNeighbor = col + step.Item2;

                    if (rowNeighbor < 0 || rowNeighbor >= ROWS || colNeighbor < 0 || colNeighbor >= COLS)
                    {
                        continue;
                    }

                    char cellValue = lines[rowNeighbor][colNeighbor];
                    if (!char.IsDigit(cellValue) && cellValue != '.')
                    {
                        isPartNumber = true;
                        Console.WriteLine($"Found a symbol at position [{rowNeighbor},{colNeighbor}] : {lines[rowNeighbor][colNeighbor]}, current number: {number}");
                    }

                }
            }
            if (number > 0 && isPartNumber)
            {
                Console.WriteLine($"Number found: {number}");
                sum += number;
            }
            number = 0;
            isPartNumber = false;
        }

        Console.WriteLine($"Sum: {sum}");
    }

    public void SolvePartTwo()
    {
        var lines = File.ReadAllLines("day3.txt");

        int ROWS = lines.Length;
        int COLS = lines[0].Length;
        int sum = 0;

        var asterisks = new Dictionary<(int, int), List<int>>();
        var asterisksPosition = new HashSet<(int, int)>();

        for (int row = 0; row < ROWS; row++)
        {
            int number = 0;
            for (int col = 0; col < COLS; col++)
            {
                if (!char.IsDigit(lines[row][col]))
                {
                    if (number > 0 && asterisksPosition.Count > 0)
                    {
                        foreach (var position in asterisksPosition)
                        {
                            int x = position.Item1;
                            int y = position.Item2;

                            if (!asterisks.ContainsKey((x, y)))
                            {
                                asterisks[(x, y)] = [];
                            }

                            asterisks[(x, y)].Add(number);
                        }
                    }
                    number = 0;
                    asterisksPosition.Clear();
                    continue;
                }

                int value = lines[row][col] - '0';
                number = number * 10 + value;

                foreach (var step in steps)
                {
                    int rowNeighbor = row + step.Item1;
                    int colNeighbor = col + step.Item2;

                    if (rowNeighbor < 0 || rowNeighbor >= ROWS || colNeighbor < 0 || colNeighbor >= COLS)
                    {
                        continue;
                    }

                    if (lines[rowNeighbor][colNeighbor] == '*')
                    {
                        asterisksPosition.Add((rowNeighbor, colNeighbor));
                    }
                }
            }
            if (number > 0 && asterisksPosition.Count > 0)
            {
                foreach (var position in asterisksPosition)
                {
                    int x = position.Item1;
                    int y = position.Item2;

                    if (!asterisks.ContainsKey((x, y)))
                    {
                        asterisks[(x, y)] = [];
                    }

                    asterisks[(x, y)].Add(number);
                }
            }
            number = 0;
            asterisksPosition.Clear();
        }

        foreach (var asterisk in asterisks)
        {
            if(asterisk.Value.Count == 2)
            {
                sum += asterisk.Value[0] * asterisk.Value[1];
            }
        }

        Console.WriteLine($"Sum: {sum}");
    }
}