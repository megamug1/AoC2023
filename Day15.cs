
namespace AdventOfCode2023;


public class Day15
{
    private List<string> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day15.txt");
        //string[] lines = File.ReadAllLines("inputs/Day15Sample.txt");

        List<string> instructions = new(lines[0].Split(','));

        return instructions;
    }

    public void Solve()
    {
        var instructions = Parse();

        long sum = 0;
        foreach(string instruction in instructions)
        {
            sum += HASH(instruction);
        }

        Console.WriteLine(sum);
    }

    private int HASH(string instruction)
    {
        int curr = 0;
        foreach(char c in instruction)
        {
            curr += (int)c;
            curr *= 17;
            curr %= 256;
        }

        return curr;
    }
}
