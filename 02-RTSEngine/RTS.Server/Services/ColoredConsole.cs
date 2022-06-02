using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS.Server
{
    public static class ColoredConsole
    {
        public static void WriteLine(string pText,ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(pText);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
