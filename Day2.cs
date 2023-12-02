namespace AdventOfCode2023;



public class Day2
{
    public class Game
    {
        public int id;
        public List<CubeSet> draws = new List<CubeSet>();
    }

    public class CubeSet
    {
        public int reds;
        public int greens;
        public int blues;
    }

    public List<Game> Parse()
    {
        List<Game> games = new List<Game>();

        string[] lines = File.ReadAllLines("inputs/Day2.txt");

        foreach(string line in lines)
        {
            string[] lineParts = line.Split(":");
            Game game = new Game();
            
            game.id = int.Parse(lineParts[0].Split(" ")[1]);

            string[] lineDraws = lineParts[1].Split(";");
            foreach(string lineDraw in lineDraws)
            {
                string[] lineColors = lineDraw.Split(",");

                CubeSet set = new CubeSet();

                foreach(string lineColor in lineColors)
                {
                    string[] colorParts = lineColor.Trim().Split(" ");

                    if(colorParts[1] == "red") set.reds = int.Parse(colorParts[0]);
                    if(colorParts[1] == "blue") set.blues = int.Parse(colorParts[0]);
                    if(colorParts[1] == "green") set.greens = int.Parse(colorParts[0]);
                }

                game.draws.Add(set);
            }

            games.Add(game);
        }

        return games;
    }

    public void Solve()
    {
        List<Game> games = Parse();

        int sum = 0;

        foreach(Game game in games)
        {
            bool passed = true;
            foreach(CubeSet draw in game.draws)
            {
                if(draw.reds > 12 || draw.greens > 13 || draw.blues > 14) 
                {
                    passed = false;
                    break;
                }
            }
            if(passed) sum += game.id;
        }

        Console.WriteLine(sum);
    }
}
