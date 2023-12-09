
namespace AdventOfCode2023;


public class Day5
{
    private class Almanac
    {
        public List<long> seeds = new List<long>();
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

        public long ConvertSeed(long seed)
        {
            foreach(Converter converter in converters)
            {
                if(converter.Contains(seed))
                {
                    return converter.Convert(seed);
                }
            }

            return seed;
        }
    }

    private class Converter
    {
        public long destinationStart;
        public long sourceStart;
        public long length;
        public long offset;

        public Converter(long destinationStart, long sourceStart, long length)
        {
            this.destinationStart = destinationStart;
            this.sourceStart = sourceStart;
            this.length = length;
            offset = destinationStart - sourceStart;
        }

        public bool Contains(long seed)
        {
            return sourceStart <= seed && seed <= sourceStart + length;
        }

        public long Convert(long seed)
        {
            return seed + offset;
        }
    }

    private Almanac Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day5.txt");
        //string[] lines = File.ReadAllLines("inputs/Day5Sample.txt");

        Almanac almanac = new Almanac();

        string[] seedStrings = lines[0].Split(' ');
        for(int i = 1; i < seedStrings.Length; i++)
        {
            almanac.seeds.Add(long.Parse(seedStrings[i]));
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

        return almanac;
    }

    public void Solve()
    {
        Almanac almanac = Parse();

        List<long> seeds = new List<long>(almanac.seeds);

        foreach(Map map in almanac.maps)
        {
            for(int i = 0; i < seeds.Count; i++)
            {
                seeds[i] = map.ConvertSeed(seeds[i]);
            }
        }

        Console.WriteLine(seeds.Min());
    }
}
