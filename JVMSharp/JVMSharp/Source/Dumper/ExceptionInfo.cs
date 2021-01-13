using System;
using System.Collections.Generic;

namespace JVMSharp
{
    class ExceptionInfo
    {
        public ushort StartPC;
        public ushort EndPC;
        public ushort HandlerPC;
        public ushort CatchType;

        public ExceptionInfo()
        {
            StartPC = 0;
            EndPC = 0;
            HandlerPC = 0;
            CatchType = 0;
        }

        public void Print(int Indent)
        {
            ConsoleHelper.PrintIndent(Indent);
            Console.WriteLine("StartPC={0} EndPC={1} HandlerPC={2} CatchType={3}", StartPC, EndPC, HandlerPC, CatchType);
        }

        public static void PrintExceptionInfos(int Indent, List<ExceptionInfo> ExceptionInfos)
        {
            int Count = ExceptionInfos.Count;
            for (int i = 0; i < Count; i++)
            {
                ExceptionInfo ExceptionInfo = ExceptionInfos[i];
                ConsoleHelper.PrintIndent(Indent);
                Console.WriteLine("Exception {0}:", i);
                ExceptionInfo.Print(Indent + 2);
            }
        }
    }
}
