using System.Collections.Generic;

namespace JVMSharp
{
    delegate void NativeMethod(Frame CurrentFrame);

    class NativeMethods
    {
        static NativeMethods Instance = new NativeMethods();

        Dictionary<string, NativeMethod> NativeMethodsMap;

        public static NativeMethods GetInstance()
        {
            return Instance;
        }

        NativeMethods()
        {
            NativeMethodsMap = new Dictionary<string, NativeMethod>();
        }

        public void RegisterNativeMethod(string ClassName, string MethodName, string MethodDescriptor, NativeMethod NativeMethod)
        {
            string Key = string.Format("{0}-{1}-{2}", ClassName, MethodName, MethodDescriptor);
            NativeMethodsMap[Key] = NativeMethod;
        }

        public NativeMethod FindNativeMethod(string ClassName, string MethodName, string MethodDescriptor)
        {
            string Key = string.Format("{0}-{1}-{2}", ClassName, MethodName, MethodDescriptor);
            NativeMethod NativeMethod;
            NativeMethodsMap.TryGetValue(Key, out NativeMethod);
            return NativeMethod;
        }
    }
}
