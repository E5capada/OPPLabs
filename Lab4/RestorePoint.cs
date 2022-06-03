using System;
using System.Collections.Generic;

namespace OPPLab4_2
{
    public abstract class RestorePoint
    {
        private static ulong _nextId = 1;

        public RestorePoint PreviousPoint;
        public readonly ulong Id;
        public readonly Dictionary<string, CopyFileInfo> Info;
        public readonly DateTime CreationDate;

        public RestorePoint(RestorePoint previousPoint, Dictionary<string, CopyFileInfo> info, DateTime creationDate)
        {
            PreviousPoint = previousPoint;
            Id = _nextId++;
            Info = info;
            CreationDate = creationDate;
        }

        public long Size()
        {
            long allSize = 0;
            foreach (var info in Info)
            {
                allSize += info.Value.BackupSize;
            }

            return allSize;
        }
    }

    public class FullBackupPoint : RestorePoint
    {
        public FullBackupPoint(RestorePoint previousPoint, Dictionary<string, CopyFileInfo> info, DateTime creationDate) : base(previousPoint, info, creationDate)
        {

        }
    }

    public class IncrementalBackupPoint : RestorePoint
    {
        public IncrementalBackupPoint(RestorePoint previousPoint, Dictionary<string, CopyFileInfo> info, DateTime creationDate) : base(previousPoint, info, creationDate)
        {

        }
    }
}
