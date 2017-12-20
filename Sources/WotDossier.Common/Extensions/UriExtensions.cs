using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WotDossier.Common.Extensions
{
    public static class UriExtensions
    {
	    private static HttpClient Client { get; } = new HttpClient();

        public static void Delete(this Uri uri)
        {
            try
            {
                var request = WebRequest.Create(uri);
                request.Method = "DELETE";
                request.GetResponse();
            }
            catch
            {
                
            }
        }

        public static T Get<T>(this Uri uri)
        {
            //client.BaseAddress = uri;
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            T result = default(T);
            HttpResponseMessage response = Client.GetAsync(uri).Result;
            if (response.IsSuccessStatusCode)
            {
                //if (response.Content.Headers.ContentEncoding.Any(x => x == "gzip"))
                //{
                //    // Decompress manually
                //    using (var s = response.Content.ReadAsStreamAsync())
                //    {
                //        using (var decompressed = new GZipStream(s, CompressionMode.Decompress))
                //        {
                //            using (var rdr = new StreamReader(decompressed))
                //            {
                //                return rdr.ReadToEndAsync();
                //            }
                //        }
                //    }
                //}
                //else
                    result = response.Content.ReadAsAsync<T>().Result;
            }
            return result;
        }

	    public static string Download(this Uri uri)
	    {
		    var result = string.Empty;
		    HttpResponseMessage response = Client.GetAsync(uri).Result;
		    if (response.IsSuccessStatusCode)
		    {
			    result = response.Content.ReadAsStringAsync().Result;
		    }
		    return result;
	    }

		public static void Post(this Uri uri, string data)
        {
            var client = new WebClient();
            client.Proxy.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                //client.Headers[HttpRequestHeader.ContentLength] = data.Length.ToString(CultureInfo.InvariantCulture);
                client.UploadStringAsync(uri, data);
            }
            finally
            {
                client.Dispose();
            }
        }

        public static void Post(this Uri uri, byte[] data)
        {
            var client = new WebClient();
            client.Proxy.Credentials = CredentialCache.DefaultCredentials;
            try
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/octet-stream";
                //client.Headers[HttpRequestHeader.ContentLength] = data.Length.ToString(CultureInfo.InvariantCulture);
                client.UploadDataAsync(uri, data);
            }
            finally
            {
                client.Dispose();
            }
        }
    }
}