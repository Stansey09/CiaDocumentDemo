using CIADocServer.Domain.DataStore;
using CIADocServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIADocServer.Domain.Services
{
    public class DocumentService : IDocumentService
    {
        ICensorshipRulesDataStore censorDataStore;
        IDocumentDataStore documentDataStore;

        public DocumentService(ICensorshipRulesDataStore censorRuleDS, IDocumentDataStore documentDS)
        {
            censorDataStore = censorRuleDS;
            documentDataStore = documentDS;
        }

        public void AddDocument(ClassifiedDocument document)
        {
            this.documentDataStore.StoreNewDocument(document);
        }

        ///<inheritdoc/>
        public string GetCensoredText(string docTitle, ClearanceLevel userLevel = ClearanceLevel.Unclassifed, IEnumerable<string> specialAuthorizations = null)
        {
            // Get the global censor rules.
            var globalCensorRules = censorDataStore.GetCensorRules();

            //filter out rules that do not apply to the user due to clearance level or special authorizations.
            var applicableCensorRules = globalCensorRules.Where(cr => cr.MinimumClearance > userLevel &&
                (specialAuthorizations == null || cr.SpecialAuthorizations.All(sa => !specialAuthorizations.Contains(sa))))
                .ToList();

            //get the text of the document to be censored
            string textToCensor = documentDataStore.GetDocument(docTitle).DocumentText;

            //okay so what I want to do now is find all replacements that need to be made and remember them

            //list of "replacements", item1 start index, item 2 length, item 3 string to replace with.
            List<Tuple<int, int, string>> replacements = new List<Tuple<int, int, string>>();
            foreach(CensorRules rule in applicableCensorRules)
            {
                foreach(string classifiedTerm in rule.ClassifiedTerms)
                {
                    if(classifiedTerm == string.Empty)
                    {
                        //this wont work with an empty string nor should it.
                        continue;
                    }

                    int index = 0;
                    while (index < textToCensor.Length)
                    {
                        index = textToCensor.IndexOf(classifiedTerm, index);
                        if(index == -1)
                        {
                            //no more instances of the classified term.
                            break;
                        }
                        replacements.Add(new Tuple<int, int, string>(index, classifiedTerm.Length, rule.ReplacementTerm));
                        index += classifiedTerm.Length;
                    }
                }
            }

            //Now I want combine any overlapping replaced sections, as well as combining their replacement text. Perhaps the censor rules swap in code names, or include a declassification date,
            //or provide some other information to the reader other than REDACTED. I don't want to lose any of this. I still want them in order.
            Tuple<int, int, string> priorReplacement = null;
            List<Tuple<int, int, string>> combinedOrderedReplacements = new List<Tuple<int, int, string>>();
            foreach (var replacement in replacements.OrderBy(o => o.Item1).ThenBy(o => o.Item2))
            {
                if(priorReplacement == null)
                {
                    priorReplacement = replacement;
                    continue;
                }

                if(priorReplacement.Item1 + priorReplacement.Item2 >= replacement.Item1)
                {
                    //this replacement overlaps, combine em.
                    int endOfOverlap = Math.Max(priorReplacement.Item1 + priorReplacement.Item2, replacement.Item1 + replacement.Item2);
                    int overlapLength = endOfOverlap - priorReplacement.Item1;
                    string newReplacementString = priorReplacement.Item3 + "-" + replacement.Item3;
                    priorReplacement = new Tuple<int, int, string>(priorReplacement.Item1, overlapLength, newReplacementString);
                }
                else
                {
                    combinedOrderedReplacements.Add(priorReplacement);
                    priorReplacement = replacement;
                }
            }
            
            if(priorReplacement != null)
            {
                combinedOrderedReplacements.Add(priorReplacement);
            }
            
            //lastly, in reverse order, I want to swap out the censored text with the replacement text. This way I don't break the indexes I am using.
            for( int i = combinedOrderedReplacements.Count -1; i >= 0; i--)
            {
                textToCensor = textToCensor.Remove(combinedOrderedReplacements[i].Item1, combinedOrderedReplacements[i].Item2);
                textToCensor = textToCensor.Insert(combinedOrderedReplacements[i].Item1, combinedOrderedReplacements[i].Item3);
            }

            return textToCensor;
        }

        ///<inheritdoc/>
        public IEnumerable<string> GetDocumentTitles()
        {
            return documentDataStore.GetDocumentTitles();
        }
    }
}
