using CIADocServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIADocServer.Domain.Services
{
    public interface IDocumentService
    {
        /// <summary>
        /// Gets a list of the document titles stored in the database.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetDocumentTitles();

        /// <summary>
        /// Gets the text of document named by <paramref name="docTitle"/>, applies any censorship rules that apply to the user, and returns the censored text
        /// </summary>
        /// <param name="docTitle">the title of the document being requested</param>
        /// <param name="userLevel">the clearance level of the user. Optional. If absent assumes the lowest possible clearance</param>
        /// <param name="specialAuthorizations">the special authorizations possesed by the user. Null is treated as empty. Optional. If absent assumes no special authorizations. </param>
        /// <returns>The text censored as applicable to the user</returns>
        string GetCensoredText(string docTitle, ClearanceLevel userLevel = ClearanceLevel.Unclassifed, IEnumerable<string> specialAuthorizations = null);

        /// <summary>
        /// Add a document to the datastore
        /// </summary>
        /// <param name="document"></param>
        void AddDocument(ClassifiedDocument document);
    }
}
