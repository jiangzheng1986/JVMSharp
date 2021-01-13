using System;

namespace JVMSharp
{
    public static class ConsoleHelper
    {
        public static void PrintIndent(int Indent)
        {
            for (int i = 0; i < Indent; i++)
            {
                Console.Write(" ");
            }
        }
    }
}
