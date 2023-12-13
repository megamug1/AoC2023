
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode2023;


public class Day12
{

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
            foreach(string damage in damages)
            {
                damageLengths.Add(int.Parse(damage));
            }

            result.Add((parts[0], damageLengths));
        }
        return result;
    }

    public void Solve()
    {
        var data = Parse();

        int sum = 0;
        foreach((string spring, List<int> damageLengths) springCondition in data)
        {
            List<string> result = CollectOptions(springCondition.spring, springCondition.damageLengths);
            sum += result.Count;
        }

        Console.WriteLine(sum);
    }

    private List<string> CollectOptions(string spring, List<int> damageLengths)
    {
        List<string> result = new List<string>();

        int currDamageLength = damageLengths[0];

        for(int i = 0; i + currDamageLength <= spring.Length;)
        {
            //determine string to work with
            string currSpring = spring.Substring(i);

            //find earliest match
            int earliestMatch = FindEarliestMatch(currSpring, currDamageLength);

            //exit if no matches left
            if(earliestMatch == -1) 
            {
                break;
            }

            if(damageLengths.Count == 1)
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
                    int replaceIndex = i + earliestMatch;
                    string damageString = "";
                    for(int d = 0; d < currDamageLength; d++) damageString += "#";
                    string newSpring = spring.Remove(replaceIndex, damageString.Length).Insert(replaceIndex, damageString);
                    result.Add(newSpring);
                }
            }
            else if(earliestMatch + currDamageLength + 1 < currSpring.Length)
            {
                //recurse on count options with substring after match
                int subIndex = earliestMatch + currDamageLength + 1;
                string subSpring = currSpring.Substring(subIndex);
                List<int> subDamageLengths = new List<int>(damageLengths);
                subDamageLengths.RemoveAt(0);

                List<string> subResult = CollectOptions(subSpring, subDamageLengths);

                int replaceIndex = i + earliestMatch;
                string damageString = "";
                for(int d = 0; d < currDamageLength; d++) damageString += "#";
                string newSpring = spring.Remove(replaceIndex, damageString.Length).Insert(replaceIndex, damageString);
                
                string prefix = newSpring.Remove(i + subIndex, subSpring.Length);
                foreach(string s in subResult)
                {
                    result.Add(prefix + s);
                }
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

        return result;
    }

    private int FindEarliestMatch(string spring, int damageLength)
    {
        for(int i = 0; i + damageLength <= spring.Length; i++)
        {
            bool matches = true;

            for(int j = 0; j < damageLength && j+i < spring.Length; j++)
            {
                if(spring[j+i] != '#' && spring[j+i] != '?')
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
