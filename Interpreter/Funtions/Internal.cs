using System;

namespace CardsFunctions
{
    public class Internal
    {
        public void ShowLine(string text)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("TEXT");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("]: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
        }
    }
}