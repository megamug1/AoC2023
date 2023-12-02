namespace AdventOfCode2023;

class Day1Part2
{
    public void Solve()
    {
        string[] lines = File.ReadAllLines("inputs/Day1.txt");
        //string[] lines = File.ReadAllLines("inputs/Day1Part2Sample.txt");

        int sum = 0;
        foreach(string line in lines)
        {
            int value = GetCalibrationValue(line);
            sum += value;
        }

        Console.WriteLine(sum);
    }

    private int GetCalibrationValue(string line)
    {
        int first = 0;
        for(int i = 0; i < line.Length; i++)
        {
            int digit = GetDigit(line, i);
            if(digit > -1)
            {
                first = digit;
                break;
            }
        }

        int second = 0;
        for(int i = 0; i < line.Length; i++)
        {
            int digit = GetDigit(line, line.Length - i - 1);
            if(digit > -1)
            {
                second = digit;
                break;
            }
        }

        return first * 10 + second;
    }

    private int GetDigit(string line, int i)
    {
        char c = line[i];
        if(Char.IsDigit(c))
        {
            return c - '0';
        }

        string substring = line[i..];
        if(substring.StartsWith("one")) return 1;
        if(substring.StartsWith("two")) return 2;
        if(substring.StartsWith("three")) return 3;
        if(substring.StartsWith("four")) return 4;
        if(substring.StartsWith("five")) return 5;
        if(substring.StartsWith("six")) return 6;
        if(substring.StartsWith("seven")) return 7;
        if(substring.StartsWith("eight")) return 8;
        if(substring.StartsWith("nine")) return 9;

        return -1;
    }
}
