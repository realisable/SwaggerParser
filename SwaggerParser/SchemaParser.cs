using NJsonSchema;
using NSwag;
using Realisable.Connectors;
using System.Text.RegularExpressions;
using System;
using Realisable.Resources;
using System.Collections.Generic;

namespace SwaggerParser
{
    public class SchemaParser
    {
        const string URL_OPTION = "url";

        public void Initialise(ISchemaSource source)
        {
            var document = SwaggerDocument.FromJsonAsync(source.Get()).Result;

            Parse(document);
        }

        private void Parse(SwaggerDocument document)
        {
            var pullMeta = new MetaDataDocument();
            var pushMeta = new MetaDataDocument();

            foreach (KeyValuePair<string, SwaggerOperations> path in document.Paths)
            {
                foreach (var item in path.Value)
                {
                    SwaggerOperationMethod method = item.Key;
                    SwaggerOperation operation = item.Value;

                    switch (method)
                    {
                        case SwaggerOperationMethod.Get:
                            pullMeta.AddImport(GetReadImport(path.Key, method, operation));
                            break;
                        case SwaggerOperationMethod.Patch:
                        case SwaggerOperationMethod.Post:
                        case SwaggerOperationMethod.Put:
                        case SwaggerOperationMethod.Delete:
                            pushMeta.AddImport(GetWriteImport(path.Key, method, operation));
                            break;
                        case SwaggerOperationMethod.Undefined:
                        case SwaggerOperationMethod.Options:
                        case SwaggerOperationMethod.Head:
                            continue;
                        default:
                            break;
                    }
                }
            }
        }

        private SystemImport GetReadImport(string path, SwaggerOperationMethod method, SwaggerOperation operation)
        {
            string description = string.IsNullOrWhiteSpace(operation.Description) ? operation.OperationId : operation.Description;

            var result = new SystemImport(operation.OperationId, description, eERPUpdateOperation.eAll);

            PopulateReadOptions(path, result, operation);

            return result;
        }

        private void PopulateReadOptions(string path, SystemImport result, SwaggerOperation operation)
        {            
            result.Options[URL_OPTION] = path;

            foreach (var response in operation.Responses)
            {
                var key = response.Key;
                var value = response.Value;

                // I think here NSwag may need extending so we can properly traverse the response
                JsonSchema4 schema = value.ActualResponseSchema;

                if (key == "200")
                {
                    //TODO: for example here we're not getting any of the properties.
                    foreach (var required in schema.RequiredProperties)
                    {
                        var s = required.ToString();
                    }

                    foreach (var property in schema.Properties)
                    {
                        var s = property.ToString();
                    }
                }
                
            }
        }

        private void PopulateWriteOptions(string path, SystemImport result, SwaggerOperation operation)
        {
            result.Options[URL_OPTION] = path;
        }

        private SystemImport GetWriteImport(string path, SwaggerOperationMethod method, SwaggerOperation operation)
        {
            string description = string.IsNullOrWhiteSpace(operation.Description) ? operation.OperationId : operation.Description;

            eERPUpdateOperation updateOps = eERPUpdateOperation.eInsert;

            switch (method)
            {
                case SwaggerOperationMethod.Post:
                    updateOps = eERPUpdateOperation.eInsert;
                    break;
                case SwaggerOperationMethod.Put:
                    updateOps = eERPUpdateOperation.eUpdate;
                    break;
                case SwaggerOperationMethod.Delete:
                    updateOps = eERPUpdateOperation.eDelete;
                    break;
                case SwaggerOperationMethod.Patch:
                    updateOps = eERPUpdateOperation.eUpdate;
                    break;
                default:
                    break;
            }

            var result = new SystemImport(operation.OperationId, description, updateOps);

            return result;
        }        
    }
}
