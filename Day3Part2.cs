using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2023;



public class Day3Part2
{
    static Position[,] grid;
    static int width;
    static int height;

    public class Position
    {
        public int y;
        public int x;
        public char value;

        public List<int> gearRatios;

        public Position(int y, int x, char value)
        {
            this.y = y;
            this.x = x;
            this.value = value;
            gearRatios = new List<int>();
        }

        public bool IsDigit()
        {
            return Char.IsDigit(value);
        }

        public int Digit
        {
            get{ return value - '0'; }
        }

        public bool IsGear()
        {
            return value == '*';
        }

        public void CollectNearbyGears(HashSet<Position> nearbyGears)
        {
            for(int currY = Math.Max(0, y-1); currY < Math.Min(y+2, height); currY++)
            {
                for(int currX = Math.Max(0, x-1); currX < Math.Min(x+2, width); currX++)
                {
                    Position currPos = grid[currY,currX];
                    if(currPos.IsGear()) nearbyGears.Add(currPos);
                }
            }
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


        for(int y = 0; y < height; y++)
        {
            int num = 0;
            HashSet<Position> nearbyGears = new HashSet<Position>();

            for(int x = 0; x < width; x++)
            {
                Position pos = grid[y,x];

                if(pos.IsDigit())
                {
                    num = num * 10 + pos.Digit;
                    pos.CollectNearbyGears(nearbyGears);
                }
                else
                {
                    foreach(Position nearbyGear in nearbyGears)
                    {
                        nearbyGear.gearRatios.Add(num);
                    }
                    num = 0;
                    nearbyGears = new HashSet<Position>();
                }
            }

            foreach(Position nearbyGear in nearbyGears)
            {
                nearbyGear.gearRatios.Add(num);
            }
        }

        int sum = 0;
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                Position pos = grid[y,x];
                if(pos.gearRatios.Count == 2)
                {
                    sum += pos.gearRatios[0] * pos.gearRatios[1];
                }
            }
        }

        Console.WriteLine(sum);
    }
}
