using System;
using System.Collections.Generic;

namespace OPPLab4_2
{
    public interface ICleaningAlgorithm
    {
        // ToDO: список точек, которые нужно удалить + удаление из диска
        public RestorePoint Clean(RestorePoint lastPoint, out bool areMorePointsLeft);  // Очистка, возвращается последняя точка
        public long CountLeftPoints(RestorePoint lastPoint);  // Подсчет без удаления
    }

    class CountCleaningAlgorithm : ICleaningAlgorithm
    {
        public readonly long LastNPoints;

        public CountCleaningAlgorithm(long lastNPoints)
        {
            LastNPoints = lastNPoints;
        }


        public RestorePoint Clean(RestorePoint lastPoint, out bool areMorePointsLeft)
        {
            areMorePointsLeft = false;
            var k = LastNPoints - 1;
            var point = lastPoint;

            while (point != null && (k > 0 || point is IncrementalBackupPoint))
            {
                point = point.PreviousPoint;
                --k;
            }

            if (k <= 0 && point != null) point.PreviousPoint = null;
            if (k < 0) areMorePointsLeft = true;
            return lastPoint;
        }

        public long CountLeftPoints(RestorePoint lastPoint)
        {
            var k = LastNPoints - 1;
            var point = lastPoint;

            while (point != null && (k > 0 || point is IncrementalBackupPoint))
            {
                point = point.PreviousPoint;
                --k;
            }

            return LastNPoints - k;
        }
    }

    public class DateCleaningAlgorithm : ICleaningAlgorithm
    {
        public readonly DateTime Date;

        public DateCleaningAlgorithm(DateTime date)
        {
            Date = date;
        }

        public RestorePoint Clean(RestorePoint lastPoint, out bool areMorePointsLeft)
        {
            areMorePointsLeft = false;
            var point = lastPoint;
            RestorePoint prevPoint = null;

            while (point != null && (point.CreationDate >= Date || point is IncrementalBackupPoint))
            {
                prevPoint = point;
                point = point.PreviousPoint;
            }

            if (prevPoint != null)
                prevPoint.PreviousPoint = null;

            return lastPoint;
        }

        public long CountLeftPoints(RestorePoint lastPoint)
        {
            long k = 0;
            var point = lastPoint;

            while (point != null && (point.CreationDate >= Date || point is IncrementalBackupPoint))
            {
                point = point.PreviousPoint;
                k++;
            }

            return k;
        }
    }


    public class HybridCleaningAlgorithm : ICleaningAlgorithm
    {
        // Коллекция алгоритмов

        public List<ICleaningAlgorithm> Algorithms { get; }
        public readonly bool MinimumFlag;

        public HybridCleaningAlgorithm(List<ICleaningAlgorithm> algorithms, bool minimumFlag)
        {
            Algorithms = algorithms;
            MinimumFlag = minimumFlag;
        }

        public RestorePoint Clean(RestorePoint lastPoint, out bool areMorePointsLeft)
        {
            ICleaningAlgorithm selectedAlgorithm = null;
            long numberOfLeftPoints = 0;
            var isNumberSet = false;

            foreach (var algorithm in Algorithms)
            {
                if (MinimumFlag)
                {
                    // minimum
                    if (algorithm.CountLeftPoints(lastPoint) < numberOfLeftPoints || !isNumberSet)
                    {
                        isNumberSet = true;
                        numberOfLeftPoints = algorithm.CountLeftPoints(lastPoint);
                        selectedAlgorithm = algorithm;
                    }
                }
                else
                {
                    // maximum
                    if (algorithm.CountLeftPoints(lastPoint) > numberOfLeftPoints || !isNumberSet)
                    {
                        isNumberSet = true;
                        numberOfLeftPoints = algorithm.CountLeftPoints(lastPoint);
                        selectedAlgorithm = algorithm;
                    }
                }
            }

            if (selectedAlgorithm == null)
                throw new NullReferenceException("Nothing selected for cleaning algorithm");

            return selectedAlgorithm.Clean(lastPoint, out areMorePointsLeft);
        }

        public long CountLeftPoints(RestorePoint lastPoint)
        {
            long numberOfLeftPoints = 0;
            var isNumberSet = false;

            foreach (var algorithm in Algorithms)
            {
                if (MinimumFlag)
                {
                    // minimum
                    if (algorithm.CountLeftPoints(lastPoint) < numberOfLeftPoints || !isNumberSet)
                    {
                        isNumberSet = true;
                        numberOfLeftPoints = algorithm.CountLeftPoints(lastPoint);
                    }
                }
                else
                {
                    // maximum
                    if (algorithm.CountLeftPoints(lastPoint) > numberOfLeftPoints || !isNumberSet)
                    {
                        isNumberSet = true;
                        numberOfLeftPoints = algorithm.CountLeftPoints(lastPoint);
                    }
                }
            }

            return numberOfLeftPoints;
        }

        public class SizeCleaningAlgorithm : ICleaningAlgorithm
        {
            public readonly long MaxSize;

            public SizeCleaningAlgorithm(long maxSizeInBytes)
            {
                MaxSize = maxSizeInBytes;
            }

            public RestorePoint Clean(RestorePoint lastPoint, out bool areMorePointsLeft)
            {
                areMorePointsLeft = false;
                var point = lastPoint;
                RestorePoint prevPoint = null;
                long currentSize = 0;

                while (point != null && (currentSize < MaxSize || point is IncrementalBackupPoint))
                {
                    if (point.Size() + currentSize <= MaxSize || point is IncrementalBackupPoint)
                    {
                        currentSize += point.Size();
                        prevPoint = point;
                        point = point.PreviousPoint;
                        continue;
                    }
                    break;
                }

                if (prevPoint != null)
                    prevPoint.PreviousPoint = null;

                if (currentSize > MaxSize) areMorePointsLeft = true;

                return lastPoint;
            }

            public long CountLeftPoints(RestorePoint lastPoint)
            {
                long k = 0;
                var point = lastPoint;
                long currentSize = 0;

                while (point != null && (currentSize < MaxSize || point is IncrementalBackupPoint))
                {
                    if (point.Size() + currentSize <= MaxSize)
                    {
                        k++;
                        currentSize += point.Size();
                        point = point.PreviousPoint;
                        continue;
                    }
                    break;
                }

                return k;
            }
        }
    }
}

