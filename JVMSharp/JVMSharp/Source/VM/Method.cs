namespace JVMSharp
{
    class Method : Member
    {
        public ushort MaxStack;
        public ushort MaxLocals;
        public int ArgumentSlotCount;
        public byte[] Code;
        public NativeMethod NativeMethod;

        public Method(Class Class, MemberInfo MethodInfo)
            : base(Class, MethodInfo)
        {
            int AttributesCount = MethodInfo.Attributes.Count;
            for (int i = 0; i < AttributesCount; i++)
            {
                AttributeInfo AttributeInfo = MethodInfo.Attributes[i];
                string AttributeNameString = AttributeInfo.AttributeNameString;
                if (AttributeNameString == AttributeInfo.AttributeName_Code)
                {
                    AttributeInfo_Code AttributeInfo_Code = (AttributeInfo_Code)AttributeInfo;
                    MaxStack = AttributeInfo_Code.MaxStack;
                    MaxLocals = AttributeInfo_Code.MaxLocals;
                    Code = AttributeInfo_Code.Code;
                    break;
                }
            }
            bool bIsStatic = IsStatic();
            ArgumentSlotCount = Descriptor.CalculateArgumentSlotCount(bIsStatic);
            bool bIsNative = IsNative();
            if (bIsNative)
            {
                InjectCodeForNativeMethod();
            }
        }

        public bool IsNative()
        {
            return (AccessFlags_ & AccessFlags.Native) != 0;
        }

        public void InjectCodeForNativeMethod()
        {
            MaxStack = 4;
            MaxLocals = (ushort)ArgumentSlotCount;
            Code = new byte[2];
            Code[0] = (byte)Opcode.impdep1;
            string ReturnType = Descriptor.MemberType;
            switch (ReturnType)
            {
                case "void":
                    Code[1] = (byte)Opcode.return_;
                    break;
                case "boolean":
                case "byte":
                case "char":
                case "short":
                case "int":
                    Code[1] = (byte)Opcode.ireturn;
                    break;
                case "long":
                    Code[1] = (byte)Opcode.lreturn;
                    break;
                case "float":
                    Code[1] = (byte)Opcode.freturn;
                    break;
                case "double":
                    Code[1] = (byte)Opcode.dreturn;
                    break;
                default:
                    Code[1] = (byte)Opcode.areturn;
                    break;
            }
        }
    }
}
