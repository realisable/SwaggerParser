using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwaggerParser.Test
{
    [TestClass]
    public class SchemaSourceTest
    {
        [TestMethod]
        public void EmbeddedSchemaSource_ValidSource_ContentReturned()
        {
            ISchemaSource source = new EmbeddedSchemaSource();
            source.Initialise("sage200online.json");

            string content = source.Get();

            Assert.IsFalse(string.IsNullOrWhiteSpace(content));
        }

        [TestMethod]
        public void EmbeddedSchemaSource_InvalidSource_ExceptionThrown()
        {

        }
    }
}
