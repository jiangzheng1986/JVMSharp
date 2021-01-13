using System.Collections.Generic;

namespace JVMSharp
{
    class Thread
    {
        public Stack<Frame> FrameStack;
        public Frame CurrentFrame;
        public Method CurrentMethod;
        public Class CurrentClass;
        public byte[] CurrentCode;
        public uint ProgramCounter;
        public bool bRunning;

        public List<int> Cases;       // for optimization of lookupswitch
        public List<int> Offsets;     // for optimization of lookupswitch and tableswitch

        public Thread()
        {
            FrameStack = new Stack<Frame>();
            CurrentFrame = new Frame(null);
            CurrentMethod = null;
            CurrentClass = null;
            CurrentCode = null;
            ProgramCounter = 0;
            bRunning = false;
            Cases = new List<int>();
            Offsets = new List<int>();
        }

        void Call(Method Method)
        {
            Frame FrameInvoker = CurrentFrame;
            CurrentFrame.ProgramCounter = ProgramCounter;
            FrameStack.Push(CurrentFrame);
            Frame FrameNew = new Frame(Method);
            CurrentFrame = FrameNew;
            CurrentMethod = Method;
            CurrentClass = Method.Class;
            CurrentCode = FrameNew.Method.Code;
            ProgramCounter = 0;
            int ArgumentSlotCount = CurrentMethod.ArgumentSlotCount;
            if (ArgumentSlotCount > 0)
            {
                for (int i = ArgumentSlotCount - 1; i >= 0; i--)
                {
                    object Value = FrameInvoker.PopOperand1();
                    CurrentFrame.SetLocal(i, Value);
                }
            }
        }

        void ReturnInt()
        {
            int Value = CurrentFrame.PopOperandInt();
            Return();
            CurrentFrame.PushOperandInt(Value);
        }

        void ReturnLong()
        {
            long Value = CurrentFrame.PopOperandLong();
            Return();
            CurrentFrame.PushOperandLong(Value);
        }

        void ReturnFloat()
        {
            float Value = CurrentFrame.PopOperandFloat();
            Return();
            CurrentFrame.PushOperandFloat(Value);
        }

        void ReturnDouble()
        {
            double Value = CurrentFrame.PopOperandDouble();
            Return();
            CurrentFrame.PushOperandDouble(Value);
        }

        void ReturnReference()
        {
            object Object = CurrentFrame.PopOperandReference();
            Return();
            CurrentFrame.PushOperandReference(Object);
        }

        void Return()
        {
            Frame FrameLast = FrameStack.Pop();
            CurrentFrame = FrameLast;
            Method Method = FrameLast.Method;
            if (Method == null)
            {
                bRunning = false;
                CurrentMethod = null;
                CurrentClass = null;
                CurrentCode = null;
            }
            else
            {
                CurrentMethod = FrameLast.Method;
                CurrentClass = CurrentMethod.Class;
                CurrentCode = CurrentMethod.Code;
            }
            ProgramCounter = FrameLast.ProgramCounter;
        }

        byte FetchByte()
        { 
            return CurrentCode[ProgramCounter++];
        }

        ushort FetchUShort()
        {
            uint Byte1 = CurrentCode[ProgramCounter++];
            uint Byte2 = CurrentCode[ProgramCounter++];
            return (ushort)((Byte1 << 8) | Byte2);
        }

        short FetchOffset()
        {
            uint Byte1 = CurrentCode[ProgramCounter++];
            uint Byte2 = CurrentCode[ProgramCounter++];
            return (short)(ushort)((Byte1 << 8) | Byte2);
        }

        void JumpOffset(uint SavedProgramCounter, int Offset)
        {
            ProgramCounter = (uint)(SavedProgramCounter + Offset);
        }

        int FetchInt()
        {
            uint Byte1 = CurrentCode[ProgramCounter++];
            uint Byte2 = CurrentCode[ProgramCounter++];
            uint Byte3 = CurrentCode[ProgramCounter++];
            uint Byte4 = CurrentCode[ProgramCounter++];
            return (int)((Byte1 << 24) | (Byte2 << 16) | (Byte3 << 8) | Byte4);
        }

        void PaddingTo4BytesAligned()
        {
            ProgramCounter = (ProgramCounter + 3) & 0xFFFFFFFC;
        }

        void Execute_ldc(ushort ConstantIndex)
        {
            Constant Constant = CurrentClass.Constants[ConstantIndex];
            if (Constant is Utf8Constant)
            {
                Utf8Constant Utf8Constant = (Utf8Constant)Constant;
                CurrentFrame.PushOperandReference(Utf8Constant.Utf8);
            }
            else if (Constant is IntConstant)
            {
                IntConstant IntConstant = (IntConstant)Constant;
                CurrentFrame.PushOperandInt(IntConstant.Int);
            }
            else if (Constant is StringConstant)
            {
                StringConstant StringConstant = (StringConstant)Constant;
                ushort StringIndex = StringConstant.StringIndex;
                string String = CurrentClass.ConstantIndexToString(StringIndex);
                CurrentFrame.PushOperandReference(String);
            }
            //else if (ClassRefConstant) //TODO: Fix me!
            //{
            //  CurrentFrame.PushOperandReference(Object);
            //}
        }

        void Execute_ldc2(ushort ConstantIndex)
        {
            Constant Constant = CurrentClass.Constants[ConstantIndex];
            if (Constant is LongConstant)
            {
                LongConstant LongConstant = (LongConstant)Constant;
                CurrentFrame.PushOperandLong(LongConstant.Long);
            }
            else if (Constant is DoubleConstant)
            {
                DoubleConstant DoubleConstant = (DoubleConstant)Constant;
                CurrentFrame.PushOperandDouble(DoubleConstant.Double);
            }
        }

        public void Execute(Method Method)
        {
            bRunning = true;
            Call(Method);
            while (bRunning)
            {
                uint SavedProgramCounter = ProgramCounter;
                Opcode Opcode1 = (Opcode)FetchByte();
                switch (Opcode1)
                {
                    case Opcode.nop:
                        break;
                    case Opcode.aconst_null:
                        CurrentFrame.PushOperandReference(null);
                        break;
                    case Opcode.iconst_m1:
                        CurrentFrame.PushOperandInt(-1);
                        break;
                    case Opcode.iconst_0:
                        CurrentFrame.PushOperandInt(0);
                        break;
                    case Opcode.iconst_1:
                        CurrentFrame.PushOperandInt(1);
                        break;
                    case Opcode.iconst_2:
                        CurrentFrame.PushOperandInt(2);
                        break;
                    case Opcode.iconst_3:
                        CurrentFrame.PushOperandInt(3);
                        break;
                    case Opcode.iconst_4:
                        CurrentFrame.PushOperandInt(4);
                        break;
                    case Opcode.iconst_5:
                        CurrentFrame.PushOperandInt(5);
                        break;
                    case Opcode.lconst_0:
                        CurrentFrame.PushOperandLong(0L);
                        break;
                    case Opcode.lconst_1:
                        CurrentFrame.PushOperandLong(1L);
                        break;
                    case Opcode.fconst_0:
                        CurrentFrame.PushOperandFloat(0.0f);
                        break;
                    case Opcode.fconst_1:
                        CurrentFrame.PushOperandFloat(1.0f);
                        break;
                    case Opcode.fconst_2:
                        CurrentFrame.PushOperandFloat(2.0f);
                        break;
                    case Opcode.dconst_0:
                        CurrentFrame.PushOperandDouble(0.0);
                        break;
                    case Opcode.dconst_1:
                        CurrentFrame.PushOperandDouble(1.0);
                        break;
                    case Opcode.bipush:
                        CurrentFrame.PushOperandInt((sbyte)FetchByte());
                        break;
                    case Opcode.sipush:
                        CurrentFrame.PushOperandInt((short)FetchUShort());
                        break;
                    case Opcode.ldc:
                        {
                            ushort ConstantIndex = (ushort)FetchByte();
                            Execute_ldc(ConstantIndex);
                        }
                        break;
                    case Opcode.ldc_w:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Execute_ldc(ConstantIndex);
                        }
                        break;
                    case Opcode.ldc2_w:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Execute_ldc2(ConstantIndex);
                        }
                        break;
                    case Opcode.iload:
                        CurrentFrame.PushOperandInt(CurrentFrame.GetLocalInt(FetchByte()));
                        break;
                    case Opcode.lload:
                        CurrentFrame.PushOperandLong(CurrentFrame.GetLocalLong(FetchByte()));
                        break;
                    case Opcode.fload:
                        CurrentFrame.PushOperandFloat(CurrentFrame.GetLocalFloat(FetchByte()));
                        break;
                    case Opcode.dload:
                        CurrentFrame.PushOperandDouble(CurrentFrame.GetLocalDouble(FetchByte()));
                        break;
                    case Opcode.aload:
                        CurrentFrame.PushOperandReference(CurrentFrame.GetLocalReference(FetchByte()));
                        break;
                    case Opcode.iload_0:
                        CurrentFrame.PushOperandInt(CurrentFrame.GetLocalInt(0));
                        break;
                    case Opcode.iload_1:
                        CurrentFrame.PushOperandInt(CurrentFrame.GetLocalInt(1));
                        break;
                    case Opcode.iload_2:
                        CurrentFrame.PushOperandInt(CurrentFrame.GetLocalInt(2));
                        break;
                    case Opcode.iload_3:
                        CurrentFrame.PushOperandInt(CurrentFrame.GetLocalInt(3));
                        break;
                    case Opcode.lload_0:
                        CurrentFrame.PushOperandLong(CurrentFrame.GetLocalLong(0));
                        break;
                    case Opcode.lload_1:
                        CurrentFrame.PushOperandLong(CurrentFrame.GetLocalLong(1));
                        break;
                    case Opcode.lload_2:
                        CurrentFrame.PushOperandLong(CurrentFrame.GetLocalLong(2));
                        break;
                    case Opcode.lload_3:
                        CurrentFrame.PushOperandLong(CurrentFrame.GetLocalLong(3));
                        break;
                    case Opcode.fload_0:
                        CurrentFrame.PushOperandFloat(CurrentFrame.GetLocalFloat(0));
                        break;
                    case Opcode.fload_1:
                        CurrentFrame.PushOperandFloat(CurrentFrame.GetLocalFloat(1));
                        break;
                    case Opcode.fload_2:
                        CurrentFrame.PushOperandFloat(CurrentFrame.GetLocalFloat(2));
                        break;
                    case Opcode.fload_3:
                        CurrentFrame.PushOperandFloat(CurrentFrame.GetLocalFloat(3));
                        break;
                    case Opcode.dload_0:
                        CurrentFrame.PushOperandDouble(CurrentFrame.GetLocalDouble(0));
                        break;
                    case Opcode.dload_1:
                        CurrentFrame.PushOperandDouble(CurrentFrame.GetLocalDouble(1));
                        break;
                    case Opcode.dload_2:
                        CurrentFrame.PushOperandDouble(CurrentFrame.GetLocalDouble(2));
                        break;
                    case Opcode.dload_3:
                        CurrentFrame.PushOperandDouble(CurrentFrame.GetLocalDouble(3));
                        break;
                    case Opcode.aload_0:
                        CurrentFrame.PushOperandReference(CurrentFrame.GetLocalReference(0));
                        break;
                    case Opcode.aload_1:
                        CurrentFrame.PushOperandReference(CurrentFrame.GetLocalReference(1));
                        break;
                    case Opcode.aload_2:
                        CurrentFrame.PushOperandReference(CurrentFrame.GetLocalReference(2));
                        break;
                    case Opcode.aload_3:
                        CurrentFrame.PushOperandReference(CurrentFrame.GetLocalReference(3));
                        break;
                    case Opcode.iaload:
                        {
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            int[] Array = (int[])ArrayObject.Array;
                            int Value = Array[Index];
                            CurrentFrame.PushOperandInt(Value);
                        }
                        break;
                    case Opcode.laload:
                        {
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            long[] Array = (long[])ArrayObject.Array;
                            long Value = Array[Index];
                            CurrentFrame.PushOperandLong(Value);
                        }
                        break;
                    case Opcode.faload:
                        {
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            float[] Array = (float[])ArrayObject.Array;
                            float Value = Array[Index];
                            CurrentFrame.PushOperandFloat(Value);
                        }
                        break;
                    case Opcode.daload:
                        {
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            double[] Array = (double[])ArrayObject.Array;
                            double Value = Array[Index];
                            CurrentFrame.PushOperandDouble(Value);
                        }
                        break;
                    case Opcode.aaload:
                        {
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            object[] Array = (object[])ArrayObject.Array;
                            object Value = Array[Index];
                            CurrentFrame.PushOperandReference(Value);
                        }
                        break;
                    case Opcode.baload:
                        {
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            int Value = 0;
                            if (ArrayObject.Array is bool[])
                            {
                                bool[] Array = (bool[])ArrayObject.Array;
                                Value = Array[Index] ? 1 : 0;
                            }
                            else
                            {
                                byte[] Array = (byte[])ArrayObject.Array;
                                Value = Array[Index];
                            }
                            CurrentFrame.PushOperandInt(Value);
                        }
                        break;
                    case Opcode.caload:
                        {
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            char[] Array = (char[])ArrayObject.Array;
                            char Value = Array[Index];
                            CurrentFrame.PushOperandInt(Value);
                        }
                        break;
                    case Opcode.saload:
                        {
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            short[] Array = (short[])ArrayObject.Array;
                            short Value = Array[Index];
                            CurrentFrame.PushOperandInt(Value);
                        }
                        break;
                    case Opcode.istore:
                        CurrentFrame.SetLocalInt(FetchByte(), CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.lstore:
                        CurrentFrame.SetLocalLong(FetchByte(), CurrentFrame.PopOperandLong());
                        break;
                    case Opcode.fstore:
                        CurrentFrame.SetLocalFloat(FetchByte(), CurrentFrame.PopOperandFloat());
                        break;
                    case Opcode.dstore:
                        CurrentFrame.SetLocalDouble(FetchByte(), CurrentFrame.PopOperandDouble());
                        break;
                    case Opcode.astore:
                        CurrentFrame.SetLocalReference(FetchByte(), CurrentFrame.PopOperandReference());
                        break;
                    case Opcode.istore_0:
                        CurrentFrame.SetLocalInt(0, CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.istore_1:
                        CurrentFrame.SetLocalInt(1, CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.istore_2:
                        CurrentFrame.SetLocalInt(2, CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.istore_3:
                        CurrentFrame.SetLocalInt(3, CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.lstore_0:
                        CurrentFrame.SetLocalLong(0, CurrentFrame.PopOperandLong());
                        break;
                    case Opcode.lstore_1:
                        CurrentFrame.SetLocalLong(1, CurrentFrame.PopOperandLong());
                        break;
                    case Opcode.lstore_2:
                        CurrentFrame.SetLocalLong(2, CurrentFrame.PopOperandLong());
                        break;
                    case Opcode.lstore_3:
                        CurrentFrame.SetLocalLong(3, CurrentFrame.PopOperandLong());
                        break;
                    case Opcode.fstore_0:
                        CurrentFrame.SetLocalFloat(0, CurrentFrame.PopOperandFloat());
                        break;
                    case Opcode.fstore_1:
                        CurrentFrame.SetLocalFloat(1, CurrentFrame.PopOperandFloat());
                        break;
                    case Opcode.fstore_2:
                        CurrentFrame.SetLocalFloat(2, CurrentFrame.PopOperandFloat());
                        break;
                    case Opcode.fstore_3:
                        CurrentFrame.SetLocalFloat(3, CurrentFrame.PopOperandFloat());
                        break;
                    case Opcode.dstore_0:
                        CurrentFrame.SetLocalDouble(0, CurrentFrame.PopOperandDouble());
                        break;
                    case Opcode.dstore_1:
                        CurrentFrame.SetLocalDouble(1, CurrentFrame.PopOperandDouble());
                        break;
                    case Opcode.dstore_2:
                        CurrentFrame.SetLocalDouble(2, CurrentFrame.PopOperandDouble());
                        break;
                    case Opcode.dstore_3:
                        CurrentFrame.SetLocalDouble(3, CurrentFrame.PopOperandDouble());
                        break;
                    case Opcode.astore_0:
                        CurrentFrame.SetLocalReference(0, CurrentFrame.PopOperandReference());
                        break;
                    case Opcode.astore_1:
                        CurrentFrame.SetLocalReference(1, CurrentFrame.PopOperandReference());
                        break;
                    case Opcode.astore_2:
                        CurrentFrame.SetLocalReference(2, CurrentFrame.PopOperandReference());
                        break;
                    case Opcode.astore_3:
                        CurrentFrame.SetLocalReference(3, CurrentFrame.PopOperandReference());
                        break;
                    case Opcode.iastore:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            int[] Array = (int[])ArrayObject.Array;
                            Array[Index] = Value;
                        }
                        break;
                    case Opcode.lastore:
                        {
                            long Value = CurrentFrame.PopOperandLong();
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            long[] Array = (long[])ArrayObject.Array;
                            Array[Index] = Value;
                        }
                        break;
                    case Opcode.fastore:
                        {
                            float Value = CurrentFrame.PopOperandFloat();
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            float[] Array = (float[])ArrayObject.Array;
                            Array[Index] = Value;
                        }
                        break;
                    case Opcode.dastore:
                        {
                            double Value = CurrentFrame.PopOperandDouble();
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            double[] Array = (double[])ArrayObject.Array;
                            Array[Index] = Value;
                        }
                        break;
                    case Opcode.aastore:
                        {
                            object Value = CurrentFrame.PopOperandReference();
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            object[] Array = (object[])ArrayObject.Array;
                            Array[Index] = Value;
                        }
                        break;
                    case Opcode.bastore:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            if (ArrayObject.Array is bool[])
                            {
                                bool[] Array = (bool[])ArrayObject.Array;
                                Array[Index] = (Value != 0);
                            }
                            else
                            {
                                byte[] Array = (byte[])ArrayObject.Array;
                                Array[Index] = (byte)Value;
                            }
                        }
                        break;
                    case Opcode.castore:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            char[] Array = (char[])ArrayObject.Array;
                            Array[Index] = (char)Value;
                        }
                        break;
                    case Opcode.sastore:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            int Index = CurrentFrame.PopOperandInt();
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            if (Index < 0 || Index >= ArrayLength)
                            {
                                DebugHelper.Assert(false);
                            }
                            short[] Array = (short[])ArrayObject.Array;
                            Array[Index] = (short)Value;
                        }
                        break;
                    case Opcode.pop:
                        CurrentFrame.PopOperand1();
                        break;
                    case Opcode.pop2:
                        CurrentFrame.PopOperand2();
                        break;
                    case Opcode.dup:
                        CurrentFrame.DupOperand();
                        break;
                    case Opcode.dup_x1:
                        CurrentFrame.DupOperandX1();
                        break;
                    case Opcode.dup_x2:
                        CurrentFrame.DupOperandX2();
                        break;
                    case Opcode.dup2:
                        CurrentFrame.DupOperand2();
                        break;
                    case Opcode.dup2_x1:
                        CurrentFrame.DupOperand2X1();
                        break;
                    case Opcode.dup2_x2:
                        CurrentFrame.DupOperand2X2();
                        break;
                    case Opcode.swap:
                        CurrentFrame.SwapOperands();
                        break;
                    case Opcode.iadd:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            CurrentFrame.PushOperandInt(Value1 + Value2);
                        }
                        break;
                    case Opcode.ladd:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            CurrentFrame.PushOperandLong(Value1 + Value2);
                        }
                        break;
                    case Opcode.fadd:
                        {
                            float Value2 = CurrentFrame.PopOperandFloat();
                            float Value1 = CurrentFrame.PopOperandFloat();
                            CurrentFrame.PushOperandFloat(Value1 + Value2);
                        }
                        break;
                    case Opcode.dadd:
                        {
                            double Value2 = CurrentFrame.PopOperandDouble();
                            double Value1 = CurrentFrame.PopOperandDouble();
                            CurrentFrame.PushOperandDouble(Value1 + Value2);
                        }
                        break;
                    case Opcode.isub:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            CurrentFrame.PushOperandInt(Value1 - Value2);
                        }
                        break;
                    case Opcode.lsub:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            CurrentFrame.PushOperandLong(Value1 - Value2);
                        }
                        break;
                    case Opcode.fsub:
                        {
                            float Value2 = CurrentFrame.PopOperandFloat();
                            float Value1 = CurrentFrame.PopOperandFloat();
                            CurrentFrame.PushOperandFloat(Value1 - Value2);
                        }
                        break;
                    case Opcode.dsub:
                        {
                            double Value2 = CurrentFrame.PopOperandDouble();
                            double Value1 = CurrentFrame.PopOperandDouble();
                            CurrentFrame.PushOperandDouble(Value1 - Value2);
                        }
                        break;
                    case Opcode.imul:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            CurrentFrame.PushOperandInt(Value1 * Value2);
                        }
                        break;
                    case Opcode.lmul:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            CurrentFrame.PushOperandLong(Value1 * Value2);
                        }
                        break;
                    case Opcode.fmul:
                        {
                            float Value2 = CurrentFrame.PopOperandFloat();
                            float Value1 = CurrentFrame.PopOperandFloat();
                            CurrentFrame.PushOperandFloat(Value1 * Value2);
                        }
                        break;
                    case Opcode.dmul:
                        {
                            double Value2 = CurrentFrame.PopOperandDouble();
                            double Value1 = CurrentFrame.PopOperandDouble();
                            CurrentFrame.PushOperandDouble(Value1 * Value2);
                        }
                        break;
                    case Opcode.idiv:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            CurrentFrame.PushOperandInt(Value1 / Value2);
                        }
                        break;
                    case Opcode.ldiv:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            CurrentFrame.PushOperandLong(Value1 / Value2);
                        }
                        break;
                    case Opcode.fdiv:
                        {
                            float Value2 = CurrentFrame.PopOperandFloat();
                            float Value1 = CurrentFrame.PopOperandFloat();
                            CurrentFrame.PushOperandFloat(Value1 / Value2);
                        }
                        break;
                    case Opcode.ddiv:
                        {
                            double Value2 = CurrentFrame.PopOperandDouble();
                            double Value1 = CurrentFrame.PopOperandDouble();
                            CurrentFrame.PushOperandDouble(Value1 / Value2);
                        }
                        break;
                    case Opcode.irem:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            CurrentFrame.PushOperandInt(Value1 % Value2);
                        }
                        break;
                    case Opcode.lrem:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            CurrentFrame.PushOperandLong(Value1 % Value2);
                        }
                        break;
                    case Opcode.frem:
                        {
                            float Value2 = CurrentFrame.PopOperandFloat();
                            float Value1 = CurrentFrame.PopOperandFloat();
                            CurrentFrame.PushOperandFloat(Value1 % Value2);
                        }
                        break;
                    case Opcode.drem:
                        {
                            double Value2 = CurrentFrame.PopOperandDouble();
                            double Value1 = CurrentFrame.PopOperandDouble();
                            CurrentFrame.PushOperandDouble(Value1 % Value2);
                        }
                        break;
                    case Opcode.ineg:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            CurrentFrame.PushOperandInt(-Value);
                        }
                        break;
                    case Opcode.lneg:
                        {
                           long Value = CurrentFrame.PopOperandLong();
                            CurrentFrame.PushOperandLong(-Value);
                        }
                        break;
                    case Opcode.fneg:
                        {
                            float Value = CurrentFrame.PopOperandFloat();
                            CurrentFrame.PushOperandFloat(-Value);
                        }
                        break;
                    case Opcode.dneg:
                        {
                            double Value = CurrentFrame.PopOperandDouble();
                            CurrentFrame.PushOperandDouble(-Value);
                        }
                        break;
                    case Opcode.ishl:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            int Shift = Value2 & 0x1f;
                            int Value = Value1 << Shift;
                            CurrentFrame.PushOperandInt(Value);
                        }
                        break;
                    case Opcode.lshl:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            int Shift = ((int)Value2 & 0x3f);
                            long Value = Value1 << Shift;
                            CurrentFrame.PushOperandLong(Value);
                        }
                        break;
                    case Opcode.ishr:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            int Shift = Value2 & 0x1f;
                            int Value = Value1 >> Shift;
                            CurrentFrame.PushOperandInt(Value);
                        }
                        break;
                    case Opcode.lshr:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            int Shift = ((int)Value2 & 0x3f);
                            long Value = Value1 >> Shift;
                            CurrentFrame.PushOperandLong(Value);
                        }
                        break;
                    case Opcode.iushr:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            int Shift = Value2 & 0x1f;
                            int Value = Value1 >> Shift;
                            CurrentFrame.PushOperandInt(Value);
                            DebugHelper.Assert(false);
                        }
                        break;
                    case Opcode.lushr:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            int Shift = ((int)Value2 & 0x3f);
                            long Value = Value1 >> Shift;
                            CurrentFrame.PushOperandLong(Value);
                            DebugHelper.Assert(false);
                        }
                        break;
                    case Opcode.iand:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            int Value = Value1 & Value2;
                            CurrentFrame.PushOperandInt(Value);
                        }
                        break;
                    case Opcode.land:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            long Value = Value1 & Value2;
                            CurrentFrame.PushOperandLong(Value);
                        }
                        break;
                    case Opcode.ior:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            int Value = Value1 | Value2;
                            CurrentFrame.PushOperandInt(Value);
                        }
                        break;
                    case Opcode.lor:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            long Value = Value1 | Value2;
                            CurrentFrame.PushOperandLong(Value);
                        }
                        break;
                    case Opcode.ixor:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            int Value = Value1 ^ Value2;
                            CurrentFrame.PushOperandInt(Value);
                        }
                        break;
                    case Opcode.lxor:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            long Value = Value1 ^ Value2;
                            CurrentFrame.PushOperandLong(Value);
                        }
                        break;
                    case Opcode.iinc:
                        {
                            byte LocalIndex = FetchByte();
                            byte Immediate = FetchByte();
                            int Value = CurrentFrame.GetLocalInt(LocalIndex);
                            Value += (sbyte)Immediate;
                            CurrentFrame.SetLocalInt(LocalIndex, Value);
                        }
                        break;
                    case Opcode.i2l:
                        CurrentFrame.PushOperandLong((long)CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.i2f:
                        CurrentFrame.PushOperandFloat((float)CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.i2d:
                        CurrentFrame.PushOperandDouble((double)CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.l2i:
                        CurrentFrame.PushOperandInt((int)CurrentFrame.PopOperandLong());
                        break;
                    case Opcode.l2f:
                        CurrentFrame.PushOperandFloat((float)CurrentFrame.PopOperandLong());
                        break;
                    case Opcode.l2d:
                        CurrentFrame.PushOperandDouble((double)CurrentFrame.PopOperandLong());
                        break;
                    case Opcode.f2i:
                        CurrentFrame.PushOperandInt((int)CurrentFrame.PopOperandFloat());
                        break;
                    case Opcode.f2l:
                        CurrentFrame.PushOperandLong((long)CurrentFrame.PopOperandFloat());
                        break;
                    case Opcode.f2d:
                        CurrentFrame.PushOperandDouble((double)CurrentFrame.PopOperandFloat());
                        break;
                    case Opcode.d2i:
                        CurrentFrame.PushOperandInt((int)CurrentFrame.PopOperandDouble());
                        break;
                    case Opcode.d2l:
                        CurrentFrame.PushOperandLong((long)CurrentFrame.PopOperandDouble());
                        break;
                    case Opcode.d2f:
                        CurrentFrame.PushOperandFloat((float)CurrentFrame.PopOperandDouble());
                        break;
                    case Opcode.i2b:
                        CurrentFrame.PushOperandInt(CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.i2c:
                        CurrentFrame.PushOperandInt(CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.i2s:
                        CurrentFrame.PushOperandInt(CurrentFrame.PopOperandInt());
                        break;
                    case Opcode.lcmp:
                        {
                            long Value2 = CurrentFrame.PopOperandLong();
                            long Value1 = CurrentFrame.PopOperandLong();
                            if (Value1 < Value2)
                            {
                                CurrentFrame.PushOperandInt(1);
                            }
                            else if (Value1 == Value2)
                            {
                                CurrentFrame.PushOperandInt(0);
                            }
                            else
                            {
                                CurrentFrame.PushOperandInt(-1);
                            }
                        }
                        break;
                    case Opcode.fcmpl:
                        {
                            float Value2 = CurrentFrame.PopOperandFloat();
                            float Value1 = CurrentFrame.PopOperandFloat();
                            if (Value1 > Value2)
                            {
                                CurrentFrame.PushOperandInt(1);
                            }
                            else if (Value1 == Value2)
                            {
                                CurrentFrame.PushOperandInt(0);
                            }
                            else if (Value1 < Value2)
                            {
                                CurrentFrame.PushOperandInt(-1);
                            }
                            else 
                            {
                                CurrentFrame.PushOperandInt(-1);
                            }
                        }
                        break;
                    case Opcode.fcmpg:
                        {
                            float Value2 = CurrentFrame.PopOperandFloat();
                            float Value1 = CurrentFrame.PopOperandFloat();
                            if (Value1 > Value2)
                            {
                                CurrentFrame.PushOperandInt(1);
                            }
                            else if (Value1 == Value2)
                            {
                                CurrentFrame.PushOperandInt(0);
                            }
                            else if (Value1 < Value2)
                            {
                                CurrentFrame.PushOperandInt(-1);
                            }
                            else
                            {
                                CurrentFrame.PushOperandInt(1);
                            }
                        }
                        break;
                    case Opcode.dcmpl:
                        {
                            double Value2 = CurrentFrame.PopOperandDouble();
                            double Value1 = CurrentFrame.PopOperandDouble();
                            if (Value1 > Value2)
                            {
                                CurrentFrame.PushOperandInt(1);
                            }
                            else if (Value1 == Value2)
                            {
                                CurrentFrame.PushOperandInt(0);
                            }
                            else if (Value1 < Value2)
                            {
                                CurrentFrame.PushOperandInt(-1);
                            }
                            else
                            {
                                CurrentFrame.PushOperandInt(-1);
                            }
                        }
                        break;
                    case Opcode.dcmpg:
                        {
                            double Value2 = CurrentFrame.PopOperandDouble();
                            double Value1 = CurrentFrame.PopOperandDouble();
                            if (Value1 > Value2)
                            {
                                CurrentFrame.PushOperandInt(1);
                            }
                            else if (Value1 == Value2)
                            {
                                CurrentFrame.PushOperandInt(0);
                            }
                            else if (Value1 < Value2)
                            {
                                CurrentFrame.PushOperandInt(-1);
                            }
                            else
                            {
                                CurrentFrame.PushOperandInt(1);
                            }
                        }
                        break;
                    case Opcode.ifeq:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            if (Value == 0)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.ifne:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            if (Value != 0)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.iflt:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            if (Value < 0)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.ifge:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            if (Value >= 0)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.ifgt:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            if (Value > 0)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.ifle:
                        {
                            int Value = CurrentFrame.PopOperandInt();
                            if (Value <= 0)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.if_icmpeq:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            if (Value1 == Value2)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.if_icmpne:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            if (Value1 != Value2)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.if_icmplt:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            if (Value1 < Value2)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.if_icmpge:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            if (Value1 >= Value2)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.if_icmpgt:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            if (Value1 > Value2)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.if_icmple:
                        {
                            int Value2 = CurrentFrame.PopOperandInt();
                            int Value1 = CurrentFrame.PopOperandInt();
                            if (Value1 <= Value2)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.if_acmpeq:
                        {
                            object Object2 = CurrentFrame.PopOperandReference();
                            object Object1 = CurrentFrame.PopOperandReference();
                            if (Object1 == Object2)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.if_acmpne:
                        {
                            object Object2 = CurrentFrame.PopOperandReference();
                            object Object1 = CurrentFrame.PopOperandReference();
                            if (Object1 != Object2)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.goto_:
                        {
                            int Offset = FetchOffset();
                            JumpOffset(SavedProgramCounter, Offset);
                        }
                        break;
                    case Opcode.tableswitch:
                        {
                            PaddingTo4BytesAligned();
                            int DefaultOffset = FetchInt();
                            int LowValue = FetchInt();
                            int HighValue = FetchInt();
                            int ValueCount = HighValue - LowValue + 1;
                            Offsets.Resize(ValueCount);
                            for (int i = 0; i < ValueCount; i++)
                            {
                                Offsets[i] = FetchInt();
                            }
                            int Key = CurrentFrame.PopOperandInt();
                            int Offset;
                            if (Key >= LowValue && Key <= HighValue)
                            {
                                Offset = Offsets[Key - LowValue];
                            }
                            else
                            {
                                Offset = DefaultOffset;
                            }
                            JumpOffset(SavedProgramCounter, Offset);
                        }
                        break;
                    case Opcode.lookupswitch:
                        {
                            PaddingTo4BytesAligned();
                            int DefaultOffset = FetchInt();
                            int NumberOfPairs = FetchInt();
                            Cases.Resize(NumberOfPairs);
                            Offsets.Resize(NumberOfPairs);
                            for (int i = 0; i < NumberOfPairs; i++)
                            {
                                Cases[i] = FetchInt();
                                Offsets[i] = FetchInt();
                            }
                            int Offset = DefaultOffset;
                            int Key = CurrentFrame.PopOperandInt();
                            for (int j = 0; j < NumberOfPairs; j++)
                            {
                                if (Cases[j] == Key)
                                {
                                    Offset = Offsets[j];
                                    break;
                                }
                            }
                            JumpOffset(SavedProgramCounter, Offset);
                        }
                        break;
                    case Opcode.ireturn:
                        ReturnInt();
                        break;
                    case Opcode.lreturn:
                        ReturnLong();
                        break;
                    case Opcode.freturn:
                        ReturnFloat();
                        break;
                    case Opcode.dreturn:
                        ReturnDouble();
                        break;
                    case Opcode.areturn:
                        ReturnReference();
                        break;
                    case Opcode.return_:
                        Return();
                        break;
                    case Opcode.invokevirtual:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Method TargetMethod = CurrentClass.FindMethodByMethodRef(ConstantIndex);
                            if (TargetMethod != null)
                            {
                                int ArgumentSlotCount = TargetMethod.ArgumentSlotCount;
                                Object This = (Object)CurrentFrame.GetReferenceFromTop(ArgumentSlotCount - 1);
                                Class TargetClass = This.Class;
                                Method RealTargetMethod = TargetClass.FindMethodRecursive(TargetMethod.NameString, TargetMethod.DescriptorString);  //TODO: Optimize me with vftable
                                Call(RealTargetMethod);
                            }
                        }
                        break;
                    case Opcode.invokespecial:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Method TargetMethod = CurrentClass.FindMethodByMethodRef(ConstantIndex);
                            if (TargetMethod != null)
                            {
                                Call(TargetMethod);
                            }
                        }
                        break;
                    case Opcode.invokestatic:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Method TargetMethod = CurrentClass.FindMethodByMethodRef(ConstantIndex);
                            if (TargetMethod != null)
                            {
                                Call(TargetMethod);
                            }
                        }
                        break;
                    case Opcode.invokeinterface:
                        {
                            ushort ConstantIndex = FetchUShort();
                            FetchByte();
                            FetchByte();
                            Method TargetMethod = CurrentClass.FindMethodByMethodRef(ConstantIndex);
                            if (TargetMethod != null)
                            {
                                Call(TargetMethod);
                            }
                        }
                        break;
                    case Opcode.getstatic:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Field TargetField = CurrentClass.FindFieldByFieldRef(ConstantIndex);
                            Class TargetClass1 = TargetField.Class;
                            char FirstChar = StringHelper.GetChar(TargetField.DescriptorString, 0);
                            object Value = TargetClass1.StaticVariables.Values[TargetField.FieldIndex];
                            if (FirstChar == 'D' || FirstChar == 'J')
                            {
                                CurrentFrame.PushOperand2(Value);
                            }
                            else
                            {
                                CurrentFrame.PushOperand1(Value);
                            }
                        }
                        break;
                    case Opcode.putstatic:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Field TargetField = CurrentClass.FindFieldByFieldRef(ConstantIndex);
                            Class TargetClass1 = TargetField.Class;
                            char FirstChar = StringHelper.GetChar(TargetField.DescriptorString, 0);
                            object Value;
                            if (FirstChar == 'D' || FirstChar == 'J')
                            {
                                Value = CurrentFrame.PopOperand2();
                            }
                            else
                            {
                                Value = CurrentFrame.PopOperand1();
                            }
                            TargetClass1.StaticVariables.Values[TargetField.FieldIndex] = Value;
                        }
                        break;
                    case Opcode.getfield:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Object TargetObject = (Object)CurrentFrame.PopOperandReference();
                            Field TargetField = CurrentClass.FindFieldByFieldRef(ConstantIndex);
                            Class TargetClass1 = TargetField.Class;
                            char FirstChar = StringHelper.GetChar(TargetField.DescriptorString, 0);
                            object Value = TargetObject.Fields.Values[TargetField.FieldIndex];
                            if (FirstChar == 'D' || FirstChar == 'J')
                            {
                                CurrentFrame.PushOperand2(Value);
                            }
                            else
                            {
                                CurrentFrame.PushOperand1(Value);
                            }
                        }
                        break;
                    case Opcode.putfield:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Field TargetField = CurrentClass.FindFieldByFieldRef(ConstantIndex);
                            Class TargetClass1 = TargetField.Class;
                            char FirstChar = StringHelper.GetChar(TargetField.DescriptorString, 0);
                            object Value;
                            if (FirstChar == 'D' || FirstChar == 'J')
                            {
                                Value = CurrentFrame.PopOperand2();
                            }
                            else
                            {
                                Value = CurrentFrame.PopOperand1();
                            }
                            Object TargetObject = (Object)CurrentFrame.PopOperandReference();
                            TargetObject.Fields.Values[TargetField.FieldIndex] = Value;
                        }
                        break;
                    case Opcode.new_:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Class TargetClass = CurrentClass.FindClassByClassRef(ConstantIndex);
                            Object NewObject = new Object(TargetClass);
                            CurrentFrame.PushOperandReference(NewObject);
                        }
                        break;
                    case Opcode.newarray:
                        {
                            int ArrayLength = CurrentFrame.PopOperandInt();
                            string ArrayClassName = "";
                            object Array = null;
                            ValueType ValueType = (ValueType)FetchByte();
                            switch (ValueType)
                            {
                                case ValueType.Boolean:
                                    ArrayClassName = "boolean[]";
                                    Array = new bool[ArrayLength];
                                    break;
                                case ValueType.Char:
                                    ArrayClassName = "char[]";
                                    Array = new char[ArrayLength];
                                    break;
                                case ValueType.Float:
                                    ArrayClassName = "float[]";
                                    Array = new float[ArrayLength];
                                    break;
                                case ValueType.Double:
                                    ArrayClassName = "double[]";
                                    Array = new double[ArrayLength];
                                    break;
                                case ValueType.Byte:
                                    ArrayClassName = "byte[]";
                                    Array = new byte[ArrayLength];
                                    break;
                                case ValueType.Short:
                                    ArrayClassName = "short[]";
                                    Array = new short[ArrayLength];
                                    break;
                                case ValueType.Int:
                                    ArrayClassName = "int[]";
                                    Array = new int[ArrayLength];
                                    break;
                                case ValueType.Long:
                                    ArrayClassName = "long[]";
                                    Array = new long[ArrayLength];
                                    break;
                                default:
                                    DebugHelper.Assert(false);
                                    break;
                            }
                            Class ArrayClass = ClassLoader.GetInstance().CreateArrayClass(ArrayClassName);
                            Object ArrayObject = new Object(ArrayClass);
                            ArrayObject.SetArrayLength(ArrayLength);
                            ArrayObject.Array = Array;
                            CurrentFrame.PushOperandReference(ArrayObject);
                        }
                        break;
                    case Opcode.anewarray:
                        {
                            ushort ConstantIndex = FetchUShort();
                            int ArrayLength = CurrentFrame.PopOperandInt();
                            Class TargetClass = CurrentClass.FindClassByClassRef(ConstantIndex);
                            string ArrayClassName = TargetClass.ClassInfo.ThisClassString + "[]";
                            Class ArrayClass = ClassLoader.GetInstance().CreateArrayClass(ArrayClassName);
                            Object ArrayObject = new Object(ArrayClass);
                            ArrayObject.SetArrayLength(ArrayLength);
                            ArrayObject.Array = new object[ArrayLength];
                            CurrentFrame.PushOperandReference(ArrayObject);
                        }
                        break;
                    case Opcode.arraylength:
                        {
                            Object ArrayObject = (Object)CurrentFrame.PopOperandReference();
                            int ArrayLength = ArrayObject.GetArrayLength();
                            CurrentFrame.PushOperandInt(ArrayLength);
                        }
                        break;
                    case Opcode.checkcast:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Class TargetClass = CurrentClass.FindClassByClassRef(ConstantIndex);
                            Object TargetObject = (Object)CurrentFrame.PopOperandReference();
                            if (TargetObject == null)
                            {
                                CurrentFrame.PushOperandReference(null);
                            }
                            if (TargetObject.IsInstanceOf(TargetClass))
                            {
                                CurrentFrame.PushOperandReference(TargetObject);
                            }
                            else
                            {
                                DebugHelper.Assert(false);  //TODO: Raise ClassCastException
                                CurrentFrame.PushOperandReference(null);
                            }
                        }
                        break;
                    case Opcode.instanceof:
                        {
                            ushort ConstantIndex = FetchUShort();
                            Class TargetClass = CurrentClass.FindClassByClassRef(ConstantIndex);
                            Object TargetObject = (Object)CurrentFrame.PopOperandReference();
                            if (TargetObject == null)
                            {
                                CurrentFrame.PushOperandInt(0);
                            }
                            if (TargetObject.IsInstanceOf(TargetClass))
                            {
                                CurrentFrame.PushOperandInt(1);
                            }
                            else
                            {
                                CurrentFrame.PushOperandInt(0);
                            }
                        }
                        break;
                    case Opcode.wide:
                        uint SavedProgramCounter1 = ProgramCounter;
                        Opcode1 = (Opcode)FetchByte();
                        switch (Opcode1)
                        {
                            case Opcode.iload:
                                CurrentFrame.PushOperandInt(CurrentFrame.GetLocalInt(FetchUShort()));
                                break;
                            case Opcode.lload:
                                CurrentFrame.PushOperandLong(CurrentFrame.GetLocalLong(FetchUShort()));
                                break;
                            case Opcode.fload:
                                CurrentFrame.PushOperandFloat(CurrentFrame.GetLocalFloat(FetchUShort()));
                                break;
                            case Opcode.dload:
                                CurrentFrame.PushOperandDouble(CurrentFrame.GetLocalDouble(FetchUShort()));
                                break;
                            case Opcode.aload:
                                CurrentFrame.PushOperandReference(CurrentFrame.GetLocalReference(FetchUShort()));
                                break;
                            case Opcode.istore:
                                CurrentFrame.SetLocalInt(FetchUShort(), CurrentFrame.PopOperandInt());
                                break;
                            case Opcode.lstore:
                                CurrentFrame.SetLocalLong(FetchUShort(), CurrentFrame.PopOperandLong());
                                break;
                            case Opcode.fstore:
                                CurrentFrame.SetLocalFloat(FetchUShort(), CurrentFrame.PopOperandFloat());
                                break;
                            case Opcode.dstore:
                                CurrentFrame.SetLocalDouble(FetchUShort(), CurrentFrame.PopOperandDouble());
                                break;
                            case Opcode.astore:
                                CurrentFrame.SetLocalReference(FetchUShort(), CurrentFrame.PopOperandReference());
                                break;
                            case Opcode.iinc:
                                {
                                    ushort LocalIndex = FetchUShort();
                                    ushort Immediate = FetchUShort();
                                    int Value = CurrentFrame.GetLocalInt(LocalIndex);
                                    Value += (short)Immediate;
                                    CurrentFrame.SetLocalInt(LocalIndex, Value);
                                }
                                break;
                            case Opcode.ret:
                                DebugHelper.Assert(false);
                                break;
                            default:
                                DebugHelper.Assert(false);
                                break;
                        }
                        break;
                    case Opcode.ifnull:
                        {
                            object Object = CurrentFrame.PopOperandReference();
                            if (Object == null)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.ifnonnull:
                        {
                            object Object = CurrentFrame.PopOperandReference();
                            if (Object != null)
                            {
                                JumpOffset(SavedProgramCounter, FetchOffset());
                            }
                        }
                        break;
                    case Opcode.goto_w:
                        {
                            int Offset = FetchInt();
                            JumpOffset(SavedProgramCounter, Offset);
                        }
                        break;
                    case Opcode.breakpoint:
                        DebugHelper.Assert(false);
                        break;
                    case Opcode.impdep1:
                        {
                            if (CurrentMethod.NativeMethod == null)
                            {
                                string ClassName = CurrentClass.ClassInfo.ThisClassString;
                                string MethodName = CurrentMethod.NameString;
                                string MethodDescriptor = CurrentMethod.DescriptorString;
                                CurrentMethod.NativeMethod = NativeMethods.GetInstance().FindNativeMethod(ClassName, MethodName, MethodDescriptor);
                            }
                            NativeMethod NativeMethod = CurrentMethod.NativeMethod;
                            if (NativeMethod != null)
                            {
                                NativeMethod(CurrentFrame);
                            }
                        }
                        break;
                    default:
                        DebugHelper.Assert(false);
                        break;
                }
            }
        }
    }
}
