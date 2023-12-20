namespace AdventOfCode2023;


public class Day20Part2
{
    private class Module
    {
        public string name;
        public char type; //b = brodcast, % = flip flop, & = conjunction
        public long lowCount;
        public long highCount;
        public List<string> outputs = new();
        public List<string> inputs = new();

        //flip flop only
        public bool on;

        //conjunction only
        public Dictionary<string, bool> mem = new();
        public Dictionary<string, long> lowCounts = new();
        public Dictionary<string, long> highCounts = new();

        public void Initialize(Dictionary<string, Module> modules)
        {
            foreach(string output in outputs)
            {
                var receiver = modules[output];

                receiver.inputs.Add(name);

                if(receiver.type == '&')
                {
                    receiver.mem[name] = false;
                    receiver.lowCounts[name] = 0;
                    receiver.highCounts[name] = 0;
                }
            }
        }

        public void HandlePulse(long buttonCount, List<(bool pulse, string source, string destination)> toProcess, string source, bool pulse)
        {
            if(pulse) highCount ++;
            else lowCount ++;

            switch(type)
            {
                case '#':
                    //dead end, no op
                    break;
                case 'b':
                    foreach(var output in outputs)
                    {
                        toProcess.Add((pulse, name, output));
                    }
                    break;
                case '%':
                    if(pulse) break; //high pulse is ignored
                    on = !on; //switch on/off state

                    foreach(var output in outputs)
                    {
                        toProcess.Add((on, name, output));
                    }
                    break;
                case '&':
                    mem[source] = pulse;
                    
                    if(pulse) highCounts[source] += 1;
                    else lowCounts[source] += 1;

                    bool toSend = true;
                    foreach(bool inMem in mem.Values) toSend &= inMem;
                    toSend = !toSend;
                    
                    foreach(var output in outputs)
                    {
                        toProcess.Add((toSend, name, output));
                    }
                    break;
            }

            //gt, nl, vr, lr
            //change this to get high pulses to the inputs of jq
            if(name == "jq" && source == "lr" && pulse)
            {
                Console.WriteLine(buttonCount + "\t" + source + "\t" + pulse + "\t" + highCounts[source] + "\t" + (lowCounts[source] + highCounts[source]));
            }
        }

        public void Print()
        {
            string result = name + "(" + (lowCount + highCount) + ", " + highCount + ")";

            if(type == '%')
            {
                result += ":" + on;
            }
            else
            {
                result += ":[";

                foreach(var kvp in mem)
                {
                    result += kvp.Key + "(" + lowCounts[kvp.Key] + highCounts[kvp.Key] + ", " + highCounts[kvp.Key] + ")" + ":" + kvp.Value + ", ";
                }

                result += "]";
            }

            Console.WriteLine(result);
        }

        public void PrintDeepReverse(List<string> seen, Dictionary<string, Module> modules, string prefix)
        {
            if(seen.Contains(name)) return;
            seen.Add(name);
            Console.WriteLine(prefix + type + name);

            foreach(string input in inputs)
            {
                if(!modules.ContainsKey(input)) continue;

                modules[input].PrintDeepReverse(seen, modules, prefix + "  ");
            }
        }
    }

    private Dictionary<string, Module> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day20.txt");
        //string[] lines = File.ReadAllLines("inputs/Day20Sample.txt");
        //string[] lines = File.ReadAllLines("inputs/Day20Sample2.txt");

        Dictionary<string, Module> modules = new();

        List<string> allOutputs = new();

        foreach(string line in lines)
        {
            string[] parts = line.Split(" -> ");

            Module module = new();
            if(parts[0] == "broadcaster")
            {
                module.name = parts[0];
                module.type = 'b';
            }
            else if(parts[0][0] == '%')
            {
                module.name = parts[0].Substring(1);
                module.type = '%';
            }
            else //if(parts[0][0] == '&')
            {
                module.name = parts[0].Substring(1);
                module.type = '&';
            }

            string[] outputs = parts[1].Split(", ");
            module.outputs.AddRange(outputs);
            allOutputs.AddRange(outputs);

            modules.Add(module.name, module);
        }

        foreach(string output in allOutputs)
        {
            if(!modules.ContainsKey(output))
            {
                Module newModule = new Module();
                newModule.name = output;
                newModule.type = '#';
                modules.Add(output, newModule);
            }
        }

        foreach(Module module in modules.Values)
        {
            module.Initialize(modules);
        }

        return modules;
    }

    public void Solve()
    {
        var modules = Parse();

        for(int i = 1; i <= 100000; i++) 
        {
            PushButton(i, modules);
        }
    }

    private void PushButton(long pushCount, Dictionary<string, Module> modules)
    {
        List<(bool pulse, string source, string destination)> toProcess = new();
        toProcess.Add((false, "button", "broadcaster"));
        
        while(toProcess.Count != 0)
        {
            var next = toProcess[0];
            toProcess.RemoveAt(0);

            Module module = modules[next.destination];
            module.HandlePulse(pushCount, toProcess, next.source, next.pulse);
        }
    }
}
