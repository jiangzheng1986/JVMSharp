namespace JVMSharp
{
    class Field : Member
    {
        public bool bStatic;
        public ushort FieldIndex;
        public ushort ConstValueIndex;

        public Field(Class Class, MemberInfo FieldInfo)
            : base(Class, FieldInfo)
        {
            bStatic = (AccessFlags_ & AccessFlags.Static) != 0;
            FieldIndex = 0;
            ConstValueIndex = 0;
        }

        public bool IsLongOrDouble()
        {
            return DescriptorString == "D" || DescriptorString == "J";
        }
    }
}
