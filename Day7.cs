
namespace AdventOfCode2023;


public class Day7
{
    private class Hand : IComparable
    {
        public int[] cards = new int[5];
        public string strCards;
        public int bid;

        public HandType handType;

        public enum HandType
        {
            FIVE_OF_A_KIND,
            FOUR_OF_A_KIND,
            FULL_HOUSE,
            THREE_OF_A_KIND,
            TWO_PAIR,
            ONE_PAIR,
            HIGH_CARD,
        }

        public void Init()
        {
            int[] counts = new int[15];
            foreach(int card in cards)
            {
                counts[card] ++;
            }

            if(counts.Contains(5)) handType = HandType.FIVE_OF_A_KIND;
            else if(counts.Contains(4)) handType = HandType.FOUR_OF_A_KIND;
            else if(counts.Contains(3) && counts.Contains(2)) handType = HandType.FULL_HOUSE;
            else if(counts.Contains(3)) handType = HandType.THREE_OF_A_KIND;
            else if(Array.FindAll(counts, count => count == 2).Length == 2) handType = HandType.TWO_PAIR;
            else if(counts.Contains(2)) handType = HandType.ONE_PAIR;
            else handType = HandType.HIGH_CARD;
        }

        public int CompareTo(object? obj)
        {
            Hand that = (Hand) obj;

            if(this.handType == that.handType)
            {
                if(this.cards[0] == that.cards[0])
                {
                    if(this.cards[1] == that.cards[1])
                    {
                        if(this.cards[2] == that.cards[2])
                        {
                            if(this.cards[3] == that.cards[3])
                            {
                                return that.cards[4] - this.cards[4];
                            }
                            else
                            {
                                return that.cards[3] - this.cards[3];
                            }
                        }
                        else
                        {
                            return that.cards[2] - this.cards[2];
                        }
                    }
                    else
                    {
                        return that.cards[1] - this.cards[1];
                    }
                }
                else
                {
                    return that.cards[0] - this.cards[0];
                }
            }
            else
            {
                return this.handType.CompareTo(that.handType);
            }
        }
    }

    private class Game
    {
        public List<Hand> hands;

        public void Init()
        {
            foreach(Hand hand in hands) hand.Init();
            hands.Sort();
            hands.Reverse();
        }
    }

    private List<Hand> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day7.txt");
        //string[] lines = File.ReadAllLines("inputs/Day7Sample.txt");
        
        List<Hand> hands = new List<Hand>();

        foreach(string line in lines)
        {
            string[] parts = line.Split(' ');
            Hand hand = new Hand();

            hand.bid = int.Parse(parts[1]);
            hand.strCards = parts[0];

            for(int i = 0; i < 5; i++)
            {
                char card = parts[0][i];
                switch(card)
                {
                    case > '0' and <= '9':
                        hand.cards[i] = card - '0';
                        break;
                    case 'T':
                        hand.cards[i] = 10;
                        break;
                    case 'J':
                        hand.cards[i] = 11;
                        break;
                    case 'Q':
                        hand.cards[i] = 12;
                        break;
                    case 'K':
                        hand.cards[i] = 13;
                        break;
                    case 'A':
                        hand.cards[i] = 14;
                        break;
                }
            }

            hands.Add(hand);
        }

        return hands;
    }

    public void Solve()
    {
        Game game = new Game();
        game.hands = Parse();
        game.Init();

        int total = 0;
        for(int i = 0; i < game.hands.Count; i++)
        {
            Hand hand = game.hands[i];
            total += hand.bid * (i+1);
        }

        Console.WriteLine(total);
    }
}
