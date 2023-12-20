namespace AdventOfCode2023;


public class Day20
{
    private class Module
    {
        public string name;
        public char type; //b = brodcast, % = flip flop, & = conjunction
        public List<string> outputs = new();

        //flip flop only
        public bool on;

        //conjunction only
        public Dictionary<string, bool> mem = new();

        public void Initialize(Dictionary<string, Module> modules)
        {
            foreach(string output in outputs)
            {
                if(!modules.ContainsKey(output)) continue;

                var receiver = modules[output];
                if(receiver.type == '&')
                {
                    receiver.mem[name] = false;
                }
            }
        }

        public (int high, int low) HandlePulse(List<(bool pulse, string source, string destination)> toProcess, (int high, int low) pulseCounts, string source, bool pulse)
        {
            switch(type)
            {
                case 'b':
                    foreach(var output in outputs)
                    {
                        if(pulse) pulseCounts.high ++;
                        else pulseCounts.low ++;

                        toProcess.Add((pulse, name, output));
                    }
                    break;
                case '%':
                    if(pulse) break; //high pulse is ignored
                    on = !on; //switch on/off state

                    foreach(var output in outputs)
                    {
                        if(on) pulseCounts.high ++;
                        else pulseCounts.low ++;

                        toProcess.Add((on, name, output));
                    }
                    break;
                case '&':
                    mem[source] = pulse;
                    bool toSend = true;
                    foreach(bool inMem in mem.Values) toSend &= inMem;
                    toSend = !toSend;
                    
                    foreach(var output in outputs)
                    {
                        if(toSend) pulseCounts.high ++;
                        else pulseCounts.low ++;

                        toProcess.Add((toSend, name, output));
                    }
                    break;
            }

            return pulseCounts;
        }
    }

    private Dictionary<string, Module> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day20.txt");
        //string[] lines = File.ReadAllLines("inputs/Day20Sample.txt");
        //string[] lines = File.ReadAllLines("inputs/Day20Sample2.txt");

        Dictionary<string, Module> modules = new();

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

            modules.Add(module.name, module);
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
        (int high, int low) pulseCounts = (0, 0);


        for(int i = 0; i < 1000; i++)
        {
            List<(bool pulse, string source, string destination)> toProcess = new();
            toProcess.Add((false, "button", "broadcaster"));
            pulseCounts.low++;

            while(toProcess.Count != 0)
            {
                var next = toProcess[0];
                toProcess.RemoveAt(0);

                if(!modules.ContainsKey(next.destination)) continue;

                Module module = modules[next.destination];
                pulseCounts = module.HandlePulse(toProcess, pulseCounts, next.source, next.pulse);
            }
        }

        Console.WriteLine(pulseCounts.high * pulseCounts.low);
    }
}
