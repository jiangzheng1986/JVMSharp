namespace JVMSharp
{ 
    class Member
    {
        public Class Class;
        public MemberInfo MemberInfo;

        public ushort AccessFlags_;
        public string NameString;
        public string DescriptorString;

        public Descriptor Descriptor;

        public Member(Class Class, MemberInfo MemberInfo)
        {
            this.Class = Class;
            this.MemberInfo = MemberInfo;

            AccessFlags_ = MemberInfo.AccessFlags;
            NameString = MemberInfo.NameString;
            DescriptorString = MemberInfo.DescriptorString;

            Descriptor = new Descriptor();
            Descriptor.Parse(DescriptorString);
        }

        public bool IsStatic()
        {
            return (AccessFlags_ & AccessFlags.Static) != 0;
        }
    }
}
