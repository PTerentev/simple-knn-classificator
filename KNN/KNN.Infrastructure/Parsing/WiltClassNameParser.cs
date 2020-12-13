using KNN.Models.Input;
using System;

namespace KNN.Infrastructure.Parsing
{
    /// <summary>
    /// Wilt class name parser.
    /// </summary>
    public static class WiltClassNameParser
    {
        /// <summary>
        /// Parse.
        /// </summary>
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
