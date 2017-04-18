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
            this.mCookies = cookies;
            cts = new CancellationTokenSource();
        }

        public HttpResponseHeaderCollection headers { get => res.Headers; }
        public HttpCookieCollection cookies { get => mCookies; }

        /// <summary>
        /// Get response body content
        /// </summary>
        /// <returns><c>Task&lt;string&gt;</c></returns>
        public async Task<string> Content(string charset = "")
        {
            if (charset != "")
                res.Content.Headers.ContentType.CharSet = charset;
            else if (res.Content.Headers.ContentType.CharSet == "")
                res.Content.Headers.ContentType.CharSet = "UTF-8";
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
