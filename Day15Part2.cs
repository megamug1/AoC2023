namespace AdventOfCode2023;


public class Day15Part2
{
    class Instruction
    {
        public string label;
        public bool dash;
        public int focalLength;
    }

    private List<Instruction> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day15.txt");
        //string[] lines = File.ReadAllLines("inputs/Day15Sample.txt");

        List<Instruction> instructions = new();

        foreach(string line in lines[0].Split(','))
        {
            if(line.Contains('-'))
            {
                Instruction inst = new();
                inst.dash = true;
                inst.label = line.Split('-')[0];
                instructions.Add(inst);
            }
            else
            {
                Instruction inst = new();
                inst.dash = false;
                inst.label = line.Split('=')[0];
                inst.focalLength = int.Parse(line.Split('=')[1]);
                instructions.Add(inst);
            }
        }

        return instructions;
    }

    public void Solve()
    {
        var instructions = Parse();

        Dictionary<string, int> focalLengths = new Dictionary<string, int>();
        List<string>[] boxes = new List<string>[256];

        for(int b = 0; b < boxes.Length; b++)
        {
            boxes[b] = new();
        }

        foreach(var instruction in instructions)
        {
            //sum += HASH(instruction);
            int box = HASH(instruction.label);

            if(instruction.dash)
            {
                boxes[box].Remove(instruction.label);
            }
            else
            {
                if(!boxes[box].Contains(instruction.label))
                {
                    boxes[box].Add(instruction.label);
                }
                focalLengths[instruction.label] = instruction.focalLength;
            }
        }

        long sum = 0;
        for(int b = 0; b < boxes.Length; b++)
        {
            for(int l = 0; l < boxes[b].Count; l++)
            {
                string label = boxes[b][l];
                sum += (b+1) * (l+1) * focalLengths[label];
            }
        }

        Console.WriteLine(sum);
    }

    private int HASH(string instruction)
    {
        int curr = 0;
        foreach(char c in instruction)
        {
            curr += (int)c;
            curr *= 17;
            curr %= 256;
        }

        return curr;
    }
}
