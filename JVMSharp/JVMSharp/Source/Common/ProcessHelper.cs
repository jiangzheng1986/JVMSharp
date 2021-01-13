using System;
using System.Diagnostics;
using System.IO;

namespace JVMSharp
{
	public static class ProcessHelper
    {
		public static void Execute(string Program, string Parameters)
		{
			try
			{
				Process.Start(Program, Parameters);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public static void OpenWebPage(string Url)
		{
			Process Process = new Process();
			Process.StartInfo.UseShellExecute = true;
			Process.StartInfo.FileName = Url;
			try
			{
				Process.Start();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		public static void OpenFolder(string FolderPath)
        {
			string Parameters = "\"" + PathHelper.ToWindowsForm(FolderPath) + "\"";
			Execute("explorer.exe", Parameters);
		}

        public static void OpenFile(string FilePath)
        {
            Process Process = new Process();
            Process.StartInfo.UseShellExecute = true;
            Process.StartInfo.Verb = "open";
            Process.StartInfo.FileName = FilePath;
            Process.StartInfo.CreateNoWindow = false;
            try
            {
                Process.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void OpenCommandLine(string FolderPath)
		{
            Process Process = new Process();
            Process.StartInfo.UseShellExecute = true;
            Process.StartInfo.FileName = "cmd.exe";
            Process.StartInfo.WorkingDirectory = FolderPath;
            Process.StartInfo.CreateNoWindow = false;
            try
            {
                Process.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void OpenContainingFolder(string FilePath)
		{
			string Parameters = "/select,\"" + PathHelper.ToWindowsForm(FilePath) + "\"";
			Execute("explorer.exe", Parameters);
		}
    }
}
