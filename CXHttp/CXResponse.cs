using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private HttpCookieCollection cookies;

        /// <summary>
        /// Constructor: from <see cref="HttpResponseMessage"/>
        /// </summary>
        /// <param name="res"><see cref="HttpResponseMessage"/></param>
        public CXResponse(HttpResponseMessage res, HttpCookieCollection cookies = null)
        {
            this.res = res;
            this.cookies = cookies;
            cts = new CancellationTokenSource();
        }

        /// <summary>
        /// Get response headers
        /// </summary>
        /// <returns><see cref="HttpResponseHeaderCollection"/></returns>
        public HttpResponseHeaderCollection Headers()
        {
            return res.Headers;
        }

        /// <summary>
        /// Get cookies
        /// </summary>
        /// <returns></returns>
        public HttpCookieCollection Cookies()
        {
            return cookies;
        }

        /// <summary>
        /// Get specific response header
        /// </summary>
        /// <param name="key">Specific header name</param>
        /// <returns>Specific header value</returns>
        public string Header(string key)
        {
            return res.Headers[key];
        }

        /// <summary>
        /// Get response body content
        /// </summary>
        /// <returns><c>Task&lt;string&gt;</c></returns>
        public async Task<string> Content()
        {
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
