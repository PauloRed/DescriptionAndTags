using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.Xrm.Sdk.Metadata;

namespace DescriptionAndTags
{
    public class OneRecord //one record of CSV
    {
        public string RowType { get; set; }
        public int LanguageCode { get; set; }
        public string CanBeUpdated { get; set; }
        public string EntityLogicalName { get; set; }
        public string EntitySchemaName { get; set; }
        public string EntityDescription { get; set; }
        public string AttributeLogicalName { get; set; }
        public string AttributeSchemaName { get; set; }
        public string AttributeType { get; set; }
        public string AttributeDescription { get; set; }
        public string Tag0 { get; set; }
        public string Tag1 { get; set; }
        public string Tag2 { get; set; }
        public string Tag3 { get; set; }
        public string Tag4 { get; set; }
        public string Tag5 { get; set; }
        public string Tag6 { get; set; }
        public string Tag7 { get; set; }
        public string Tag8 { get; set; }
        public string Tag9 { get; set; }
    }



    public class clLanguage: CultureInfo //allows culture information to be shown in combo box
    {
        public clLanguage(int iCultureidentifier) : base(iCultureidentifier)
        {
        }

        public override string ToString() //overrides display
        {
            return this.EnglishName + " (" + this.LCID.ToString() + ")";
        }
    }


    class clOneAttribute //data for one attribute
    {
        public string logicalName { get; set; }
        public string schemaName { get; set; }
        public string attributeTypeDescription { get; set; }
        public string description { get; set; }
        public int LanguageCode { get; set; }
        public string[] Tags { get; set; }
        public bool IsRollupField { get; set; } //true if this is a rollup field - needed to ensure that linked fields (_date and_state) are not updated
        public bool NotUpdateAble { get; set; } //true if not updateable - for example if a _date or _state field from a rollup.
        public int? DefaultOptionSetValue { get; set; } //Default option set value - needed as updating the  description seems to set the defauilt option set value to unknown = looks like SDK bug

        /// <summary>
        /// needed
        /// </summary>
        /// <param name="logicalName"></param>
        /// <param name="schemaName"></param>
        /// <param name="attributeTypeDescription"></param>
        /// <param name="description"></param>
        /// <param name="LanguageCode"></param>
        /// <param name="Tags"></param>

        public clOneAttribute(string logicalName, string schemaName, string attributeTypeDescription, string description, int LanguageCode, string[] Tags, bool IsRollupField)
        {
            this.logicalName = logicalName;
            this.schemaName = schemaName;
            this.attributeTypeDescription = attributeTypeDescription;
            this.description = description;
            this.LanguageCode = LanguageCode;
            this.Tags = Tags;
            this.IsRollupField = IsRollupField;
            this.NotUpdateAble = false; //at this stage always assume it is updateable
        }

        public string GetTaggedDescription(bool bUseTags, string TagOpeningString, string TagClosingString)
        {
            if (bUseTags)
            {
                string s = " ";
                for (int i = 0; i < 10; i++)
                {
                    if (Tags[i] != "")
                    {
                        s += TagOpeningString + Tags[i] + TagClosingString;
                    }
                }
                return description + s;
            }
            else
            {
                return description;
            }
        }
    }
    class clOneEntity //stores data for one entity
    {
        public string logicalName { get; set; }
        public string schemaName { get; set;}
        public string description { get; set; }
        public int LanguageCode { get; set; }
        public string[] Tags { get; set; }
        public bool NotUpdateAble { get; set; } //true if description not updateable - for example activitymimeattachment

        public SortedDictionary<string,clOneAttribute> attributes { get; set; } //Holds all attributes

        public clOneEntity()
        {
            attributes = new SortedDictionary<string, clOneAttribute>();
        }

        public clOneEntity(string logicalName, string schemaName, string description, int LanguageCode,SortedDictionary<string, clOneAttribute> attributes, string[] Tags)
        {
            this.logicalName = logicalName;
            this.schemaName = schemaName;
            this.description = description;
            this.LanguageCode = LanguageCode;
            this.attributes = attributes;
            this.Tags = Tags;
            this.NotUpdateAble = false;
        }

        public string GetTaggedDescription(bool bUseTags, string TagOpeningString, string TagClosingString)
        {
            if (bUseTags)
            {
                string s=""+(char)0; //*** experiment add null
                for (int i=0; i < 10; i++)
                {
                    if (Tags[i] !="")
                    {
                        s += TagOpeningString + Tags[i] + TagClosingString;
                    }
                }
                return description + s;
            }
            else
            {
                return description;
            }
        }
    }

    class clEntities //holds set of all entities
    {
        public SortedDictionary<string, clOneEntity> entities { get; set; }

        public clEntities()
        {
            entities = new SortedDictionary<string, clOneEntity>();
        }

        public void AddEntity(string logicalName, string schemaName, string description, int LanguageCode, SortedDictionary<string, clOneAttribute> attributes, string[] Tags)
        {
            entities.Add(logicalName, new clOneEntity(logicalName, schemaName, description, LanguageCode,attributes,Tags));
        }



    }
}
