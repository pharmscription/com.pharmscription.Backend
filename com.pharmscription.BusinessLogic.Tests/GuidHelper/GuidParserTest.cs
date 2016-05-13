using System;
using System.Diagnostics.CodeAnalysis;
using com.pharmscription.BusinessLogic.GuidHelper;
using com.pharmscription.Infrastructure.Exception;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.pharmscription.BusinessLogic.Tests.GuidHelper
{
    [TestClass]
    [ExcludeFromCodeCoverage]
    public class GuidParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestGuidParseThrowsOnNull()
        {
            GuidParser.ParseGuid(null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidArgumentException))]
        public void TestGuidParseThrowsOnGarbage()
        {
            GuidParser.ParseGuid("sdfkjsdfk");
        }

        [TestMethod]
        public void TestGuidParse()
        {
            var guid = GuidParser.ParseGuid("1baf86b0-1e14-4f4c-b05a-5c9dd00e8e38");
            Assert.IsNotNull(guid);
            Assert.AreNotEqual(Guid.Empty, guid);
        }
    }
}
