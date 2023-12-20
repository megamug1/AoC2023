namespace AdventOfCode2023;


public class Day19
{
    class RuleNode
    {
        public string workflowName;
        public string rulePath;

        public (int max, int min) xBounds = (int.MaxValue, -1);
        public (int max, int min) mBounds = (int.MaxValue, -1);
        public (int max, int min) aBounds = (int.MaxValue, -1);
        public (int max, int min) sBounds = (int.MaxValue, -1);

        public List<RuleNode> children = new();

        public void CopyBoundsFrom(RuleNode other)
        {
            xBounds = (other.xBounds.max, other.xBounds.min);
            mBounds = (other.mBounds.max, other.mBounds.min);
            aBounds = (other.aBounds.max, other.aBounds.min);
            sBounds = (other.sBounds.max, other.sBounds.min);
        }

        //return X if does not match, otherwise workflow name if matching
        public string CheckPart(Part part)
        {
            if(part.x >= xBounds.max) return "X";
            if(part.x <= xBounds.min) return "X";
            if(part.m >= mBounds.max) return "X";
            if(part.m <= mBounds.min) return "X";
            if(part.a >= aBounds.max) return "X";
            if(part.a <= aBounds.min) return "X";
            if(part.s >= sBounds.max) return "X";
            if(part.s <= sBounds.min) return "X";

            return workflowName;
        }
    }

    class Part
    {
        public int x;
        public int m;
        public int a;
        public int s;
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

    private (Dictionary<string, List<Rule>> workflows, List<Part> parts) Parse()
    {
        string[] lines = File.ReadAllLines("inputs/Day19.txt");
        //string[] lines = File.ReadAllLines("inputs/Day19Sample.txt");

        Dictionary<string, List<Rule>> workflows = new();
        List<Part> parts = new();
        foreach(string line in lines)
        {
            if(line == "") continue;

            if(!line.StartsWith('{'))
            {
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
            else
            {
                string partString = line.Substring(1, line.Length - 2);
                string[] partParts = partString.Split(',');

                Part part = new();
                part.x = int.Parse(partParts[0].Substring(2));
                part.m = int.Parse(partParts[1].Substring(2));
                part.a = int.Parse(partParts[2].Substring(2));
                part.s = int.Parse(partParts[3].Substring(2));

                parts.Add(part);
            }
        }

        return (workflows, parts);
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
        var data = Parse();

        List<RuleNode> leaves = new();
        List<(string name, RuleNode parent)> toProcess = new();

        RuleNode root = new RuleNode();

        toProcess.Add(("in", root));

        while(toProcess.Count() > 0)
        {
            var curr = toProcess[0];
            toProcess.RemoveAt(0);

            //create node out of each rule
            var currRules = data.workflows[curr.name];
            List<Rule> negativeRules = new List<Rule>();

            foreach(Rule rule in currRules)
            {
                ProcessRule(leaves, toProcess, curr.name, curr.parent, rule, negativeRules);
                negativeRules.Add(rule);
            }

        }

        long sum = 0;
        foreach(Part part in data.parts)
        {
            foreach(RuleNode rule in leaves)
            {
                string matches = rule.CheckPart(part);

                if(matches == "R") 
                {
                    break;
                }

                if(matches == "A")
                {
                    sum += part.x + part.m + part.a + part.s;
                }
            }
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
