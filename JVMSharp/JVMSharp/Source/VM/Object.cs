namespace JVMSharp
{
    class Object
    {
        public Class Class;
        public Slots Fields;
        public object Array;

        public Object(Class Class)
        {
            this.Class = Class;
            int MemberFieldsCount = Class.MemberFieldsCount;
            Fields = new Slots(MemberFieldsCount);
            Array = null;
        }

        public bool IsInstanceOf(Class InClass)
        {
            return InClass.IsSubClassOrImplementOf(Class);
        }

        public void SetArrayLength(int ArrayLength)
        {
            if (Fields.Values.Count <= 0)
            {
                Fields.Values.Resize(1);
            }
            Fields.Values[0] = ArrayLength;
        }

        public int GetArrayLength()
        {
            return (int)Fields.Values[0];
        }
    }
}
