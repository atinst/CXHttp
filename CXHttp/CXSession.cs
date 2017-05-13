using System.Collections.Generic;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace CXHttpNS
{
    public class CXSession
    {

        public static Dictionary<string, CXSession> sessions = new Dictionary<string, CXSession>();

        string id;
        private HttpBaseProtocolFilter filter;
        private HttpClient httpClient;

        public CXRequest Req { get => new CXRequest(filter, httpClient); }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Optional Session id</param>
        public CXSession(string id = "")
        {
            this.id = id;
            if (id != "")
                sessions.Add(id, this);

            filter = new HttpBaseProtocolFilter();
            httpClient = new HttpClient(filter);
        }

        /// <summary>
        /// Clear authentication cache
        /// </summary>
        public void ClearAuthenticationCache()
        {
            filter.ClearAuthenticationCache();
        }
    }
}
