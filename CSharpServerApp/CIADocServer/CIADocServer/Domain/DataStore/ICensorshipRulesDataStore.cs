using CIADocServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIADocServer.Domain.DataStore
{
    public interface ICensorshipRulesDataStore
    {
        /// <summary>
        /// Returns an enumerable of the censorship rules in the data store
        /// </summary>
        /// <returns>enumerable of CensorRules</returns>
        public IEnumerable<CensorRules> GetCensorRules();

        /// <summary>
        /// Adds a new global censor rule to data stroe
        /// </summary>
        /// <param name="newRule">the rule to add.</param>
        public void AddCensorRule(CensorRules newRule);
    }
}
