
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2023;


public class Day17Part2
{
    private const int MAX_MOVES = 10;
    private const int MIN_MOVES = 4;
    private const int UP = 0;
    private const int RIGHT = 1;
    private const int DOWN = 2;
    private const int LEFT = 3;

    private const int MAX_VALUE = int.MaxValue;


    private class Block
    {
        //the minimum cost of moving into this block from the given direction/magnitude
        public int[] minCosts;
        public string[] pathTo;

        public int cost;

        public Block(int cost)
        {
            this.cost = cost;

            minCosts = new int[MAX_MOVES*4];
            for(int i = 0; i < MAX_MOVES*4; i++)
            {
                minCosts[i] = MAX_VALUE;
            }

            pathTo = new string[MAX_MOVES*4];
            for(int i = 0; i < MAX_MOVES*4; i++)
            {
                pathTo[i] = "";
            }
        }

        public int ComputeTotalMinCost()
        {
            int min = MAX_VALUE;
            for(int i = 0; i < MAX_MOVES*4; i++)
            {
                min = Math.Min(min, minCosts[i]);
            }

            return min;
        }
    }

    private Block[,] Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day17.txt");
        //string[] lines = File.ReadAllLines("inputs/Day17Sample.txt");
        //string[] lines = File.ReadAllLines("inputs/Day17Sample2.txt");

        Block[,] grid = new Block[lines.Length, lines[0].Length];

        for(int y = 0; y < lines.Length; y++)
        {
            for(int x = 0; x < lines[y].Length; x++)
            {
                grid[y,x] = new Block(lines[y][x] - '0');
            }
        }

        for(int i = 0; i < MAX_MOVES*4; i++)
        {
            grid[0,0].minCosts[i] = 0;
        }
        return grid;
    }

    public void Solve()
    {
        var grid = Parse();

        // position => min cost, includes everything that still needs processing
        List<(int y, int x, int dir, int cost)> toProcess = new();

        toProcess.Add((0, 0, -1, 0));

        while(true)
        {
            int minCost = MAX_VALUE;
            var prevMove = toProcess.First();
            var minIndex = 0;
            for(int i = 0; i < toProcess.Count; i++)
            {
                var next = toProcess[i];
                if(next.cost < minCost)
                {
                    minIndex = i;
                    minCost = next.cost;
                    prevMove = next;
                }
            }
            toProcess.RemoveAt(minIndex);

            if(prevMove.y == grid.GetLength(0) - 1 && prevMove.x == grid.GetLength(1) - 1)
            {
                //at target
                break;
            }

            for(int dir = 0; dir < MAX_MOVES*4; dir++)
            {
                if(!CanMove(grid, (prevMove.y, prevMove.x), prevMove.dir, dir)) continue;

                ProcessMove(toProcess, grid, prevMove, dir);
            }
        }

        Block bottomRight = grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1];
        Console.WriteLine(bottomRight.ComputeTotalMinCost());

        int minI = 0;
        int minValue = int.MaxValue;
        for(int i = 0; i < MAX_MOVES*4; i++)
        {
            if(bottomRight.minCosts[i] < minValue)
            {
                minValue = bottomRight.minCosts[i];
                minI = i;
            }
        }

        PrintPath(grid, bottomRight.pathTo[minI]);
    }

    private void PrintPath(Block[,] grid, string path)
    {
        char[,] output = new char[grid.GetLength(0), grid.GetLength(1)];

        for(int y = 0; y < grid.GetLength(0); y++)
        {
            for(int x = 0; x < grid.GetLength(1); x++)
            {
                output[y,x] = (char)('0' + grid[y,x].cost);
            }
        }

        (int y, int x) position = (0, 0);
        foreach(char c in path)
        {
            switch(c)
            {
                case '^':
                    position = (position.y-1, position.x);
                    break;
                case '>':
                    position = (position.y, position.x+1);
                    break;
                case 'v':
                    position = (position.y+1, position.x);
                    break;
                case '<':
                    position = (position.y, position.x-1);
                    break;
            }

            output[position.y, position.x] = c;
        }

        for(int y = 0; y < grid.GetLength(0); y++)
        {
            for(int x = 0; x < grid.GetLength(1); x++)
            {
                Console.Write(output[y,x]);
            }
            Console.WriteLine();
        }
    }

    private void ProcessMove(List<(int y, int x, int dir, int cost)> toProcess, 
                            Block[,] grid, 
                            (int y, int x, int dir, int cost) prevMove, 
                            int dir)
    {
        int generalDir = dir / MAX_MOVES;
        (int y, int x) newPosition = (-1, -1);
        char dirChar = '~';
        if(generalDir == UP)
        {
            newPosition = (prevMove.y-1, prevMove.x);
            dirChar = '^';
        }
        else if(generalDir == RIGHT)
        {
            newPosition = (prevMove.y, prevMove.x+1);
            dirChar = '>';
        }
        else if(generalDir == DOWN)
        {
            newPosition = (prevMove.y+1, prevMove.x);
            dirChar = 'v';
        }
        else //if(generalDir == LEFT)
        {
            newPosition = (prevMove.y, prevMove.x-1);
            dirChar = '<';
        }
        
        if(newPosition.y < 0 ||
            newPosition.y >= grid.GetLength(0) ||
            newPosition.x < 0 ||
            newPosition.x >= grid.GetLength(1)) 
        {
            return;
        }

        Block fromBlock = grid[prevMove.y, prevMove.x];
        Block toBlock = grid[newPosition.y, newPosition.x];

        int newCost = prevMove.cost + toBlock.cost;

        if(newCost < toBlock.minCosts[dir])
        {
            string prevPath = "";
            if(prevMove.dir >= 0) prevPath = fromBlock.pathTo[prevMove.dir];
            toBlock.pathTo[dir] = prevPath + dirChar;
            toBlock.minCosts[dir] = newCost;
            toProcess.Add((newPosition.y, newPosition.x, dir, newCost));
        }
    }

    private bool CanMove(Block[,] grid, (int y, int x) position, int prevMove, int move)
    {
        int moveCount = move % MAX_MOVES;

        if(prevMove == -1) return moveCount == 0;

        int prevMoveCount = prevMove % MAX_MOVES;
        int prevDirection = prevMove / MAX_MOVES;
        int direction = move / MAX_MOVES;
        
        if(prevDirection == direction)
        {
            return (moveCount - prevMoveCount) == 1;
        }
        else
        {
            if(Math.Abs(direction - prevDirection) == 2) return false;
            if(prevMoveCount < MIN_MOVES - 1) return false;
            if(DistanceToEdge(grid, position, direction) <= MIN_MOVES) 
            {
                return false;
            }
            return moveCount == 0;
        }
    }

    private int DistanceToEdge(Block[,] grid, (int y, int x) position, int generalDir)
    {
        switch(generalDir)
        {
            case UP:
                return position.y + 1;
            case RIGHT:
                return grid.GetLength(1) - position.x;
            case DOWN:
                return grid.GetLength(0) - position.y;
            case LEFT:
                return position.x + 1;
        }

        throw new Exception("Not a general direction");
    }
}
