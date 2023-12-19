namespace AdventOfCode2023;


public class Day12Part2
{
    Dictionary<string, long> solved = new Dictionary<string, long>();

    private List<(string, List<int>)> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day12.txt");
        //string[] lines = File.ReadAllLines("inputs/Day12Sample.txt");

        List<(string, List<int>)> result = new List<(string, List<int>)>();
        foreach(string line in lines)
        {
            string[] parts = line.Split(' ');
            string[] damages = parts[1].Split(',');

            List<int> damageLengths = new List<int>();
            string record = "";
            for(int i = 0; i < 5; i++)
            {
                record += parts[0];
                foreach(string damage in damages)
                {
                    damageLengths.Add(int.Parse(damage));
                }

                if(i != 4)
                {
                    record += '?';
                }
            }


            result.Add((record, damageLengths));
        }
        return result;
    }

    public void Solve()
    {
        var data = Parse();

        long sum = 0;
        int i = 0;
        foreach((string spring, List<int> damageLengths) springCondition in data)
        {
            long result = CountOptions(springCondition.spring, springCondition.damageLengths);
            sum += result;
            Console.WriteLine("" + i + "\t" + result);
            i++;
        }

        Console.WriteLine(sum);
    }

    private long CountOptions(string spring, List<int> damages)
    {
        string key = spring + string.Join(',', damages);

        if(solved.ContainsKey(key)) 
        {
            return solved[key];
        }

        long result = 0;

        int currDamageLength = damages[0];
        int totalDamage = damages.Sum();
        

        for(int i = 0; i + currDamageLength <= spring.Length;)
        {
            //determine string to work with
            string currSpring = spring[i..];
            
            int spotsLeft = currSpring.Replace(".", "").Length;
            if(spotsLeft < totalDamage) break;

            //find earliest match
            int earliestMatch = FindEarliestMatch(currSpring, currDamageLength);

            //exit if no matches left
            if(earliestMatch == -1) 
            {
                break;
            }

            if(damages.Count == 1)
            {
                //no more damage to check, skip recurring
                bool laterSpring = false;
                for(int j = earliestMatch + currDamageLength; j < currSpring.Length; j++)
                {
                    if(currSpring[j] == '#') 
                    {
                        laterSpring = true;
                        break;
                    }
                }
                if(!laterSpring) 
                {
                    result += 1;
                }
            }
            else if(earliestMatch + currDamageLength + 1 < currSpring.Length)
            {
                //recurse on count options with substring after match
                int subIndex = earliestMatch + currDamageLength + 1;
                string subSpring = currSpring[subIndex..];
                List<int> subDamageLengths = new List<int>(damages);
                subDamageLengths.RemoveAt(0);

                long subResult = CountOptions(subSpring, subDamageLengths);

                result += subResult;
            }

            int newIndex = i + earliestMatch + 1;
            bool newBadSpring = false;
            for(int j = i; j < newIndex; j++)
            {
                //break if this was a '#' because we have to match them all and we would be skipping that one
                if(spring[j] == '#' ) 
                {
                    newBadSpring = true;
                    break;
                }
            }
            if(newBadSpring) 
            {
                break;
            }

            i = newIndex;
        }

        solved.Add(key, result);
        return result;
    }

    private int FindEarliestMatch(string spring, int damageLength)
    {
        for(int i = 0; i + damageLength <= spring.Length; i++)
        {
            bool matches = true;

            for(int j = 0; j < damageLength && j+i < spring.Length; j++)
            {
                char c = spring[j+i];
                if(c != '#' && c != '?')
                {
                    matches = false;
                    break;
                }
            }

            if(matches) 
            {
                //make sure the next value is not a '#'
                if(i + damageLength == spring.Length || spring[i + damageLength] != '#')
                {
                    return i;
                }
            }

            if(spring[i] == '#')
            {
                //can't move past a known damage
                break;
            }
        }

        return -1;
    }
}
