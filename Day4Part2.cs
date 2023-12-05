
namespace AdventOfCode2023;


public class Day4Part2
{
    private class Card
    {
        public List<int> winners = new List<int>();
        public List<int> candidates = new List<int>();
        public int copies = 1;

        public int GetMatchCount()
        {
            int matchCount = 0;
            for(int w = 0, c = 0; w < winners.Count && c < candidates.Count;)
            {
                int winner = winners[w];
                int candidate = candidates[c];

                if(winner == candidate) matchCount ++;

                if(winner <= candidate) w ++;
                if(candidate <= winner) c ++;
            }

            return matchCount;
        }
    }

    private List<Card> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day4.txt");
        //string[] lines = File.ReadAllLines("inputs/Day4Sample.txt");

        List<Card> cards = new List<Card>();

        foreach(string line in lines)
        {
            Card card = new Card();

            string[] parts = line.Split(':', '|');

            string[] winners = parts[1].Trim().Split(' ');
            string[] candidates = parts[2].Trim().Split(' ');

            foreach(string winner in winners)
            {
                if(winner == "") continue;
                card.winners.Add(int.Parse(winner));
            }
            foreach(string candidate in candidates)
            {
                if(candidate == "") continue;
                card.candidates.Add(int.Parse(candidate));
            }

            card.winners.Sort();
            card.candidates.Sort();

            cards.Add(card);
        }

        return cards;
    }

    public void Solve()
    {
        List<Card> cards = Parse();

        int sum = 0;

        for(int c = 0; c < cards.Count; c++)
        {
            Card card = cards[c];

            sum += card.copies;

            int matches = card.GetMatchCount();

            for(int n = 1; n <= matches; n++)
            {
                cards[c+n].copies += card.copies;
            }
        }

        Console.WriteLine(sum);
    }
}
