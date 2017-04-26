using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace CXHttpNS
{
    public class CXResponse
    {
        private CancellationTokenSource cts;
        private HttpResponseMessage res;
        public HttpCookieCollection mCookies;

        /// <summary>
        /// Constructor: from <see cref="HttpResponseMessage"/>
        /// </summary>
        /// <param name="res"><see cref="HttpResponseMessage"/></param>
        public CXResponse(HttpResponseMessage res, HttpCookieCollection cookies = null)
        {
            this.res = res;
            mCookies = cookies;
            cts = new CancellationTokenSource();
        }

        public HttpResponseHeaderCollection Headers { get { return res.Headers; } }
        public HttpCookieCollection Cookies { get { return mCookies; } }

        /// <summary>
        /// Get response body content
        /// </summary>
        /// <returns><c>Task&lt;string&gt;</c></returns>
        public async Task<string> Content(string charset = "")
        {
            if (charset != "") {
                if (res.Content.Headers.ContentType == null)
                {
                    res.Content.Headers.ContentType = new HttpMediaTypeHeaderValue("text/plain");
                }
                res.Content.Headers.ContentType.CharSet = charset;
            }
            return await res.Content.ReadAsStringAsync().AsTask(cts.Token);
        }

        /// <summary>
        /// Cancel reading content
        /// </summary>
        public void Cancel()
        {
            cts.Cancel();
            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (cts != null)
            {
                cts.Dispose();
                cts = null;
            }
        }
    }
}
