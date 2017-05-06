using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Windows.Web.Http.Filters;
using System.Threading;
using Windows.Security.Cryptography.Certificates;
using Windows.Security.Credentials;
using Windows.Foundation;

namespace CXHttpNS
{
    public class CXRequest
    {
        private CancellationTokenSource cts;
        private HttpBaseProtocolFilter mFilter;
        private HttpClient mHttpClient;

        private string url;
        private HttpRequestHeaderCollection mHeaders;

        private Dictionary<string, string> data = new Dictionary<string, string>();
        private string rawData = "";

        private IHttpContent mHttpContent = null;

        private bool isClearCookies = false;

        public HttpBaseProtocolFilter Filter { get { return mFilter; } }
        public HttpRequestHeaderCollection HeaderCollection { get { return mHeaders; } }
        public HttpClient HttpClient { get { return mHttpClient; } }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public CXRequest()
        {
            mFilter = new HttpBaseProtocolFilter();
            mHttpClient = new HttpClient(mFilter);
            cts = new CancellationTokenSource();
            mHeaders = mHttpClient.DefaultRequestHeaders;
        }

        /// <summary>
        /// Constructor: from URL
        /// </summary>
        /// <param name="url">Request URL</param>
        public CXRequest(string url)
        {
            mFilter = new HttpBaseProtocolFilter();
            mHttpClient = new HttpClient(mFilter);
            cts = new CancellationTokenSource();
            mHeaders = mHttpClient.DefaultRequestHeaders;
            this.url = url;
        }

        /// <summary>
        /// Constructor: from existed <see cref="HttpClient"/>
        /// </summary>
        /// <param name="filter"><see cref="HttpBaseProtocolFilter"/></param>
        /// <param name="client"><see cref="mHttpClient"/></param>
        public CXRequest(HttpBaseProtocolFilter filter, HttpClient client)
        {
            mFilter = filter;
            mHttpClient = client;
            cts = new CancellationTokenSource();
            mHeaders = mHttpClient.DefaultRequestHeaders;
        }

        /// <summary>
        /// Reset request parameters
        /// </summary>
        private void Clean()
        {
            url = "";
            data.Clear();
            rawData = "";
            mHeaders.Clear();
            isClearCookies = false;
        }

        /// <summary>
        /// Set auto redirect allowed
        /// </summary>
        /// <param name="allow">is allowed</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest AllowAutoRedirect(bool allow = true)
        {
            mFilter.AllowAutoRedirect = allow;
            return this;
        }

        /// <summary>
        /// Set UI allowed
        /// </summary>
        /// <param name="allow">is allowed</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest AllowUI(bool allow = true)
        {
            mFilter.AllowUI = allow;
            return this;
        }

        /// <summary>
        /// Set automatic decompression enabled
        /// </summary>
        /// <param name="enable">is enabled</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest AutomaticDecompression(bool enable = true)
        {
            mFilter.AutomaticDecompression = enable;
            return this;
        }

        /// <summary>
        /// Set cache read behavior
        /// </summary>
        /// <param name="behavior"><see cref="HttpCacheReadBehavior"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest CacheReadBehavior(HttpCacheReadBehavior behavior = HttpCacheReadBehavior.Default)
        {
            mFilter.CacheControl.ReadBehavior = behavior;
            return this;
        }

        /// <summary>
        /// Set client certificate
        /// </summary>
        /// <param name="cert"><see cref="Certificate"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest ClientCertificate(Certificate cert)
        {
            mFilter.ClientCertificate = cert;
            return this;
        }

        /// <summary>
        /// Set cookie usage behavior
        /// </summary>
        /// <param name="behavior"><see cref="HttpCookieUsageBehavior"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest CookieUsageBehavior(HttpCookieUsageBehavior behavior)
        {
            mFilter.CookieUsageBehavior = behavior;
            return this;
        }

        /// <summary>
        /// Set an ignorable server certificate error
        /// </summary>
        /// <param name="error"><see cref="ChainValidationResult"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest IgnoreServerCertificateError(ChainValidationResult error)
        {
            mFilter.IgnorableServerCertificateErrors.Add(error);
            return this;
        }

        /// <summary>
        /// Set a list of ignorable server certificate errors
        /// </summary>
        /// <param name="error">a list of <see cref="ChainValidationResult"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest IgnoreServerCertificateErrors(IList<ChainValidationResult> errors)
        {
            mFilter.IgnorableServerCertificateErrors.Concat(errors);
            return this;
        }

        /// <summary>
        /// Set cache write behavior
        /// </summary>
        /// <param name="behavior"><see cref="HttpCacheWriteBehavior"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest CacheWriteBehavior(HttpCacheWriteBehavior behavior = HttpCacheWriteBehavior.Default)
        {
            mFilter.CacheControl.WriteBehavior = behavior;
            return this;
        }

        /// <summary>
        /// Set max connections per server
        /// </summary>
        /// <param name="n">max connections</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest MaxConnectionsPerServer(uint n)
        {
            mFilter.MaxConnectionsPerServer = n;
            return this;
        }

        /// <summary>
        /// Set max HTTP version
        /// </summary>
        /// <param name="ver"><see cref="HttpVersion"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest MaxHTTPVersion(HttpVersion ver)
        {
            mFilter.MaxVersion = ver;
            return this;
        }

        /// <summary>
        /// Set proxy credential
        /// </summary>
        /// <param name="credential"><see cref="PasswordCredential"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest ProxyCredential(PasswordCredential credential)
        {
            mFilter.ProxyCredential = credential;
            return this;
        }

        /// <summary>
        /// Set server credential
        /// </summary>
        /// <param name="credential"><see cref="ServerCredential "/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest ServerCredential(PasswordCredential credential)
        {
            mFilter.ServerCredential = credential;
            return this;
        }

        /// <summary>
        /// Set proxy used
        /// </summary>
        /// <param name="use">is used</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest UseProxy(bool use)
        {
            mFilter.UseProxy = use;
            return this;
        }

        TypedEventHandler<HttpBaseProtocolFilter, HttpServerCustomValidationRequestedEventArgs> customValidationHandler = null;
        public CXRequest ServerCustomValidationRequested(TypedEventHandler<HttpBaseProtocolFilter, HttpServerCustomValidationRequestedEventArgs> handler)
        {
            if (customValidationHandler != null)
                mFilter.ServerCustomValidationRequested -= customValidationHandler;
            customValidationHandler = handler;
            mFilter.ServerCustomValidationRequested += customValidationHandler;
            return this;
        }

        /// <summary>
        /// Set URL
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Url(string url)
        {
            Clean();
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
            this.mHeaders.Add(key, value);
            return this;
        }

        /// <summary>
        /// Concatenate header collection
        /// </summary>
        /// <param name="headers"><see cref="HttpRequestHeaderCollection"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Headers(HttpRequestHeaderCollection headers)
        {
            this.mHeaders.Concat(headers);
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
        /// Set custom HTTP content (data)
        /// </summary>
        /// <param name="data"><see cref="IHttpContent"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Data(IHttpContent data)
        {
            mHttpContent = data;
            return this;
        }

        /// <summary>
        /// Set cookie
        /// </summary>
        /// <param name="cookie"><see cref="HttpCookie"/></param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest Cookie(HttpCookie cookie)
        {
            mFilter.CookieManager.SetCookie(cookie);
            return this;
        }

        /// <summary>
        /// Set if clear cookies before request
        /// </summary>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest ClearCookies()
        {
            isClearCookies = true;
            return this;
        }

        /// <summary>
        /// Clear cookies of specific URL string
        /// </summary>
        /// <param name="url">URL string</param>
        /// <returns><see cref="CXRequest"/></returns>
        public CXRequest ClearCookies(string url)
        {
            isClearCookies = true;
            ClearCookies(new Uri(url));
            isClearCookies = false;
            return this;
        }

        /// <summary>
        /// Clear cookies of specific URI
        /// </summary>
        /// <param name="uri"><see cref="System.Uri"/></param>
        private void ClearCookies(Uri uri)
        {
            if (isClearCookies)
            {
                foreach (HttpCookie cookie in mFilter.CookieManager.GetCookies(uri))
                {
                    mFilter.CookieManager.DeleteCookie(cookie);
                }
            }
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
        /// Make a GET request
        /// </summary>
        /// <returns><c>Task&lt;Response&gt;</c></returns>
        public async Task<CXResponse> Get()
        {
            string tempUrl = url;
            if (mHttpContent != null)
            {
                tempUrl += "?" + await mHttpContent.ReadAsStringAsync().AsTask(cts.Token).ConfigureAwait(false);
            }
            else if (rawData != "")
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
            ClearCookies(uri);
            HttpResponseMessage res = await mHttpClient.GetAsync(uri).AsTask(cts.Token).ConfigureAwait(false);
            return new CXResponse(res, mFilter.CookieManager.GetCookies(uri));
        }

        /// <summary>
        /// Make a POST request
        /// </summary>
        /// <returns><c>Task&lt;Response&gt;</c></returns>
        public async Task<CXResponse> Post()
        {
            IHttpContent content;
            if (mHttpContent != null)
            {
                content = mHttpContent;
            }
            else if (rawData != "")
            {
                content = new HttpStringContent(rawData);
            }
            else
            {
                content = new HttpFormUrlEncodedContent(data);
            }
            Uri uri = new Uri(url);
            ClearCookies(uri);
            HttpResponseMessage res = await mHttpClient.PostAsync(uri, content).AsTask(cts.Token).ConfigureAwait(false);
            return new CXResponse(res, mFilter.CookieManager.GetCookies(uri));
        }

        /// <summary>
        /// Make a DELETE request
        /// </summary>
        /// <returns><c>Task&lt;Response&gt;</c></returns>
        public async Task<CXResponse> Delete()
        {
            Uri uri = new Uri(url);
            ClearCookies(uri);
            HttpResponseMessage res = await mHttpClient.DeleteAsync(uri).AsTask(cts.Token).ConfigureAwait(false);
            return new CXResponse(res, mFilter.CookieManager.GetCookies(uri));
        }

        /// <summary>
        /// Make a PUT request
        /// </summary>
        /// <returns><c>Task&lt;Response&gt;</c></returns>
        public async Task<CXResponse> Put()
        {
            IHttpContent content;
            if (mHttpContent != null)
            {
                content = mHttpContent;
            }
            else if (rawData != "")
            {
                content = new HttpStringContent(rawData);
            }
            else
            {
                content = new HttpFormUrlEncodedContent(data);
            }
            Uri uri = new Uri(url);
            ClearCookies(uri);
            HttpResponseMessage res = await mHttpClient.PutAsync(uri, content).AsTask(cts.Token).ConfigureAwait(false);
            return new CXResponse(res, mFilter.CookieManager.GetCookies(uri));
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (mFilter != null)
            {
                mFilter.Dispose();
                mFilter = null;
            }

            if (mHttpClient != null)
            {
                mHttpClient.Dispose();
                mHttpClient = null;
            }

            if (cts != null)
            {
                cts.Dispose();
                cts = null;
            }
        }
    }
}
