using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Dijk1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                if (args.Length != 1)
                {
                    throw new ArgumentException("You must supply a single filename as an argument.");
                }

                var filename = args[0];

                var edgeList = new List<(string, string, int)>();

                var linenumber = 1;
                foreach (var line in File.ReadLines(filename).Select(x=>x.Trim()))
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    var tokens = Regex.Split(line, @"[ \t]+");
                    if (tokens.Length != 3)
                    {
                        Console.Error.WriteLine($"at line {linenumber}: there must be three tokens");
                    }

                    if (!tokens[2].All(char.IsDigit))
                    {
                        Console.Error.WriteLine($"at line {linenumber}: the third token on a line must be an integer (the edge weight)");
                    }

                    var weight = Convert.ToInt32(tokens[2]);

                    edgeList.Add((tokens[0], tokens[1], weight));
                }

                var solver = new Solver(edgeList);
                solver.Solve("1", "5");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                var codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var progname = Path.GetFileNameWithoutExtension(codeBase);
                Console.Error.WriteLine(progname + ": Error: " + ex.Message);
            }

        }
    }
}
