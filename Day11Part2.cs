
namespace AdventOfCode2023;


public class Day11Part2
{
    const long EXPANSION = 999999;

    private List<(long y, long x)> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day11.txt");
        //string[] lines = File.ReadAllLines("inputs/Day11Sample.txt");

        List<int> emptyRowIds = new List<int>();

        for(int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if(!line.Contains('#'))
            {
                emptyRowIds.Add(i);
            }
        }

        List<int> emptyColIds = new List<int>();

        for(int i = 0; i < lines[0].Length; i++)
        {
            bool found = false;
            for(int j = 0; j < lines.Length; j++)
            {
                if(lines[j][i] == '#')
                {
                    found = true;
                    break;
                }
            }

            if(!found) emptyColIds.Add(i);
        }

        List<(long y, long x)> galaxys = new List<(long y, long x)>();
        
        int emptyRows = 0;

        for(int y = 0; y < lines.Length; y++)
        {
            if(emptyRowIds.Count > 0 && emptyRowIds[0] == y)
            {
                emptyRowIds.RemoveAt(0);
                emptyRows ++;
                continue;
            }

            int emptyCols = 0;
            List<int> tempEmptyColIds = new List<int>(emptyColIds);

            for(int x = 0; x < lines[0].Length; x++)
            {
                if(tempEmptyColIds.Count > 0 && tempEmptyColIds[0] == x)
                {
                    tempEmptyColIds.RemoveAt(0);
                    emptyCols++;
                    continue;
                }

                if(lines[y][x] == '#') galaxys.Add((y+(emptyRows*EXPANSION), x+(emptyCols*EXPANSION)));
            }
        }

        return galaxys;
    }

    public void Solve()
    {
        List<(long y, long x)> galaxys = Parse();

        long sum = 0;

        for(int i = 0; i < galaxys.Count; i++)
        {
            for(int j = i+1; j < galaxys.Count; j++)
            {
                var g1 = galaxys[i];
                var g2 = galaxys[j];

                sum += Math.Abs(g1.y - g2.y) + Math.Abs(g1.x - g2.x);
            }
        }

        Console.WriteLine(sum);
    }

}
