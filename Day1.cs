namespace AdventOfCode2023;

class Day1
{
    public void Solve()
    {
        string[] lines = File.ReadAllLines("inputs/Day1.txt");
        //string[] lines = File.ReadAllLines("inputs/Day1Sample.txt");

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
            char c = line[i];
            if(Char.IsDigit(c))
            {
                first = c - '0';
                break;
            }
        }

        int second = 0;
        for(int i = 0; i < line.Length; i++)
        {
            char c = line[line.Length - i - 1];
            if(Char.IsDigit(c))
            {
                second = c - '0';
                break;
            }
        }

        return first * 10 + second;
    }
}
