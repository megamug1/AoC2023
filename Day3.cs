
namespace AdventOfCode2023;



public class Day3
{
    static Position[,] grid;
    static int width;
    static int height;

    public class Position
    {
        public int y;
        public int x;
        public char value;

        public Position(int y, int x, char value)
        {
            this.y = y;
            this.x = x;
            this.value = value;
        }

        public bool IsDigit()
        {
            return Char.IsDigit(value);
        }

        public int Digit
        {
            get{ return value - '0'; }
        }

        public bool IsSymbol()
        {
            return !IsDigit() && value != '.';
        }

        public bool IsNearSymbol()
        {
            for(int currY = Math.Max(0, y-1); currY < Math.Min(y+2, height); currY++)
            {
                for(int currX = Math.Max(0, x-1); currX < Math.Min(x+2, width); currX++)
                {
                    if(grid[currY,currX].IsSymbol()) return true;
                }
            }

            return false;
        }
    }

    public void Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day3.txt");
        //string[] lines = File.ReadAllLines("inputs/Day3Sample.txt");

        height = lines.Length;
        width = lines[0].Length;
        grid = new Position[height, width];

        for(int y = 0; y < height; y ++)
        {
            string line = lines[y];

            for(int x = 0; x < width; x++)
            {
                grid[y,x] = new Position(y, x, line[x]);
            }
        }
    }

    public void Solve()
    {
        Parse();

        int sum = 0;

        for(int y = 0; y < height; y++)
        {
            int num = 0;
            bool isPart = false;

            for(int x = 0; x < width; x++)
            {
                Position pos = grid[y,x];

                if(pos.IsDigit())
                {
                    num = num * 10 + pos.Digit;
                    if(pos.IsNearSymbol()) isPart = true;
                }
                else
                {
                    if(isPart) sum += num;
                    num = 0;
                    isPart = false;
                }
            }

            if(isPart) sum += num;
        }

        Console.WriteLine(sum);
    }
}
