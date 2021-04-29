using CIADocServer.Domain.DataStore;
using CIADocServer.Domain.Models;
using CIADocServer.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace DocumentServiceUnitTests
{
    [TestClass]
    public class DocumentServiceUnitTests
    {
        Mock<IDocumentDataStore> mockDocStore;
        Mock<ICensorshipRulesDataStore> mockCensorStore;

        ClassifiedDocument testDocument;
        List<CensorRules> testRules;

        DocumentService serviceUnderTest;

        [TestInitialize]
        public void setupTest()
        {
            mockDocStore = new Mock<IDocumentDataStore>();
            mockCensorStore = new Mock<ICensorshipRulesDataStore>();

            testRules = new List<CensorRules>();
            testDocument = new ClassifiedDocument()
            {
                DocumentTitle = "testDocument",
                DocumentText = "This is great test string great"
            };

            mockDocStore.Setup(o => o.GetDocument("testDocument")).Returns(testDocument);
            mockCensorStore.Setup(o => o.GetCensorRules()).Returns(testRules);
            serviceUnderTest = new DocumentService(mockCensorStore.Object, mockDocStore.Object);

        }

        [TestMethod]
        public void GetCensorRules_OneRuleTwoTermsUserLacksClearance_ReplacementsOccurCorrectly()
        {
            // arrange 
            CensorRules cr = new CensorRules()
            {
                Id = 7,
                ClassifiedTerms = new List<string>() { "great", "string" },
                MinimumClearance = ClearanceLevel.Confidential,
                ReplacementTerm = "REDACTED",
                SpecialAuthorizations = new List<string>()
            };
            testRules.Add(cr);

            //act 
            var result = serviceUnderTest.GetCensoredText("testDocument");

            //assert
            Assert.AreEqual("This is REDACTED test REDACTED REDACTED", result);
        }

        [TestMethod]
        public void GetCensorRules_OneRuleTwoTermsUserHasClearance_NoReplacements()
        {
            // arrange 
            CensorRules cr = new CensorRules()
            {
                Id = 7,
                ClassifiedTerms = new List<string>() { "great", "string" },
                MinimumClearance = ClearanceLevel.Confidential,
                ReplacementTerm = "REDACTED",
                SpecialAuthorizations = new List<string>()
            };
            testRules.Add(cr);

            //act 
            var result = serviceUnderTest.GetCensoredText("testDocument", ClearanceLevel.Confidential);

            //assert
            Assert.AreEqual(testDocument.DocumentText, result);
        }

        [TestMethod]
        public void GetCensorRules_OneRuleTwoTermsUserHasSA_NoReplacements()
        {
            // arrange 
            CensorRules cr = new CensorRules()
            {
                Id = 7,
                ClassifiedTerms = new List<string>() { "great", "string" },
                MinimumClearance = ClearanceLevel.Confidential,
                ReplacementTerm = "REDACTED",
                SpecialAuthorizations = new List<string>() { "SAS112"}
            };
            testRules.Add(cr);

            //act 
            var result = serviceUnderTest.GetCensoredText("testDocument", ClearanceLevel.Unclassifed, new List<string>() { "SAS112" });

            //assert
            Assert.AreEqual(testDocument.DocumentText, result);
        }

        [TestMethod]
        public void GetCensorRules_TwoRulesBothApplyToUser_ReplacementsOccurCorrectly()
        {
            // arrange 
            CensorRules cr = new CensorRules()
            {
                Id = 7,
                ClassifiedTerms = new List<string>() { "great" },
                MinimumClearance = ClearanceLevel.Confidential,
                ReplacementTerm = "REDACTED",
                SpecialAuthorizations = new List<string>()
            };
            testRules.Add(cr);

            cr = new CensorRules()
            {
                Id = 8,
                ClassifiedTerms = new List<string>() { "string" },
                MinimumClearance = ClearanceLevel.Confidential,
                ReplacementTerm = "REDACTED",
                SpecialAuthorizations = new List<string>()
            };
            testRules.Add(cr);

            //act 
            var result = serviceUnderTest.GetCensoredText("testDocument");

            //assert
            Assert.AreEqual("This is REDACTED test REDACTED REDACTED", result);
        }

        [TestMethod]
        public void GetCensorRules_TwoRulesOneAppliesToUser_ReplacementsOccurCorrectly()
        {
            // arrange 
            CensorRules cr = new CensorRules()
            {
                Id = 7,
                ClassifiedTerms = new List<string>() { "great" },
                MinimumClearance = ClearanceLevel.TopSecret,
                ReplacementTerm = "REDACTED",
                SpecialAuthorizations = new List<string>()
            };
            testRules.Add(cr);

            cr = new CensorRules()
            {
                Id = 8,
                ClassifiedTerms = new List<string>() { "string" },
                MinimumClearance = ClearanceLevel.Confidential,
                ReplacementTerm = "REDACTED",
                SpecialAuthorizations = new List<string>()
            };
            testRules.Add(cr);

            //act 
            var result = serviceUnderTest.GetCensoredText("testDocument", ClearanceLevel.Confidential);

            //assert
            Assert.AreEqual("This is REDACTED test string REDACTED", result);
        }

        [TestMethod]
        public void GetCensorRules_OverlappingReplacements_ReplacementsOccurCorrectly()
        {
            // arrange 
            CensorRules cr = new CensorRules()
            {
                Id = 7,
                ClassifiedTerms = new List<string>() { "great" },
                MinimumClearance = ClearanceLevel.Confidential,
                ReplacementTerm = "REDACTED",
                SpecialAuthorizations = new List<string>()
            };
            testRules.Add(cr);

            cr = new CensorRules()
            {
                Id = 8,
                ClassifiedTerms = new List<string>() { "great test" },
                MinimumClearance = ClearanceLevel.Confidential,
                ReplacementTerm = "OVERLAP",
                SpecialAuthorizations = new List<string>()
            };
            testRules.Add(cr);

            //act 
            var result = serviceUnderTest.GetCensoredText("testDocument");

            //assert
            Assert.AreEqual("This is REDACTED-OVERLAP string REDACTED", result);
        }


    }
}
