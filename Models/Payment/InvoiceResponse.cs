using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using PaymentService.Models.Entities;

namespace PaymentService.Models.Payment
{
    public interface IInvoiceResponse
    {
        public string? this[string key] { get; }
    }

    public class UrlQueryInvoiceResponse : IInvoiceResponse
    {
        readonly NameValueCollection keys;
        
        public string? this[string key] { get { return keys[key]; } }

        public UrlQueryInvoiceResponse (string url)
        {
            Uri uriObject = new (url);
            keys = new(HttpUtility.ParseQueryString(uriObject.Query));
        }
    }

    public class JsonInvoiceResponse : IInvoiceResponse
    {
        readonly NameValueCollection keys = new();
        
        public string? this[string key] { get { return keys[key]; } }

        public JsonInvoiceResponse (string json)
        {
            JsonDocument jsonDoc = JsonDocument.Parse(json);

            void ProcessJsonElement(JsonElement element, string prefix)
            {
                switch (element.ValueKind)
                {
                    case JsonValueKind.Number:
                        // trim last period character 
                        keys[prefix[..^1]] = element.GetDouble().ToString();
                        return;
                    case JsonValueKind.String:
                        // trim last period character 
                        keys[prefix[..^1]] = element.GetString();
                        return;
                    case JsonValueKind.Object:
                        foreach (var child in element.EnumerateObject())
                        {
                            ProcessJsonElement(child.Value, prefix + child.Name + ".");
                        }
                        return;
                    case JsonValueKind.Array:
                        int index = 0;
                        foreach (var child in element.EnumerateArray())
                        {
                            ProcessJsonElement(child, prefix + index + ".");
                            index++;
                        }
                        keys[prefix + "Length"] = index.ToString();
                        return;
                    default:
                        return;
                }
            }
            ProcessJsonElement(jsonDoc.RootElement, ".");
        }
    }
}