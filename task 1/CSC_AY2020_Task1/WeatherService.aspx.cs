using System;
using System.Xml;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CSC_AY2020
{
    public partial class WeatherService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XmlDocument wsResponseXmlDoc = new XmlDocument();

            //http://api.worldweatheronline.com/premium/v1/weather.ashx?key=1a4d0ea614b244e78dc65205202810&q=London&format=xml&num_of_days=5
            //id=jipx(spacetime0)
            UriBuilder url = new UriBuilder();
            url.Scheme = "http";// Same as "http://"

            url.Host = "api.worldweatheronline.com";
            url.Path = "premium/v1/weather.ashx";// change to v2
            url.Query = "key=102915d3fa844bcf8f652919210101&q=London&format=xml&num_of_days=5";

            //Make a HTTP request to the global weather web service
            wsResponseXmlDoc = MakeRequest(url.ToString());
            if (wsResponseXmlDoc != null)
            {
                //display the XML response for user
                String xmlString = wsResponseXmlDoc.InnerXml;
                Response.ContentType = "text/xml";
                Response.Write(xmlString);

                // Save the document to a file and auto-indent the output.
                XmlTextWriter writer = new XmlTextWriter(Server.MapPath("xmlweather.xml"), null);
                writer.Formatting = Formatting.Indented;
                wsResponseXmlDoc.Save(writer);

                //You're never closing the writer, so I would expect it to keep the file open. That will stop future attempts to open the file

                writer.Close();
            }
            else
            {
                Response.ContentType = "text/html";
                Response.Write("<h2> error  accessing web service </h2>");
            }


        }


        public static XmlDocument MakeRequest(string requestUrl)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                request.Timeout = 15 * 1000;
                request.KeepAlive = false;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(response.GetResponseStream());
                return xmlDoc;


            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}