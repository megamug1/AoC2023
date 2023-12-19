namespace AdventOfCode2023;


public class Day18
{

    private List<(char dir, int dist)> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day18.txt");
        //string[] lines = File.ReadAllLines("inputs/Day18Sample.txt");

        List<(char dir, int dist)> instructions = new();

        foreach(string line in lines)
        {
            string[] parts = line.Split(' ');
            char dir = parts[0][0];
            int dist = int.Parse(parts[1]);
            instructions.Add((dir, dist));
        }

        return instructions;
    }

    public void Solve()
    {
        var instructions = Parse();
        var bounds = FindBounds(instructions);
        char[,] ground = new char[bounds.height + 2, bounds.width + 2];
        for(int y = 0; y < ground.GetLength(0); y++)
        {
            for(int x = 0; x < ground.GetLength(1); x++)
            {
                ground[y, x] = '+';
            }
        }

        (int y, int x) currPos = (bounds.startY + 1, bounds.startX + 1);
        ground[currPos.y, currPos.x] = '#';

        foreach(var instruction in instructions)
        {
            switch(instruction.dir)
            {
                case 'U':
                    for(int step = 1; step <= instruction.dist; step++)
                    {
                        currPos.y--;
                        ground[currPos.y, currPos.x] = '#';
                    }
                    break;
                case 'R':
                    for(int step = 1; step <= instruction.dist; step++)
                    {
                        currPos.x++;
                        ground[currPos.y, currPos.x] = '#';
                    }
                    break;
                case 'D':
                    for(int step = 1; step <= instruction.dist; step++)
                    {
                        currPos.y++;
                        ground[currPos.y, currPos.x] = '#';
                    }
                    break;
                case 'L':
                    for(int step = 1; step <= instruction.dist; step++)
                    {
                        currPos.x--;
                        ground[currPos.y, currPos.x] = '#';
                    }
                    break;
            }
        }

        Fill(ground);

        Print(ground);

        Console.WriteLine(CountHoles(ground));
    }

    private (int width, int height, int startX, int startY) FindBounds(List<(char dir, int dist)> instructions)
    {
        int minX = 0;
        int minY = 0;
        int maxX = 0;
        int maxY = 0;

        int currX = 0;
        int currY = 0;

        foreach(var instruction in instructions)
        {
            switch(instruction.dir)
            {
                case 'U':
                    currY -= instruction.dist;
                    break;
                case 'R':
                    currX += instruction.dist;
                    break;
                case 'D':
                    currY += instruction.dist;
                    break;
                case 'L':
                    currX -= instruction.dist;
                    break;
            }

            minX = Math.Min(minX, currX);
            minY = Math.Min(minY, currY);
            maxX = Math.Max(maxX, currX);
            maxY = Math.Max(maxY, currY);
        }

        return (1 + maxX - minX, 1 + maxY - minY, -minX, -minY);
    }

    private void Fill(char[,] ground)
    {
        List<(int y, int x)> toFill = new();
        toFill.Add((0, 0));

        while(toFill.Count != 0)
        {
            var position = toFill[0];
            toFill.RemoveAt(0);
            if(position.y < 0 || position.y >= ground.GetLength(0) ||
                position.x < 0 || position.x >= ground.GetLength(1))
                {
                    continue;
                }
            
            char currValue = ground[position.y, position.x];
            if(currValue == '#' || currValue == '.') continue;

            ground[position.y, position.x] = '.';

            toFill.Add((position.y - 1, position.x));//up
            toFill.Add((position.y, position.x + 1));//right
            toFill.Add((position.y, position.x - 1));//left
            toFill.Add((position.y + 1, position.x));//down
        }
    }

    private int CountHoles(char[,] ground)
    {
        int total = 0;
        for(int y = 0; y < ground.GetLength(0); y++)
        {
            for(int x = 0; x < ground.GetLength(1); x++)
            {
                if(ground[y,x] == '#' || ground[y,x] == '+') total++;
            }
        }
        return total;
    }

    private void Print(char[,] ground)
    {
        for(int y = 0; y < ground.GetLength(0); y++)
        {
            for(int x = 0; x < ground.GetLength(1); x++)
            {
                Console.Write(ground[y ,x]);
            }
            Console.WriteLine();
        }
    }
}
