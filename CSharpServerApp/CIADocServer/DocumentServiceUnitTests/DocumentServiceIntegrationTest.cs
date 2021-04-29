using CIADocServer.Domain.DataStore;
using CIADocServer.Domain.Models;
using CIADocServer.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace DocumentServiceUnitTests
{
    /// <summary>
    /// test with the "real" datastores just to make sure those work befor before I try to run this thing for real.
    /// </summary>
    [TestClass]
    public class DocumentServiceIntegrationTest
    {

        [TestMethod]
        public void GetCensorRules_OneRuleTwoTermsUserLacksClearance_ReplacementsOccurCorrectly()
        {
            //arrange
            InMemoryCensorRuleDataStore censorRuleDataStore = new InMemoryCensorRuleDataStore();
            InMemoryDocumentDataStore documentDataStore = new InMemoryDocumentDataStore();

            DocumentService serviceUnderTest = new DocumentService(censorRuleDataStore, documentDataStore);

            //act 
            var result = serviceUnderTest.GetCensoredText("testDocument");

            //assert
            Assert.AreEqual("This is REDACTED test REDACTED REDACTED", result);
        }
    }
}