using System;
using System.Runtime.InteropServices;

namespace JVMSharp
{
    public static class SystemHelper
    {
        static PlatformType _PlatformType;
        static SystemHelper()
        {
            PlatformID PlatformID = Environment.OSVersion.Platform;
            if (PlatformID == PlatformID.Win32NT)
            {
                _PlatformType = PlatformType.Windows;
            }
            else if (PlatformID == PlatformID.MacOSX)
            {
                _PlatformType = PlatformType.MacOSX;
            }
            else if (PlatformID == PlatformID.Unix)
            {
                _PlatformType = PlatformType.Linux;
            }
            else
            {
                _PlatformType = PlatformType.Other;
            }
        }

        public static PlatformType GetPlatformType()
        {
            return _PlatformType;
        }

        public static bool IsWindows()
        {
            return _PlatformType == PlatformType.Windows;
        }

        public static bool IsMacOSX()
        {
            return _PlatformType == PlatformType.MacOSX;
        }

        public static bool IsLinux()
        {
            return _PlatformType == PlatformType.Linux;
        }

        public static double GetTime()
        { 
            return System_GetTime();
        }

        public static long GetTimeMs()
        { 
            return (long)(System_GetTime() * 1000.0);
        }

        public static long GetTimeUs()
        {
            return (long)(System_GetTime() * 1000000.0);
        }

        public static double GetTimeSinceEpoch()
        {
            return System_GetTimeSinceEpoch();
        }

        public static long GetTimeSinceEpochMs()
        {
            return (long)(System_GetTimeSinceEpoch() * 1000.0);
        }

        public static long GetTimeSinceEpochUs()
        {
            return (long)(System_GetTimeSinceEpoch() * 1000000.0);
        }

        public static void ExitProcess(int ExitCode)
        {
            Environment.Exit(ExitCode);
        }
        
        [DllImport("EditorUICore", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        static extern double System_GetTime();

        [DllImport("EditorUICore", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        static extern double System_GetTimeSinceEpoch();
    }
}
