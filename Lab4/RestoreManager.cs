using System;
using System.Collections.Generic;
using System.IO;

namespace OPPLab4_2
{

    public class RestoreManager
    {
        public List<string> ObjectsForBackup { get; }
        public RestorePoint LastPoint { get; protected set; }
        public ICleaningAlgorithm CleaningAlgorithm { get; set; }

        public RestoreManager()
        {
            ObjectsForBackup = new List<string>();
            LastPoint = null;
            CleaningAlgorithm = null;
        }

        public void CreateCustomBackup(CreatingAndAndStoring backupCreator)
        {
            LastPoint = backupCreator.CreateBackup(LastPoint, ObjectsForBackup);
        }

        public void CreateFullBackup()
        {
            LastPoint = new FullBackupCreatingAndAndStoring(".", TypeOfStoring.Archive).CreateBackup(LastPoint, ObjectsForBackup);
        }

        public void CreateIncrementalBackup()
        {
            if (LastPoint == null) throw new Exception("BackUp is First");
            LastPoint = new IncrementalCreatingAndAndStoring(".", TypeOfStoring.Folder).CreateBackup(LastPoint, ObjectsForBackup);
        }

        public void Clean()
        {
            LastPoint = CleaningAlgorithm.Clean(LastPoint, out var areMorePointsLeft);
            if (areMorePointsLeft)
                throw new Exception("MORE POINTS LEFT");
        }

        public long CountLeftPoints()
        {
            return CleaningAlgorithm.CountLeftPoints(LastPoint);
        }

        public void AddObjectForBackupList(string path)
        {
            if (!File.Exists(path)) throw new Exception("FILE NOT FOUND");
            if (!ObjectsForBackup.Contains(path))
                ObjectsForBackup.Add(path);
        }

        public void RemoveObjectFromBackupList(string path)
        {
            if (ObjectsForBackup.Contains(path)) ObjectsForBackup.Remove(path);
        }

        public void PrintHistory()
        {
            var point = LastPoint;
            while (point != null)
            {
                Console.WriteLine($"Point ID: {point.Id}");
                Console.WriteLine($"Point type: {point.GetType()}");
                Console.WriteLine($"Total size: {point.Size()} bytes");
                Console.WriteLine($"Creation date: {point.CreationDate}");
                Console.WriteLine("Saved files:\n");

                foreach (var file in point.Info)
                {
                    Console.Write($"{file.Key} ");
                    Console.WriteLine($"{file.Value.BackupSize} bytes");
                }

                Console.WriteLine("-----------------\n");

                point = point.PreviousPoint;
            }
        }
    }
}

