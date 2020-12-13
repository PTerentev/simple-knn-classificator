using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using KNN.Models.Input;

namespace KNN.Infrastructure.Parsing
{
    /// <summary>
    /// Wilt data set parser.
    /// </summary>
    public static class WiltDataSetParser
    {
        /// <summary>
        /// Parse Wilt entities.
        /// </summary>
        public static IEnumerable<WiltEntity> ParseEntities(string path)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<WiltEntity>();
            }
        }
    }
}
