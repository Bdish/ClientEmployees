using AccessToWebApi.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccessToWebApi
{
    public class HTTPForEmployee
    {
        public static HTTP http;

        public HTTPForEmployee()
        {
            if (http == null)
            {
                http = new HTTP();
            }
        }

        public Employee Post(Employee newEmployee)
        {
            string result = "";
            string strNewEmployee = JsonConvert.SerializeObject(newEmployee);
            result = http.Request("https://localhost:44365/api/eployee", "Post", strNewEmployee);
            return JsonConvert.DeserializeObject<Employee>(result);
        }

        public IEnumerable<Employee> Get()
        {
            string result = "";
            result = http.Request("https://localhost:44365/api/eployee", "Get");
            return JsonConvert.DeserializeObject<IEnumerable<Employee>>(result);
        }


    }
}
