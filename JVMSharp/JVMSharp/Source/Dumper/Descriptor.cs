using System;
using System.Collections.Generic;

namespace JVMSharp
{
    class Descriptor
    {
        public bool bIsFunction;
        public string MemberType;
        public List<string> ParameterTypes;

        public Descriptor()
        {
            bIsFunction = false;
            MemberType = "";
            ParameterTypes = new List<string>();
        }

        string ProcessTypeString(string TypeString)
        {
            return TypeString.Replace("/", ".").Replace("$", ".");
        }

        string ParseType(string DescriptorString, ref int i)
        {
            string Type = "";
            char Char = StringHelper.GetChar(DescriptorString, i++);
            if (Char == 'B')
            {
                Type = "byte";
            }
            else if (Char == 'C')
            {
                Type = "char";
            }
            else if (Char == 'D')
            {
                Type = "double";
            }
            else if (Char == 'F')
            {
                Type = "float";
            }
            else if (Char == 'I')
            {
                Type = "int";
            }
            else if (Char == 'J')
            {
                Type = "long";
            }
            else if (Char == 'L')
            {
                while (true)
                {
                    Char = StringHelper.GetChar(DescriptorString, i++);
                    if (Char == ';')
                    {
                        break;
                    }
                    else
                    {
                        Type += Char;
                    }
                }
                Type = ProcessTypeString(Type);
            }
            else if (Char == 'S')
            {
                Type = "short";
            }
            else if (Char == 'V')
            {
                Type = "void";
            }
            else if (Char == 'Z')
            {
                Type = "boolean";
            }
            else if (Char == '[')
            {
                string SubType = ParseType(DescriptorString, ref i);
                Type = SubType + "[]";
            }
            else
            {
                DebugHelper.Assert(false);
            }
            return Type;
        }

        public void Parse(string DescriptorString)
        {
            ParameterTypes.Clear();
            int i = 0;
            char Char = StringHelper.GetChar(DescriptorString, i);
            if (Char != '(')
            {
                this.bIsFunction = false;
                this.MemberType = ParseType(DescriptorString, ref i);
            }
            else
            {
                this.bIsFunction = true;
                i++;
                while (true)
                {
                    Char = StringHelper.GetChar(DescriptorString, i);
                    if (Char == ')')
                    {
                        i++;
                        break;
                    }
                    string ParameterType = ParseType(DescriptorString, ref i);
                    ParameterTypes.Add(ParameterType);
                }
                this.MemberType = ParseType(DescriptorString, ref i);
            }
        }

        public void Print(string MemberName)
        {
            if (bIsFunction == false)
            {
                Console.Write("{0} {1}", MemberType, MemberName);
            }
            else
            {
                Console.Write("{0} {1}(", MemberType, MemberName);
                int Count = ParameterTypes.Count;
                for (int i1 = 0; i1 < Count; i1++)
                {
                    string ParameterType = ParameterTypes[i1];
                    Console.Write(ParameterType);
                    if (i1 != Count - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.Write(")");
            }
        }

        public int CalculateArgumentSlotCount(bool bStatic)
        {
            int ArgumentSlotCount = 0;
            foreach (string ParameterType in ParameterTypes)
            {
                ArgumentSlotCount++;
                if (ParameterType == "long" || ParameterType == "double")
                {
                    ArgumentSlotCount++;
                }
            }
            if (bStatic == false)
            {
                ArgumentSlotCount++;
            }
            return ArgumentSlotCount;
        }
    }
}
