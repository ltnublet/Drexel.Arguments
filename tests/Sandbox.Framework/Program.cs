using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Drexel.Arguments;
using Drexel.Collections.Generic;

namespace Sandbox.Framework
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ArgumentParser parser = new ArgumentParser(
                ParseStyle.DOS,
                new SetAdapter<Argument>(new HashSet<Argument>())
                {
                    new RuntimeArgument(
                        "A",
                        new List<string>() { "A" },
                        new List<string>(),
                        "A"),
                    new RuntimeArgument(
                        "B",
                        new List<string>() { "B" },
                        new List<string>(),
                        "B",
                        false,
                        new OperandCount(2, 2)),
                    new RuntimeArgument(
                        "C",
                        new List<string>() { "C" },
                        new List<string>(),
                        "C"),
                    new RuntimeArgument(
                        "E",
                        new List<string>() { "E" },
                        new List<string>(),
                        "E",
                        operandCount: OperandCount.Flag),
                    new RuntimeArgument(
                        "F",
                        new List<string>() { "F" },
                        new List<string>(),
                        "F",
                        operandCount: OperandCount.Flag),
                    new RuntimeArgument(
                        "G",
                        new List<string>() { "G" },
                        new List<string>(),
                        "G",
                        operandCount: OperandCount.Flag),
                    new RuntimeArgument(
                        "H",
                        new List<string>() { "H" },
                        new List<string>(),
                        "H",
                        operandCount: OperandCount.Flag),
                });

            args = new string[]
            {
                "unparented1",
                "/A",
                "alpha",
                "unparented2",
                "/B",
                "beta1",
                "beta2",
                "/EFG",
                "unparented3",
                "/C",
                "gamma",
                "/H"
            };

            ParseResult result = parser.Parse(args);
        }
    }
}
