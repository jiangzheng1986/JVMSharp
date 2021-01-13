using System;

namespace JVMSharp
{
    class Disassembler
    {
        public static void Disassemble(byte[] Code, int Indent, ClassInfo ClassInfo)
        {
            Stream Stream = new Stream(Code);
            Disassemble(Stream, Indent, ClassInfo);
        }

        static void Disassembler_Disassemble_Offset_UShort(Stream Stream, string InstructionName, int CurrentCode)
        {
            ushort UShort = Stream.ReadBigEndianUnsignedInt16();
            int Offset = (short)UShort;
            Console.WriteLine("{0} {1}", InstructionName, CurrentCode + Offset);
        }

        static void Disassembler_Disassemble_Offset_UInt(Stream Stream, string InstructionName, int CurrentCode)
        {
            uint UInt = Stream.ReadBigEndianUnsignedInt32();
            int Offset = (int)UInt;
            Console.WriteLine("{0} {1}", InstructionName, CurrentCode + Offset);
        }

        static void Disassembler_Disassemble_ConstantIndex_Byte(Stream Stream, string InstructionName, ClassInfo ClassInfo)
        {
            byte ConstantIndex = Stream.ReadUnsignedInt8();
            Console.Write("{0} {1}\t\t; ", InstructionName, ConstantIndex);
            ClassInfo.PrintConstant(ConstantIndex);
            Console.WriteLine();
        }

        static void Disassembler_Disassemble_ConstantIndex_UShort(Stream Stream, string InstructionName, ClassInfo ClassInfo)
        {
            ushort ConstantIndex = Stream.ReadBigEndianUnsignedInt16();
            Console.Write("{0} {1}\t\t; ", InstructionName, ConstantIndex);
            ClassInfo.PrintConstant(ConstantIndex);
            Console.WriteLine();
        }

        static void Disassembler_Disassemble_Local_Byte(Stream Stream, string InstructionName)
        {
            byte LocalIndex = Stream.ReadUnsignedInt8();
            Console.WriteLine("{0} {1}", InstructionName, LocalIndex);
        }

        static void Disassembler_Disassemble_Local_UShort(Stream Stream, string InstructionName)
        {
            ushort LocalIndex = Stream.ReadBigEndianUnsignedInt16();
            Console.WriteLine("{0} {1}", InstructionName, LocalIndex);
        }

        static void PaddingTo4BytesAligned(Stream Stream)
        {
            uint DataPointer = (uint)Stream.GetDataPointer();
            DataPointer = (DataPointer + 3) & 0xFFFFFFFC;
            Stream.SetDataPointer((int)DataPointer);
        }

        static void Disassemble(Stream Stream, int Indent, ClassInfo ClassInfo)
        {
            int StreamLength = Stream.GetLength();
            while (true)
            {
                int DataPointer = Stream.GetDataPointer();
                if (DataPointer >= StreamLength)
                {
                    break;
                }
                int CurrentCode = DataPointer;
                ConsoleHelper.PrintIndent(Indent);
                Console.Write("{0}: ", CurrentCode.ToString("000"));
                Opcode Opcode = (Opcode)Stream.ReadUnsignedInt8();
                switch (Opcode)
                {
                    case Opcode.nop:
                        Console.WriteLine("nop");
                        break;
                    case Opcode.aconst_null:
                        Console.WriteLine("aconst_null");
                        break;
                    case Opcode.iconst_m1:
                        Console.WriteLine("iconst_m1");
                        break;
                    case Opcode.iconst_0:
                        Console.WriteLine("iconst_0");
                        break;
                    case Opcode.iconst_1:
                        Console.WriteLine("iconst_1");
                        break;
                    case Opcode.iconst_2:
                        Console.WriteLine("iconst_2");
                        break;
                    case Opcode.iconst_3:
                        Console.WriteLine("iconst_3");
                        break;
                    case Opcode.iconst_4:
                        Console.WriteLine("iconst_4");
                        break;
                    case Opcode.iconst_5:
                        Console.WriteLine("iconst_5");
                        break;
                    case Opcode.lconst_0:
                        Console.WriteLine("lconst_0");
                        break;
                    case Opcode.lconst_1:
                        Console.WriteLine("lconst_1");
                        break;
                    case Opcode.fconst_0:
                        Console.WriteLine("fconst_0");
                        break;
                    case Opcode.fconst_1:
                        Console.WriteLine("fconst_1");
                        break;
                    case Opcode.fconst_2:
                        Console.WriteLine("fconst_2");
                        break;
                    case Opcode.dconst_0:
                        Console.WriteLine("dconst_0");
                        break;
                    case Opcode.dconst_1:
                        Console.WriteLine("dconst_1");
                        break;
                    case Opcode.bipush:
                        {
                            byte Byte = Stream.ReadUnsignedInt8();
                            int Value = (sbyte)Byte;
                            Console.WriteLine("bipush {0}", Value);
                        }
                        break;
                    case Opcode.sipush:
                        {
                            ushort UShort = Stream.ReadBigEndianUnsignedInt16();
                            int Value = (short)UShort;
                            Console.WriteLine("sipush {0}", Value);
                        }
                        break;
                    case Opcode.ldc:
                        Disassembler_Disassemble_ConstantIndex_Byte(Stream, "ldc", ClassInfo);
                        break;
                    case Opcode.ldc_w:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "ldc_w", ClassInfo);
                        break;
                    case Opcode.ldc2_w:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "ldc2_w", ClassInfo);
                        break;
                    case Opcode.iload:
                        Disassembler_Disassemble_Local_Byte(Stream, "iload");
                        break;
                    case Opcode.lload:
                        Disassembler_Disassemble_Local_Byte(Stream, "lload");
                        break;
                    case Opcode.fload:
                        Disassembler_Disassemble_Local_Byte(Stream, "fload");
                        break;
                    case Opcode.dload:
                        Disassembler_Disassemble_Local_Byte(Stream, "dload");
                        break;
                    case Opcode.aload:
                        Disassembler_Disassemble_Local_Byte(Stream, "aload");
                        break;
                    case Opcode.iload_0:
                        Console.WriteLine("iload_0");
                        break;
                    case Opcode.iload_1:
                        Console.WriteLine("iload_1");
                        break;
                    case Opcode.iload_2:
                        Console.WriteLine("iload_2");
                        break;
                    case Opcode.iload_3:
                        Console.WriteLine("iload_3");
                        break;
                    case Opcode.lload_0:
                        Console.WriteLine("lload_0");
                        break;
                    case Opcode.lload_1:
                        Console.WriteLine("lload_1");
                        break;
                    case Opcode.lload_2:
                        Console.WriteLine("lload_2");
                        break;
                    case Opcode.lload_3:
                        Console.WriteLine("lload_3");
                        break;
                    case Opcode.fload_0:
                        Console.WriteLine("fload_0");
                        break;
                    case Opcode.fload_1:
                        Console.WriteLine("fload_1");
                        break;
                    case Opcode.fload_2:
                        Console.WriteLine("fload_2");
                        break;
                    case Opcode.fload_3:
                        Console.WriteLine("fload_3");
                        break;
                    case Opcode.dload_0:
                        Console.WriteLine("dload_0");
                        break;
                    case Opcode.dload_1:
                        Console.WriteLine("dload_1");
                        break;
                    case Opcode.dload_2:
                        Console.WriteLine("dload_2");
                        break;
                    case Opcode.dload_3:
                        Console.WriteLine("dload_3");
                        break;
                    case Opcode.aload_0:
                        Console.WriteLine("aload_0");
                        break;
                    case Opcode.aload_1:
                        Console.WriteLine("aload_1");
                        break;
                    case Opcode.aload_2:
                        Console.WriteLine("aload_2");
                        break;
                    case Opcode.aload_3:
                        Console.WriteLine("aload_3");
                        break;
                    case Opcode.iaload:
                        Console.WriteLine("iaload");
                        break;
                    case Opcode.laload:
                        Console.WriteLine("laload");
                        break;
                    case Opcode.faload:
                        Console.WriteLine("faload");
                        break;
                    case Opcode.daload:
                        Console.WriteLine("daload");
                        break;
                    case Opcode.aaload:
                        Console.WriteLine("aaload");
                        break;
                    case Opcode.baload:
                        Console.WriteLine("baload");
                        break;
                    case Opcode.caload:
                        Console.WriteLine("caload");
                        break;
                    case Opcode.saload:
                        Console.WriteLine("saload");
                        break;
                    case Opcode.istore:
                        Disassembler_Disassemble_Local_Byte(Stream, "istore");
                        break;
                    case Opcode.lstore:
                        Disassembler_Disassemble_Local_Byte(Stream, "lstore");
                        break;
                    case Opcode.fstore:
                        Disassembler_Disassemble_Local_Byte(Stream, "fstore");
                        break;
                    case Opcode.dstore:
                        Disassembler_Disassemble_Local_Byte(Stream, "dstore");
                        break;
                    case Opcode.astore:
                        Disassembler_Disassemble_Local_Byte(Stream, "astore");
                        break;
                    case Opcode.istore_0:
                        Console.WriteLine("istore_0");
                        break;
                    case Opcode.istore_1:
                        Console.WriteLine("istore_1");
                        break;
                    case Opcode.istore_2:
                        Console.WriteLine("istore_2");
                        break;
                    case Opcode.istore_3:
                        Console.WriteLine("istore_3");
                        break;
                    case Opcode.lstore_0:
                        Console.WriteLine("lstore_0");
                        break;
                    case Opcode.lstore_1:
                        Console.WriteLine("lstore_1");
                        break;
                    case Opcode.lstore_2:
                        Console.WriteLine("lstore_2");
                        break;
                    case Opcode.lstore_3:
                        Console.WriteLine("lstore_3");
                        break;
                    case Opcode.fstore_0:
                        Console.WriteLine("fstore_0");
                        break;
                    case Opcode.fstore_1:
                        Console.WriteLine("fstore_1");
                        break;
                    case Opcode.fstore_2:
                        Console.WriteLine("fstore_2");
                        break;
                    case Opcode.fstore_3:
                        Console.WriteLine("fstore_3");
                        break;
                    case Opcode.dstore_0:
                        Console.WriteLine("dstore_0");
                        break;
                    case Opcode.dstore_1:
                        Console.WriteLine("dstore_1");
                        break;
                    case Opcode.dstore_2:
                        Console.WriteLine("dstore_2");
                        break;
                    case Opcode.dstore_3:
                        Console.WriteLine("dstore_3");
                        break;
                    case Opcode.astore_0:
                        Console.WriteLine("astore_0");
                        break;
                    case Opcode.astore_1:
                        Console.WriteLine("astore_1");
                        break;
                    case Opcode.astore_2:
                        Console.WriteLine("astore_2");
                        break;
                    case Opcode.astore_3:
                        Console.WriteLine("astore_3");
                        break;
                    case Opcode.iastore:
                        Console.WriteLine("iastore");
                        break;
                    case Opcode.lastore:
                        Console.WriteLine("lastore");
                        break;
                    case Opcode.fastore:
                        Console.WriteLine("fastore");
                        break;
                    case Opcode.dastore:
                        Console.WriteLine("dastore");
                        break;
                    case Opcode.aastore:
                        Console.WriteLine("aastore");
                        break;
                    case Opcode.bastore:
                        Console.WriteLine("bastore");
                        break;
                    case Opcode.castore:
                        Console.WriteLine("castore");
                        break;
                    case Opcode.sastore:
                        Console.WriteLine("sastore");
                        break;
                    case Opcode.pop:
                        Console.WriteLine("pop");
                        break;
                    case Opcode.pop2:
                        Console.WriteLine("pop2");
                        break;
                    case Opcode.dup:
                        Console.WriteLine("dup");
                        break;
                    case Opcode.dup_x1:
                        Console.WriteLine("dup_x1");
                        break;
                    case Opcode.dup_x2:
                        Console.WriteLine("dup_x2");
                        break;
                    case Opcode.dup2:
                        Console.WriteLine("dup2");
                        break;
                    case Opcode.dup2_x1:
                        Console.WriteLine("dup2_x1");
                        break;
                    case Opcode.dup2_x2:
                        Console.WriteLine("dup2_x2");
                        break;
                    case Opcode.swap:
                        Console.WriteLine("swap");
                        break;
                    case Opcode.iadd:
                        Console.WriteLine("iadd");
                        break;
                    case Opcode.ladd:
                        Console.WriteLine("ladd");
                        break;
                    case Opcode.fadd:
                        Console.WriteLine("fadd");
                        break;
                    case Opcode.dadd:
                        Console.WriteLine("dadd");
                        break;
                    case Opcode.isub:
                        Console.WriteLine("isub");
                        break;
                    case Opcode.lsub:
                        Console.WriteLine("lsub");
                        break;
                    case Opcode.fsub:
                        Console.WriteLine("fsub");
                        break;
                    case Opcode.dsub:
                        Console.WriteLine("dsub");
                        break;
                    case Opcode.imul:
                        Console.WriteLine("imul");
                        break;
                    case Opcode.lmul:
                        Console.WriteLine("lmul");
                        break;
                    case Opcode.fmul:
                        Console.WriteLine("fmul");
                        break;
                    case Opcode.dmul:
                        Console.WriteLine("dmul");
                        break;
                    case Opcode.idiv:
                        Console.WriteLine("idiv");
                        break;
                    case Opcode.ldiv:
                        Console.WriteLine("ldiv");
                        break;
                    case Opcode.fdiv:
                        Console.WriteLine("fdiv");
                        break;
                    case Opcode.ddiv:
                        Console.WriteLine("ddiv");
                        break;
                    case Opcode.irem:
                        Console.WriteLine("irem");
                        break;
                    case Opcode.lrem:
                        Console.WriteLine("lrem");
                        break;
                    case Opcode.frem:
                        Console.WriteLine("frem");
                        break;
                    case Opcode.drem:
                        Console.WriteLine("drem");
                        break;
                    case Opcode.ineg:
                        Console.WriteLine("ineg");
                        break;
                    case Opcode.lneg:
                        Console.WriteLine("lneg");
                        break;
                    case Opcode.fneg:
                        Console.WriteLine("fneg");
                        break;
                    case Opcode.dneg:
                        Console.WriteLine("dneg");
                        break;
                    case Opcode.ishl:
                        Console.WriteLine("ishl");
                        break;
                    case Opcode.lshl:
                        Console.WriteLine("lshl");
                        break;
                    case Opcode.ishr:
                        Console.WriteLine("ishr");
                        break;
                    case Opcode.lshr:
                        Console.WriteLine("lshr");
                        break;
                    case Opcode.iushr:
                        Console.WriteLine("iushr");
                        break;
                    case Opcode.lushr:
                        Console.WriteLine("lushr");
                        break;
                    case Opcode.iand:
                        Console.WriteLine("iand");
                        break;
                    case Opcode.land:
                        Console.WriteLine("land");
                        break;
                    case Opcode.ior:
                        Console.WriteLine("ior");
                        break;
                    case Opcode.lor:
                        Console.WriteLine("lor");
                        break;
                    case Opcode.ixor:
                        Console.WriteLine("ixor");
                        break;
                    case Opcode.lxor:
                        Console.WriteLine("lxor");
                        break;
                    case Opcode.iinc:
                        {
                            byte LocalIndex = Stream.ReadUnsignedInt8();
                            byte Immediate = Stream.ReadUnsignedInt8();
                            int Value = (int)(sbyte)Immediate;
                            Console.WriteLine("iinc {0} {1}", LocalIndex, Value);
                        }
                        break;
                    case Opcode.i2l:
                        Console.WriteLine("i2l");
                        break;
                    case Opcode.i2f:
                        Console.WriteLine("i2f");
                        break;
                    case Opcode.i2d:
                        Console.WriteLine("i2d");
                        break;
                    case Opcode.l2i:
                        Console.WriteLine("l2i");
                        break;
                    case Opcode.l2f:
                        Console.WriteLine("l2f");
                        break;
                    case Opcode.l2d:
                        Console.WriteLine("l2d");
                        break;
                    case Opcode.f2i:
                        Console.WriteLine("f2i");
                        break;
                    case Opcode.f2l:
                        Console.WriteLine("f2l");
                        break;
                    case Opcode.f2d:
                        Console.WriteLine("f2d");
                        break;
                    case Opcode.d2i:
                        Console.WriteLine("d2i");
                        break;
                    case Opcode.d2l:
                        Console.WriteLine("d2l");
                        break;
                    case Opcode.d2f:
                        Console.WriteLine("d2f");
                        break;
                    case Opcode.i2b:
                        Console.WriteLine("i2b");
                        break;
                    case Opcode.i2c:
                        Console.WriteLine("i2c");
                        break;
                    case Opcode.i2s:
                        Console.WriteLine("i2s");
                        break;
                    case Opcode.lcmp:
                        Console.WriteLine("lcmp");
                        break;
                    case Opcode.fcmpl:
                        Console.WriteLine("fcmpl");
                        break;
                    case Opcode.fcmpg:
                        Console.WriteLine("fcmpg");
                        break;
                    case Opcode.dcmpl:
                        Console.WriteLine("dcmpl");
                        break;
                    case Opcode.dcmpg:
                        Console.WriteLine("dcmpg");
                        break;
                    case Opcode.ifeq:
                        Disassembler_Disassemble_Offset_UShort(Stream, "ifeq", CurrentCode);
                        break;
                    case Opcode.ifne:
                        Disassembler_Disassemble_Offset_UShort(Stream, "ifne", CurrentCode);
                        break;
                    case Opcode.iflt:
                        Disassembler_Disassemble_Offset_UShort(Stream, "iflt", CurrentCode);
                        break;
                    case Opcode.ifge:
                        Disassembler_Disassemble_Offset_UShort(Stream, "ifge", CurrentCode);
                        break;
                    case Opcode.ifgt:
                        Disassembler_Disassemble_Offset_UShort(Stream, "ifgt", CurrentCode);
                        break;
                    case Opcode.ifle:
                        Disassembler_Disassemble_Offset_UShort(Stream, "ifle", CurrentCode);
                        break;
                    case Opcode.if_icmpeq:
                        Disassembler_Disassemble_Offset_UShort(Stream, "if_icmpeq", CurrentCode);
                        break;
                    case Opcode.if_icmpne:
                        Disassembler_Disassemble_Offset_UShort(Stream, "if_icmpne", CurrentCode);
                        break;
                    case Opcode.if_icmplt:
                        Disassembler_Disassemble_Offset_UShort(Stream, "if_icmplt", CurrentCode);
                        break;
                    case Opcode.if_icmpge:
                        Disassembler_Disassemble_Offset_UShort(Stream, "if_icmpge", CurrentCode);
                        break;
                    case Opcode.if_icmpgt:
                        Disassembler_Disassemble_Offset_UShort(Stream, "if_icmpgt", CurrentCode);
                        break;
                    case Opcode.if_icmple:
                        Disassembler_Disassemble_Offset_UShort(Stream, "if_icmple", CurrentCode);
                        break;
                    case Opcode.if_acmpeq:
                        Disassembler_Disassemble_Offset_UShort(Stream, "if_acmpeq", CurrentCode);
                        break;
                    case Opcode.if_acmpne:
                        Disassembler_Disassemble_Offset_UShort(Stream, "if_acmpne", CurrentCode);
                        break;
                    case Opcode.goto_:
                        Disassembler_Disassemble_Offset_UShort(Stream, "goto", CurrentCode);
                        break;
                    case Opcode.jsr:
                        Disassembler_Disassemble_Offset_UShort(Stream, "jsr", CurrentCode);
                        break;
                    case Opcode.ret:
                        Disassembler_Disassemble_Local_Byte(Stream, "ret");
                        break;
                    case Opcode.tableswitch:
                        {
                            PaddingTo4BytesAligned(Stream);
                            int DefaultOffset = Stream.ReadBigEndianInt32();
                            int LowValue = Stream.ReadBigEndianInt32();
                            int HighValue = Stream.ReadBigEndianInt32();
                            Console.Write("tableswitch {0} {1} {2} (", CurrentCode + DefaultOffset, LowValue, HighValue);
                            int ValueCount = HighValue - LowValue + 1;
                            for (int i = 0; i < ValueCount; i++)
                            {
                                int CaseOffset = Stream.ReadBigEndianInt32();
                                Console.Write("{0}", CurrentCode + CaseOffset);
                                if (i != ValueCount - 1)
                                {
                                    Console.Write(",");
                                }
                            }
                            Console.WriteLine(")");
                        }
                        break;
                    case Opcode.lookupswitch:
                        {
                            PaddingTo4BytesAligned(Stream);
                            int DefaultOffset = Stream.ReadBigEndianInt32();
                            int NumberOfPairs = Stream.ReadBigEndianInt32();
                            Console.Write("lookupswitch {0} {1} (", CurrentCode + DefaultOffset, NumberOfPairs);
                            for (int i = 0; i < NumberOfPairs; i++)
                            {
                                int KeyValue = Stream.ReadBigEndianInt32();
                                int CaseOffset = Stream.ReadBigEndianInt32();
                                Console.Write("{0}:{1}", KeyValue, CurrentCode + CaseOffset);
                                if (i != NumberOfPairs - 1)
                                {
                                    Console.Write(",");
                                }
                            }
                            Console.WriteLine(")");
                        }
                        break;
                    case Opcode.ireturn:
                        Console.WriteLine("ireturn");
                        break;
                    case Opcode.lreturn:
                        Console.WriteLine("lreturn");
                        break;
                    case Opcode.freturn:
                        Console.WriteLine("freturn");
                        break;
                    case Opcode.dreturn:
                        Console.WriteLine("dreturn");
                        break;
                    case Opcode.areturn:
                        Console.WriteLine("areturn");
                        break;
                    case Opcode.return_:
                        Console.WriteLine("return");
                        break;
                    case Opcode.getstatic:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "getstatic", ClassInfo);
                        break;
                    case Opcode.putstatic:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "putstatic", ClassInfo);
                        break;
                    case Opcode.getfield:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "getfield", ClassInfo);
                        break;
                    case Opcode.putfield:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "putfield", ClassInfo);
                        break;
                    case Opcode.invokevirtual:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "invokevirtual", ClassInfo);
                        break;
                    case Opcode.invokespecial:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "invokespecial", ClassInfo);
                        break;
                    case Opcode.invokestatic:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "invokestatic", ClassInfo);
                        break;
                    case Opcode.invokeinterface:
                        {
                            ushort ConstantIndex = Stream.ReadBigEndianUnsignedInt16();
                            byte Byte1 = Stream.ReadUnsignedInt8();
                            byte Byte2 = Stream.ReadUnsignedInt8();
                            DebugHelper.Assert(Byte2 == 0);
                            Console.WriteLine("invokeinterface {0} {1} 0\t\t; ", ConstantIndex, Byte1);
                            ClassInfo.PrintConstant(ConstantIndex);
                        }
                        break;
                    case Opcode.invokedynamic:
                        {
                            ushort ConstantIndex = Stream.ReadBigEndianUnsignedInt16();
                            byte Byte1 = Stream.ReadUnsignedInt8();
                            byte Byte2 = Stream.ReadUnsignedInt8();
                            DebugHelper.Assert(Byte1 == 0);
                            DebugHelper.Assert(Byte2 == 0);
                            Console.WriteLine("invokedynamic {0} 0 0\t\t; ", ConstantIndex);
                            ClassInfo.PrintConstant(ConstantIndex);
                        }
                        break;
                    case Opcode.new_:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "new", ClassInfo);
                        break;
                    case Opcode.newarray:
                        {
                            ValueType ValueType = (ValueType)Stream.ReadUnsignedInt8();
                            string ValueTypeString = ValueType.ToString();
                            Console.WriteLine("newarray {0}\t\t; {1}", ValueType, ValueTypeString);
                        }
                        break;
                    case Opcode.anewarray:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "anewarray", ClassInfo);
                        break;
                    case Opcode.arraylength:
                        Console.WriteLine("arraylength");
                        break;
                    case Opcode.athrow:
                        Console.WriteLine("athrow");
                        break;
                    case Opcode.checkcast:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "checkcast", ClassInfo);
                        break;
                    case Opcode.instanceof:
                        Disassembler_Disassemble_ConstantIndex_UShort(Stream, "instanceof", ClassInfo);
                        break;
                    case Opcode.monitorenter:
                        Console.WriteLine("monitorenter");
                        break;
                    case Opcode.monitorexit:
                        Console.WriteLine("monitorexit");
                        break;
                    case Opcode.wide:
                        Console.WriteLine("wide:\n");
                        ConsoleHelper.PrintIndent(Indent);
                        CurrentCode = Stream.GetDataPointer();
                        Console.WriteLine("{0} ", CurrentCode.ToString("000"));
                        Opcode = (Opcode)Stream.ReadUnsignedInt8();
                        switch (Opcode)
                        {
                            case Opcode.iload:
                                Disassembler_Disassemble_Local_UShort(Stream, "iload");
                                break;
                            case Opcode.lload:
                                Disassembler_Disassemble_Local_UShort(Stream, "lload");
                                break;
                            case Opcode.fload:
                                Disassembler_Disassemble_Local_UShort(Stream, "fload");
                                break;
                            case Opcode.dload:
                                Disassembler_Disassemble_Local_UShort(Stream, "dload");
                                break;
                            case Opcode.aload:
                                Disassembler_Disassemble_Local_UShort(Stream, "aload");
                                break;
                            case Opcode.istore:
                                Disassembler_Disassemble_Local_UShort(Stream, "istore");
                                break;
                            case Opcode.lstore:
                                Disassembler_Disassemble_Local_UShort(Stream, "lstore");
                                break;
                            case Opcode.fstore:
                                Disassembler_Disassemble_Local_UShort(Stream, "fstore");
                                break;
                            case Opcode.dstore:
                                Disassembler_Disassemble_Local_UShort(Stream, "dstore");
                                break;
                            case Opcode.astore:
                                Disassembler_Disassemble_Local_UShort(Stream, "astore");
                                break;
                            case Opcode.iinc:
                                {
                                    ushort LocalIndex = Stream.ReadBigEndianUnsignedInt16();
                                    ushort Immediate = Stream.ReadBigEndianUnsignedInt16();
                                    int Value = (short)Immediate;
                                    Console.WriteLine("iinc {0} {1}", LocalIndex, Value);
                                }
                                break;
                            case Opcode.ret:
                                Disassembler_Disassemble_Local_UShort(Stream, "ret");
                                break;
                            default:
                                Console.WriteLine("opecode={0}", Opcode);
                                DebugHelper.Assert(false);
                                break;
                        }
                        break;
                    case Opcode.multianewarray:
                        {
                            ushort ConstantIndex = Stream.ReadBigEndianUnsignedInt16();
                            byte Dimension = Stream.ReadUnsignedInt8();
                            Console.WriteLine("multianewarray {0} {1}\t\t; ", ConstantIndex, Dimension);
                            ClassInfo.PrintConstant(ConstantIndex);
                        }
                        break;
                    case Opcode.ifnull:
                        Disassembler_Disassemble_Offset_UShort(Stream, "ifnull", CurrentCode);
                        break;
                    case Opcode.ifnonnull:
                        Disassembler_Disassemble_Offset_UShort(Stream, "ifnonnull", CurrentCode);
                        break;
                    case Opcode.goto_w:
                        Disassembler_Disassemble_Offset_UInt(Stream, "goto_w", CurrentCode);
                        break;
                    case Opcode.jsr_w:
                        Disassembler_Disassemble_Offset_UInt(Stream, "jsr_w", CurrentCode);
                        break;
                    case Opcode.breakpoint:
                        Console.WriteLine("breakpoint");
                        break;
                    case Opcode.impdep1:
                        Console.WriteLine("impdep1");
                        break;
                    case Opcode.impdep2:
                        Console.WriteLine("impdep2");
                        break;
                    default:
                        Console.WriteLine("opecode={0}", Opcode);
                        DebugHelper.Assert(false);
                        break;
                }
            }
        }
    }
}
