using System.Diagnostics;

namespace JVMSharp
{
    public static class DebugHelper
    {
        public static void Assert(bool bCondition)
        {
            if (bCondition == false)
            {
                Debug.Assert(bCondition);
            }
        }
    }
}
