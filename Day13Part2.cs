
using System.Text;

namespace AdventOfCode2023;


public class Day13Part2
{

    private class Pattern
    {
        public List<string> rows = new();
        public List<string> cols;

        public bool rowMirror = false;

        public int mirrorIndex = -1;

        public void Print()
        {
            foreach(string r in rows)
            {
                Console.WriteLine(r);
            }
            Console.WriteLine();
            foreach(string c in cols)
            {
                Console.WriteLine(c);
            }
            Console.WriteLine();
        }

        public void FixSmudge(int y, int x)
        {
            char newChar = rows[y][x] == '.' ? '#' : '.';
            
            StringBuilder sb = new StringBuilder(rows[y]);
            sb[x] = newChar;
            rows[y] = sb.ToString();

            sb = new StringBuilder(cols[x]);
            sb[y] = newChar;
            cols[x] = sb.ToString();
        }

        public bool HasMirror()
        {
            return _ComputeMirror().mirrorIndex >= 0;
        }

        public void ComputeMirror()
        {
            var result = _ComputeMirror();
            rowMirror = result.rowMirror;
            mirrorIndex = result.mirrorIndex;
        }

        private (bool rowMirror, int mirrorIndex) _ComputeMirror()
        {
            int index = CheckMirror(rows);
            if(index >= 0 && (!rowMirror || mirrorIndex != index))
            {
                return (true, index);
            }

            index = CheckMirror(cols);
            if(index >= 0 && (rowMirror || mirrorIndex != index))
            {
                return (false, index);
            }

            rows.Reverse();
            index = CheckMirror(rows);
            rows.Reverse();
            if(index >= 0 && (!rowMirror || mirrorIndex != rows.Count - index))
            {
                return (true, rows.Count - index);
            }

            cols.Reverse();
            index = CheckMirror(cols);
            cols.Reverse();
            if(index >= 0 && (rowMirror || mirrorIndex != cols.Count - index))
            {
                return (false, cols.Count - index);
            }

            return (false, -1);
        }

        private int CheckMirror(List<string> lines)
        {   
            string lastRow = lines[lines.Count - 1];
            for(int i = 1; i < lines.Count; i+=2)
            {
                if(lines[i] == lines[0])
                {
                    if(IsMirror(lines, i))
                    {
                        return (i+1)/2;
                    }
                }
            }

            return -1;
        }

        private bool IsMirror(List<string> lines, int startIndex)
        {
            int check = startIndex / 2;
            for(int i = 0; i <= check; i++)
            {
                if(lines[i] != lines[startIndex - i]) return false;
            }

            return true;
        }
    }

    private List<Pattern> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day13.txt");
        //string[] lines = File.ReadAllLines("inputs/Day13Sample.txt");

        List<Pattern> patterns = new();
        Pattern pattern = new();
        patterns.Add(pattern);
        foreach(string line in lines)
        {
            if(line == "")
            {
                pattern.cols = Transpose(pattern.rows);
                pattern = new();
                patterns.Add(pattern);
                continue;
            }

            pattern.rows.Add(line);
        }
        pattern.cols = Transpose(pattern.rows);

        return patterns;
    }

    public void Solve()
    {
        var patterns = Parse();

        int sum = 0;
        foreach(var pattern in patterns)
        {
            bool mirrorFound = false;
            pattern.ComputeMirror();
            for(int y = 0; y < pattern.rows.Count && !mirrorFound; y++)
            {
                for(int x = 0; x < pattern.rows[0].Length && !mirrorFound; x++)
                {
                    pattern.FixSmudge(y, x);

                    if(pattern.HasMirror())
                    {
                        mirrorFound = true;

                        pattern.ComputeMirror();

                        if(pattern.rowMirror)
                        {
                            sum += 100 * pattern.mirrorIndex;
                        }
                        else
                        {
                            sum += pattern.mirrorIndex;
                        }
                    }

                    pattern.FixSmudge(y, x);
                }
            }
        }

        Console.WriteLine(sum);
    }

    private static List<string> Transpose(List<string> rows)
    {
        List<string> result = new List<string>();
        for(int i = 0; i < rows[0].Length; i++) result.Add("");

        for(int y = 0; y < rows.Count; y++)
        {
            for(int x = 0; x < rows[y].Length; x++)
            {
                result[x] += rows[y][x];
            }
        }

        return result;
    }
}
