using System;
using System.Collections.Generic;

namespace JVMSharp
{
    class AttributeInfoHelper
    {
        public static void PrintAttributes(int Indent, List<AttributeInfo> Attributes)
        {
            int Count = Attributes.Count;
            for (int i = 0; i < Count; i++)
            {
                AttributeInfo AttributeInfo = Attributes[i];
                ConsoleHelper.PrintIndent(Indent);
                Console.WriteLine("Attribute {0}:", i);
                AttributeInfo.Print(Indent + 2);
            }
        }
    }

    class AttributeInfo
    {
        public const string AttributeName_SourceFile = "SourceFile";
        public const string AttributeName_Code = "Code";
        public const string AttributeName_LineNumberTable = "LineNumberTable";

        public ushort AttributeNameIndex;
        public string AttributeNameString;
        public uint AttributeLength;

        public virtual void Print(int Indent)
        {
            ConsoleHelper.PrintIndent(Indent);
            Console.WriteLine("Name=\"{0}\" Size={1}", AttributeNameString, AttributeLength);
        }
    }

    class AttributeInfo_SourceFile : AttributeInfo
    {
        public ushort SourceFileIndex;
        public string SourceFileString;

        public override void Print(int Indent)
        {
            base.Print(Indent);
            ConsoleHelper.PrintIndent(Indent);
            Console.WriteLine("SourceFileIndex={0} SourceFileString=\"{1}\"", SourceFileIndex, SourceFileString);
        }
    }

    class AttributeInfo_Code : AttributeInfo
    {
        public ushort MaxStack;
        public ushort MaxLocals;
        public uint CodeLength;
        public byte[] Code;
        public List<ExceptionInfo> ExceptionTable;
        public List<AttributeInfo> Attributes;

        public override void Print(int Indent)
        {
            base.Print(Indent);
            ConsoleHelper.PrintIndent(Indent);
            Console.WriteLine("MaxStack={0} MaxLocals={1} CodeLength={2}", MaxStack, MaxLocals, CodeLength);

            ExceptionInfo.PrintExceptionInfos(Indent + 2, ExceptionTable);
            AttributeInfoHelper.PrintAttributes(Indent + 2, Attributes);
        }
    }
}
