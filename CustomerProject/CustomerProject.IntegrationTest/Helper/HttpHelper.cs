using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerProject.IntegrationTest.Helper
{
    internal class HttpHelper
    {
        public static StringContent GetJsonHttpContent(object items)
        {
            return new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");
        }

        internal static class Urls
        {
            public readonly static string GetAllCustomers = "/api/Customer/GetAllAsync";
            public readonly static string GetCustomer = "/api/Customer/GetAsync";
            public readonly static string AddCustomer = "/api/Customer/AddAsync";
            public readonly static string EditCustomer = "/api/Customer/UpdateAsync";
            public readonly static string DeleteCustomers = "/api/Customer/DeleteAsync";
        }
    }
}
