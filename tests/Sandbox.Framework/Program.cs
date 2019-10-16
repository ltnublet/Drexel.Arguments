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
            Program.MsBuildMain(args);
        }

        public static void MsBuildTest(string[] args)
        {
            ArgumentParser parser = new ArgumentParser(
                ParseStyle.MSBuild,
                new SetAdapter<Argument>(new HashSet<Argument>())
                {
                    new RuntimeArgument(
                        "ParameterA",
                        new List<string>() { "a" },
                        new List<string>() { "ParameterA" },
                        "A parameter.",
                        new CountBounds(1, null)),
                    new RuntimeArgument(
                        "ParameterB",
                        new List<string>() { "b" },
                        new List<string>() { "ParameterB" },
                        "Another parameter.",
                        new CountBounds(1, null)),
                });

            args = new string[]
            {
                "/a:Foo",
                "bar",
                "baz",
                "/b:1",
                "2",
                "3",
                "/a:Skibble",
                "skobble",
                "/a:Skidoodle",
                "skidoddle",
            };

            ParseResult result = parser.Parse(args);
        }

        public static void MsBuildMain(string[] args)
        {
            ArgumentParser parser = new ArgumentParser(
                ParseStyle.MSBuild,
                new SetAdapter<Argument>(new HashSet<Argument>())
                {
                    new RuntimeArgument(
                        "Alpha",
                        new List<string>() { "A", "a" },
                        new List<string>() { "Alpha", "alpha" },
                        "Alpha"),
                    new RuntimeArgument(
                        "Bravo",
                        new List<string>() { "B", "b" },
                        new List<string>() { "Bravo", "bravo" },
                        "Bravo",
                        new CountBounds(2, 3)),
                    new RuntimeArgument(
                        "Charlie",
                        new List<string>() { "C", "c" },
                        new List<string>() { "Charlie", "charlie" },
                        "Charlie"),
                    new RuntimeArgument(
                        "Delta",
                        new List<string>() { "D", "d" },
                        new List<string>() { "Delta", "delta" },
                        "Delta"),
                    new RuntimeArgument(
                        "Echo",
                        new List<string>() { "E", "e" },
                        new List<string>() { "Echo", "echo" },
                        "Echo",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "Foxtrot",
                        new List<string>() { "F", "f" },
                        new List<string>() { "Foxtrot", "foxtrot" },
                        "Foxtrot",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "Golf",
                        new List<string>() { "G", "g" },
                        new List<string>() { "Golf", "golf" },
                        "Golf",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "Hotel",
                        new List<string>() { "H", "h" },
                        new List<string>() { "Hotel", "hotel" },
                        "Hotel",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "India",
                        new List<string>() { "I", "i" },
                        new List<string>() { "India", "india" },
                        "India",
                        operandCount: new CountBounds(2, null)),
                });

            args = new string[]
            {
                "unparented1",
                "/A:alpha",
                "unparented2",
                "-b",
                "beta1",
                "beta2",
                "beta3",
                "/b:beta4",
                "beta5",
                "-Echo",
                "-F",
                "/Golf",
                "unparented3",
                "-C",
                "gamma",
                "-B",
                "beta6",
                "beta7",
                "beta8",
                "-H",
                "-India:iota1:continued",
                "iota2",
                "iota3",
            };

            ParseResult result = parser.Parse(args);
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
                        new CountBounds(2, 3)),
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
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "F",
                        new List<string>() { "F" },
                        new List<string>(),
                        "F",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "G",
                        new List<string>() { "G" },
                        new List<string>(),
                        "G",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "H",
                        new List<string>() { "H" },
                        new List<string>(),
                        "H",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "I",
                        new List<string>() { "I" },
                        new List<string>(),
                        "I",
                        operandCount: new CountBounds(2, null)),
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
                        new CountBounds(2, 3)),
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
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "F",
                        new List<string>() { "F" },
                        new List<string>(),
                        "F",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "G",
                        new List<string>() { "G" },
                        new List<string>(),
                        "G",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "H",
                        new List<string>() { "H" },
                        new List<string>(),
                        "H",
                        operandCount: CountBounds.Flag),
                    new RuntimeArgument(
                        "I",
                        new List<string>() { "I" },
                        new List<string>(),
                        "I",
                        operandCount: new CountBounds(2, null)),
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
