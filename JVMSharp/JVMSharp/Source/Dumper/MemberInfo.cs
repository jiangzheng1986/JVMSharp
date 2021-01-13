using System.Collections.Generic;

namespace JVMSharp
{
    class MemberInfo
    {
        public ushort AccessFlags;
        public ushort NameIndex;
        public string NameString;
        public ushort DescriptorIndex;
        public string DescriptorString;
        public List<AttributeInfo> Attributes;
    }
}
