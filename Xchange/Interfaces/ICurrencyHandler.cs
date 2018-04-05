using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xchange.Interfaces
{
    public interface ICurrencyHandler
    {
        IDictionary<string, decimal> CurrencyValueDict { get; }
        IList<string> Currencies { get; }
        DateTime? LastRetrieval { get; }
        bool IsCurrencyValid(string currency);
    }
}
