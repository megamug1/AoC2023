
namespace AdventOfCode2023;


public class Day17
{
    private const int UP1 = 0;
    private const int UP2 = 1;
    private const int UP3 = 2;
    private const int RIGHT1 = 3;
    private const int RIGHT2 = 4;
    private const int RIGHT3 = 5;
    private const int DOWN1 = 6;
    private const int DOWN2 = 7;
    private const int DOWN3 = 8;
    private const int LEFT1 = 9;
    private const int LEFT2 = 10;
    private const int LEFT3 = 11;

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

            minCosts = new int[12];
            for(int i = 0; i < 12; i++)
            {
                minCosts[i] = MAX_VALUE;
            }

            pathTo = new string[12];
            for(int i = 0; i < 12; i++)
            {
                pathTo[i] = "";
            }
        }

        public int ComputeTotalMinCost()
        {
            int min = MAX_VALUE;
            for(int i = 0; i < 12; i++)
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

        Block[,] grid = new Block[lines.Length, lines[0].Length];

        for(int y = 0; y < lines.Length; y++)
        {
            for(int x = 0; x < lines[y].Length; x++)
            {
                grid[y,x] = new Block(lines[y][x] - '0');
            }
        }

        for(int i = 0; i < 12; i++)
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
            var bestMove = toProcess.First();
            var minIndex = 0;
            for(int i = 0; i < toProcess.Count; i++)
            {
                var next = toProcess[i];
                if(next.cost < minCost)
                {
                    minIndex = i;
                    minCost = next.cost;
                    bestMove = next;
                }
            }
            toProcess.RemoveAt(minIndex);

            if(bestMove.y == grid.GetLength(0) - 1 && bestMove.x == grid.GetLength(1) - 1)
            {
                //at target
                break;
            }

            if(bestMove.dir != UP3 && bestMove.dir != DOWN1 && bestMove.dir != DOWN2 && bestMove.dir != DOWN3)
            {
                ProcessUp(toProcess, grid, bestMove);
            }
            if(bestMove.dir != RIGHT3 && bestMove.dir != LEFT1 && bestMove.dir != LEFT2 && bestMove.dir != LEFT3)
            {
                ProcessRight(toProcess, grid, bestMove);
            }
            if(bestMove.dir != DOWN3 && bestMove.dir != UP1 && bestMove.dir != UP2 && bestMove.dir != UP3)
            {
                ProcessDown(toProcess, grid, bestMove);
            }
            if(bestMove.dir != LEFT3 && bestMove.dir != RIGHT1 && bestMove.dir != RIGHT2 && bestMove.dir != RIGHT3)
            {
                ProcessLeft(toProcess, grid, bestMove);
            }
        }

        Block bottomRight = grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1];
        Console.WriteLine(bottomRight.ComputeTotalMinCost());

        PrintPath(grid, bottomRight.pathTo[6]);
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

    private void ProcessUp(List<(int y, int x, int dir, int cost)> toProcess, 
                            Block[,] grid, 
                            (int y, int x, int dir, int cost) prevMove)
    {
        (int y, int x) newPosition = (prevMove.y-1, prevMove.x);
        if(newPosition.y < 0) return;

        Block fromBlock = grid[prevMove.y, prevMove.x];
        Block toBlock = grid[newPosition.y, newPosition.x];

        int newCost = prevMove.cost + toBlock.cost;

        int dir = UP1;
        if(prevMove.dir == UP1)
        {
            dir = UP2;
        }
        if(prevMove.dir == UP2)
        {
            dir = UP3;
        }
        
        if(newCost < toBlock.minCosts[dir])
        {
            string prevPath = "";
            if(prevMove.dir >= 0) prevPath = fromBlock.pathTo[prevMove.dir];
            toBlock.pathTo[dir] = prevPath + '^';
            toBlock.minCosts[dir] = newCost;
            toProcess.Add((newPosition.y, newPosition.x, dir, newCost));
        }
    }

    private void ProcessRight(List<(int y, int x, int dir, int cost)> toProcess, 
                            Block[,] grid, 
                            (int y, int x, int dir, int cost) prevMove)
    {
        (int y, int x) newPosition = (prevMove.y, prevMove.x+1);
        if(newPosition.x >= grid.GetLength(1)) return;

        Block fromBlock = grid[prevMove.y, prevMove.x];
        Block toBlock = grid[newPosition.y, newPosition.x];

        int newCost = prevMove.cost + toBlock.cost;

        int dir = RIGHT1;
        if(prevMove.dir == RIGHT1)
        {
            dir = RIGHT2;
        }
        if(prevMove.dir == RIGHT2)
        {
            dir = RIGHT3;
        }
        
        if(newCost < toBlock.minCosts[dir])
        {
            string prevPath = "";
            if(prevMove.dir >= 0) prevPath = fromBlock.pathTo[prevMove.dir];
            toBlock.pathTo[dir] = prevPath + '>';
            toBlock.minCosts[dir] = newCost;
            toProcess.Add((newPosition.y, newPosition.x, dir, newCost));
        }
    }

    private void ProcessDown(List<(int y, int x, int dir, int cost)> toProcess, 
                            Block[,] grid, 
                            (int y, int x, int dir, int cost) prevMove)
    {
        (int y, int x) newPosition = (prevMove.y+1, prevMove.x);
        if(newPosition.y >= grid.GetLength(0)) return;

        Block fromBlock = grid[prevMove.y, prevMove.x];
        Block toBlock = grid[newPosition.y, newPosition.x];

        int newCost = prevMove.cost + toBlock.cost;

        int dir = DOWN1;
        if(prevMove.dir == DOWN1)
        {
            dir = DOWN2;
        }
        if(prevMove.dir == DOWN2)
        {
            dir = DOWN3;
        }
        
        if(newCost < toBlock.minCosts[dir])
        {
            string prevPath = "";
            if(prevMove.dir >= 0) prevPath = fromBlock.pathTo[prevMove.dir];
            toBlock.pathTo[dir] = prevPath + 'v';
            toBlock.minCosts[dir] = newCost;
            toProcess.Add((newPosition.y, newPosition.x, dir, newCost));
        }
    }

    private void ProcessLeft(List<(int y, int x, int dir, int cost)> toProcess, 
                            Block[,] grid, 
                            (int y, int x, int dir, int cost) prevMove)
    {
        (int y, int x) newPosition = (prevMove.y, prevMove.x-1);
        if(newPosition.x < 0) return;

        Block fromBlock = grid[prevMove.y, prevMove.x];
        Block toBlock = grid[newPosition.y, newPosition.x];

        int newCost = prevMove.cost + toBlock.cost;

        int dir = LEFT1;
        if(prevMove.dir == LEFT1)
        {
            dir = LEFT2;
        }
        if(prevMove.dir == LEFT2)
        {
            dir = LEFT3;
        }
        
        if(newCost < toBlock.minCosts[dir])
        {
            string prevPath = "";
            if(prevMove.dir >= 0) prevPath = fromBlock.pathTo[prevMove.dir];
            toBlock.pathTo[dir] = prevPath + '<';
            toBlock.minCosts[dir] = newCost;
            toProcess.Add((newPosition.y, newPosition.x, dir, newCost));
        }
    }
}
