namespace JVMSharp
{
    static class EndianHelper
    {
        public static ushort Reverse(ushort Value)
        {
            uint Temp1 = 0;
            uint Temp2 = Value;
            Temp1 = Temp2 >> 8;
            Temp1 = Temp1 | ((Temp2 & 0xff) << 8);
            return (ushort)Temp1;
        }

        public static uint Reverse(uint Value)
        {
            uint Byte1 = ((Value & 0x000000ff) >> 0);
            uint Byte2 = ((Value & 0x0000ff00) >> 8);
            uint Byte3 = ((Value & 0x00ff0000) >> 16);
            uint Byte4 = ((Value & 0xff000000) >> 24);
            return Byte4 | (Byte3 << 8) | (Byte2 << 16) | (Byte1 << 24);
        }

        public static ulong Reverse(ulong Value)
        {
            ulong Byte1 = ((Value & 0x00000000000000ff) >> 0);
            ulong Byte2 = ((Value & 0x000000000000ff00) >> 8);
            ulong Byte3 = ((Value & 0x0000000000ff0000) >> 16);
            ulong Byte4 = ((Value & 0x00000000ff000000) >> 24);
            ulong Byte5 = ((Value & 0x000000ff00000000) >> 32);
            ulong Byte6 = ((Value & 0x0000ff0000000000) >> 40);
            ulong Byte7 = ((Value & 0x00ff000000000000) >> 48);
            ulong Byte8 = ((Value & 0xff00000000000000) >> 56);
            return Byte8 | (Byte7 << 8) | (Byte6 << 16) | (Byte5 << 24) | (Byte4 << 32) | (Byte3 << 40) | (Byte2 << 48) | (Byte1 << 56);
        }
    }
}
