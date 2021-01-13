using System.Collections.Generic;

namespace JVMSharp
{
    class Slots
    {
        public List<object> Values;

        public Slots(int MaxValues)
        {
            Values = new List<object>(MaxValues);
            Values.Resize(MaxValues);
        }
    }
}
