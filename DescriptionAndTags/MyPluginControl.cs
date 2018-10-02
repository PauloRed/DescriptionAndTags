using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XrmToolBox.Extensibility;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using McTools.Xrm.Connection;
using System.IO;
using CsvHelper;
using System.Globalization;
using Microsoft.Crm.Sdk.Messages;

namespace DescriptionAndTags
{
    public partial class MyPluginControl : PluginControlBase
    {
        private Settings mySettings;

        private clEntities CRMEntitySet; //the entity information read from CRM
        private clEntities FileEntitySet; //the entity set read from a file
        const int MaxNumberTags = 10;
        const string nullString = "\0";

        //Variables with numbers actually read/updated
        int iEntityCount; 
        int iEntityUpdatedCount;
        int iAttributeCount;
        int iAttributeUpdatedCount;

        //Variables to hold counts of ones to be updated - used in count mode
        int iEntitiesOrAttributesToUpdate;

        public MyPluginControl()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            ShowInfoNotification("Provided by Clew Consulting, developer of Scanner Plus for CRM", new Uri("https://clew-consulting.com"));
            LogInfo("DescriptionAndTags Plugin started");

            //Get list of language codes and populate lookup
            //Get list of languages packed installed in CRM
            RetrieveProvisionedLanguagesRequest reqLangs = new RetrieveProvisionedLanguagesRequest();
            RetrieveProvisionedLanguagesResponse respLangs = (RetrieveProvisionedLanguagesResponse)Service.Execute(reqLangs);
            //int[] InstalledLanguages = null;
            var resp = respLangs.Results.FirstOrDefault().Value;
            int[] InstalledLanguages = (int[])resp;

            //Get all cultures but only add if one of the installed language packs
            CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);
            foreach (CultureInfo culture in cultures)
            {
                if (InstalledLanguages.Contains(culture.LCID)) //Only show if one of the language packs enabled
                {
                    clLanguage Lang = new clLanguage(culture.LCID);
                    cbLanguage.Items.Add(Lang);
                }
            }

            // Loads or creates the settings for the plugin

            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();
                cbFieldDelimiter.SelectedIndex = 0;
                cbStringDelimiter.SelectedIndex = 0;
                cbTagDelimiter.SelectedIndex = 0;
                cxUseTags.Checked = false;      
                LogWarning("Settings not found => a new settings file has been created!");
            }
            else
            {
                //Not very elegant code but good enough ;-)
                if (mySettings.CSVDelimiter is null)
                {
                    cbFieldDelimiter.SelectedIndex = 0;
                }
                else
                {
                    int i = cbFieldDelimiter.Items.IndexOf(mySettings.CSVDelimiter);
                    if (i>=0)
                    {
                        cbFieldDelimiter.SelectedIndex = i;
                    }
                    else
                    {
                        cbFieldDelimiter.SelectedIndex = 0;
                    }
                }

                if (mySettings.TextDelimiter is null)
                {
                    cbStringDelimiter.SelectedIndex = 0;
                }
                else
                {
                    int i = cbStringDelimiter.Items.IndexOf(mySettings.TextDelimiter);
                    if (i >= 0)
                    {
                        cbStringDelimiter.SelectedIndex = i;
                    }
                    else
                    {
                        cbStringDelimiter.SelectedIndex = 0;
                    }
                }

                if (mySettings.TagOpeningString is null)
                {

                }
                else
                {
                    string[] sOpening = { "[", "[[", "{", "}}" };
                    int i = Array.IndexOf(sOpening, mySettings.TagOpeningString);
                    if (i>-0)
                    {
                        cbTagDelimiter.SelectedIndex = i;
                    }
                    else
                    {
                        cbTagDelimiter.SelectedIndex = 0;
                    }
                }
                txtFileNameLocation.Text = mySettings.LastFilePath;

                //Populate current culture
                if (mySettings.LanguageCode==0)
                {
                    mySettings.LanguageCode= CultureInfo.CurrentCulture.LCID; //get current culture
                }
                //Set the current drop down to the previous culture or the default one
                int j=0;
                foreach (clLanguage lang in cbLanguage.Items)
                {
                    if (lang.LCID== mySettings.LanguageCode)
                    {
                        cbLanguage.SelectedIndex = j;
                        break;
                    }
                    j++;
                }
                cxUseTags.Checked = mySettings.UseTags;
                cbTagDelimiter.Enabled = mySettings.UseTags;
                cxDetailedLog.Checked = mySettings.DetailedLogs;
                cxReadasIfPublished.Checked = mySettings.ReadAsIfPublished;

                LogInfo2("Settings found and loaded");
            }
            SettingsManager.Instance.Save(GetType(), mySettings);
        }
        /// <summary>
        /// Logs detailed information if detailed logging enabled
        /// </summary>
        /// <param name="sMessage"></param>
        private void LogInfo2(string sMessage)
        {
            if (mySettings.DetailedLogs)
            {
                LogInfo(sMessage);
            }
        }

        private void tsbClose_Click(object sender, EventArgs e)
        {
            CloseTool();
        }

        /// <summary>
        /// This event occurs when the plugin is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyPluginControl_OnCloseTool(object sender, EventArgs e)
        {
            // Before leaving, save the settings
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            if (mySettings is null) mySettings = new Settings(); //Fix PSR as this can be called before load

            mySettings.LastUsedOrganizationWebappUrl = detail.WebApplicationUrl;
            LogInfo("Connection has changed to: {0}", detail.WebApplicationUrl);
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (rbExport.Checked)
            {
                string sPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (txtFileNameLocation.Text != "")
                {
                    sPath=Path.GetDirectoryName(txtFileNameLocation.Text);
                }
                var sfDialog = new SaveFileDialog
                {
                    Filter = "CSV file|*.csv",
                    Title = "Select a location for the file to be exported",
                    RestoreDirectory = true,
                    InitialDirectory = sPath

                };
                if (sfDialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtFileNameLocation.Text = sfDialog.FileName;
                }
            }
            else
            {
                string sPath = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (txtFileNameLocation.Text != "")
                {
                    sPath = Path.GetDirectoryName(txtFileNameLocation.Text);
                }
                var sfDialog = new OpenFileDialog
                {
                    Filter = "CSV file|*.csv",
                    Title = "Select a location for the file to be imported",
                    RestoreDirectory = true,
                    InitialDirectory = sPath

                };
                if (sfDialog.ShowDialog(this) == DialogResult.OK)
                {
                    txtFileNameLocation.Text = sfDialog.FileName;
                }
            }
        }

        private string RemoveNulls(string s)
        {
            return s.Replace(nullString, "");
        }

        private void ExtractTags(string sDescription, out string[] Tags, out string UpdatedDescription)
        {
            sDescription= RemoveNulls(sDescription); //***experiement get rid of nulls
            int iTagNo = 0;
            int iTagLength = mySettings.TagOpeningString.Length;
            string[] sTags = { "", "", "", "", "", "", "", "", "", "" };
            while ((sDescription.IndexOf(mySettings.TagOpeningString) >= 0) && (sDescription.IndexOf(mySettings.TagOpeningString) < sDescription.IndexOf(mySettings.TagClosingString) && (iTagNo < MaxNumberTags)))
            {
                int iTagStart = sDescription.IndexOf(mySettings.TagOpeningString);
                int iTagEnd = sDescription.IndexOf(mySettings.TagClosingString);
                sTags[iTagNo] = sDescription.Substring(iTagStart + iTagLength, iTagEnd - iTagStart - iTagLength);
                iTagNo++;
                sDescription = sDescription.Remove(iTagStart, iTagEnd - iTagStart + iTagLength).Trim();
            }
            Tags = sTags;
            UpdatedDescription = sDescription;
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (rbExport.Checked)
            {
                RunExport();
            }
            else
            {
                RunImport();
            }
               
        }

        /// <summary>
        /// Reads entities into memory
        /// </summary>
        public void ReadEntitiesFromCRM()
        {
            LogInfo("Reading CRM metadata from CRM");
            CRMEntitySet = new clEntities(); //initialise
            string[] Tags = { "", "", "", "", "", "", "", "", "", "" };

            RetrieveAllEntitiesRequest request = new RetrieveAllEntitiesRequest()
            {
                EntityFilters = EntityFilters.All
            };
            request.RetrieveAsIfPublished = cxReadasIfPublished.Checked;

            RetrieveAllEntitiesResponse response = (RetrieveAllEntitiesResponse)Service.Execute(request);
            LogInfo2("Entity data read from CRM");

            foreach (EntityMetadata currentEntity in response.EntityMetadata)
            {
                LogInfo2("Processsing entity "+ currentEntity.LogicalName);

                if (currentEntity.IsCustomizable.Value) //only output items for customisable entities
                {
                    string sDescription;
                    //Get attributes
                    SortedDictionary<string, clOneAttribute> attributes = new SortedDictionary<string, clOneAttribute>();
                    foreach (AttributeMetadata att in currentEntity.Attributes)
                    {
                        LogInfo2("Processsing attribute " + att.LogicalName);
                        sDescription = att.Description.LocalizedLabels.Where(l => l.LanguageCode == mySettings.LanguageCode).FirstOrDefault() != null ? att.Description.LocalizedLabels.Where(l => l.LanguageCode == mySettings.LanguageCode).FirstOrDefault().Label : "";
                        if (mySettings.UseTags)
                        {
                            ExtractTags(sDescription, out Tags, out sDescription);
                        }
                        attributes.Add(att.LogicalName, new clOneAttribute(att.LogicalName, att.SchemaName, att.AttributeType.Value.ToString(), sDescription, mySettings.LanguageCode, Tags,(att.SourceType != null && att.SourceType>0)));
                        if (att.AttributeType.Value.ToString()=="Picklist")
                        {
                            attributes[att.LogicalName].DefaultOptionSetValue = ((PicklistAttributeMetadata)att).DefaultFormValue;
                        }
                    }

                    sDescription = currentEntity.Description.LocalizedLabels.Where(l => l.LanguageCode == mySettings.LanguageCode).FirstOrDefault() != null ? currentEntity.Description.LocalizedLabels.Where(l => l.LanguageCode == mySettings.LanguageCode).FirstOrDefault().Label : "";
                    if (mySettings.UseTags)
                    {
                        ExtractTags(sDescription, out Tags, out sDescription);
                    }

                    //Flag linked rollup fields _state and _date as these are hidden and cannot be updated
                    foreach(string sKey in attributes.Keys)
                    {
                        if (attributes[sKey].IsRollupField) //is this a rollup
                        {
                            if (attributes.Keys.Contains(attributes[sKey].logicalName+"_date")) //if so and if the _date fields exists then flag it
                            {
                                attributes[attributes[sKey].logicalName + "_date"].NotUpdateAble = true;
                            }
                            if (attributes.Keys.Contains(attributes[sKey].logicalName + "_state")) //if so and if the _state fields exists then flag it
                            {
                                attributes[attributes[sKey].logicalName + "_state"].NotUpdateAble = true;
                            }
                        }
                    }

                    //Add entity to set
                    CRMEntitySet.AddEntity(currentEntity.LogicalName, currentEntity.SchemaName, sDescription, mySettings.LanguageCode, attributes,Tags);
                    if (!currentEntity.IsRenameable.Value)
                    {
                        CRMEntitySet.entities[currentEntity.LogicalName].NotUpdateAble = true; //flag if cannot be updated
                    }
                }
            }
        }

        public void WriteEntityData(string sFileName)
        {
            LogInfo("Writing CRM metadata to file: " + sFileName);
            iEntityCount = 0;
            iAttributeCount = 0;
            OneRecord rec = new OneRecord();
            var csv = new CsvWriter((new System.IO.StreamWriter(sFileName,false, Encoding.UTF8))); //Create csv writer
            csv.Configuration.Delimiter = mySettings.CSVDelimiter;
            csv.Configuration.Quote = mySettings.TextDelimiter[0];

            csv.WriteHeader<OneRecord>();
            csv.Flush();
            csv.NextRecord();
            foreach (string sKey in CRMEntitySet.entities.Keys)
            {
                LogInfo2("Writing entity row" + sKey);
                clOneEntity ent = CRMEntitySet.entities[sKey];
                //write entity row
                rec.RowType = "Entity";
                rec.LanguageCode = mySettings.LanguageCode;
                rec.EntityLogicalName = ent.logicalName;
                rec.EntitySchemaName = ent.schemaName;
                rec.EntityDescription = ent.description;
                rec.CanBeUpdated = ent.NotUpdateAble ? "No" : "Yes";
                rec.Tag0 = ent.Tags[0];
                rec.Tag1 = ent.Tags[1];
                rec.Tag2 = ent.Tags[2];
                rec.Tag3 = ent.Tags[3];
                rec.Tag4 = ent.Tags[4];
                rec.Tag5 = ent.Tags[5];
                rec.Tag6 = ent.Tags[6];
                rec.Tag7 = ent.Tags[7];
                rec.Tag8 = ent.Tags[8];
                rec.Tag9 = ent.Tags[9];

                csv.WriteRecord<OneRecord>(rec);
                csv.Flush();
                csv.NextRecord();
                iEntityCount++;
                rec.EntityDescription = ""; //To avoid people updating the wrong one, blank out entity descriptions for attributes
                foreach (string sKey2 in ent.attributes.Keys)
                {
                    LogInfo2("Writing attribute row" + sKey2);
                    rec.RowType = "Attribute";
                    clOneAttribute att = ent.attributes[sKey2];
                    rec.AttributeLogicalName = att.logicalName;
                    rec.AttributeSchemaName = att.schemaName;
                    rec.AttributeType = att.attributeTypeDescription;
                    rec.AttributeDescription = att.description;
                    rec.CanBeUpdated = att.NotUpdateAble ? "No" : "Yes";
                    rec.Tag0 = att.Tags[0];
                    rec.Tag1 = att.Tags[1];
                    rec.Tag2 = att.Tags[2];
                    rec.Tag3 = att.Tags[3];
                    rec.Tag4 = att.Tags[4];
                    rec.Tag5 = att.Tags[5];
                    rec.Tag6 = att.Tags[6];
                    rec.Tag7 = att.Tags[7];
                    rec.Tag8 = att.Tags[8];
                    rec.Tag9 = att.Tags[9];
                    csv.WriteRecord<OneRecord>(rec);
                    csv.Flush();
                    csv.NextRecord();
                    iAttributeCount++;
                }
            }
            csv.Flush();
            csv.Dispose();
        }
        private string[] GetTagsFromCSVRecord(OneRecord rec)
        {
            string[] s = new string[10];
            s[0] = rec.Tag0;
            s[1] = rec.Tag1;
            s[2] = rec.Tag2;
            s[3] = rec.Tag3;
            s[4] = rec.Tag4;
            s[5] = rec.Tag5;
            s[6] = rec.Tag6;
            s[7] = rec.Tag7;
            s[8] = rec.Tag8;
            s[9] = rec.Tag9;
            return s;
        }

        public void ReadEntityDataFromFile(string sFileName)
        {
            LogInfo("Reading CRM metadata from file: "+sFileName);
            FileEntitySet = new clEntities(); //initialise target
            OneRecord rec;
            var csv = new CsvReader(new System.IO.StreamReader(sFileName,Encoding.UTF8));
            csv.Configuration.Delimiter = mySettings.CSVDelimiter;
            csv.Configuration.Quote = mySettings.TextDelimiter[0];
            csv.Read();
            csv.ReadHeader();
            while (csv.Read())
            {
                rec = csv.GetRecord<OneRecord>();
                if (rec.RowType == "Entity" || rec.RowType == "Attribute") //check entity exists and if so update, otherwise create
                {
                    LogInfo2("Read row of type " + rec.RowType + " with entity " + rec.EntityLogicalName + ((rec.RowType == "Attribute") ? ", attribute " + rec.AttributeLogicalName : ""));
                    string[] Tags = GetTagsFromCSVRecord(rec);
                    if (!FileEntitySet.entities.ContainsKey(rec.EntityLogicalName))
                    {
                        FileEntitySet.AddEntity(rec.EntityLogicalName, rec.EntitySchemaName, rec.EntityDescription, rec.LanguageCode, new SortedDictionary<string, clOneAttribute>(), Tags);

                    }
                    else if (rec.RowType == "Entity")
                    {
                        FileEntitySet.entities[rec.EntityLogicalName].schemaName = rec.EntitySchemaName;
                        FileEntitySet.entities[rec.EntityLogicalName].description = rec.EntityDescription;
                    }
                }
                if (rec.RowType == "Attribute")
                {
                    string[] Tags = GetTagsFromCSVRecord(rec);
                    if (!FileEntitySet.entities[rec.EntityLogicalName].attributes.ContainsKey(rec.AttributeLogicalName))
                    {
                        FileEntitySet.entities[rec.EntityLogicalName].attributes.Add(
                            rec.AttributeLogicalName, new clOneAttribute(rec.AttributeLogicalName, rec.AttributeSchemaName, rec.AttributeType,rec.AttributeDescription,rec.LanguageCode,Tags,false));
                    }
                    else
                    {
                        FileEntitySet.entities[rec.EntityLogicalName].attributes[rec.AttributeLogicalName].attributeTypeDescription = rec.AttributeType;
                        FileEntitySet.entities[rec.EntityLogicalName].attributes[rec.AttributeLogicalName].schemaName = rec.AttributeSchemaName;
                        FileEntitySet.entities[rec.EntityLogicalName].attributes[rec.AttributeLogicalName].description = rec.AttributeDescription;
                    }
                }

                if (rec.RowType != "Entity" && rec.RowType != "Attribute")
                {
                    throw new Exception("Row must have first cell of Entity or Attribute. Suggests file corruption");
                }
            }
        }

        public void UpdateEntityData(BackgroundWorker w, bool bCountMode)
        {
            iEntityCount = 0;
            iEntityUpdatedCount = 0;
            iAttributeCount = 0;
            iAttributeUpdatedCount=0;

            if (bCountMode) //if this is count mode then initialise counter
            {
                iEntitiesOrAttributesToUpdate = 0;
            }

            LogInfo("Apply updates to CRM (if any)");

            foreach (string sKey in FileEntitySet.entities.Keys) //loop over entities read from file
            {
                iEntityCount++;
                if (CRMEntitySet.entities.ContainsKey(sKey)) //only do this if there is a corresponding entity in CRM - should almost always be so
                {
                    if (RemoveNulls(FileEntitySet.entities[sKey].GetTaggedDescription(mySettings.UseTags,mySettings.TagOpeningString,mySettings.TagClosingString)) != RemoveNulls(CRMEntitySet.entities[sKey].GetTaggedDescription(mySettings.UseTags, mySettings.TagOpeningString, mySettings.TagClosingString)))
                    {
                        //Entity description has changed so update it unless not updateable
                        if (!CRMEntitySet.entities[sKey].NotUpdateAble)
                        {
                            if (bCountMode)
                            {
                                iEntitiesOrAttributesToUpdate++;
                            }
                            else
                            {
                                string sDescription = FileEntitySet.entities[sKey].GetTaggedDescription(mySettings.UseTags, mySettings.TagOpeningString, mySettings.TagClosingString);
                                LogInfo2(string.Format("About to update entity {0} with description {1}", sKey, sDescription));
                                EntityMetadata entityMeta = new EntityMetadata()
                                {
                                    LogicalName = sKey,
                                    Description = new Microsoft.Xrm.Sdk.Label(sDescription, FileEntitySet.entities[sKey].LanguageCode)
                                };

                                UpdateEntityRequest UpdateRequest = new UpdateEntityRequest()
                                {
                                    Entity = entityMeta
                                };

                                UpdateEntityResponse updateResp = (UpdateEntityResponse)Service.Execute(UpdateRequest);
                                iEntityUpdatedCount++;
                                w.ReportProgress(-1, "Entity/attribute updated. Completed: " + ((iAttributeUpdatedCount + iEntityUpdatedCount) * 100 / iEntitiesOrAttributesToUpdate).ToString() + "%");
                            }
                        }
                        else
                        {
                            LogWarning("Entity description for " + sKey + " could not be updated as this is not permitted by CRM");
                        }

                    }
                    foreach (string sKey2 in FileEntitySet.entities[sKey].attributes.Keys)
                    {
                        iAttributeCount++;
                        if (CRMEntitySet.entities[sKey].attributes.ContainsKey(sKey2)) //only do this if attribute present
                        {
                            if (!CRMEntitySet.entities[sKey].attributes[sKey2].NotUpdateAble) //skip if not updateable
                            {
                                if (RemoveNulls(FileEntitySet.entities[sKey].attributes[sKey2].GetTaggedDescription(mySettings.UseTags, mySettings.TagOpeningString, mySettings.TagClosingString)) != RemoveNulls(CRMEntitySet.entities[sKey].attributes[sKey2].GetTaggedDescription(mySettings.UseTags, mySettings.TagOpeningString, mySettings.TagClosingString)))
                                {
                                    if (bCountMode)
                                    {
                                        iEntitiesOrAttributesToUpdate++;
                                    }
                                    else
                                    {
                                        string sDescription = FileEntitySet.entities[sKey].attributes[sKey2].GetTaggedDescription(mySettings.UseTags, mySettings.TagOpeningString, mySettings.TagClosingString);
                                        LogInfo2(string.Format("About to update attribute {0} in entity {1} with description {2}", sKey2, sKey, sDescription));

                                        //Construct metadata. Note special for picklist
                                        if ((CRMEntitySet.entities[sKey].attributes[sKey2].attributeTypeDescription == "Picklist")
                                            && CRMEntitySet.entities[sKey].attributes[sKey2].DefaultOptionSetValue != null) //this shows that it is a picklist with non-default default
                                        {
                                            PicklistAttributeMetadata atttributeMeta = new PicklistAttributeMetadata()
                                            {
                                                LogicalName = sKey2,
                                                Description = new Microsoft.Xrm.Sdk.Label(sDescription, FileEntitySet.entities[sKey].attributes[sKey2].LanguageCode),
                                                DefaultFormValue = CRMEntitySet.entities[sKey].attributes[sKey2].DefaultOptionSetValue
                                            };
                                            UpdateAttributeRequest UpdateRequest = new UpdateAttributeRequest()
                                            {
                                                Attribute = atttributeMeta,
                                                EntityName = sKey
                                            };
                                            iAttributeUpdatedCount++;
                                            UpdateAttributeResponse updateResp = (UpdateAttributeResponse)Service.Execute(UpdateRequest);
                                        }
                                        else
                                        {
                                            AttributeMetadata atttributeMeta = new AttributeMetadata()
                                            {
                                                LogicalName = sKey2,
                                                Description = new Microsoft.Xrm.Sdk.Label(sDescription, FileEntitySet.entities[sKey].attributes[sKey2].LanguageCode)
                                            };
                                            UpdateAttributeRequest UpdateRequest = new UpdateAttributeRequest()
                                            {
                                                Attribute = atttributeMeta,
                                                EntityName = sKey
                                            };
                                            iAttributeUpdatedCount++;
                                            UpdateAttributeResponse updateResp = (UpdateAttributeResponse)Service.Execute(UpdateRequest);
                                        }
                                        w.ReportProgress(-1, "Entity/attribute updated. Completed: "+ ((iAttributeUpdatedCount + iEntityUpdatedCount) * 100 / iEntitiesOrAttributesToUpdate).ToString()+"%");
                                    }
                                }
                            }
                            else
                            {
                                LogWarning("Skipping field " + sKey2 + " as not updateable");
                            }
                        }
                        else
                        {
                            LogWarning("Atttibute " + sKey2 + " in entity " + sKey + " from input file not found in CRM. This record will be ignored");
                        }
                    }
                }
                else
                {
                    LogWarning("Entity " + sKey + " from input file not found in CRM. This record will be ignored");
                }
            }
        }

        private void ShowException(Exception ex)
        {
            MessageBox.Show("An error has occurred:\n " + ex.Message+"\n Full details are in log file");
            LogError("An error has occurred:\n " + ex.Message+"\nInner Exception:\n"+ex.InnerException);
        }

        private void RunExport()
        {

            if (File.Exists(txtFileNameLocation.Text))
            {
                if (MessageBox.Show("File " + txtFileNameLocation.Text + " already exists. Are you sure you want to overwrite it?", "File Exists", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }
            }
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Reading CRM entity data then writing to file",
                Work = (w, e) =>
                {
                    try
                    {
                        ReadEntitiesFromCRM();
                        w.ReportProgress(-1, "CRM entity data retrieved");
                        WriteEntityData(txtFileNameLocation.Text);
                    }
                    catch (Exception ex)
                    {

                        ShowException(ex);
                    }

                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    MessageBox.Show(string.Format("Export complete.\n   Wrote {0} entity and {1} attribute records", iEntityCount, iAttributeCount), "Export Complete");
                },
                AsyncArgument = null,
                // Progress information panel size
                MessageWidth = 340,
                MessageHeight = 150
            });
        }

        private void RunImport()
        {

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Updating CRM entity data from file",
                Work = (w, e) =>
                {
                    try
                    {
                        ReadEntitiesFromCRM(); //read the entities so that we can compare before writing
                            w.ReportProgress(-1, "CRM entity data retrieved");
                        ReadEntityDataFromFile(txtFileNameLocation.Text);
                        w.ReportProgress(-1, "File entity data read");
                        UpdateEntityData(w,true); //Get counts so that can report on progress
                        UpdateEntityData(w,false);
                    }
                    catch (Exception ex)
                    {
                        ShowException(ex);
                    }

                },
                ProgressChanged = e =>
                {
                    SetWorkingMessage(e.UserState.ToString());
                },
                PostWorkCallBack = e =>
                {
                    MessageBox.Show(string.Format("CRM update complete.\n   Updated {0} entity descriptions out of {1} entity records read\n   Updated {2} attribute descriptions out of {3} attribute records read", iEntityUpdatedCount, iEntityCount,
                        iAttributeUpdatedCount, iAttributeCount), "Import Complete");
                },
                AsyncArgument = null,
                // Progress information panel size
                MessageWidth = 340,
                MessageHeight = 150
            });
        }

        private void rbExport_CheckedChanged(object sender, EventArgs e)
        {
            btnExecute.Text = rbExport.Checked ? "Run Export" : "Run Import";
        }

        private void cbFieldDelimiter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFieldDelimiter.SelectedIndex)
            {
                case 0:
                    {
                        mySettings.CSVDelimiter = ",";
                    }
                    break;
                case 1:
                    {
                        mySettings.CSVDelimiter = "|";
                    }
                    break;
                case 2:
                    {
                        mySettings.CSVDelimiter = ";";
                    }
                    break;
                default:
                    break;
            }
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        private void cbStringDelimiter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbStringDelimiter.SelectedIndex)
            {
                case 0:
                    {
                        mySettings.TextDelimiter = "\"";
                    }
                    break;
                case 1:
                    {
                        mySettings.TextDelimiter = "'";
                    }
                    break;
                default:
                    break;
            }
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        private void cbTagDelimiter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbTagDelimiter.SelectedIndex)
            {
                case 0:
                    {
                        mySettings.TagOpeningString = "[";
                        mySettings.TagClosingString = "]";
                    }
                    break;
                case 1:
                    {
                        mySettings.TagOpeningString = "[[";
                        mySettings.TagClosingString = "]]";
                    }
                    break;
                case 2:
                    {
                        mySettings.TagOpeningString = "{";
                        mySettings.TagClosingString = "}";
                    }
                    break;
                case 3:
                    {
                        mySettings.TagOpeningString = "{{";
                        mySettings.TagClosingString = "}}";
                    }
                    break;
                default:
                    break;
            }
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        private void txtFileNameLocation_TextChanged(object sender, EventArgs e)
        {
            mySettings.LastFilePath = txtFileNameLocation.Text;
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        private void cbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbLanguage.SelectedIndex>=0)
            {
                mySettings.LanguageCode = ((clLanguage)cbLanguage.Items[cbLanguage.SelectedIndex]).LCID;
                SettingsManager.Instance.Save(GetType(), mySettings);
            }
        }

        private void btnOpenLogFolder_Click(object sender, EventArgs e)
        {
            //stem.Diagnostics.Process.Start(Application.UserAppDataPath);
            OpenLogFile();
        }

        private void cxUseTags_CheckedChanged(object sender, EventArgs e)
        {
            cbTagDelimiter.Enabled = cxUseTags.Checked;
            mySettings.UseTags = cxUseTags.Checked;
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        private void cxReadasIfPublished_CheckedChanged(object sender, EventArgs e)
        {
            mySettings.ReadAsIfPublished = cxReadasIfPublished.Checked;
            SettingsManager.Instance.Save(GetType(), mySettings);
        }

        private void cxDetailedLog_CheckedChanged(object sender, EventArgs e)
        {
            mySettings.DetailedLogs = cxDetailedLog.Checked;
            SettingsManager.Instance.Save(GetType(), mySettings);
        }
    }
}
