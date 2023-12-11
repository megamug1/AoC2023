
using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode2023;


public class Day10Part2
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
        public bool isLoop;
        public bool isExterior;

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
            lines = File.ReadAllLines("inputs/Day10Sample3.txt");
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
        MarkLoop(nodes);

        bool[,] visited = new bool[nodes.GetLength(0) + 1, nodes.GetLength(1) + 1];
        MarkExterior(nodes, visited, (0, 0));

        for(int y = 0; y < nodes.GetLength(0); y++)
        {
            for(int x = 0; x < nodes.GetLength(1); x++)
            {
                if(nodes[y, x].isStart)
                {
                    Console.Write('S');
                }
                else if(nodes[y, x].isLoop)
                {
                    Console.Write('.');
                }
                else if(nodes[y, x].isExterior)
                {
                    Console.Write('X');
                }
                else
                {
                    Console.Write('O');
                }
            }
            Console.WriteLine();
        }

        int count = 0;
        for(int y = 0; y < nodes.GetLength(0); y++)
        {
            for(int x = 0; x < nodes.GetLength(1); x++)
            {
                if(!nodes[y, x].isExterior) count++;
            }
        }

        Console.WriteLine(count);
    }

    private void MarkLoop(Node[,] nodes)
    {
        (int y, int x) curr = start;
        Dir currDir = startDir;

        do
        {
            Node currNode = nodes[curr.y, curr.x];

            currNode.isLoop = true;
            currNode.isExterior = true;

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

        } while(curr != start);
    }

    private void MarkExterior(Node[,] nodes, bool[,] visited, (int y, int x) pos)
    {
        if(visited[pos.y, pos.x]) return;

        visited[pos.y, pos.x] = true;

        //mark nodes
        if(pos.y > 0 && pos.x > 0)
        {
            //top left
            nodes[pos.y - 1, pos.x - 1].isExterior = true;
        }
        if(pos.y > 0 && pos.x < nodes.GetLength(1))
        {
            //top right
            nodes[pos.y - 1, pos.x].isExterior = true;
        }
        if(pos.y < nodes.GetLength(0) && pos.x > 0)
        {
            //bot left
            nodes[pos.y, pos.x - 1].isExterior = true;
        }
        if(pos.y < nodes.GetLength(0) && pos.x < nodes.GetLength(1))
        {
            //bot right
            nodes[pos.y, pos.x].isExterior = true;
        }

        //visit next
        if(CanCrossUp(nodes, pos))
        {
            MarkExterior(nodes, visited, (pos.y-1, pos.x));
        }

        if(CanCrossDown(nodes, pos))
        {
            MarkExterior(nodes, visited, (pos.y+1, pos.x));
        }

        if(CanCrossLeft(nodes, pos))
        {
            MarkExterior(nodes, visited, (pos.y, pos.x-1));
        }

        if(CanCrossRight(nodes, pos))
        {
            MarkExterior(nodes, visited, (pos.y, pos.x+1));
        }
    }

    private bool CanCrossLeft(Node[,] nodes, (int y, int x) pos)
    {
        if(pos.x == 0) return false;
        if(pos.y == 0) return true;
        if(pos.y == nodes.GetLength(0)) return true;

        Node top = nodes[pos.y - 1, pos.x - 1];
        Node bot = nodes[pos.y, pos.x - 1];

        if(!top.isLoop || !bot.isLoop) return true;

        switch(top.type)
        {
                case '.': return true;
                case '|': return false;
                case '-': return true;
                case 'L': return true;
                case 'J': return true;
                case 'F': return false;
                case '7': return false;
        }

        throw new Exception("shouldn't get here");
    }

    private bool CanCrossRight(Node[,] nodes, (int y, int x) pos)
    {
        if(pos.x == nodes.GetLength(1)) return false;
        if(pos.y == 0) return true;
        if(pos.y == nodes.GetLength(0)) return true;

        Node top = nodes[pos.y - 1, pos.x];
        Node bot = nodes[pos.y, pos.x];

        if(!top.isLoop || !bot.isLoop) return true;

        switch(top.type)
        {
                case '.': return true;
                case '|': return false;
                case '-': return true;
                case 'L': return true;
                case 'J': return true;
                case 'F': return false;
                case '7': return false;
        }

        throw new Exception("shouldn't get here");
    }

    private bool CanCrossUp(Node[,] nodes, (int y, int x) pos)
    {
        if(pos.y == 0) return false;
        if(pos.x == 0) return true;
        if(pos.x == nodes.GetLength(1)) return true;

        Node left = nodes[pos.y - 1, pos.x - 1];
        Node right = nodes[pos.y - 1, pos.x];

        if(!left.isLoop || !right.isLoop) return true;

        switch(left.type)
        {
                case '.': return true;
                case '|': return true;
                case '-': return false;
                case 'L': return false;
                case 'J': return true;
                case 'F': return false;
                case '7': return true;
        }

        return true;
    }

    private bool CanCrossDown(Node[,] nodes, (int y, int x) pos)
    {
        if(pos.y == nodes.GetLength(0)) return false;
        if(pos.x == 0) return true;
        if(pos.x == nodes.GetLength(1)) return true;

        Node left = nodes[pos.y, pos.x - 1];
        Node right = nodes[pos.y, pos.x];

        if(!left.isLoop || !right.isLoop) return true;

        switch(left.type)
        {
                case '.': return true;
                case '|': return true;
                case '-': return false;
                case 'L': return false;
                case 'J': return true;
                case 'F': return false;
                case '7': return true;
        }

        return true;
    }
}
