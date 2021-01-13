using System.Collections.Generic;

namespace JVMSharp
{
    class Class
    {
        public ClassInfo ClassInfo;

        public Class SuperClass;

        public List<Constant> Constants;

        public List<Field> Fields;
        public List<Method> Methods;

        public ushort StaticFieldsCount;
        public ushort MemberFieldsCount;

        public Slots StaticVariables;

        public Class(ClassInfo ClassInfo)
        {
            this.ClassInfo = ClassInfo;

            string SuperClassString = ClassInfo.SuperClassString;
            SuperClass = ClassLoader.GetInstance().LoadClass(SuperClassString);

            StaticFieldsCount = 0;
            MemberFieldsCount = 0;

            Constants = ClassInfo.Constants;

            List<MemberInfo> FieldInfos = ClassInfo.FieldInfos;
            Fields = new List<Field>(FieldInfos.Count);
            ushort StaticFieldIndex = 0;
            ushort MemberFieldIndex = 0;
            foreach (MemberInfo FieldInfo in FieldInfos)
            {
                Field Field = new Field(this, FieldInfo);
                bool bIsLongOrDouble = Field.IsLongOrDouble();
                if (Field.bStatic)
                {
                    Field.FieldIndex = StaticFieldIndex;
                    StaticFieldIndex++;
                    if (bIsLongOrDouble)
                    {
                        StaticFieldIndex++;
                    }
                }
                else
                {
                    Field.FieldIndex = MemberFieldIndex;
                    MemberFieldIndex++;
                    if (bIsLongOrDouble)
                    {
                        MemberFieldIndex++;
                    }
                }
                Fields.Add(Field);
            }
            StaticFieldsCount = StaticFieldIndex;
            MemberFieldsCount = MemberFieldIndex;

            StaticVariables = new Slots(StaticFieldsCount);

            List<MemberInfo> MethodInfos = ClassInfo.MethodInfos;
            Methods = new List<Method>(MethodInfos.Count);
            foreach (MemberInfo MethodInfo in MethodInfos)
            {
                Method Method = new Method(this, MethodInfo);
                Methods.Add(Method);
            }
        }

        public string ConstantIndexToString(ushort ConstantIndex)
        {
            Utf8Constant Utf8Constant = (Utf8Constant)Constants[ConstantIndex];
            return Utf8Constant.Utf8;
        }

        public string ClassIndexToClassName(ushort ClassIndex)
        {
            ClassRefConstant ClassRefConstant = (ClassRefConstant)Constants[ClassIndex];
            ushort ClassNameIndex = ClassRefConstant.NameIndex;
            return ConstantIndexToString(ClassNameIndex);
        }

        public Field FindField(string NameString, string DescriptorString)
        {
            foreach (Field Field in Fields)
            {
                if (Field.NameString == NameString && Field.DescriptorString == DescriptorString)
                {
                    return Field;
                }
            }
            return null;
        }

        public Method FindMethod(string NameString, string DescriptorString)
        {
            foreach (Method Method in Methods)
            {
                if (Method.NameString == NameString && Method.DescriptorString == DescriptorString)
                {
                    return Method;
                }
            }
            return null;
        }

        public Method FindMethodRecursive(string NameString, string DescriptorString)
        {
            foreach (Method Method in Methods)
            {
                if (Method.NameString == NameString && Method.DescriptorString == DescriptorString)
                {
                    return Method;
                }
            }
            if (SuperClass != null)
            {
                return SuperClass.FindMethodRecursive(NameString, DescriptorString);
            }
            return null;
        }

        public Class FindClassByClassRef(int ConstantIndex)
        {
            ClassRefConstant ClassRefConstant = (ClassRefConstant)Constants[ConstantIndex];
            if (ClassRefConstant.CachedClass == null)
            {
                ushort ClassNameIndex = ClassRefConstant.NameIndex;
                string ClassName = ConstantIndexToString(ClassNameIndex);
                Class TargetClass = ClassLoader.GetInstance().LoadClass(ClassName);
                ClassRefConstant.CachedClass = TargetClass;
            }
            return ClassRefConstant.CachedClass;
        }

        public Field FindFieldByFieldRef(int ConstantIndex)
        {
            FieldRefConstant FieldRefConstant = (FieldRefConstant)Constants[ConstantIndex];
            if (FieldRefConstant.CachedField == null)
            {
                string ClassName = ClassIndexToClassName(FieldRefConstant.ClassIndex);
                NameAndTypeConstant NameAndTypeConstant = (NameAndTypeConstant)Constants[FieldRefConstant.NameAndTypeIndex];
                string FieldName = ConstantIndexToString(NameAndTypeConstant.NameIndex);
                string FieldDescriptor = ConstantIndexToString(NameAndTypeConstant.DescriptorIndex);
                Class TargetClass = ClassLoader.GetInstance().LoadClass(ClassName);
                FieldRefConstant.CachedField = TargetClass.FindField(FieldName, FieldDescriptor);
            }
            return FieldRefConstant.CachedField;
        }

        public Method FindMethodByMethodRef(int ConstantIndex)
        {
            MethodRefConstant MethodRefConstant = (MethodRefConstant)Constants[ConstantIndex];
            if (MethodRefConstant.CachedMethod == null)
            {
                string ClassName = ClassIndexToClassName(MethodRefConstant.ClassIndex);
                if (ClassName == "java/lang/Object")
                {
                    return null;
                }
                NameAndTypeConstant NameAndTypeConstant = (NameAndTypeConstant)Constants[MethodRefConstant.NameAndTypeIndex];
                string MethodName = ConstantIndexToString(NameAndTypeConstant.NameIndex);
                string MethodDescriptor = ConstantIndexToString(NameAndTypeConstant.DescriptorIndex);
                Class TargetClass = ClassLoader.GetInstance().LoadClass(ClassName);
                MethodRefConstant.CachedMethod = TargetClass.FindMethodRecursive(MethodName, MethodDescriptor);
                if (MethodRefConstant.CachedMethod == null)
                {
                    DebugHelper.Assert(false);
                }
            }
            return MethodRefConstant.CachedMethod;
        }

        public bool IsSubClassOrImplementOf(Class Class)
        {
            return IsSubClassOf(Class) || IsImplementOf(Class);
        }

        public bool IsSubClassOf(Class Class)
        {
            DebugHelper.Assert(Class != null);
            Class Class1 = this;
            while (true)
            {
                if (Class1 == Class)
                {
                    return true;
                }
                if (Class1 == null)
                {
                    return false;
                }
                Class1 = Class1.SuperClass;
            }
        }

        public bool IsImplementOf(Class Class)
        {
            return false; //TODO: Fix me!
        }

        public bool IsArray()
        {
            return ClassInfo.ThisClassString.Contains("[");
        }
    }
}
