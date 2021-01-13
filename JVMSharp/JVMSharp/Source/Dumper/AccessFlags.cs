using System;

namespace JVMSharp
{
    class AccessFlags
    {
        public const ushort Public          = 0x0001;     // class field method
        public const ushort Private         = 0x0002;     // field method
        public const ushort Protected       = 0x0004;     // field method
        public const ushort Static          = 0x0008;     // field method
        public const ushort Final           = 0x0010;     // class field method
        public const ushort Super           = 0x0020;     // class
        public const ushort Synchronized    = 0x0020;     // method
        public const ushort Volatile        = 0x0040;     // field
        public const ushort Bridge          = 0x0040;     // method
        public const ushort Transient       = 0x0080;     // field
        public const ushort VarArgs         = 0x0080;     // method
        public const ushort Native          = 0x0100;     // method
        public const ushort Interface       = 0x0200;     // class
        public const ushort Abstract        = 0x0400;     // class method
        public const ushort Strict          = 0x0800;     // method
        public const ushort Synthetic       = 0x1000;     // class field method
        public const ushort Annotation      = 0x2000;     // class
        public const ushort Enum            = 0x4000;     // class field

        public static void PrintAccessFlags(bool bShowRawAccessFlags, ElementType ElementType, ushort AccessFlagsValue)
        {
            if (bShowRawAccessFlags)
            {
                Console.Write("{0} ", AccessFlagsValue.ToString("X4"));
            }
            if (ElementType == ElementType.Class)
            {
                if ((AccessFlagsValue & AccessFlags.Public) != 0)
                {
                    Console.Write("public ");
                }
                if ((AccessFlagsValue & AccessFlags.Final) != 0)
                {
                    Console.Write("final ");
                }
                if ((AccessFlagsValue & AccessFlags.Super) != 0)
                {
                    Console.Write("super ");
                }
                if ((AccessFlagsValue & AccessFlags.Interface) != 0)
                {
                    Console.Write("interface ");
                }
                if ((AccessFlagsValue & AccessFlags.Abstract) != 0)
                {
                    Console.Write("abstract ");
                }
                if ((AccessFlagsValue & AccessFlags.Synthetic) != 0)
                {
                    Console.Write("synthetic ");
                }
                if ((AccessFlagsValue & AccessFlags.Annotation) != 0)
                {
                    Console.Write("annotation ");
                }
                if ((AccessFlagsValue & AccessFlags.Enum) != 0)
                {
                    Console.Write("enum ");
                }
            }
            else if (ElementType == ElementType.Field)
            {
                if ((AccessFlagsValue & AccessFlags.Public) != 0)
                {
                    Console.Write("public ");
                }
                if ((AccessFlagsValue & AccessFlags.Private) != 0)
                {
                    Console.Write("private ");
                }
                if ((AccessFlagsValue & AccessFlags.Protected) != 0)
                {
                    Console.Write("protected ");
                }
                if ((AccessFlagsValue & AccessFlags.Static) != 0)
                {
                    Console.Write("static ");
                }
                if ((AccessFlagsValue & AccessFlags.Final) != 0)
                {
                    Console.Write("final ");
                }
                if ((AccessFlagsValue & AccessFlags.Volatile) != 0)
                {
                    Console.Write("volatile ");
                }
                if ((AccessFlagsValue & AccessFlags.Transient) != 0)
                {
                    Console.Write("transient ");
                }
                if ((AccessFlagsValue & AccessFlags.Synthetic) != 0)
                {
                    Console.Write("synthetic ");
                }
                if ((AccessFlagsValue & AccessFlags.Enum) != 0)
                {
                    Console.Write("enum ");
                }
            }
            else if (ElementType == ElementType.Method)
            {
                if ((AccessFlagsValue & AccessFlags.Public) != 0)
                {
                    Console.Write("public ");
                }
                if ((AccessFlagsValue & AccessFlags.Private) != 0)
                {
                    Console.Write("private ");
                }
                if ((AccessFlagsValue & AccessFlags.Protected) != 0)
                {
                    Console.Write("protected ");
                }
                if ((AccessFlagsValue & AccessFlags.Static) != 0)
                {
                    Console.Write("static ");
                }
                if ((AccessFlagsValue & AccessFlags.Final) != 0)
                {
                    Console.Write("final ");
                }
                if ((AccessFlagsValue & AccessFlags.Synchronized) != 0)
                {
                    Console.Write("synchronized ");
                }
                if ((AccessFlagsValue & AccessFlags.Bridge) != 0)
                {
                    Console.Write("bridge ");
                }
                if ((AccessFlagsValue & AccessFlags.VarArgs) != 0)
                {
                    Console.Write("varargs ");
                }
                if ((AccessFlagsValue & AccessFlags.Native) != 0)
                {
                    Console.Write("native ");
                }
                if ((AccessFlagsValue & AccessFlags.Abstract) != 0)
                {
                    Console.Write("abstract ");
                }
                if ((AccessFlagsValue & AccessFlags.Strict) != 0)
                {
                    Console.Write("strict ");
                }
                if ((AccessFlagsValue & AccessFlags.Synthetic) != 0)
                {
                    Console.Write("synthetic ");
                }
            }
        }
    }
}
