namespace Drexel.Arguments
{
    /// <summary>
    /// Indicates the type of parsing style that should be applied.
    /// </summary>
    public enum ParseStyle
    {
        /// <summary>
        /// Indicates that parsing should be done in "DOS-style". Only short names are supported. Short names must be a
        /// single character. The short name '/' is illegal. Short names are prefixed by a single forward slash. Flags
        /// can be concatenated. The value of an argument is given immediately after the argument, separated by a
        /// space.
        /// </summary>
        /// <example>
        /// <code>/q /i input.csv /o output.xlsx /fbr</code>
        /// </example>
        DOS,

        /// <summary>
        /// Indicates that parsing should be done in "Windows-style". Both short and long names are supported, but
        /// there must be no overlap between short and long names. Short names must be a single character. The short
        /// and long name '/' is illegal. Names are prefixed by a single forward slash. Flags can be concatenated, but
        /// if the combination of flags results in a recognized long name, the long name will be used instead.
        /// </summary>
        /// <example>
        /// <code>/quiet /input input.csv /o output.xlsx /fbr</code>
        /// </example>
        Windows,

        /// <summary>
        /// Indicates that parsing should be done in "POSIX-style". Only short names are supported. Short names must be
        /// a single character. The short name '-' is illegal. Short names are prefixed by a single hyphen. Flags can
        /// be concatenated. A special argument '--' means that anything specified after it appears is a value, even if
        /// it begins with a hypen. The value of an argument may be attached directly to the argument, or appear
        /// immediately after it.
        /// </summary>
        /// <example>
        /// <code>-q -i input.csv -o output.scv -fbr</code>
        /// </example>
        Posix,

        /// <summary>
        /// Indicates that parsing should be done in "GNU-style"; GNU-style is the same as <see cref="Posix"/>, but
        /// adds support for long names. Long names cannot contain the character '='. Long names are prefixed by a
        /// double-hyphen. The value of an argument may be attached directly to the argument, prefixed by '=', or
        /// appear immediately after it.
        /// </summary>
        /// <example>
        /// <code>-q --input input.csv --output=output.csv -fbr</code>
        /// </example>
        GNU,

        /// <summary>
        /// Indicates that parsing should be done in "MSBuild-style". Both short and long names are supported, but
        /// there must be no overlap between short and long names. The short and long names '/', '-', and ':' are
        /// illegal. Names cannot contain the character ':'. Names may be prefixed by either a single forward
        /// slash, or a single hyphen. Flags cannot be concatenated.
        /// </summary>
        /// <example>
        /// <code>/q /input:input.csv -output:1 output1.csv /output:2 output2.csv -fbr</code>
        /// </example>
        MSBuild,
    }
}
