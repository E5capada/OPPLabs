using System;
using System.Collections.Generic;


namespace OPPLab4_2
{
    public enum TypeOfStoring
    {
        Archive,
        Folder
    }

    public abstract class CreatingAndAndStoring
    {
        public string BackupPath { get; protected set; }
        public TypeOfStoring Type { get; protected set; }

        public CreatingAndAndStoring(string backupPath, TypeOfStoring type)
        {
            BackupPath = backupPath;
            Type = type;
        }

        public abstract RestorePoint CreateBackup(RestorePoint lastPoint, List<string> objectsForBackup);

        private void CreateArchibe()
        {
            //createArchive
        }

        private void CreateFolder()
        {
            //createFolder
        }

        public void CopyFile(string filePath)
        {
            if (Type == TypeOfStoring.Archive)
            {
                CreateArchibe();
            }
            else CreateFolder();
        }

        public void RestoreFile(string filePath)
        {
            if (Type == TypeOfStoring.Archive)
            {
                
            }
            else
            {

            }
        }
    }

    public class FullBackupCreatingAndAndStoring : CreatingAndAndStoring
    {
        public FullBackupCreatingAndAndStoring(string backupPath, TypeOfStoring type) : base(backupPath, type)
        {}

        public override RestorePoint CreateBackup(RestorePoint lastPoint, List<string> objectsForBackup)
        {
            var list = new Dictionary<string, CopyFileInfo>();

            foreach (var filePath in objectsForBackup)
            {
                list.Add(filePath, new CopyFileInfo(filePath, new FileInfo(filePath).Length, DateTimeProvider.Now, BackupPath));
                //CopyFile(filePath);
            }
            return new FullBackupPoint(lastPoint, list, DateTimeProvider.Now);
        }
    }

    public class IncrementalCreatingAndAndStoring : CreatingAndAndStoring
    {
        public IncrementalCreatingAndAndStoring(string backupPath, TypeOfStoring type) : base(backupPath, type)
        {}

        public override RestorePoint CreateBackup(RestorePoint lastPoint, List<string> objectsForBackup)
        {
            var lastFullBackupPoint = lastPoint;

            if (lastFullBackupPoint == null)
                throw new Exception("BACKUP DON NOT HAVE INCRIMENT ");

            while (lastFullBackupPoint != null && !(lastFullBackupPoint is FullBackupPoint))
                lastFullBackupPoint = lastFullBackupPoint.PreviousPoint;

             //

            var list = new Dictionary<string, CopyFileInfo>();
            foreach (var filePath in objectsForBackup)
            {
                // Упрощение: Если нет, или изменился размер файла
                if (!lastPoint.Info.ContainsKey(filePath) || lastPoint.Info[filePath].BackupSize != new FileInfo(filePath).Length)
                {
                    var point = lastPoint;
                    while (!point.Info.ContainsKey(filePath) && !(point is FullBackupPoint))
                        point = point.PreviousPoint;

                    var size = new FileInfo(filePath).Length;
                    if (!point.Info.ContainsKey(filePath))
                        size = Math.Abs(size - point.Info[filePath].BackupSize);

                    list.Add(filePath, new CopyFileInfo(filePath, size, DateTimeProvider.Now, BackupPath));
                    //CopyFile(filePath);
                }
            }

            if (list.Count == 0) throw new Exception("NoChangesForIncrementalBackup");

            return new IncrementalBackupPoint(lastPoint, list, DateTimeProvider.Now);
        }
    }

}