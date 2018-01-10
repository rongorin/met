using MetopeMVCApp.Models.Custom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace MetopeMVCApp.Services
{
    public class ClientApiService<T>  
    {
       // private string BASE_URL = "http://localhost:54179/api/";

        // ConfigurationManager.AppSettings["GenericEntityId"]) ; 

        private string BASE_URL =   ConfigurationManager.AppSettings["MetopeWebApiAddr"];
        private string FilePath = "";
        //this is an example of logging database functionaily:
        private void LogInfo(string logmessage)
        { 
            System.IO.File.AppendAllText(FilePath, logmessage);
        } 


        //Generic Get servie
        public IEnumerable<T> findAll<t>(string svcEndPoint, decimal entityID, string filePath) 
        {
            IEnumerable<T> pvs = null;
            FilePath = filePath;
          
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);
                //HTTP GET
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responseTask = client.GetAsync(svcEndPoint + "?EntityID=" + entityID);
                LogInfo("\r\n R1 base:" + BASE_URL + svcEndPoint + "?EntityID=" + entityID);

                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<T>>();
                    readTask.Wait();
                    LogInfo("\r\n R1 success" + "\n");
                     pvs = readTask.Result;
                     LogInfo("\r\n R1 count:" + readTask.Result.Count().ToString() );
                }
                else //web api sent error response 
                {
                    LogInfo("R1 Failure return othing" + "\n");
                    //log response status h ere..

                    //var ex = CreateApiException(result);
                    //throw ex;
                    pvs = Enumerable.Empty<T>(); 
                }
            } 
            return pvs;
        }
        //public T find<T>(string svcEndPoint, decimal entityId, decimal securityId, int dividendAnnNumber)
        //{

        //     T   pvs ;
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(BASE_URL);
        //        //HTTP GET
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var responseTask = client.GetAsync(svcEndPoint + "?EntityID=" + entityId + "&SecurityID=" + securityId + "&dividendAnnNumber=" + dividendAnnNumber);
        //        responseTask.Wait();

        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<T>();
        //            readTask.Wait();

        //            pvs = readTask.Result;
        //        }
        //        else //web api sent error response 
        //        {
        //            //log response status here.. 
        //            return default(T);
        //            //var ex = CreateApiException(result);
        //            //throw ex;
        //            //pvs = Enumerable.Empty<T>(); 
        //        }
        //    }
        //    return pvs;
        //}
        public T find<t>(string svcEndPoint, decimal entityId, decimal securityId, decimal dividendAnnNumber)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(BASE_URL);
                var response = httpClient.GetAsync(svcEndPoint + "?EntityID=" + entityId + "&SecurityID=" + securityId + "&dividendAnnNumber=" + dividendAnnNumber).Result;
 
                return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
        }
        //public T edit<T>(string svcEndPoint, decimal entityId, decimal securityId, int dividendAnnNumber)
        //{

        //    T pvs;
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(BASE_URL);
        //        //HTTP GET
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var responseTask = client.PutAsJsonAsync(svcEndPoint + "?EntityID=" + entityId + "&SecurityID=" + securityId + "&dividendAnnNumber=" + dividendAnnNumber, <T>);
        //        responseTask.Wait();

        //        var result = responseTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsAsync<T>();
        //            readTask.Wait();

        //            pvs = readTask.Result;
        //        }
        //        else //web api sent error response 
        //        {
        //            //log response status here.. 
        //            return default(T);
        //            //var ex = CreateApiException(result);
        //            //throw ex;
        //            //pvs = Enumerable.Empty<T>(); 
        //        }
        //    }
        //    return pvs;
        //} 
        public bool  create( T pv, string urlExtension)
        {
            using (var client = new HttpClient())
            { 
                client.BaseAddress = new Uri(BASE_URL);

                //HTTP POST
                HttpResponseMessage resp = client.PostAsJsonAsync<T>(urlExtension, pv ).Result;
                return resp.IsSuccessStatusCode;

                //if (result.IsSuccessStatusCode)
                //{
                //    return RedirectToAction("Index");
                //} 
            }
            //return false;
        }
        public bool edit(Metope.DAL.Security_Dividend_Split pv, string urlExtension)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);

                //HTTP POST
                HttpResponseMessage resp = client.PutAsJsonAsync<Metope.DAL.Security_Dividend_Split>(urlExtension + "?EntityID=" + pv.Entity_ID + "&SecurityID=" + pv.Security_ID + "&dividendAnnNumber=" + pv.Dividend_Annual_Number, pv).Result;
                return resp.IsSuccessStatusCode;

                //if (result.IsSuccessStatusCode)
                //{
                //    return RedirectToAction("Index");
                //}

            }
            //return false;
        }
        public bool delete(string urlExtension, decimal entityId, decimal securityId, decimal dividendAnnNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BASE_URL);

                //HTTP POST 
                HttpResponseMessage resp = client.DeleteAsync(urlExtension + "/Delete?EntityID=" + entityId + "&SecurityID=" + securityId + "&dividendAnnNumber=" + dividendAnnNumber).Result;
                return resp.IsSuccessStatusCode;

                //if (result.IsSuccessStatusCode)
                //{
                //    return RedirectToAction("Index");
                //}

            }
            //return false;
        }

      ///  
      ///  unwrapping errors on the client, taken from  https://www.codeproject.com/Articles/825274/ASP-NET-Web-Api-Unwrapping-HTTP-Error-Results-and 
      /// <returns></returns>
        public static ApiException CreateApiException(HttpResponseMessage response)
        {
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            // Create an anonymous object to use as the template for deserialization:
            var anonymousErrorObject =
                new { message = "", ModelState = new Dictionary<string, string[]>() };

            // Deserialize:
            var deserializedErrorObject =
                JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            // Now wrap into an exception which best fullfills the needs of your application:
            var ex = new ApiException(response);

            // Sometimes, there may be Model Errors:
            if (deserializedErrorObject.ModelState != null)
            {
                var errors =
                    deserializedErrorObject.ModelState
                                            .Select(kvp => string.Join(". ", kvp.Value));
                for (int i = 0; i < errors.Count(); i++)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(i, errors.ElementAt(i));
                }
            }
            // Othertimes, there may not be Model Errors:
            else
            {
                var error =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
                foreach (var kvp in error)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(kvp.Key, kvp.Value);
                }
            }
            return ex;
        }

    }
}