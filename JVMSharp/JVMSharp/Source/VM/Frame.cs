using System.Collections.Generic;

namespace JVMSharp
{
    class Frame
    {
        public List<object> LocalVariables;
        public List<object> OperandStack;
        public Method Method;
        public uint ProgramCounter;

        public Frame(Method Method)
        {
            this.Method = Method;
            int MaxLocals = 0;
            int MaxStack = 0;
            if (Method != null)
            {
                MaxLocals = Method.MaxLocals;
                MaxStack = Method.MaxStack;
            }
            LocalVariables = new List<object>(MaxLocals);
            LocalVariables.Resize(MaxLocals);
            OperandStack = new List<object>(MaxStack);
            ProgramCounter = 0;
        }

        public void SetLocalInt(int Index, int Value)
        {
            LocalVariables[Index] = Value;
        }

        public int GetLocalInt(int Index)
        {
            return (int)LocalVariables[Index];
        }

        public void SetLocalLong(int Index, long Value)
        {
            LocalVariables[Index] = Value;
        }

        public long GetLocalLong(int Index)
        {
            return (long)LocalVariables[Index];
        }

        public void SetLocalFloat(int Index, float Value)
        {
            LocalVariables[Index] = Value;
        }

        public float GetLocalFloat(int Index)
        {
            return (float)LocalVariables[Index];
        }

        public void SetLocalDouble(int Index, double Value)
        {
            LocalVariables[Index] = Value;
        }

        public double GetLocalDouble(int Index)
        {
            return (double)LocalVariables[Index];
        }

        public void SetLocalReference(int Index, object Object)
        {
            LocalVariables[Index] = Object;
        }

        public object GetLocalReference(int Index)
        {
            return LocalVariables[Index];
        }

        public void SetLocal(int Index, object Object)
        {
            LocalVariables[Index] = Object;
        }

        public object GetLocal(int Index)
        {
            return LocalVariables[Index];
        }

        public void PushOperandInt(int Value)
        {
            OperandStack.Add(Value);
        }

        public int PopOperandInt()
        {
            return (int)PopOperand1();
        }

        public void PushOperandLong(long Value)
        {
            OperandStack.Add(Value);
            OperandStack.Add(null);
        }

        public long PopOperandLong()
        {
            PopOperand1();
            return (long)PopOperand1();
        }

        public void PushOperandFloat(float Value)
        {
            OperandStack.Add(Value);
        }

        public float PopOperandFloat()
        {
            return (float)PopOperand1();
        }

        public void PushOperandDouble(double Value)
        {
            OperandStack.Add(Value);
            OperandStack.Add(null);
        }

        public double PopOperandDouble()
        {
            PopOperand1();
            return (double)PopOperand1();
        }

        public void PushOperandReference(object Object)
        {
            OperandStack.Add(Object);
        }

        public object PopOperandReference()
        {
            return PopOperand1();
        }

        public object GetReferenceFromTop(int N)
        {
            return OperandStack[OperandStack.Count - 1 - N];
        }

        public void PushOperand1(object Value)
        {
            OperandStack.Add(Value);
        }

        public void PushOperand2(object Value)
        {
            OperandStack.Add(Value);
            OperandStack.Add(null);
        }

        public object PopOperand1()
        {
            int Index = OperandStack.Count - 1;
            object Value = OperandStack[Index];
            OperandStack.RemoveAt(Index);
            return Value;
        }

        public object PopOperand2()
        {
            PopOperand1();
            return PopOperand1();
        }

        public void DupOperand()
        {
            object Top = PopOperand1();
            OperandStack.Add(Top);
            OperandStack.Add(Top);
        }

        public void DupOperandX1()
        {
            object Top1 = PopOperand1();
            object Top2 = PopOperand1();
            OperandStack.Add(Top1);
            OperandStack.Add(Top2);
            OperandStack.Add(Top1);
        }

        public void DupOperandX2()
        {
            object Top1 = PopOperand1();
            object Top2 = PopOperand1();
            object Top3 = PopOperand1();
            OperandStack.Add(Top1);
            OperandStack.Add(Top3);
            OperandStack.Add(Top2);
            OperandStack.Add(Top1);
        }

        public void DupOperand2()
        {
            PopOperand1();
            object Top = PopOperand1();
            OperandStack.Add(Top);
            OperandStack.Add(null);
            OperandStack.Add(Top);
            OperandStack.Add(null);
        }

        public void DupOperand2X1()
        {
            object Top1_1 = PopOperand1();
            object Top1_2 = PopOperand1();
            object Top2 = PopOperand1();
            OperandStack.Add(Top1_2);
            OperandStack.Add(Top1_1);
            OperandStack.Add(Top2);
            OperandStack.Add(Top1_2);
            OperandStack.Add(Top1_1);
        }

        public void DupOperand2X2()
        {
            object Top1_1 = PopOperand1();
            object Top1_2 = PopOperand1();
            object Top2_1 = PopOperand1();
            object Top2_2 = PopOperand1();
            OperandStack.Add(Top1_2);
            OperandStack.Add(Top1_1);
            OperandStack.Add(Top2_2);
            OperandStack.Add(Top2_1);
            OperandStack.Add(Top1_2);
            OperandStack.Add(Top1_1);
        }

        public void SwapOperands()
        {
            object Top1 = PopOperand1();
            object Top2 = PopOperand1();
            OperandStack.Add(Top1);
            OperandStack.Add(Top2);
        }
    }
}
