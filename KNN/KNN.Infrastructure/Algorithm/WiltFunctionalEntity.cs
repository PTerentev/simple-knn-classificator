using System;
using KNN.Infrastructure.Parsing;
using KNN.Models.Input;

namespace KNN.Infrastructure.Algorithm
{
    /// <summary>
    /// Wilt functional entity.
    /// </summary>
    internal class WiltFunctionalEntity
    {
        public WiltEntity WiltEntity { get; }

        public WiltClasses WiltClass => WiltClassNameParser.Parse(WiltEntity);

        public WiltFunctionalEntity(WiltEntity wiltEntity)
        {
            WiltEntity = wiltEntity;
        }

        public double CalculateDistance(WiltFunctionalEntity functionalEntity)
        {
            return Math.Sqrt(GetDifferencesSum(WiltEntity, functionalEntity.WiltEntity));
        }

        private static double GetDifferencesSum(WiltEntity first, WiltEntity second)
        {
            double sum = 0;
            sum += GetDifferenceSquare(first.GlcmPan, second.GlcmPan);
            sum += GetDifferenceSquare(first.MeanGreen, second.MeanGreen);
            sum += GetDifferenceSquare(first.MeanNir, second.MeanNir); 
            sum += GetDifferenceSquare(first.MeanRed, second.MeanRed);
            sum += GetDifferenceSquare(first.SdPan, second.SdPan);

            return sum;
        }

        private static double GetDifferenceSquare(double a, double b)
        {
            return Math.Pow(a - b, 2);
        }
    }
}
