namespace AdventOfCode2023;


public class Day14
{
    private List<List<char>> Parse()
    {
        //string[] lines = File.ReadAllLines("inputs/Day14.txt");
        string[] lines = File.ReadAllLines("inputs/Day14Sample.txt");

        List<List<char>> result = new();

        
        foreach(string line in lines)
        {
            List<char> row = new();
            result.Add(row);
            foreach(char c in line)
            {
                row.Add(c);
            }
        }

        return result;
    }

    public void Solve()
    {
        var platform = Parse();
        Print(platform);
        ShiftRocks(platform);
        Print(platform);

        int result = ComputeLoad(platform);

        Console.WriteLine(result);
    }

    private void Print(List<List<char>> platform)
    {
        for(int y = 0; y < platform.Count; y++)
        {
            for(int x = 0; x < platform[y].Count; x++)
            {
                Console.Write(platform[y][x]);
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private void ShiftRocks(List<List<char>> platform)
    {
        for(int x = 0; x < platform[0].Count; x++)
        {
            int emptyIndex = 0;
            for(int y = 0; y < platform.Count; y++)
            {
                char c = platform[y][x];

                switch(c)
                {
                    case '.':
                        //ignore, nothing here;
                        break;
                    case '#':
                        emptyIndex = y+1;
                        break;
                    case 'O':
                        if(y > emptyIndex)
                        {
                            Swap(platform, y, x, emptyIndex, x);
                            emptyIndex ++;
                        }
                        else
                        {
                            emptyIndex = y+1;
                        }
                        break;
                }
            }
        }
    }


    private void Swap(List<List<char>> platform, int y1, int x1, int y2, int x2)
    {
        char temp = platform[y1][x1];
        platform[y1][x1] = platform[y2][x2];
        platform[y2][x2] = temp;
    }

    private int ComputeLoad(List<List<char>> platform)
    {
        int load = 0;

        for(int y = 0; y < platform.Count; y++)
        {
            for(int x = 0; x < platform[y].Count; x++)
            {
                if(platform[y][x] == 'O')
                {
                    load += platform.Count - y;
                }
            }
        }

        return load;
    }
}
