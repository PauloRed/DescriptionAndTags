using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DescriptionAndTags
{
    /// <summary>
    /// This class can help you to store settings for your plugin
    /// </summary>
    /// <remarks>
    /// This class must be XML serializable
    /// </remarks>
    public class Settings
    {
        public string LastUsedOrganizationWebappUrl { get; set; }
        public string CSVDelimiter { get; set; }
        public string TextDelimiter { get; set; }
        public string TagOpeningString { get; set; }
        public string TagClosingString { get; set; }
        public string LastFilePath { get; set; }
        public int LanguageCode { get; set; }
        public bool UseTags { get; set; }

        public bool ReadAsIfPublished { get; set; }
        public bool DetailedLogs { get; set; }
    }
}