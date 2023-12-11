
namespace AdventOfCode2023;


public class Day10
{
    private (int, int) start = (-1, -1);
    private Dir startDir = Dir.L;
    
    private enum Dir
    {
        U, D, L, R
    }

    private class Node
    {
        public char type;
        public bool isStart;

        public bool CanEnter(Dir dir)
        {
            switch(type)
            {
                case '.': return false;
                case '|': return dir == Dir.U || dir == Dir.D;
                case '-': return dir == Dir.L || dir == Dir.R;
                case 'L': return dir == Dir.D || dir == Dir.L;
                case 'J': return dir == Dir.D || dir == Dir.R;
                case 'F': return dir == Dir.U || dir == Dir.L;
                case '7': return dir == Dir.U || dir == Dir.R;
            }

            return false;
        }
        public Dir GetLeaveDir(Dir enterDir)
        {
            switch(type)
            {
                case '.': throw new ArgumentException("not possilbe to enter that way");
                case '|': 
                    if(enterDir == Dir.U) return Dir.U;
                    if(enterDir == Dir.D) return Dir.D;
                    throw new ArgumentException("not possilbe to enter that way");
                case '-':
                    if(enterDir == Dir.L) return Dir.L;
                    if(enterDir == Dir.R) return Dir.R;
                    throw new ArgumentException("not possilbe to enter that way");
                case 'L': 
                    if(enterDir == Dir.D) return Dir.R;
                    if(enterDir == Dir.L) return Dir.U;
                    throw new ArgumentException("not possilbe to enter that way");
                case 'J': 
                    if(enterDir == Dir.D) return Dir.L;
                    if(enterDir == Dir.R) return Dir.U;
                    throw new ArgumentException("not possilbe to enter that way");
                case 'F': 
                    if(enterDir == Dir.U) return Dir.R;
                    if(enterDir == Dir.L) return Dir.D;
                    throw new ArgumentException("not possilbe to enter that way");
                case '7': 
                    if(enterDir == Dir.U) return Dir.L;
                    if(enterDir == Dir.R) return Dir.D;
                    throw new ArgumentException("not possilbe to enter that way");
            }
            throw new ArgumentException("not possilbe to enter that way");
        }
    }

    private Node[,] Parse()
    {
        bool isSample = false;

        string[] lines;
        if(!isSample)
        {
            lines = File.ReadAllLines("inputs/Day10.txt");
        }
        else
        {
            lines = File.ReadAllLines("inputs/Day10Sample.txt");
        }

        Node[,] nodes = new Node[lines.Length, lines[0].Length];

        for(int y = 0; y < nodes.GetLength(0); y++)
        {
            string line = lines[y];
            for(int x = 0; x < nodes.GetLength(1); x++)
            {
                char c = line[x];

                Node node = new Node();
                node.type = c;
                node.isStart = false;
                if(c == 'S')
                {
                    node.isStart = true;
                    if(!isSample)
                    {
                        node.type = 'J';
                        startDir = Dir.R;
                    }
                    else
                    {
                        node.type = 'F';
                        startDir = Dir.L;
                    }
                    start = (y, x);
                }
                nodes[y, x] = node;

            }
        }

        return nodes;
    }

    public void Solve()
    {
        Node[,] nodes = Parse();

        (int y, int x) curr = start;
        Dir currDir = startDir;
        int count = 0;

        do
        {
            Node currNode = nodes[curr.y, curr.x];
            currDir = currNode.GetLeaveDir(currDir);

            switch(currDir)
            {
                case Dir.U: 
                    curr = (curr.y - 1, curr.x);
                    break;
                case Dir.D: 
                    curr = (curr.y + 1, curr.x);
                    break;
                case Dir.L: 
                    curr = (curr.y, curr.x - 1);
                    break;
                case Dir.R: 
                    curr = (curr.y, curr.x + 1);
                    break;
            }

            count ++;

        } while(curr != start);
        
        Console.WriteLine(count);
        Console.WriteLine((count + 1) / 2);
    }

}
