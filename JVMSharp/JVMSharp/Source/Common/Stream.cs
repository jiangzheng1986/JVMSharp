using System;
using System.IO;

namespace JVMSharp
{
    public class Stream
    {
		MemoryStream _MemoryStream;
		BinaryWriter _BinaryWriter;
		BinaryReader _BinaryReader;

		public Stream()
		{
			_MemoryStream = new MemoryStream();
			_BinaryWriter = new BinaryWriter(_MemoryStream);
			_BinaryReader = new BinaryReader(_MemoryStream);
		}

		public Stream(int Capacity)
		{
			_MemoryStream = new MemoryStream(Capacity);
			_BinaryWriter = new BinaryWriter(_MemoryStream);
			_BinaryReader = new BinaryReader(_MemoryStream);
		}

		public Stream(byte[] Buffer)
		{
			_MemoryStream = new MemoryStream(Buffer.Length);
			_BinaryWriter = new BinaryWriter(_MemoryStream);
			_BinaryReader = new BinaryReader(_MemoryStream);
			Write(Buffer);
			SetDataPointer(0);
		}		

		public byte[] GetData()
		{
			return _MemoryStream.ToArray();
		}

		public int GetLength()
		{
			return (int)_MemoryStream.Length;
		}

		public int GetDataPointer()
		{
			return (int)_MemoryStream.Position;
		}

		public void SetDataPointer(int DataPointer)
		{
			_MemoryStream.Position = DataPointer;
		}

		public void ResetDataPointer()
		{
			_MemoryStream.Position = 0;
		}

		public void Skip(int Bytes)
		{
			_MemoryStream.Position += Bytes;
		}

		public void ReserveStream(int Capacity)
		{
			_MemoryStream.Capacity = Capacity;
		}

		public void ResizeStream(int Length)
		{
			_MemoryStream.SetLength(Length);
		}

		public byte[] Read(int Count)
		{
			return _BinaryReader.ReadBytes(Count);
		}

		public char[] ReadChars(int Count)
		{
			return _BinaryReader.ReadChars(Count);
		}

		public void Write(byte[] Buffer)
		{
			_BinaryWriter.Write(Buffer, 0, Buffer.Length);
		}

		public void Write(byte[] Buffer, int Length)
		{
			_BinaryWriter.Write(Buffer, 0, Length);
		}

		public void WriteChars(char[] Buffer, int Length)
		{
			_BinaryWriter.Write(Buffer, 0, Length);
		}

		public bool ReadBool()
		{
			return _BinaryReader.ReadBoolean();
		}

		public void WriteBool(bool b)
		{
			_BinaryWriter.Write(b);
		}

		public char ReadInt8()
		{
			return _BinaryReader.ReadChar();
		}

		public void WriteInt8(char c)
		{
			_BinaryWriter.Write(c);
		}

		public byte ReadUnsignedInt8()
		{
			return _BinaryReader.ReadByte();
		}

		public void WriteUnsignedInt8(byte b)
		{
			_BinaryWriter.Write(b);
		}

		public short ReadInt16()
		{
			return _BinaryReader.ReadInt16();
		}

		public void WriteInt16(short s)
		{
			_BinaryWriter.Write(s);
		}

		public ushort ReadUnsignedInt16()
		{
			return _BinaryReader.ReadUInt16();
		}

		public void WriteUnsignedInt16(ushort us)
		{
			_BinaryWriter.Write(us);
		}

		public int ReadInt32()
		{
			return _BinaryReader.ReadInt32();
		}

		public void WriteInt32(int i)
		{
			_BinaryWriter.Write(i);
		}

		public uint ReadUnsignedInt32()
		{
			return _BinaryReader.ReadUInt32();
		}

		public void WriteUnsignedInt32(uint ui)
		{
			_BinaryWriter.Write(ui);
		}

		public long ReadInt64()
		{
			return _BinaryReader.ReadInt64();
		}

		public void WriteInt64(long l)
		{
			_BinaryWriter.Write(l);
		}

		public ulong ReadUnsignedInt64()
		{
			return _BinaryReader.ReadUInt64();
		}

		public void WriteUnsignedInt64(ulong ul)
		{
			_BinaryWriter.Write(ul);
		}

		public float ReadFloat32()
		{
			return _BinaryReader.ReadSingle();
		}

		public void WriteFloat32(float f)
		{
			_BinaryWriter.Write(f);
		}

		public double ReadFloat64()
		{
			return _BinaryReader.ReadDouble();
		}

		public void WriteFloat64(double d)
		{
			_BinaryWriter.Write(d);
		}

		public string ReadString()
		{
			int n = ReadInt32();
			char[] Bytes = ReadChars(n);
			return new string(Bytes);
		}

		public void WriteString(string s)
		{
			int n = s.Length;
			WriteInt32(n);
			WriteChars(s.ToCharArray(), n);
		}

		public int WriteDummyCount()
		{
			int SavedDataPointer = GetDataPointer();
			WriteInt32(0);
			return SavedDataPointer;
		}

		public void WriteRealCount(int SavedDataPointer, int Count)
		{
			int SavedDataPointer1 = GetDataPointer();
			SetDataPointer(SavedDataPointer);
			WriteInt32(Count);
			SetDataPointer(SavedDataPointer1);
		}

		public bool CompareTo(Stream Stream)
		{
			if (Stream == null)
			{
				return false;
			}
			long Length1 = _MemoryStream.Length;
			long Length2 = Stream._MemoryStream.Length;
			if (Length1 != Length2)
			{
				return false;
			}
			if (Length1 == 0)
			{
				return true;
			}
			byte[] Buffer1 = _MemoryStream.GetBuffer();
			byte[] Buffer2 = Stream._MemoryStream.GetBuffer();
			for (int i = 0; i < Length1; i++)
			{
				byte Byte1 = Buffer1[i];
				byte Byte2 = Buffer2[i];
				if (Byte1 != Byte2)
				{
					return false;
				}
			}
			return true;
		}

		public Stream Copy()
		{			
			int StreamLength = GetLength();
			Stream StreamNew = new Stream(StreamLength);
			byte[] Bytes = StreamNew.GetData();
			StreamNew.Write(Bytes);
			StreamNew.SetDataPointer(0);
			return StreamNew;
		}

        public ushort ReadBigEndianUnsignedInt16()
        {
            return EndianHelper.Reverse(_BinaryReader.ReadUInt16());
        }

        public short ReadBigEndianInt16()
        {
            return (short)EndianHelper.Reverse(_BinaryReader.ReadUInt16());
        }

        public uint ReadBigEndianUnsignedInt32()
        {
            return EndianHelper.Reverse(_BinaryReader.ReadUInt32());
        }

        public int ReadBigEndianInt32()
        {
            return (int)EndianHelper.Reverse(_BinaryReader.ReadUInt32());
        }

        public ulong ReadBigEndianUnsignedInt64()
        {
            return EndianHelper.Reverse(_BinaryReader.ReadUInt64());
        }

        public long ReadBigEndianInt64()
        {
            return (long)EndianHelper.Reverse(_BinaryReader.ReadUInt64());
        }

        public float ReadBigEndianFloat32()
        {
            uint UInt = ReadBigEndianUnsignedInt32();
            return BitConverter.Int32BitsToSingle((int)UInt);
        }

        public double ReadBigEndianFloat64()
        {
            ulong ULong = ReadBigEndianUnsignedInt64();
            return BitConverter.Int64BitsToDouble((long)ULong);
        }

        public string ReadBigEndianShortString()
        {
            ushort n = ReadBigEndianUnsignedInt16();
            char[] Bytes = ReadChars(n);
            return new string(Bytes);
        }
    }
}
