using CIADocServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIADocServer.Domain.DataStore
{
    public class InMemoryCensorRuleDataStore : ICensorshipRulesDataStore
    {
        List<CensorRules> censorList;
        object censorLock = new object();

        public InMemoryCensorRuleDataStore()
        {
            censorList = new List<CensorRules>();

            //temporary make an API to add censore rules.
            CensorRules cr = new CensorRules()
            {
                Id = 7,
                ClassifiedTerms = new List<string>() { "great", "string" },
                MinimumClearance = ClearanceLevel.Confidential,
                ReplacementTerm = "REDACTED",
                SpecialAuthorizations = new List<string>()
            };
            censorList.Add(cr);
        }

        ///<inheritdoc/>
        public void AddCensorRule(CensorRules newRule)
        {
            lock (censorLock)
            {
                censorList.Add(newRule);
            }
        }

        ///<inheritdoc/>
        public IEnumerable<CensorRules> GetCensorRules()
        {
            lock (censorLock)
            {
                return new List<CensorRules>(censorList);
            }
        }
    }
}
