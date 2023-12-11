
namespace AdventOfCode2023;


public class Day8
{
    Dictionary<string, (string l, string r)> map;
    string directions;

    private void Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day8.txt");
        //string[] lines = File.ReadAllLines("inputs/Day8Sample.txt");

        directions = lines[0];
        map = new Dictionary<string, (string l, string r)>();

        for(int i = 2; i < lines.Length; i++)
        {
            string line = lines[i];
            
            string key = line.Substring(0, 3);
            string l = line.Substring(7, 3);
            string r = line.Substring(12, 3);

            (string l, string r) directions = (l, r);
            map.Add(key, directions);
        }
    }

    public void Solve()
    {
        Parse();

        int dirLen = directions.Length;

        string curr = "AAA";

        int step = 0;
        while(curr != "ZZZ")
        {
            bool left = directions[step % dirLen] == 'L';
            step++;

            curr = left ? map[curr].l : map[curr].r;
        }

        Console.WriteLine(step);
    }
}
