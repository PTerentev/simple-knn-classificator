using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var reader = new StreamReader(fileStream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                return csv.GetRecords<WiltEntity>().ToList();
            }
        }
    }
}
