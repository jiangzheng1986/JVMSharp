using System.Collections.Generic;
using System.IO;

namespace JVMSharp
{
    public class DirectoryWalker
    {
        List<DirectoryWalkItem> DirectoryWalkItems;

        public DirectoryWalker()
		{
			DirectoryWalkItems = new List<DirectoryWalkItem>();
		}

		public void WalkDirectory(string Directory, bool bRecursively)
		{
			DirectoryWalkItems.Clear();
			if (Directory == "")
			{
				return;
			}
			InnerWalkDirectory(Directory, bRecursively);
		}

		void InnerWalkDirectory(string Directory, bool bRecursively)
		{
			DirectoryInfo DirectoryInfo = new DirectoryInfo(Directory);
			DirectoryInfo[] SubDirectoryInfos = DirectoryInfo.GetDirectories();
			int Count = SubDirectoryInfos.Length;
			for (int i = 0; i < Count; i++)
			{
				DirectoryInfo SubDirectoryInfo = SubDirectoryInfos[i];				
				DirectoryWalkItem Item = new DirectoryWalkItem();
				Item.bIsDirectory = true;
				Item.Path = PathHelper.ToStandardForm(SubDirectoryInfo.FullName);
				Item.LastWriteTime = SubDirectoryInfo.LastWriteTime.Ticks;  //LastWriteTimeUtc?
				Item.FileSize = 0;
				DirectoryWalkItems.Add(Item);
				if (bRecursively)
				{
					InnerWalkDirectory(SubDirectoryInfo.FullName, bRecursively);
				}
			}
			FileInfo[] FileInfos = DirectoryInfo.GetFiles();
			Count = FileInfos.Length;
			for (int i = 0; i < Count; i++)
			{
				FileInfo FileInfo = FileInfos[i];
				DirectoryWalkItem Item = new DirectoryWalkItem();
				Item.bIsDirectory = false;
				Item.Path = PathHelper.ToStandardForm(FileInfo.FullName);
				Item.LastWriteTime = FileInfo.LastWriteTime.Ticks;			//LastWriteTimeUtc?
				Item.FileSize = 0;
				DirectoryWalkItems.Add(Item);
			}	
		}

		public int GetDirectoryWalkItemCount()
		{
			return DirectoryWalkItems.Count;
		}

		public DirectoryWalkItem GetDirectoryWalkItem(int i)
		{
			return DirectoryWalkItems[i];
		}
    }
}
