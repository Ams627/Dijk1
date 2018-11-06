using System;
using System.Linq;
using System.Collections.Generic;

namespace Dijk1
{
    class Solver
    {
        private readonly List<(string from, string to, int weight)> _adjList;
        private readonly HashSet<Node> _visited = new HashSet<Node>();

        class Node
        {
            public int Tentative { get; set; }
            public string Name { get; set; }
            public List<(Node node, int edge)> Neighbours { get; set; }
        }

        private readonly Dictionary<string, Node> netlist = new Dictionary<string, Node>();


        public Solver(List<(string, string, int)> adjList)
        {
            _adjList = adjList;

            foreach (var entry in _adjList)
            {
                if (!netlist.TryGetValue(entry.from, out var nodeFrom))
                {
                    nodeFrom = new Node { Tentative = int.MaxValue, Name = entry.from, Neighbours = new List<(Node net, int edge)>() };
                    netlist.Add(entry.from, nodeFrom);
                }
                if (!netlist.TryGetValue(entry.to, out var nodeTo))
                {
                    nodeTo = new Node { Tentative = int.MaxValue, Name = entry.to, Neighbours = new List<(Node net, int edge)>() };
                    netlist.Add(entry.to, nodeTo);
                }
                nodeFrom.Neighbours.Add((nodeTo, entry.weight));
            }
        }

        public void Solve(string origin, string destination)
        {
            foreach (var entry in netlist)
            {
                entry.Value.Tentative = int.MaxValue;
            }

            netlist[origin].Tentative = int.MaxValue;

            _visited.Clear();

            Node currentNode = netlist[origin];

            while (currentNode.Name != destination)
            {
                foreach (var (node, edge) in currentNode.Neighbours)
                {
                    if (!_visited.Contains(node))
                    {
                        var newTentative = edge + node.Tentative;
                        if (newTentative < node.Tentative)
                        {
                            node.Tentative = newTentative;
                        }
                    }
                }
                _visited.Add(currentNode);

                var smallest = int.MaxValue;
                var lowestNode = netlist.First().Value;

                foreach (var entry in netlist)
                {
                    if (entry.Value.Tentative < smallest)
                    {
                        smallest = entry.Value.Tentative;
                        lowestNode = entry.Value;
                    }
                }

                currentNode = lowestNode;
            }

        }
    }
}
