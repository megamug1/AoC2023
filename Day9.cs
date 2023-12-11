
namespace AdventOfCode2023;


public class Day9
{

    private List<List<long>> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day9.txt");
        //string[] lines = File.ReadAllLines("inputs/Day9Sample.txt");

        List<List<long>> result = new List<List<long>>();

        foreach(string line in lines)
        {
            string[] parts = line.Split(' ');
            List<long> data = new List<long>();

            foreach(string part in parts)
            {
                data.Add(long.Parse(part));
            }

            result.Add(data);
        }

        return result;
    }

    public void Solve()
    {
        List<List<long>> data = Parse();

        long sum = 0;

        foreach(List<long> curr in data)
        {
            long result = SolveList(curr);
            sum += result + curr[curr.Count - 1];
        }

        Console.WriteLine(sum);
    }

    private long SolveList(List<long> data)
    {
        List<long> nextData = new List<long>();

        bool hasNon0 = false;

        for(int i = 1; i < data.Count; i++)
        {
            long diff = data[i] - data[i-1];
            if(diff != 0) hasNon0 = true;

            nextData.Add(diff);
        }

        if(!hasNon0) return 0;

        long nextVal = SolveList(nextData);

        return nextData[nextData.Count - 1] + nextVal;
    }
}
