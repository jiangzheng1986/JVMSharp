using System.Collections.Generic;

namespace JVMSharp
{
    class ClassInfoLoader
    {
        static ClassInfoLoader Instance = new ClassInfoLoader();

        List<string> ClassPathList;
        Dictionary<string, ClassInfo> ClassInfos;

        public static ClassInfoLoader GetInstance()
        {
            return Instance;
        }

        public ClassInfoLoader()
        {
            ClassPathList = new List<string>();
            ClassInfos = new Dictionary<string, ClassInfo>();
        }

        public void AddClassPath(string ClassPath)
        {
            ClassPathList.Add(ClassPath);
        }

        public ClassInfo LoadClass(string ClassName)
        {
            string ClassName1 = ClassName.Replace('/', '.');
            ClassInfo ClassInfo;
            if (ClassInfos.TryGetValue(ClassName1, out ClassInfo))
            {
                return ClassInfo;
            }
            string ClassNamePath = "/" + ClassName1.Replace('.', '/') + ".class";
            foreach (string ClassPath in ClassPathList)
            {
                string Filename = ClassPath + ClassNamePath;
                if (FileHelper.IsFileExists(Filename))
                {
                    ClassInfo = new ClassInfo();
                    ClassInfo.Load(Filename);
                    ClassInfos[ClassName1] = ClassInfo;
                    return ClassInfo;
                }
            }
            return null;
        }
    }
}
