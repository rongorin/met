using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace MetopeMVCApp.Models.Custom
{
    public class Weather
    {  
        public Object getWeatherForecast()
        {
            try
            {
                //syncronous client 
                var client = new WebClient();
                var content = client.DownloadString("http://api.openweathermap.org/data/2.5/weather?q=Cape Town&units=metric&APPID=9fec8dc25b436cce0dbc88d2cbb8018a");
                var serializer = new JavaScriptSerializer();


                var jsonContent = serializer.Deserialize<Object>(content);
                return jsonContent;
            }
            catch
            {
                //return new HttpRequestValidationException();
                throw new HttpRequestValidationException();

                //Response.StatusCode = 400;
            }
        }
     
    }
}