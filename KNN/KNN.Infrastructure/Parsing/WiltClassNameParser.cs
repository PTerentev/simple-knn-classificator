using KNN.Models.Input;
using System;

namespace KNN.Infrastructure.Parsing
{
    public static class ClassNameParser
    {
        public static WiltClasses Parse(WiltEntity entity)
        {
            if (string.Equals(entity.Class, "w", StringComparison.InvariantCultureIgnoreCase))
            {
                return WiltClasses.DiseasedTrees;
            }
            else
            {
                return WiltClasses.AllOtherLandCover;
            }
        }
    }
}
