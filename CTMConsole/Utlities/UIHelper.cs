using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CTMConsole.Utlities
{
    /// <summary>
    /// A class that provides utilities for enhancing the UI
    /// </summary>
    public static class UIHelper
    {
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <param name="newLine">if set to <c>true</c> writes the line into a new line on the console; otherwise <c>false</c></param>
        internal static void WriteLine(string line, bool newLine = false)
        {
            if (newLine)
            {
                Console.WriteLine("\n" + line);
            }
            else
            {
                Console.WriteLine(line);
            }
        }

        /// <summary>
        /// Writes the main header.
        /// </summary>
        /// <param name="headerLine">The header line.</param>
        internal static void WriteMainHeader(string headerLine)
        {
            WriteLine("***********************************************************************", true);
            WriteLine(headerLine, true);
            WriteLine("***********************************************************************", true);
        }

        /// <summary>
        /// Writes the sub header.
        /// </summary>
        /// <param name="subHeaderLine">The sub header line.</param>
        internal static void WriteSubHeader(string subHeaderLine)
        {
            WriteLine("-------------------------", true);
            WriteLine(subHeaderLine, true);
            WriteLine("-------------------------", true);
        }
    }
}
