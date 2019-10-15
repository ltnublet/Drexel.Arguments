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
            Program.PosixMain(args);
        }

        public static void PosixMain(string[] args)
        {
            ArgumentParser parser = new ArgumentParser(
                ParseStyle.Posix,
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
                        new OperandCount(2, 3)),
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
                    new RuntimeArgument(
                        "I",
                        new List<string>() { "I" },
                        new List<string>(),
                        "I",
                        operandCount: new OperandCount(2, null)),
                });

            args = new string[]
            {
                "unparented1",
                "-A",
                "alpha",
                "unparented2",
                "-B",
                "beta1",
                "beta2",
                "beta3",
                "-EFG",
                "unparented3",
                "-C",
                "gamma",
                "-H",
                "-I",
                "iota1",
                "iota2",
                "iota3",
                "--",
                "-EFG",
                "-unparented4",
                "unparented5",
                "-unparented6",
                "unparented7",
            };

            ParseResult result = parser.Parse(args);
        }

        public static void DosMain(string[] args)
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
                        new OperandCount(2, 3)),
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
                    new RuntimeArgument(
                        "I",
                        new List<string>() { "I" },
                        new List<string>(),
                        "I",
                        operandCount: new OperandCount(2, null)),
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
                "beta3",
                "/EFG",
                "unparented3",
                "/C",
                "gamma",
                "/H",
                "/I",
                "iota1",
                "iota2",
                "iota3",
            };

            ParseResult result = parser.Parse(args);
        }
    }
}
