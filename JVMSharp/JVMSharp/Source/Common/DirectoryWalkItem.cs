namespace JVMSharp
{
    public class DirectoryWalkItem
    {
        public bool bIsDirectory;
        public string Path;
        public long LastWriteTime;
        public int FileSize;

        public DirectoryWalkItem()
        {
            bIsDirectory = false;
            Path = "";
            LastWriteTime = 0;
            FileSize = 0;
        }
    }
}
