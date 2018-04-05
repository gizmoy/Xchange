using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xchange.Handlers.ErrorHandler;
using Xchange.Interfaces;

namespace Xchange.Handlers.CurrencyHandler
{
    public class CurrencyHandler : ICurrencyHandler
    {
        public IDictionary<string, decimal> CurrencyValueDict { get; private set; }
        public IList<string> Currencies { get; private set; } = new List<string>();
        public DateTime? LastRetrieval { get; private set; }
        public string ReferenceCurrency { get; private set; } = "pln";


        public CurrencyHandler()
        {
            CurrencyValueDict = GetCurrencyValueDictFromWeb();

            foreach(var currency in CurrencyValueDict.Keys)
            {
                Currencies.Add(currency);
            }

            var referenceValue = CurrencyValueDict[ReferenceCurrency];

            foreach (var currency in CurrencyValueDict.Keys.ToList())
            {
                if (currency != "decimal")
                {
                    if (currency == ReferenceCurrency)
                    {
                        CurrencyValueDict[currency] = 1.0m;
                    }
                    else
                    {
                        CurrencyValueDict[currency] = 1.0m / CurrencyValueDict[currency] * referenceValue;
                    }
                }
            }
        }


        private IDictionary<string, decimal> GetCurrencyValueDictFromWeb()
        {
            var dict = new Dictionary<string, decimal>();
            var date = string.Empty;


            try
            {
                using (XmlReader xmlr = XmlReader.Create(@"http://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml"))
                {
                    xmlr.ReadToFollowing("Cube");
                    while (xmlr.Read())
                    {
                        if (xmlr.NodeType != XmlNodeType.Element) continue;
                        if (xmlr.GetAttribute("time") != null)
                        {
                            date = xmlr.GetAttribute("time");
                        }
                        else
                        {
                            dict[xmlr.GetAttribute("currency").ToLower()] = decimal.Parse(xmlr.GetAttribute("rate"), CultureInfo.InvariantCulture);
                        }

                        LastRetrieval = DateTime.Parse(date);
                    }
                }
            }
            catch (Exception)
            {
                ErrorHandlerStatic.Error("Error in retrieving currencies.");
            }

            dict["eur"] = 1.0m;
            dict["decimal"] = 1.0m;

            return dict;
        }

        public bool IsCurrencyValid(string currency)
        {
            return CurrencyValueDict.Keys.Contains(currency.ToLower());
        }
    }
}
