using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Web.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http.Filters;

namespace CXHttpNS
{
    public class CXSession
    {

        public static Dictionary<string, CXSession> sessions = new Dictionary<string, CXSession>();

        string id;
        private HttpBaseProtocolFilter filter;
        private HttpClient httpClient;

        public readonly CXRequest req;

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
            req = new CXRequest(filter, httpClient);
        }

        public void ClearAuthenticationCache()
        {
            filter.ClearAuthenticationCache();
        }
    }
}
