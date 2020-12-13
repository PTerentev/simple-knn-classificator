using System.Collections.Generic;
using System.Linq;
using KNN.Models.Input;

namespace KNN.Infrastructure.Helpers
{
    /// <summary>
    /// Normalize helper.
    /// </summary>
    public class NormalizeHelper
    {
        private List<MinimaxHelpingObject> minimaxHelpingObjects;

        public NormalizeHelper(IEnumerable<WiltEntity> initialEntities)
        {
            minimaxHelpingObjects = GetValidPropNames()
                .Select(name => new MinimaxHelpingObject(initialEntities, name))
                .ToList();
        }

        private static IEnumerable<string> GetValidPropNames()
        {
            return typeof(WiltEntity).GetProperties()
                .Where(i => i.PropertyType.IsEquivalentTo(typeof(double)))
                .Select(i => i.Name);
        }

        public IEnumerable<WiltEntity> NormalizeEntites(IEnumerable<WiltEntity> wiltEntities)
        {
            return wiltEntities.Select(e => NormalizeEntity(e));
        }

        public WiltEntity NormalizeEntity(WiltEntity wiltEntity)
        {
            var normalized = wiltEntity.Clone();
            minimaxHelpingObjects.ForEach(m => m.NormalizeValue(ref wiltEntity));
            return wiltEntity;
        }
    }
}
