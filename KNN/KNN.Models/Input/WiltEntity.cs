using CsvHelper.Configuration.Attributes;
using System;

namespace KNN.Models.Input
{
    /// <summary>
    /// Wilt data set entity.
    /// </summary>
    public class WiltEntity : ICloneable
    {
        [Name("class")]
        public string Class { get; set; }

        [Name("GLCM_pan")]
        public double GlcmPan { get; set; }

        [Name("Mean_Green")]
        public double MeanGreen { get; set; }

        [Name("Mean_Red")]
        public double MeanRed { get; set; }

        [Name("Mean_NIR")]
        public double MeanNir { get; set; }

        [Name("SD_pan")]
        public double SdPan { get; set; }

        public WiltEntity Clone()
        {
            return (WiltEntity)this.MemberwiseClone();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
