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
        private readonly bool normalizationRequred;

        public WiltClassificationService(IEnumerable<WiltEntity> wiltEntities, bool normalizationRequred)
        {
            if (normalizationRequred)
            {
                normalizeHelper = new NormalizeHelper(wiltEntities);
                wiltEntities = normalizeHelper.NormalizeEntites(wiltEntities);
            }

            this.normalizationRequred = normalizationRequred;
            this.functionalEntities = wiltEntities.Select(e => new WiltFunctionalEntity(e)).ToList();
        }

        public WiltClasses GetWiltEntityClass(WiltEntity wiltEntity, int neighbourCount)
        {
            if (normalizationRequred)
            {
                wiltEntity = normalizeHelper.NormalizeEntity(wiltEntity);
            }

            var functionalEntity = new WiltFunctionalEntity(wiltEntity);
            var neighbours = functionalEntities
                .OrderBy(e => functionalEntity.CalculateDistance(e))
                .Take(neighbourCount)
                .ToList();

            return VoteEntities(neighbours);
        }

        public double GetTestPercentage(IEnumerable<WiltEntity> testEntities, int neighbourCount)
        {
            if (normalizationRequred)
            {
                testEntities = normalizeHelper.NormalizeEntites(testEntities).ToList();
            }

            var testEntitiesCount = testEntities.Count();
            var rightClassCount = 0;

            foreach (var entity in testEntities)
            {
                var functionalEntity = new WiltFunctionalEntity(entity);
                if (functionalEntity.WiltClass == GetWiltEntityClass(entity, neighbourCount))
                {
                    rightClassCount++;
                }
            }

            return rightClassCount / testEntitiesCount;
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
