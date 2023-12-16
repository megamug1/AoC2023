
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2023;


public class Day16Part2
{
    const int UP = 1;
    const int RIGHT = 2;
    const int DOWN = 4;
    const int LEFT = 8;

    private class Grid
    {
        public char[,] mirrors;
        public int[,] energized;

        public void Reset()
        {
            for(int y = 0; y < energized.GetLength(0); y++)
            {
                for(int x = 0; x < energized.GetLength(0); x++)
                {
                    energized[y, x] = 0;
                }
            }
        }
    }

    private class Laser
    {
        public int y;
        public int x;
        public int direction;

        public Laser(int y, int x, int direction)
        {
            this.y = y;
            this.x = x;
            this.direction = direction;
        }
    }

    private Grid Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day16.txt");
        //string[] lines = File.ReadAllLines("inputs/Day16Sample.txt");

        Grid grid = new();

        grid.mirrors = new char[lines.Length, lines[0].Length];
        grid.energized = new int[lines.Length, lines[0].Length];

        for(int y = 0; y < lines.Length; y++)
        {
            for(int x = 0; x < lines[y].Length; x++)
            {
                grid.mirrors[y,x] = lines[y][x];
            }
        }

        return grid;
    }

    public void Solve()
    {
        var grid = Parse();

        int max = 0;

        for(int y = 0; y < grid.mirrors.GetLength(0); y++)
        {
            if(y == 0)
            {
                for(int x = 0; x < grid.mirrors.GetLength(1); x++)
                {
                    max = Math.Max(max, ComputeEnergy(grid, new Laser(y, x, DOWN)));
                }
            }
            if(y == grid.mirrors.GetLength(1) - 1)
            {
                for(int x = 0; x < grid.mirrors.GetLength(1); x++)
                {
                    max = Math.Max(max, ComputeEnergy(grid, new Laser(y, x, UP)));
                }
            }

            max = Math.Max(max, ComputeEnergy(grid, new Laser(y, 0, RIGHT)));
            max = Math.Max(max, ComputeEnergy(grid, new Laser(y, grid.mirrors.GetLength(1) - 1, LEFT)));
        }

        Console.WriteLine(max);
    }

    private int ComputeEnergy(Grid grid, Laser laser)
    {
        grid.energized[laser.y, laser.x] = laser.direction;
        List<Laser> lasers = new();

        lasers.Add(laser);

        while(lasers.Count > 0)
        {
            Laser? currLaser = lasers[0];
            lasers.RemoveAt(0);

            bool outOfBounds = false;
            while(!outOfBounds)
            {
                outOfBounds = !MoveLaser(grid, currLaser, lasers);
            }
        }

        int sum = 0;
        for(int y = 0; y < grid.energized.GetLength(0); y++)
        {
            for(int x = 0; x < grid.energized.GetLength(1); x++)
            {
                if(grid.energized[y, x] > 0) 
                {
                    sum++;
                }
            }
        }

        grid.Reset();
        return sum;
    }

    private bool MoveLaser(Grid grid, Laser laser, List<Laser> lasers)
    {
        switch(grid.mirrors[laser.y, laser.x])
        {
            case '.':
                return MoveLaser(grid, laser, laser.direction);
            case '\\':
                if(laser.direction == UP)
                {
                    return MoveLaser(grid, laser, LEFT);
                }
                if(laser.direction == RIGHT)
                {
                    return MoveLaser(grid, laser, DOWN);
                }
                if(laser.direction == DOWN)
                {
                    return MoveLaser(grid, laser, RIGHT);
                }
                if(laser.direction == LEFT)
                {
                    return MoveLaser(grid, laser, UP);
                }
                break;
            case '/':
                if(laser.direction == UP)
                {
                    return MoveLaser(grid, laser, RIGHT);
                }
                if(laser.direction == RIGHT)
                {
                    return MoveLaser(grid, laser, UP);
                }
                if(laser.direction == DOWN)
                {
                    return MoveLaser(grid, laser, LEFT);
                }
                if(laser.direction == LEFT)
                {
                    return MoveLaser(grid, laser, DOWN);
                }
                break;
            case '|':
                if(laser.direction == RIGHT || laser.direction == LEFT)
                {
                    Laser newLaser = new(laser.y, laser.x, laser.direction);
                    if(MoveLaser(grid, newLaser, UP))
                    {
                        lasers.Add(newLaser);
                    }
                    return MoveLaser(grid, laser, DOWN);
                }
                else
                {
                    return MoveLaser(grid, laser, laser.direction);
                }
            case '-':
                if(laser.direction == UP || laser.direction == DOWN)
                {
                    Laser newLaser = new(laser.y, laser.x, laser.direction);
                    if(MoveLaser(grid, newLaser, LEFT))
                    {
                        lasers.Add(newLaser);
                    }
                    return MoveLaser(grid, laser, RIGHT);
                }
                else
                {
                    return MoveLaser(grid, laser, laser.direction);
                }
        }

        throw new Exception("SHOULDN'T GET HERE!");
    }

    //return true if still within the bounds of the grid
    private bool MoveLaser(Grid grid, Laser laser, int direction)
    {
        switch(direction)
        {
            case UP:
                if(laser.y == 0) return false;
                laser.y--;
                break;
            case RIGHT:
                if(laser.x == grid.mirrors.GetLength(1) - 1) return false;
                laser.x++;
                break;
            case DOWN:
                if(laser.y == grid.mirrors.GetLength(0) - 1) return false;
                laser.y++;
                break;
            case LEFT:
                if(laser.x == 0) return false;
                laser.x --;
                break;
        }

        laser.direction = direction;
        if((grid.energized[laser.y, laser.x] & laser.direction) > 0) return false;
        grid.energized[laser.y, laser.x] |= direction;
        return true;
    }
}
