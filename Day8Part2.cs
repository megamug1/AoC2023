
namespace AdventOfCode2023;


public class Day8Part2
{
    Dictionary<string, (string l, string r)> map;
    List<string> starts;
    string directions;

    private void Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day8.txt");
        //string[] lines = File.ReadAllLines("inputs/Day8Sample2.txt");

        directions = lines[0];
        map = new Dictionary<string, (string l, string r)>();
        starts = new List<string>();

        for(int i = 2; i < lines.Length; i++)
        {
            string line = lines[i];
            
            string key = line.Substring(0, 3);
            string l = line.Substring(7, 3);
            string r = line.Substring(12, 3);

            (string l, string r) directions = (l, r);
            map.Add(key, directions);

            if(key[2] == 'A') starts.Add(key);
        }
    }

    public void Solve()
    {
        Parse();

        int dirLen = directions.Length;

        int[] steps = new int[starts.Count];

        for(int i = 0; i < starts.Count; i++)
        {
            string curr = starts[i];

            int step = 0;
            while(curr[2] != 'Z')
            {
                bool left = directions[step % dirLen] == 'L';
                step++;

                curr = left ? map[curr].l : map[curr].r;
            }

            steps[i] = step;
        }

        long product = 1;
        foreach(int step in steps)
        {
            product *= (long)step;
        }

        //did prime factorization manually and multiplied all unique factors to get the right answer
        Console.WriteLine(product);
    }
}
