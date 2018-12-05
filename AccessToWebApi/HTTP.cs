using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AccessToWebApi
{
    public class HTTP
    {
        private  HttpClient _client;

        public HTTP()
        {
            _client = new HttpClient();
        }

        public string Request(string url, string httpMethod, string context="")
        {
            string response = "";

            switch (httpMethod.ToUpper())
            {
                case "POST":
                    {                       
                        response = PostRequest(url,context, "application/json");
                        break;
                    }

                case "GET":
                    {
                        response = GetRequest(url);
                        break;
                    }
                    
                default:
                    throw new NotImplementedException();
            }

            return response;
        }

        private string PostRequest(string url, string context,string mediaType)
        {
            string result="";
                         
            StringContent stringContext = new StringContent(context, Encoding.UTF8, mediaType); 

            var res = _client.PostAsync(url, stringContext).Result; 

            result= res.Content.ReadAsStringAsync().Result; 
            
            return result;
        }

        private string GetRequest(string url)
        {
            string result = "";

            var res= _client.GetAsync(url).Result;

            result = res.Content.ReadAsStringAsync().Result; 

            return result;
        }

    }
}
