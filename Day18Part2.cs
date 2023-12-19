namespace AdventOfCode2023;


public class Day18Part2
{

    private List<(char dir, long dist)> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day18.txt");
        //string[] lines = File.ReadAllLines("inputs/Day18Sample.txt");

        List<(char dir, long dist)> instructions = new();

        foreach(string line in lines)
        {
            string[] parts = line.Split(' ');
            char dirNum = parts[2][7];

            char dir = 'R';
            if(dirNum == '1') dir = 'D';
            if(dirNum == '2') dir = 'L';
            if(dirNum == '3') dir = 'U';


            int dist = int.Parse(parts[2].Substring(2, 5), System.Globalization.NumberStyles.HexNumber);
            instructions.Add((dir, dist));
        }

        return instructions;
    }

    public void Solve()
    {
        var instructions = Parse();

        var bounds = FindBounds(instructions);

        (long y, long x) currPos = (bounds.startY, bounds.startX);

        long area = 0;
        
        foreach(var instruction in instructions)
        {
            switch(instruction.dir)
            {
                case 'U':
                    currPos.y -= instruction.dist;
                    break;
                case 'R':
                    currPos.x += instruction.dist;

                    area += instruction.dist * currPos.y;
                    break;
                case 'D':
                    currPos.y += instruction.dist;
                    area -= instruction.dist;
                    break;
                case 'L':
                    currPos.x -= instruction.dist;
                    area -= instruction.dist;
                    area -= instruction.dist * currPos.y;
                    break;
            }
        }

        Console.WriteLine(-(area-1));
    }    
    
    private (long width, long height, long startX, long startY) FindBounds(List<(char dir, long dist)> instructions)
    {
        long minX = 0;
        long minY = 0;
        long maxX = 0;
        long maxY = 0;

        long currX = 0;
        long currY = 0;

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

}
