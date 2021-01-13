using System.Reflection;
using System.IO;

namespace JVMSharp
{
    public static class DirectoryHelper
    {
        static string ExecutableDirectory;

        public static void SetupWorkingDirectory()
        {
            string ExecutablePath = Assembly.GetExecutingAssembly().Location;
            ExecutableDirectory = Path.GetDirectoryName(ExecutablePath);
            string DirectoryName = Path.GetFileName(ExecutableDirectory);
            if (DirectoryName.Contains("netcoreapp"))
            {
                ExecutableDirectory = Path.GetDirectoryName(ExecutableDirectory);
                ExecutableDirectory = Path.GetDirectoryName(ExecutableDirectory);
            }
            ExecutableDirectory = PathHelper.ToStandardForm(ExecutableDirectory);
            Directory.SetCurrentDirectory(ExecutableDirectory);
        }

        public static string GetExecutableDirectory()
        {
            return ExecutableDirectory;
        }

        public static bool IsDirectoryExists(string DirectoryName)
        {
            return Directory.Exists(DirectoryName);
        }

        public static void CreateDirectory(string DirectoryName)
        {
            try
            {
                if (Directory.Exists(DirectoryName) == false)
                {
                    Directory.CreateDirectory(DirectoryName);
                }
            }
            catch
            { 
            }
        }

        public static void DeleteDirectory(string DirectoryName)
        {
            bool bRecursive = true;
            Directory.Delete(DirectoryName, bRecursive);
        }

        public static void CopyDirectory(string SourceDirectory, string TargetDirectory)
        {
            DirectoryInfo SourceDirectoryInfo = new DirectoryInfo(SourceDirectory);
            DirectoryInfo TargetDirectoryInfo = new DirectoryInfo(TargetDirectory);
            InnerCopyDirectory(SourceDirectoryInfo, TargetDirectoryInfo);
        }

        static void InnerCopyDirectory(DirectoryInfo SourceDirectoryInfo, DirectoryInfo TargetDirectoryInfo)
        {
            Directory.CreateDirectory(TargetDirectoryInfo.FullName);
            foreach (FileInfo fi in SourceDirectoryInfo.GetFiles())
            {
                fi.CopyTo(Path.Combine(TargetDirectoryInfo.FullName, fi.Name), true);
            }
            foreach (DirectoryInfo ChildSourceDirectoryInfo in SourceDirectoryInfo.GetDirectories())
            {
                DirectoryInfo ChildTargetDirectoryInfo = TargetDirectoryInfo.CreateSubdirectory(ChildSourceDirectoryInfo.Name);
                InnerCopyDirectory(ChildSourceDirectoryInfo, ChildTargetDirectoryInfo);
            }
        }

        public static void RenameDirectory(string OldDirectoryName, string NewDirectoryName)
        {
            DirectoryInfo DirectoryInfo = new DirectoryInfo(OldDirectoryName);
            DirectoryInfo.MoveTo(NewDirectoryName);
        }

        public static DirectoryInfo[] GetSubDirectories(string DirectoryName)
        {
            try
            {
                DirectoryInfo DirectoryInfo = new DirectoryInfo(DirectoryName);
                DirectoryInfo[] SubDirectories = DirectoryInfo.GetDirectories();
                return SubDirectories;
            }
            catch
            {
                return new DirectoryInfo[0];
            }
        }

        public static FileInfo[] GetFilesInDirectory(string DirectoryName, bool bRecursively = false)
        {
            try
            {
                DirectoryInfo DirectoryInfo = new DirectoryInfo(DirectoryName);
                FileInfo[] Files;
                if (bRecursively == false)
                {
                    Files = DirectoryInfo.GetFiles();
                }
                else
                {
                    Files = DirectoryInfo.GetFiles("*.*", SearchOption.AllDirectories);
                }
                return Files;
            }
            catch
            {
                return new FileInfo[0];
            }
        }

        public static void GetSubDirectoriesAndFiles(string DirectoryName, out DirectoryInfo[] SubDirectories, out FileInfo[] Files)
        {
            try
            {
                DirectoryInfo DirectoryInfo = new DirectoryInfo(DirectoryName);
                SubDirectories = DirectoryInfo.GetDirectories();
                Files = DirectoryInfo.GetFiles();
            }
            catch
            {
                SubDirectories = new DirectoryInfo[0];
                Files = new FileInfo[0];
            }
        }
    }
}
