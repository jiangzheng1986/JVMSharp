using System.IO;
using System.Text;

namespace JVMSharp
{
    public static class FileHelper
    {
        public static bool IsFileExists(string Filename)
        {
            return File.Exists(Filename);
        }

        public static bool CopyFile(string Source, string Target)
        {
            try
            {
                File.Copy(Source, Target);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool RenameFile(string Filename, string NewFilename)
        {
            try
            {
                File.Move(Filename, NewFilename);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void DeleteFile(string Filename)
        {
            File.Delete(Filename);
        }

        public static Stream ReadFile(string Filename)
        {
            byte[] Bytes = null;
            try
            {
                Bytes = File.ReadAllBytes(Filename);
            }
            catch
            {
                return null;
            }
            Stream Stream = new Stream(Bytes.Length);
            Stream.Write(Bytes);
            Stream.SetDataPointer(0);
            return Stream;
        }

        public static bool WriteFile(string Filename, Stream Stream)
        {
            try
            {
                string DirectoryName = Path.GetDirectoryName(Filename);
                DirectoryHelper.CreateDirectory(DirectoryName);
                byte[] Bytes = Stream.GetData();
                File.WriteAllBytes(Filename, Bytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ReadTextFile(string Filename)
        {
            try
            {
                return File.ReadAllText(Filename);
            }
            catch
            {
                return "";
            }
        }

        public static string ReadTextFile(string Filename, Encoding Encoding)
        {
            try
            {
                return File.ReadAllText(Filename, Encoding);
            }
            catch
            {
                return "";
            }
        }

        public static string ReadTextFile(string Filename, out TextFileFormat TextFileFormat)
        {
            TextFileFormat = TextFileFormat.ANSI;
            return ReadTextFile(Filename);
        }

        public static bool WriteTextFile(string Filename, string Content)
        {
            return WriteTextFile(Filename, Content, Encoding.Unicode);
        }

        public static bool WriteTextFile(string Filename, string Content, Encoding Encoding)
        {
            try
            {
                string DirectoryName = Path.GetDirectoryName(Filename);
                DirectoryHelper.CreateDirectory(DirectoryName);
                File.WriteAllText(Filename, Content, Encoding);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool WriteTextFile(string Filename, string Content, TextFileFormat TextFileFormat)
        {
            return WriteTextFile(Filename, Content, Encoding.UTF8);
        }

        public static long GetFileModifyTime(string Filename)
        {
            try
            {
                return File.GetLastWriteTimeUtc(Filename).Ticks;
            }
            catch
            {
                return 0;
            }            
        }
    }
}
