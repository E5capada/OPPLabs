using System;
using System.Collections.Generic;
using static OPPLab4_2.HybridCleaningAlgorithm;


namespace OPPLab4_2
{
    class Programm
    {
        
        public static void Main(string[] args)
        {
            // Кейс 1
            Console.WriteLine("Кейс 1");

            var restoreManager1 = new RestoreManager();
            restoreManager1.AddObjectForBackupList("/Users/macbook/Desktop/Programming/3semester/OOP/OPPLab4_2/test-speed-100Mb.bin");
            restoreManager1.AddObjectForBackupList("/Users/macbook/Desktop/Programming/3semester/OOP/OPPLab4_2/pr_influence-770x380-1.png");

            restoreManager1.CreateFullBackup();


            restoreManager1.CreateFullBackup();
            restoreManager1.PrintHistory();

            Console.WriteLine("------------------ Cleaning --------------------------------\n");

            restoreManager1.CleaningAlgorithm = new CountCleaningAlgorithm(1);
            Console.WriteLine($"Count of points after cleaning: {restoreManager1.CountLeftPoints()}\n");

            try
            {
                restoreManager1.Clean();
            }
            catch
            {
                Console.WriteLine("more points are left");
                Console.WriteLine();
            }

            restoreManager1.PrintHistory();


            // ---------------------------------------------------------------------------------------------------------

            // Кейс 2
            Console.WriteLine();
            Console.WriteLine("Кейс 2");
            Console.WriteLine("------\n");

            var restoreManager2 = new RestoreManager();
            restoreManager2.AddObjectForBackupList("a.txt");
            restoreManager2.AddObjectForBackupList("b.txt");

            restoreManager2.CreateFullBackup();
            

            restoreManager2.CreateFullBackup();
            restoreManager2.PrintHistory();

            Console.WriteLine("------------------ Cleaning --------------------------------\n");

            restoreManager2.CleaningAlgorithm = new SizeCleaningAlgorithm(35);
            Console.WriteLine($"Count of points after cleaning: {restoreManager2.CountLeftPoints()}\n");

            try
            {
                restoreManager2.Clean();
            }
            catch 
            {
                Console.WriteLine("more points are left");
                Console.WriteLine();
            }

            restoreManager2.PrintHistory();

            // ---------------------------------------------------------------------------------------------------------

            // Кейс 3
            Console.WriteLine();
            Console.WriteLine("Кейс 3");
            Console.WriteLine("------\n");

            var restoreManager3 = new RestoreManager();
            restoreManager3.AddObjectForBackupList("c.txt");
            restoreManager3.AddObjectForBackupList("d.txt");

            restoreManager3.CreateFullBackup();

            for (var i = 0; i < 2; i++)
            {
                Console.WriteLine("Change files and press enter...");
                Console.ReadLine();
                restoreManager3.CreateIncrementalBackup();
            }

            // restoreManager3.CreateFullBackup();

            restoreManager3.PrintHistory();

            Console.WriteLine("------------------ Cleaning --------------------------------\n");

            restoreManager3.CleaningAlgorithm = new CountCleaningAlgorithm(1);
            Console.WriteLine($"Count of points after cleaning: {restoreManager3.CountLeftPoints()}\n");

            try
            {
                restoreManager3.Clean();
            }
            catch 
            {
                Console.WriteLine("more points are left");
                Console.WriteLine();
            }

            restoreManager3.PrintHistory();


            // ---------------------------------------------------------------------------------------------------------

            // Кейс 4

            DateTimeProvider.UseCustomDate = true;

            Console.WriteLine();
            Console.WriteLine("Кейс 4");
            Console.WriteLine("------\n");

            var restoreManager4 = new RestoreManager();
            restoreManager4.AddObjectForBackupList("c.txt");
            restoreManager4.AddObjectForBackupList("d.txt");

            DateTimeProvider.CustomDate = new DateTime(2020, 11, 20);
            restoreManager4.CreateFullBackup();
            DateTimeProvider.CustomDate = new DateTime(2020, 11, 21);
            restoreManager4.CreateFullBackup();
            DateTimeProvider.CustomDate = new DateTime(2020, 11, 22);
            restoreManager4.CreateFullBackup();

            Console.WriteLine("Change files and press enter...");
            Console.ReadLine();
            DateTimeProvider.CustomDate = new DateTime(2020, 11, 23);
            restoreManager4.CreateIncrementalBackup();

            restoreManager4.PrintHistory();

            Console.WriteLine("------------------ Cleaning --------------------------------\n");

            var algorithmsList = new List<ICleaningAlgorithm>()
        {
        new CountCleaningAlgorithm(2),
        new DateCleaningAlgorithm(new DateTime(2020, 11, 21))
        };

            restoreManager4.CleaningAlgorithm = new HybridCleaningAlgorithm(algorithmsList, false);
            Console.WriteLine($"Count of points after cleaning: {restoreManager4.CountLeftPoints()}\n");

            try
            {
                restoreManager4.Clean();
            }
            catch
            {
                Console.WriteLine("Warning: more points are left");
                Console.WriteLine();
            }

            restoreManager4.PrintHistory();

            DateTimeProvider.UseCustomDate = false;



            DateTimeProvider.UseCustomDate = true;

            Console.WriteLine();
            Console.WriteLine("Кейс 5");
            Console.WriteLine("------\n");

            var restoreManager5 = new RestoreManager();
            restoreManager5.AddObjectForBackupList("c.txt");
            restoreManager5.AddObjectForBackupList("d.txt");

            DateTimeProvider.CustomDate = new DateTime(2020, 11, 20);
            restoreManager5.CreateFullBackup();
            DateTimeProvider.CustomDate = new DateTime(2020, 11, 21);
            restoreManager5.CreateFullBackup();
            DateTimeProvider.CustomDate = new DateTime(2020, 11, 22);
            restoreManager5.CreateFullBackup();
            DateTimeProvider.CustomDate = new DateTime(2020, 11, 23);
            restoreManager5.CreateFullBackup();


            restoreManager5.PrintHistory();

            Console.WriteLine("------------------ Cleaning --------------------------------\n");

            restoreManager5.CleaningAlgorithm = new HybridCleaningAlgorithm(algorithmsList, true);
            Console.WriteLine($"Count of points after cleaning: {restoreManager5.CountLeftPoints()}\n");

            try
            {
                restoreManager5.Clean();
            }
            catch
            {
                throw new Exception("more points are left");//

            }

            restoreManager5.PrintHistory();

            DateTimeProvider.UseCustomDate = false;
            
        }
            
    }
}


