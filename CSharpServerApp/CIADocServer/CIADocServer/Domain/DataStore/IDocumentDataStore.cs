using CIADocServer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIADocServer.Domain.DataStore
{
    public interface IDocumentDataStore
    {
        /// <summary>
        /// Stores a new document in the datastore. Will not store a document if a document with same title exists in data storage
        /// </summary>
        /// <param name="doc">the document to be added</param>
        /// <returns>true if document is successfully stores, false otherwise.</returns>
        public bool StoreNewDocument(ClassifiedDocument doc);

        /// <summary>
        /// Returns an enumerable of the document titles in data storage
        /// </summary>
        /// <returns>enumerable of document titles</returns>
        public IEnumerable<string> GetDocumentTitles();

        /// <summary>
        /// Returns the text of the document with the given title.
        /// </summary>
        /// <param name="title">the title of the document to get the text for</param>
        /// <returns>the document text</returns>
        public ClassifiedDocument GetDocument(string title);
    }
}
