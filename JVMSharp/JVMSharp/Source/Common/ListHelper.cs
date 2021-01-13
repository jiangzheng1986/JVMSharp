using System.Collections.Generic;
using System.Linq;

namespace JVMSharp
{
    public static class ListHelper
    {
        public static void Resize<T>(this List<T> List, int Size, T Element = default(T))
        {
            int Count = List.Count;
            if (Size < Count)
            {
                List.RemoveRange(Size, Count - Size);
            }
            else if (Size > Count)
            {
                if (Size > List.Capacity)
                {
                    List.Capacity = Size;
                }
                List.AddRange(Enumerable.Repeat(Element, Size - Count));
            }
        }

        public static void Reserve<T>(this List<T> List, int Size)
        {
            if (Size > List.Capacity)
            {
                List.Capacity = Size;
            }
        }

        public static List<T> Clone<T>(this List<T> List)
        {
            List<T> List1 = new List<T>();
            List1.Capacity = List.Count;
            foreach (T t in List)
            {
                List1.Add(t);
            }
            return List1;
        }

        public static void Push<T>(this List<T> List, T t)
        {
            List.Add(t);
        }
        public static T Pop<T>(this List<T> List)
        {
            DebugHelper.Assert(List.Count > 0);
            int Index = List.Count - 1;
            T t = List[Index];
            List.RemoveAt(Index);
            return t;
        }
    }
}
