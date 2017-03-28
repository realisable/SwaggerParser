using Realisable.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerParser
{
    //internal
    public class EmbeddedSchemaSource : ISchemaSource
    {
        private Stream content;
        private string path;

        public void Initialise(string path)
        {
            this.path = path;
            Refresh();
        }

        public string Get()
        {
            using (content)
            {
                try
                {
                    content.Seek(0, SeekOrigin.Begin);
                    using (StreamReader reader = new StreamReader(content))
                    {
                        return reader.ReadToEnd();
                    }
                }
                catch (Exception Ex)
                {
                    if (Ex is ObjectDisposedException || Ex is ArgumentNullException)
                    {
                        Refresh();
                        return Get();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public void Refresh()
        {
            this.content = ResourceHelper.GetEmbeddedResource(path, this.GetType().Assembly);
        }
    }
}
