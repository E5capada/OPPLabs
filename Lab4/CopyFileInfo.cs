using System;

namespace OPPLab4_2
{
    public class FileInfo
    {
        public DateTime CreationTime;
        public long Length { get; }
        public string FilePath;
        public string BackupFilePath;

        public FileInfo(string filePath)
        {
            FilePath = filePath;
        }
    }

    public class CopyFileInfo
    {
        public DateTime CreationTime;
        public long BackupSize;
        public string FilePath;
        public string BackupFilePath;

        public CopyFileInfo(string filePath, long backupSize, DateTime creationTime, string backupFilePath)
        {
            FilePath = filePath;
            BackupSize = backupSize;
            CreationTime = creationTime;
            BackupFilePath = backupFilePath;
        }
    }
}