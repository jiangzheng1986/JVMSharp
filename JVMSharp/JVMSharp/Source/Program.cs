using System;

//TODO:

//异常处理
//  athrow
//  数组索引越界

//面向对象
//  字段继承
//  父类的初始化
//  new,putstatic,getstatic,invokestatic时类的初始化
//基础类库
//  字符串支持
//  反射
//接口
//  invokeinterface搜索接口
//  接口的instanceof判断
//多维数组
//支持jar格式
//重定向输出到文件

namespace JVMSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Java!");

            DirectoryHelper.SetupWorkingDirectory();

            Class_Console.RegisterNativeMethods();

            ClassLoader ClassLoader = ClassLoader.GetInstance();
            ClassLoader.AddClassPath("../../../Test");
            //Class Class = ClassLoader.LoadClass("FibonacciTest");
            //Class Class = ClassLoader.LoadClass("InvokeTest");
            //Class Class = ClassLoader.LoadClass("ConsoleTest");
            //Class Class = ClassLoader.LoadClass("TableSwitchTest");
            Class Class = ClassLoader.LoadClass("LookupSwitchTest");
            //Class Class = ClassLoader.LoadClass("ArrayTest");

            Method MethodMain = Class.FindMethod("main", "()V");

            Thread Thread = new Thread();
            Thread.Execute(MethodMain);

            Console.WriteLine("Done.");

            Console.ReadKey();
        }
    }
}
