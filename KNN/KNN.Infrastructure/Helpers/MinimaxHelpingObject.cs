using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KNN.Models.Input;

namespace KNN.Infrastructure.Helpers
{
    internal class MinimaxHelpingObject
    {
        private readonly PropertyInfo property;
        private double maxValue;
        private double minValue;

        public MinimaxHelpingObject(IEnumerable<WiltEntity> wiltEntities, string propertyName)
        {
            this.property = typeof(WiltEntity).GetProperty(propertyName);
            CalculateMinMax(wiltEntities);
        }

        private void CalculateMinMax(IEnumerable<WiltEntity> wiltEntities)
        {
            this.maxValue = wiltEntities.Max(e => GetValue(e));
            this.minValue = wiltEntities.Min(e => GetValue(e));
        }

        public void NormalizeValue(ref WiltEntity wiltEntity)
        {
            var normalizedValue = (GetValue(wiltEntity) - minValue) / (maxValue - minValue);
            property.SetValue(wiltEntity, normalizedValue);
        }

        private double GetValue(WiltEntity wiltEntity)
        {
            return Convert.ToDouble(property.GetValue(wiltEntity));
        }
    }
}
