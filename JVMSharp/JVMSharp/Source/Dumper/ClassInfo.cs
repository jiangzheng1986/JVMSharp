using System;
using System.Collections.Generic;

namespace JVMSharp
{
    class ClassInfo
    {
        public uint MagicNumber;
        public ushort MinorVersion;
        public ushort MajorVersion;
        public List<Constant> Constants;
        public ushort AccessFlags;
        public ushort ThisClassIndex;
        public string ThisClassString;
        public ushort SuperClassIndex;
        public string SuperClassString;
        public List<ushort> InterfaceClassIndices;
        public List<string> InterfaceClassStrings;
        public List<MemberInfo> FieldInfos;
        public List<MemberInfo> MethodInfos;
        public List<AttributeInfo> Attributes;

        public bool bShowRawAccessFlags;
        public bool bShowConstants;
        public bool bShowDisassembly;
        public bool bShowAttributes;

        public ClassInfo()
        {
            ThisClassString = "";
            SuperClassString = "Ljava/lang/Object";
            Constants = new List<Constant>();
            InterfaceClassIndices = new List<ushort>();
            InterfaceClassStrings = new List<string>();
            FieldInfos = new List<MemberInfo>();
            MethodInfos = new List<MemberInfo>();
            Attributes = new List<AttributeInfo>();
            bShowRawAccessFlags = false;
            bShowConstants = false;
            bShowDisassembly = false;
            bShowAttributes = false;
        }

        public string ConstantIndexToString(ushort ConstantIndex)
        {
            Utf8Constant Utf8Constant = (Utf8Constant)Constants[ConstantIndex];
            return Utf8Constant.Utf8;
        }

        public string ClassIndexToClassName(ushort ClassIndex)
        {
            ClassRefConstant ClassRefConstant = (ClassRefConstant)Constants[ClassIndex];
            ushort ClassNameIndex = ClassRefConstant.NameIndex;
            return ConstantIndexToString(ClassNameIndex);
        }

        public bool Load(string Filename)
        {
            Stream Stream = FileHelper.ReadFile(Filename);
            if (Stream == null)
            {
                return false;
            }

            MagicNumber = Stream.ReadBigEndianUnsignedInt32();
            MinorVersion = Stream.ReadBigEndianUnsignedInt16();
            MajorVersion = Stream.ReadBigEndianUnsignedInt16();

            ushort ConstantsCount = Stream.ReadBigEndianUnsignedInt16();
            Constants = new List<Constant>(ConstantsCount);
            Constants.Resize(ConstantsCount);
            for (int i = 1; i < ConstantsCount; i++)
            {
                ConstantType ConstantType = (ConstantType)Stream.ReadUnsignedInt8();
                switch (ConstantType)
                {
                    case ConstantType.Utf8:
                        Utf8Constant Utf8Constant = new Utf8Constant();
                        Utf8Constant.Utf8 = Stream.ReadBigEndianShortString();
                        Constants[i] = Utf8Constant;
                        break;
                    case ConstantType.Int:
                        IntConstant IntConstant = new IntConstant();
                        IntConstant.Int = (int)Stream.ReadBigEndianUnsignedInt32();
                        Constants[i] = IntConstant;
                        break;
                    case ConstantType.Float:
                        FloatConstant FloatConstant = new FloatConstant();
                        FloatConstant.Float = Stream.ReadBigEndianFloat32();
                        Constants[i] = FloatConstant;
                        break;
                    case ConstantType.Long:
                        LongConstant LongConstant = new LongConstant();
                        LongConstant.Long = (long)Stream.ReadBigEndianUnsignedInt64();
                        Constants[i] = LongConstant;
                        i++;
                        break;
                    case ConstantType.Double:
                        DoubleConstant DoubleConstant = new DoubleConstant();
                        DoubleConstant.Double = Stream.ReadBigEndianFloat64();
                        Constants[i] = DoubleConstant;
                        i++;
                        break;
                    case ConstantType.Class:
                        ClassRefConstant ClassRefConstant = new ClassRefConstant();
                        ClassRefConstant.NameIndex = Stream.ReadBigEndianUnsignedInt16();
                        Constants[i] = ClassRefConstant;
                        break;
                    case ConstantType.String:
                        StringConstant StringConstant = new StringConstant();
                        StringConstant.StringIndex = Stream.ReadBigEndianUnsignedInt16();
                        Constants[i] = StringConstant;
                        break;
                    case ConstantType.FieldRef:
                        FieldRefConstant FieldRefConstant = new FieldRefConstant();
                        FieldRefConstant.ClassIndex = Stream.ReadBigEndianUnsignedInt16();
                        FieldRefConstant.NameAndTypeIndex = Stream.ReadBigEndianUnsignedInt16();
                        Constants[i] = FieldRefConstant;
                        break;
                    case ConstantType.MethodRef:
                        MethodRefConstant MethodRefConstant = new MethodRefConstant();
                        MethodRefConstant.ClassIndex = Stream.ReadBigEndianUnsignedInt16();
                        MethodRefConstant.NameAndTypeIndex = Stream.ReadBigEndianUnsignedInt16();
                        Constants[i] = MethodRefConstant;
                        break;
                    case ConstantType.InterfaceMethodRef:
                        InterfaceMethodRefConstant InterfaceMethodRefConstant = new InterfaceMethodRefConstant();
                        InterfaceMethodRefConstant.ClassIndex = Stream.ReadBigEndianUnsignedInt16();
                        InterfaceMethodRefConstant.NameAndTypeIndex = Stream.ReadBigEndianUnsignedInt16();
                        Constants[i] = InterfaceMethodRefConstant;
                        break;
                    case ConstantType.NameAndType:
                        NameAndTypeConstant NameAndTypeConstant = new NameAndTypeConstant();
                        NameAndTypeConstant.NameIndex = Stream.ReadBigEndianUnsignedInt16();
                        NameAndTypeConstant.DescriptorIndex = Stream.ReadBigEndianUnsignedInt16();
                        Constants[i] = NameAndTypeConstant;
                        break;
                    case ConstantType.MethodHandle:
                        MethodHandleConstant MethodHandleConstant = new MethodHandleConstant();
                        MethodHandleConstant.ReferenceKind = Stream.ReadUnsignedInt8();
                        MethodHandleConstant.ReferenceIndex = Stream.ReadUnsignedInt16();
                        Constants[i] = MethodHandleConstant;
                        break;
                    case ConstantType.MethodType:
                        MethodTypeConstant MethodTypeConstant = new MethodTypeConstant();
                        MethodTypeConstant.DescriptorIndex = Stream.ReadUnsignedInt16();
                        Constants[i] = MethodTypeConstant;
                        break;
                    case ConstantType.InvokeDynamic:
                        InvokeDynamicConstant InvokeDynamicConstant = new InvokeDynamicConstant();
                        InvokeDynamicConstant.BootstrapMethodAttrIndex = Stream.ReadUnsignedInt16();
                        InvokeDynamicConstant.NameAndTypeIndex = Stream.ReadUnsignedInt16();
                        Constants[i] = InvokeDynamicConstant;
                        break;
                    default:
                        DebugHelper.Assert(false);
                        break;
                }
            }

            AccessFlags = Stream.ReadBigEndianUnsignedInt16();

            ThisClassIndex = Stream.ReadBigEndianUnsignedInt16();
            ThisClassString = ClassIndexToClassName(ThisClassIndex);

            SuperClassIndex = Stream.ReadBigEndianUnsignedInt16();
            SuperClassString = ClassIndexToClassName(SuperClassIndex);

            ushort InterfacesCount = Stream.ReadBigEndianUnsignedInt16();
            InterfaceClassIndices = new List<ushort>(InterfacesCount);
            InterfaceClassIndices.Resize(InterfacesCount);
            InterfaceClassStrings = new List<string>(InterfacesCount);
            InterfaceClassStrings.Resize(InterfacesCount);
            for (int i = 0; i < InterfacesCount; i++)
            {
                ushort InterfaceClassIndex = Stream.ReadBigEndianUnsignedInt16();
                InterfaceClassIndices[i] = InterfaceClassIndex;
                InterfaceClassStrings[i] = ClassIndexToClassName(InterfaceClassIndex);
            }

            ushort FieldsCount = Stream.ReadBigEndianUnsignedInt16();
            FieldInfos = LoadMembers(FieldsCount, Stream);

            ushort MethodsCount = Stream.ReadBigEndianUnsignedInt16();
            MethodInfos = LoadMembers(MethodsCount, Stream);

            ushort AttributesCount = Stream.ReadBigEndianUnsignedInt16();
            Attributes = LoadAttributes(AttributesCount, Stream);

            return true;
        }

        List<MemberInfo> LoadMembers(int MembersCount, Stream Stream)
        {
            List<MemberInfo> MemberInfos = new List<MemberInfo>(MembersCount);
            MemberInfos.Resize(MembersCount);
            for (int i = 0; i < MembersCount; i++)
            {
                MemberInfos[i] = LoadMember(Stream);
            }
            return MemberInfos;
        }

        MemberInfo LoadMember(Stream Stream)
        {
            MemberInfo MemberInfo = new MemberInfo();
            MemberInfo.AccessFlags = Stream.ReadBigEndianUnsignedInt16();
            MemberInfo.NameIndex = Stream.ReadBigEndianUnsignedInt16();
            MemberInfo.DescriptorIndex = Stream.ReadBigEndianUnsignedInt16();
            ushort AttributesCount = Stream.ReadBigEndianUnsignedInt16();
            MemberInfo.Attributes = LoadAttributes(AttributesCount, Stream);
            MemberInfo.NameString = ConstantIndexToString(MemberInfo.NameIndex);
            MemberInfo.DescriptorString = ConstantIndexToString(MemberInfo.DescriptorIndex);
            return MemberInfo;
        }

        List<AttributeInfo> LoadAttributes(int AttributesCount, Stream Stream)
        {
            List<AttributeInfo> Attributes = new List<AttributeInfo>(AttributesCount);
            Attributes.Resize(AttributesCount);
            for (int i = 0; i < AttributesCount; i++)
            {
                ushort AttributeNameIndex = Stream.ReadBigEndianUnsignedInt16();
                string AttributeNameString = ConstantIndexToString(AttributeNameIndex);
                uint AttributeLength = Stream.ReadBigEndianUnsignedInt32();
                AttributeInfo AttributeInfo = null;
                switch (AttributeNameString)
                {
                    case AttributeInfo.AttributeName_SourceFile:
                        AttributeInfo_SourceFile AttributeInfo_SourceFile = new AttributeInfo_SourceFile();
                        AttributeInfo_SourceFile.SourceFileIndex = Stream.ReadBigEndianUnsignedInt16();
                        AttributeInfo_SourceFile.SourceFileString = ConstantIndexToString(AttributeInfo_SourceFile.SourceFileIndex);
                        AttributeInfo = AttributeInfo_SourceFile;
                        break;
                    case AttributeInfo.AttributeName_Code:
                        AttributeInfo_Code AttributeInfo_Code = new AttributeInfo_Code();
                        AttributeInfo_Code.MaxStack = Stream.ReadBigEndianUnsignedInt16();
                        AttributeInfo_Code.MaxLocals = Stream.ReadBigEndianUnsignedInt16();
                        AttributeInfo_Code.CodeLength = Stream.ReadBigEndianUnsignedInt32();
                        AttributeInfo_Code.Code = Stream.Read((int)AttributeInfo_Code.CodeLength);
                        ushort ExceptionTableLength = Stream.ReadBigEndianUnsignedInt16();
                        AttributeInfo_Code.ExceptionTable = new List<ExceptionInfo>(ExceptionTableLength);
                        AttributeInfo_Code.ExceptionTable.Resize(ExceptionTableLength);
                        for (int i1 = 0; i1 < ExceptionTableLength; i1++)
                        {
                            ExceptionInfo ExceptionInfo = new ExceptionInfo();
                            ExceptionInfo.StartPC = Stream.ReadBigEndianUnsignedInt16();
                            ExceptionInfo.EndPC = Stream.ReadBigEndianUnsignedInt16();
                            ExceptionInfo.HandlerPC = Stream.ReadBigEndianUnsignedInt16();
                            ExceptionInfo.CatchType = Stream.ReadBigEndianUnsignedInt16();
                            AttributeInfo_Code.ExceptionTable[i1] = ExceptionInfo;
                        }
                        ushort AttributesCount1 = Stream.ReadBigEndianUnsignedInt16();
                        AttributeInfo_Code.Attributes = LoadAttributes(AttributesCount1, Stream);
                        AttributeInfo = AttributeInfo_Code;
                        break;
                    default:
                        AttributeInfo = new AttributeInfo();
                        Stream.Skip((int)AttributeLength);
                        break;
                }
                AttributeInfo.AttributeNameIndex = AttributeNameIndex;
                AttributeInfo.AttributeNameString = AttributeNameString;
                AttributeInfo.AttributeLength = AttributeLength;
                Attributes[i] = AttributeInfo;
            }
            return Attributes;
        }

        public void PrintConstant(int ConstantIndex)
        {
            Constant Constant = Constants[ConstantIndex];
            if (Constant == null)
            {
                Console.Write("<Empty>");
            }
            else if (Constant is Utf8Constant)
            {
                Utf8Constant Utf8Constant = (Utf8Constant)Constant;
                Console.Write("Utf8: \"{0}\"", Utf8Constant.Utf8);
            }
            else if (Constant is IntConstant)
            {
                IntConstant IntConstant = (IntConstant)Constant;
                Console.Write("Int: {0}", IntConstant.Int);
            }
            else if (Constant is FloatConstant)
            {
                FloatConstant FloatConstant = (FloatConstant)Constant;
                Console.Write("Float: {0}", FloatConstant.Float);
            }
            else if (Constant is LongConstant)
            {
                LongConstant LongConstant = (LongConstant)Constant;
                Console.Write("Long: {0}", LongConstant.Long);
            }
            else if (Constant is DoubleConstant)
            {
                DoubleConstant DoubleConstant = (DoubleConstant)Constant;
                Console.Write("Double: {0}", DoubleConstant.Double);
            }
            else if (Constant is ClassRefConstant)
            {
                ClassRefConstant ClassRefConstant = (ClassRefConstant)Constant;
                ushort NameIndex = ClassRefConstant.NameIndex;
                string Name = ConstantIndexToString(NameIndex);
                Console.Write("Class: NameIndex={0}(\"{1}\")", NameIndex, Name);
            }
            else if (Constant is StringConstant)
            {
                StringConstant StringConstant = (StringConstant)Constant;
                ushort StringIndex = StringConstant.StringIndex;
                string String = ConstantIndexToString(StringIndex);
                Console.Write("Class: NameIndex={0}(\"{1}\")", StringIndex, String);
            }
            else if (Constant is SymbolRefConstant)
            {
                SymbolRefConstant SymbolRefConstant = (SymbolRefConstant)Constant;
                ushort ClassIndex = SymbolRefConstant.ClassIndex;
                string ClassName = ClassIndexToClassName(ClassIndex);
                ushort NameAndTypeIndex = SymbolRefConstant.NameAndTypeIndex;
                NameAndTypeConstant NameAndTypeConstant = (NameAndTypeConstant)Constants[NameAndTypeIndex];
                ushort NameIndex = NameAndTypeConstant.NameIndex;
                string Name = ConstantIndexToString(NameIndex);
                ushort DescriptorIndex = NameAndTypeConstant.DescriptorIndex;
                string Descriptor = ConstantIndexToString(DescriptorIndex);
                if (Constant is FieldRefConstant)
                {
                    Console.Write("FieldRef: ");
                }
                if (Constant is MethodRefConstant)
                {
                    Console.Write("MethodRef: ");
                }
                if (Constant is InterfaceMethodRefConstant)
                {
                    Console.Write("InterfaceMethodRef: ");
                }
                Console.Write("ClassIndex={0}(\"{1}\") NameAndTypeIndex={2}(\"{3}\",\"{4}\")", ClassIndex, ClassName, NameAndTypeIndex, Name, Descriptor);
            }
            else if (Constant is NameAndTypeConstant)
            {
                NameAndTypeConstant NameAndTypeConstant = (NameAndTypeConstant)Constant;
                ushort NameIndex = NameAndTypeConstant.NameIndex;
                string Name = ConstantIndexToString(NameIndex);
                ushort DescriptorIndex = NameAndTypeConstant.DescriptorIndex;
                string Descriptor = ConstantIndexToString(DescriptorIndex);
                Console.Write("NameAndType: NameIndex={0}(\"{1}\") DescriptorIndex={2}(\"{3}\")", NameIndex, Name, DescriptorIndex, Descriptor);
            }
            else if (Constant is MethodHandleConstant)
            {
                Console.Write("MethodHandleConstant");
            }
            else if (Constant is MethodTypeConstant)
            {
                Console.Write("MethodTypeConstant");
            }
            else if (Constant is InvokeDynamicConstant)
            {
                Console.Write("InvokeDynamicConstant");
            }
            else
            {
                DebugHelper.Assert(false);
            }
        }

        string ProcessTypeString(string TypeString)
        {
            return TypeString.Replace("/", ".").Replace("$", ".");
        }

        void PrintDescriptor(string DescriptorString, string MemberName)
        {
            Descriptor Descriptor = new Descriptor();
            Descriptor.Parse(DescriptorString);
            Descriptor.Print(MemberName);
        }

        void PrintMember(ElementType ElementType, MemberInfo MemberInfo)
        {
            JVMSharp.AccessFlags.PrintAccessFlags(bShowRawAccessFlags, ElementType, MemberInfo.AccessFlags);
            PrintDescriptor(MemberInfo.DescriptorString, MemberInfo.NameString);
            if (ElementType == ElementType.Field)
            {
                Console.WriteLine(";");
                if (bShowAttributes)
                {
                    AttributeInfoHelper.PrintAttributes(4, MemberInfo.Attributes);
                }
            }
            else
            {
                Console.WriteLine();
                if (bShowAttributes)
                {
                    AttributeInfoHelper.PrintAttributes(4, MemberInfo.Attributes);
                }
                Console.WriteLine("  {");
                if (bShowDisassembly)
                {
                    foreach (AttributeInfo AttributeInfo in MemberInfo.Attributes)
                    {
                        if (AttributeInfo is AttributeInfo_Code)
                        {
                            AttributeInfo_Code AttributeInfo_Code = (AttributeInfo_Code)AttributeInfo;
                            Disassembler.Disassemble(AttributeInfo_Code.Code, 4, this);
                        }
                    }
                }
                Console.WriteLine("  }");
            }
        }

        void PrintMembers(ElementType ElementType, List<MemberInfo> Members)
        {
            foreach (MemberInfo MemberInfo in Members)
            {
                Console.Write("  ");
                PrintMember(ElementType, MemberInfo);
            }
        }

        public void Print()
        {
            if (bShowConstants)
            {
                Console.WriteLine("Constants:");
                int ConstantsCount = Constants.Count;
                for (int i = 0; i < ConstantsCount; i++)
                {
                    Constant Constant = Constants[i];
                    Console.Write("  {0}: ", i);
                    PrintConstant(i);
                    Console.WriteLine();
                }
            }
            JVMSharp.AccessFlags.PrintAccessFlags(bShowRawAccessFlags, ElementType.Class, AccessFlags);
            string ThisClassString1 = ProcessTypeString(ThisClassString);
            Console.Write("class {0} ", ThisClassString1);
            if (SuperClassString != "java/lang/Object")
            {
                string SuperClassString1 = ProcessTypeString(SuperClassString);
                Console.Write("extends {0}", SuperClassString1);
            }
            int InterfaceClassCount = InterfaceClassStrings.Count;
            if (InterfaceClassCount > 0)
            {
                Console.Write("implements ");
                for (int i = 0; i < InterfaceClassCount; i++)
                {
                    string InterfaceClassString = InterfaceClassStrings[i];
                    string InterfaceClassString1 = ProcessTypeString(InterfaceClassString);
                    if (i != InterfaceClassCount - 1)
                    {
                        Console.Write("{0}, ", InterfaceClassString1);
                    }
                    else
                    {
                        Console.Write("{0}", InterfaceClassString1);
                    }
                }
            }
            Console.WriteLine();
            if (bShowAttributes)
            {
                AttributeInfoHelper.PrintAttributes(2, Attributes);
            }
            Console.WriteLine("{");
            PrintMembers(ElementType.Field, FieldInfos);
            PrintMembers(ElementType.Method, MethodInfos);
            Console.WriteLine("}");
        }
    }
}
