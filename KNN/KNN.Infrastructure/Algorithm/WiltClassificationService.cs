using System;
using System.Collections.Generic;
using System.Linq;
using KNN.Infrastructure.Helpers;
using KNN.Infrastructure.Parsing;
using KNN.Models.Input;

namespace KNN.Infrastructure.Algorithm
{
    public class WiltClassificationService
    {
        private readonly List<WiltFunctionalEntity> functionalEntities;
        private readonly NormalizeHelper normalizeHelper;
        private readonly bool normalizationRequired;

        public WiltClassificationService(IEnumerable<WiltEntity> wiltEntities, bool normalizationRequired)
        {
            if (normalizationRequired)
            {
                normalizeHelper = new NormalizeHelper(wiltEntities);
                wiltEntities = normalizeHelper.NormalizeEntites(wiltEntities);
            }

            this.normalizationRequired = normalizationRequired;
            this.functionalEntities = wiltEntities.Select(e => new WiltFunctionalEntity(e)).ToList();
        }

        public WiltClasses GetWiltEntityClass(WiltEntity wiltEntity, int neighborCount)
        {
            if (normalizationRequired)
            {
                wiltEntity = normalizeHelper.NormalizeEntity(wiltEntity);
            }

            var functionalEntity = new WiltFunctionalEntity(wiltEntity);
            var neighbours = functionalEntities
                .OrderBy(e => functionalEntity.CalculateDistance(e))
                .Take(neighborCount)
                .ToList();

            return VoteEntities(neighbours);
        }

        public double GetTestPercentage(IEnumerable<WiltEntity> testEntities, int neighborCount)
        {
            if (normalizationRequired)
            {
                testEntities = normalizeHelper.NormalizeEntites(testEntities).ToList();
            }

            double testEntitiesCount = testEntities.Count();
            double rightClassCount = 0;

            foreach (var entity in testEntities)
            {
                var functionalEntity = new WiltFunctionalEntity(entity);
                if (functionalEntity.WiltClass == GetWiltEntityClass(entity, neighborCount))
                {
                    rightClassCount++;
                }
            }

            return (rightClassCount / testEntitiesCount) * 100;
        }

        private WiltClasses VoteEntities(List<WiltFunctionalEntity> entities)
        {
            var votes = new Dictionary<WiltClasses, int>();
            var names = Enum.GetNames(typeof(WiltClasses));
            
            foreach (var name in names)
            {
                var wiltClass = (WiltClasses)Enum.Parse(typeof(WiltClasses), name);
                var votesCount = entities.Where(e => e.WiltClass == wiltClass).Count();
                AddClassVotes(ref votes, wiltClass, votesCount);
            }

            var maxCount = votes.Max(pair => pair.Value);

            return votes.First(pair => pair.Value == maxCount).Key;
        }

        private static void AddClassVotes(ref Dictionary<WiltClasses, int> votesDictionary, WiltClasses wiltClass, int votesCount)
        {
            votesDictionary.Add(wiltClass, votesCount);
        }
    }
}
