
namespace AdventOfCode2023;


public class Day5Part2
{
    private class Almanac
    {
        public List<Seed> seeds = new List<Seed>();
        public List<Map> maps = new List<Map>();
    }

    private class Map
    {
        public string name;
        public List<Converter> converters = new List<Converter>();

        public void Initialize()
        {
            converters.Sort((x, y) => x.sourceStart.CompareTo(y.sourceStart));
        }

        public List<Seed> ConvertSeed(Seed seed)
        {
            List<Seed> result = new List<Seed>();

            foreach(Converter converter in converters)
            {
                //no more converters will affect the seed
                if(seed.end < converter.sourceStart) break;

                //converter is smaller than seed
                if(converter.sourceEnd < seed.start) continue;

                //break off first part of seed before converter
                if(seed.start < converter.sourceStart)
                {
                    Seed newSeed = Seed.Create(seed.start, converter.sourceStart - 1);
                    result.Add(newSeed);

                    seed = Seed.Create(converter.sourceStart, seed.end);
                }

                if(seed.end > converter.sourceEnd) //overlapping the end
                {
                    Seed newSeed = Seed.Create(seed.start, converter.sourceEnd);
                    newSeed.start += converter.offset;
                    newSeed.end += converter.offset;
                    result.Add(newSeed);

                    seed = Seed.Create(converter.sourceEnd + 1, seed.end);  
                }
                else //completly contained
                {
                    Seed newSeed = Seed.Create(seed.start, seed.end);
                    newSeed.start += converter.offset;
                    newSeed.end += converter.offset;
                    result.Add(newSeed);

                    //no more seed to process
                    seed = null;
                    break; 
                }
            }

            if(seed != null)
            {
                //what's left of the seed is outside any converters
                Seed newSeed = Seed.Create(seed.start, seed.end);
                result.Add(newSeed);
            }

            return result;
        }
    }

    private class Converter
    {
        public long destinationStart;
        public long sourceStart;
        public long sourceEnd;
        public long length;
        public long offset;

        public Converter(long destinationStart, long sourceStart, long length)
        {
            this.destinationStart = destinationStart;
            this.sourceStart = sourceStart;
            sourceEnd = sourceStart + length - 1;
            this.length = length;
            offset = destinationStart - sourceStart;
        }       
    }

    private class Seed
    {
        public long start;
        public long end;

        public Seed(long start, long length)
        {
            this.start = start;
            end = start + length - 1;
        }

        public static Seed Create(long start, long end)
        {
            return new Seed(start, end - start + 1);
        }
    }

    private Almanac Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day5.txt");
        //string[] lines = File.ReadAllLines("inputs/Day5Sample.txt");

        Almanac almanac = new Almanac();

        string[] seedStrings = lines[0].Split(' ');
        for(int i = 1; i < seedStrings.Length; i+=2)
        {
            Seed seed = new Seed(long.Parse(seedStrings[i]), long.Parse(seedStrings[i+1]));
            almanac.seeds.Add(seed);
        }

        Map currMap = new Map();
        for(int i = 1; i < lines.Length; i++)
        {
            string line = lines[i];

            if(line == "") continue;

            if(char.IsAsciiLetter(line[0]))
            {
                currMap.Initialize();

                currMap = new Map();
                currMap.name = line;
                almanac.maps.Add(currMap);
            }
            else
            {
                string[] stringVals = line.Split(' ');
                Converter converter = new Converter(long.Parse(stringVals[0]), long.Parse(stringVals[1]), long.Parse(stringVals[2]));

                currMap.converters.Add(converter);
            }
        }
        currMap.Initialize();

        return almanac;
    }

    public void Solve()
    {
        Almanac almanac = Parse();

        List<Seed> seeds = new List<Seed>(almanac.seeds);

        foreach(Map map in almanac.maps)
        {
            List<Seed> newSeeds = new List<Seed>();
            foreach(Seed seed in seeds)
            {
                newSeeds.AddRange(map.ConvertSeed(seed));
            }
            seeds = newSeeds;
        }

        long min = long.MaxValue;

        foreach(Seed s in seeds)
        {
            min = Math.Min(s.start, min);
        }

        Console.WriteLine(min);
    }
}
