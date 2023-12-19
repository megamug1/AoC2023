namespace AdventOfCode2023;


public class Day14Part2
{
    private List<List<char>> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day14.txt");
        //string[] lines = File.ReadAllLines("inputs/Day14Sample.txt");

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
        for(int i = 0; i < 1000; i++)
        {
            ShiftRocksNorth(platform);
            ShiftRocksWest(platform);
            ShiftRocksSouth(platform);
            ShiftRocksEast(platform);
            int result = ComputeLoad(platform);
            Console.WriteLine(result);
        }

        //Print(platform);
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

    private void ShiftRocksNorth(List<List<char>> platform)
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

    private void ShiftRocksWest(List<List<char>> platform)
    {
        for(int y = 0; y < platform.Count; y++)
        {
            int emptyIndex = 0;
            for(int x = 0; x < platform[0].Count; x++)
            {
                char c = platform[y][x];

                switch(c)
                {
                    case '.':
                        //ignore, nothing here;
                        break;
                    case '#':
                        emptyIndex = x+1;
                        break;
                    case 'O':
                        if(x > emptyIndex)
                        {
                            Swap(platform, y, x, y, emptyIndex);
                            emptyIndex ++;
                        }
                        else
                        {
                            emptyIndex = x+1;
                        }
                        break;
                }
            }
        }
    }

    private void ShiftRocksSouth(List<List<char>> platform)
    {
        for(int x = 0; x < platform[0].Count; x++)
        {
            int emptyIndex = platform.Count - 1;
            for(int y = platform.Count - 1; y >= 0; y--)
            {
                char c = platform[y][x];

                switch(c)
                {
                    case '.':
                        //ignore, nothing here;
                        break;
                    case '#':
                        emptyIndex = y-1;
                        break;
                    case 'O':
                        if(y < emptyIndex)
                        {
                            Swap(platform, y, x, emptyIndex, x);
                            emptyIndex --;
                        }
                        else
                        {
                            emptyIndex = y-1;
                        }
                        break;
                }
            }
        }
    }

    private void ShiftRocksEast(List<List<char>> platform)
    {
        for(int y = 0; y < platform.Count; y++)
        {
            int emptyIndex = platform[0].Count - 1;
            for(int x = platform[0].Count - 1; x >= 0; x--)
            {
                char c = platform[y][x];

                switch(c)
                {
                    case '.':
                        //ignore, nothing here;
                        break;
                    case '#':
                        emptyIndex = x-1;
                        break;
                    case 'O':
                        if(x < emptyIndex)
                        {
                            Swap(platform, y, x, y, emptyIndex);
                            emptyIndex --;
                        }
                        else
                        {
                            emptyIndex = x-1;
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
