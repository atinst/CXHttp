using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.Web.Http.Filters;
using System.Threading;

namespace CXHttpNS
{
    public class CXRequest
    {
        private CancellationTokenSource cts;
        private HttpBaseProtocolFilter filter;
        private HttpClient httpClient;

        string url;
        HttpRequestHeaderCollection headers;

        Dictionary<string, string> data = new Dictionary<string, string>();
        string rawData;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CXRequest()
        {
            filter = new HttpBaseProtocolFilter();
            httpClient = new HttpClient(filter);
            cts = new CancellationTokenSource();
            headers = httpClient.DefaultRequestHeaders;
        }

        /// <summary>
        /// Constructor: from URL
        /// </summary>
        /// <param name="url">Request URL</param>
        public CXRequest(string url)
        {
            filter = new HttpBaseProtocolFilter();
            httpClient = new HttpClient(filter);
            cts = new CancellationTokenSource();
            headers = httpClient.DefaultRequestHeaders;
            this.url = url;
        }

        /// <summary>
        /// Constructor: from existed <see cref="HttpClient"/>
        /// </summary>
        /// <param name="filter"><see cref="HttpBaseProtocolFilter"/></param>
        /// <param name="client"><see cref="httpClient"/></param>
        public CXRequest(HttpBaseProtocolFilter filter, HttpClient client)
        {
            this.filter = filter;
            httpClient = client;
            cts = new CancellationTokenSource();
            headers = httpClient.DefaultRequestHeaders;
        }

        /// <summary>
        /// Set URL
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Url(string url)
        {
            this.url = url;
            return this;
        }

        /// <summary>
        /// Add a header pair
        /// </summary>
        /// <param name="key">Header name</param>
        /// <param name="value">Header value</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Header(string key, string value)
        {
            this.headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// Concatenate header collection
        /// </summary>
        /// <param name="headers"><see cref="HttpRequestHeaderCollection"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Headers(HttpRequestHeaderCollection headers)
        {
            this.headers.Concat(headers);
            return this;
        }

        /// <summary>
        /// Add a data pair
        /// </summary>
        /// <param name="key">Data name</param>
        /// <param name="value">Data value</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Data(string key, string value)
        {
            this.data.Add(key, value);
            return this;
        }

        /// <summary>
        /// Add data pairs
        /// </summary>
        /// <param name="data">Data pair collection</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Data(Dictionary<string, string> data)
        {
            this.data.Concat(data);
            return this;
        }

        /// <summary>
        /// Add raw data
        /// </summary>
        /// <param name="data">Raw data string</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Data(string data)
        {
            this.rawData = data;
            return this;
        }

        /// <summary>
        /// Set cookies
        /// </summary>
        /// <param name="cookies"><see cref="HttpCookieCollection"/></param>
        /// <returns></returns>
        public CXRequest Cookies(HttpCookieCollection cookies)
        {
            foreach (var cookie in cookies)
            {
                filter.CookieManager.SetCookie(cookie);
            }
            return this;
        }

        /// <summary>
        /// Cancel HTTP request
        /// </summary>
        public void Cancel()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Make a get request
        /// </summary>
        /// <returns><c>Task&lt;Response&gt;</c></returns>
        public async Task<CXResponse> Get()
        {
            string tempUrl = url;
            if (rawData != "")
            {
                tempUrl += "?" + rawData;
            }
            else if (data.Count > 0)
            {
                HttpFormUrlEncodedContent content = new HttpFormUrlEncodedContent(data);
                string p = await content.ReadAsStringAsync().AsTask(cts.Token).ConfigureAwait(false);
                tempUrl += "?" + p;
            }
            Uri uri = new Uri(tempUrl);
            HttpResponseMessage res = await httpClient.GetAsync(uri).AsTask(cts.Token).ConfigureAwait(false);

            return new CXResponse(res, filter.CookieManager.GetCookies(uri));
        }

        /// <summary>
        /// Make a post request
        /// </summary>
        /// <returns><c>Task&lt;Response&gt;</c></returns>
        public async Task<CXResponse> Post()
        {
            IHttpContent content;
            if (rawData != null)
            {
                content = new HttpStringContent(rawData);
            }
            else
            {
                content = new HttpFormUrlEncodedContent(data);
            }
            Uri uri = new Uri(url);
            HttpResponseMessage res = await httpClient.PostAsync(uri, content).AsTask(cts.Token).ConfigureAwait(false);
            return new CXResponse(res, filter.CookieManager.GetCookies(uri));
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (filter != null)
            {
                filter.Dispose();
                filter = null;
            }

            if (httpClient != null)
            {
                httpClient.Dispose();
                httpClient = null;
            }

            if (cts != null)
            {
                cts.Dispose();
                cts = null;
            }
        }
    }
}
