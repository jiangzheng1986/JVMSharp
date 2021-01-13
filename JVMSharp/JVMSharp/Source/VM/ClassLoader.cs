using System.Collections.Generic;

namespace JVMSharp
{
    class ClassLoader
    {
        static ClassLoader Instance = new ClassLoader();

        Dictionary<string, Class> Classes;

        public static ClassLoader GetInstance()
        {
            return Instance;
        }

        public ClassLoader()
        {
            Classes = new Dictionary<string, Class>();
        }

        public void AddClassPath(string ClassPath)
        {
            ClassInfoLoader.GetInstance().AddClassPath(ClassPath);
        }

        public Class LoadClass(string ClassName)
        {
            string ClassName1 = ClassName.Replace('/', '.');
            Class Class = null;
            if (Classes.TryGetValue(ClassName1, out Class))
            {
                return Class;
            }
            ClassInfo ClassInfo = ClassInfoLoader.GetInstance().LoadClass(ClassName1);
            if (ClassInfo == null)
            {
                return null;
            }
            Class = new Class(ClassInfo);
            Classes[ClassName1] = Class;
            return Class;
        }

        public Class CreateArrayClass(string ArrayClassName)
        {
            string ArrayClassName1 = ArrayClassName.Replace('/', '.');
            Class Class = null;
            if (Classes.TryGetValue(ArrayClassName1, out Class))
            {
                return Class;
            }
            ClassInfo ClassInfo = new ClassInfo();
            ClassInfo.ThisClassString = ArrayClassName1;
            ClassInfo.SuperClassString = "Ljava/lang/Object";
            Class = new Class(ClassInfo);
            Classes[ArrayClassName1] = Class;
            return Class;
        }
    }
}
