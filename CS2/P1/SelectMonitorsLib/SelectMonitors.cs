using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelectedItemsContractLib;
using ChatBotModelLib;
using System.Data.Linq;
using System.Linq.Dynamic;
using DataAccessLayerContractLib;

namespace SelectMonitorsLib
{
    public class SelectMonitors : ISelectedItemsContract
    {
        private IDataAccessLayerContract dataref;
        public SelectMonitors(IDataAccessLayerContract dataref)
        {
            this.dataref = dataref;
        }
        public List<string> GetAllSelectedItems(int Feature_No, string FeatureValue)
        {
            var FeatureDictionary=dataref.ReadAttributes();
            string Feature;
            if (Feature_No==0)
            {
                Feature = "FirstFeature";
            }
            else {
                try
                {
                    Feature = FeatureDictionary[Feature_No];
                }
                catch (Exception ex)
                {
                    throw ex;
                }               
            }
            
            List<string> Selectedlist = new List<string>();
            if (!Feature.Equals(string.Empty) || !FeatureValue.Equals(string.Empty))
            {
                using (ChatBotDataModelDataContext dbcontext = new ChatBotDataModelDataContext())
                {
                    if (Feature.Equals("FirstFeature") && FeatureValue.Equals("FirstValue"))
                    {
                        var Selectedtems = dbcontext.ChatbotTable_s.Select("monitors_name");
                        foreach (var Item in Selectedtems)
                        {
                            Selectedlist.Add(Item.ToString());
                        }
                    }
                    else
                    {
                        var Selectedtems = dbcontext.ChatbotTable_s.Where(Feature + "=\"" + FeatureValue + "\"").Select("monitors_name");
                        foreach (var Item in Selectedtems)
                        {
                            Selectedlist.Add(Item.ToString());
                        }
                    }
                }
            }
            else
                throw new ArgumentException("String Empty");
            return Selectedlist;
        }
    }
}

