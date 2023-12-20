namespace AdventOfCode2023;


public class Day19Part2
{
    class RuleNode
    {
        public string workflowName;
        public string rulePath;

        public (int max, int min) xBounds = (4001, 0);
        public (int max, int min) mBounds = (4001, 0);
        public (int max, int min) aBounds = (4001, 0);
        public (int max, int min) sBounds = (4001, 0);

        public List<RuleNode> children = new();

        public void CopyBoundsFrom(RuleNode other)
        {
            xBounds = (other.xBounds.max, other.xBounds.min);
            mBounds = (other.mBounds.max, other.mBounds.min);
            aBounds = (other.aBounds.max, other.aBounds.min);
            sBounds = (other.sBounds.max, other.sBounds.min);
        }
    }

    class Rule
    {
        public char category;
        public bool greater;
        public int limit;
        public string destination;

        public Rule(char category, bool greater, int limit, string destination)
        {
            this.category = category;
            this.greater = greater;
            this.limit = limit;
            this.destination = destination;
        }
    }

    private Dictionary<string, List<Rule>> Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day19.txt");
        //string[] lines = File.ReadAllLines("inputs/Day19Sample.txt");

        Dictionary<string, List<Rule>> workflows = new();
        foreach(string line in lines)
        {
            if(line == "") break; //ignore parts

            string name = line.Substring(0, line.IndexOf('{'));
            string rules = line.Substring(line.IndexOf('{') + 1, line.Length - name.Length - 2);
            string[] rulesList = rules.Split(',');

            List<Rule> workflowRules = new();
            foreach(string ruleString in rulesList)
            {
                workflowRules.Add(ParseRule(ruleString));
            }

            workflows.Add(name, workflowRules);
        }

        return workflows;
    }

    private Rule ParseRule(string ruleString)
    {
        string[] ruleParts = ruleString.Split(':');
        string compare;
        string destination;
        if(ruleParts.Length == 2)
        {
            compare = ruleParts[0];
            destination = ruleParts[1];
        }
        else
        {
            compare = "x>-1";//fake all inclusive rule
            destination = ruleParts[0];
        }

        if(compare.Contains('>'))
        {
            string[] compareParts = compare.Split('>');
            return new Rule(compareParts[0][0], true, int.Parse(compareParts[1]), destination);
        }
        else
        {
            string[] compareParts = compare.Split('<');
            return new Rule(compareParts[0][0], false, int.Parse(compareParts[1]), destination);
        }
    }

    public void Solve()
    {
        var workflows = Parse();

        List<RuleNode> leaves = new();
        List<(string name, RuleNode parent)> toProcess = new();

        RuleNode root = new RuleNode();

        toProcess.Add(("in", root));

        while(toProcess.Count() > 0)
        {
            var curr = toProcess[0];
            toProcess.RemoveAt(0);

            //create node out of each rule
            var currRules = workflows[curr.name];
            List<Rule> negativeRules = new List<Rule>();

            foreach(Rule rule in currRules)
            {
                ProcessRule(leaves, toProcess, curr.name, curr.parent, rule, negativeRules);
                negativeRules.Add(rule);
            }

        }

        long sum = 0;
        foreach(var node in leaves)
        {
            if(node.workflowName == "R") continue;

            long xRange = node.xBounds.max - node.xBounds.min - 1;
            long mRange = node.mBounds.max - node.mBounds.min - 1;
            long aRange = node.aBounds.max - node.aBounds.min - 1;
            long sRange = node.sBounds.max - node.sBounds.min - 1;

            sum += xRange * mRange * aRange * sRange;
        }

        Console.WriteLine(sum);
    }

    private void ProcessRule(List<RuleNode> leaves, List<(string name, RuleNode parent)> toProcess, string workflowName, RuleNode parent, Rule rule, List<Rule> negativeRules)
    {
        RuleNode node = new();
        node.workflowName = workflowName;
        node.rulePath = parent.rulePath + " -> " + node.workflowName;
        node.CopyBoundsFrom(parent);

        UpdateBounds(node, rule.category, rule.greater, rule.limit);
        foreach(Rule negativeRule in negativeRules)
        {
            int adjustment = -1;
            if(negativeRule.greater) adjustment *= -1;
            UpdateBounds(node, negativeRule.category, !negativeRule.greater, negativeRule.limit + adjustment);
        }

        parent.children.Add(node);

        if(rule.destination == "A" || rule.destination == "R")
        {
            RuleNode leaf = new RuleNode();
            leaf.workflowName = rule.destination;
            leaf.rulePath = node.rulePath + " -> " + leaf.workflowName;
            leaf.CopyBoundsFrom(node);
            leaves.Add(leaf);
        }
        else
        {
            toProcess.Add((rule.destination, node));
        }
    }

    private void UpdateBounds(RuleNode node, char category, bool greater, int limit)
    {
        switch(category)
        {
            case 'x':
                if(greater)
                {
                    node.xBounds.min = Math.Max(node.xBounds.min, limit);
                }
                else
                {
                    node.xBounds.max = Math.Min(node.xBounds.max, limit);
                }
                break;
            case 'm':
                if(greater)
                {
                    node.mBounds.min = Math.Max(node.mBounds.min, limit);
                }
                else
                {
                    node.mBounds.max = Math.Min(node.mBounds.max, limit);
                }
                break;
            case 'a':
                if(greater)
                {
                    node.aBounds.min = Math.Max(node.aBounds.min, limit);
                }
                else
                {
                    node.aBounds.max = Math.Min(node.aBounds.max, limit);
                }
                break;
            case 's':
                if(greater)
                {
                    node.sBounds.min = Math.Max(node.sBounds.min, limit);
                }
                else
                {
                    node.sBounds.max = Math.Min(node.sBounds.max, limit);
                }
                break;
        }
    }
}
