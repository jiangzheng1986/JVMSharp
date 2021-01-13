namespace JVMSharp
{
    class Constant
    {
    }

    class Utf8Constant : Constant
    {
        public string Utf8;
    }

    class IntConstant : Constant
    {
        public int Int;
    }

    class FloatConstant : Constant
    {
        public float Float;
    }

    class LongConstant : Constant
    {
        public long Long;
    }

    class DoubleConstant : Constant
    {
        public double Double;
    }

    class ClassRefConstant : Constant
    {
        public ushort NameIndex;

        public Class CachedClass;
    }

    class StringConstant : Constant
    {
        public ushort StringIndex;
    }

    class SymbolRefConstant : Constant
    {
        public ushort ClassIndex;
        public ushort NameAndTypeIndex;
    }

    class FieldRefConstant : SymbolRefConstant
    {
        public Field CachedField;
    }

    class MethodRefConstant : SymbolRefConstant
    {
        public Method CachedMethod;
    }

    class InterfaceMethodRefConstant : SymbolRefConstant
    {
    }

    class NameAndTypeConstant : Constant
    {
        public ushort NameIndex;
        public ushort DescriptorIndex;
    }

    class MethodHandleConstant : Constant
    {
        public byte ReferenceKind;
        public ushort ReferenceIndex;
    }

    class MethodTypeConstant : Constant
    {
        public ushort DescriptorIndex;
    }

    class InvokeDynamicConstant : Constant
    {
        public ushort BootstrapMethodAttrIndex;
        public ushort NameAndTypeIndex;
    }
}
