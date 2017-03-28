using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SwaggerParser.Test
{
    [TestClass]
    public class SchemaParserTest
    {
        [TestMethod]
        public void TestSage200Schema()
        {
            ISchemaSource source = new EmbeddedSchemaSource();
            source.Initialise("sage200online.json");

            var target = new SchemaParser();
            target.Initialise(source);
        }
    }
}
