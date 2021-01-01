using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using task_7.Models;

static class Keys
{
    public const string ENVIRONMENT_URL = "";
    public const string USERNAME = "";
    public const string API_KEY = "";
    public const string CLIENT_ID = "";
}

namespace task_7.Controllers
{
    public class ReceiptController : ApiController
    {
        [HttpPost]
        [Route("api/v1/checkAmount")]
        public string CheckAmount(ImageFile file)
        {
            string jsonResponse = "";
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://" + Keys.ENVIRONMENT_URL + "/api/v7/partner/documents/");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("Authorization", "apikey " + Keys.USERNAME + ":" + Keys.API_KEY);
            httpWebRequest.Headers.Add("Client-id", Keys.CLIENT_ID);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"file_name\":\"" + file.name+ "\"," +
                               "\"file_data\":\"" + file.base64 + "\"}";
                streamWriter.Write(json);
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                jsonResponse = streamReader.ReadToEnd();
                Debug.WriteLine(String.Format("Response: {0}", jsonResponse));
            }
            
            return jsonResponse;
        }

    }
}
