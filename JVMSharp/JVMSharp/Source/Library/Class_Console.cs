using System;

namespace JVMSharp
{
    class Class_Console
    {
        public static void println_String(Frame CurrentFrame)
        {
            string String = (string)CurrentFrame.GetLocal(0);
            Console.WriteLine(String);
        }

        public static void println_int(Frame CurrentFrame)
        {
            int String = (int)CurrentFrame.GetLocal(0);
            Console.WriteLine(String.ToString());
        }

        public static void RegisterNativeMethods()
        {
            string ClassName = "Console";
            NativeMethods.GetInstance().RegisterNativeMethod(ClassName, "println", "(Ljava/lang/String;)V", println_String);
            NativeMethods.GetInstance().RegisterNativeMethod(ClassName, "println", "(I)V", println_int);
        }
    }
}
