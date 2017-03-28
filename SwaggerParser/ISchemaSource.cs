using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwaggerParser
{
    public interface ISchemaSource
    {
        void Initialise(string path);
        string Get();

        void Refresh();
    }
}
