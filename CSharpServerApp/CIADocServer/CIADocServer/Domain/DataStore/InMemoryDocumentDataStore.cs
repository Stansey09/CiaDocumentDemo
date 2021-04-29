using CIADocServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIADocServer.Domain.DataStore
{
    /// <summary>
    /// An implementation of IDocumentDataStore that just keeps the documents in a simple dictionary
    /// </summary>
    public class InMemoryDocumentDataStore : IDocumentDataStore
    {

        private Dictionary<string, ClassifiedDocument> documentDictionary;
        private object documentLock = new object();

        public InMemoryDocumentDataStore()
        {
            documentDictionary = new Dictionary<string, ClassifiedDocument>();

            //temporary add API to get documents in here.
            ClassifiedDocument testDocument = new ClassifiedDocument()
            {
                DocumentTitle = "testDocument",
                DocumentText = "This is great test string great"
            };
            documentDictionary.Add(testDocument.DocumentTitle, testDocument);
        }

        ///<inheritdoc/>
        public ClassifiedDocument GetDocument(string title)
        {
            lock(documentLock)
            {
                if (documentDictionary.TryGetValue(title, out var doc))
                {
                    return doc;
                }
                else
                {
                    //TODO throw better exception for proper error handling
                    throw new Exception();
                }
            }
        }

        ///<inheritdoc/>
        public IEnumerable<string> GetDocumentTitles()
        {
            lock(documentLock)
            {
                return new List<string>(documentDictionary.Keys);
            }
        }

        ///<inheritdoc/>
        public bool StoreNewDocument(ClassifiedDocument doc)
        {
            lock(documentLock)
            {
                return documentDictionary.TryAdd(doc.DocumentTitle, doc);
            }
        }
    }
}
