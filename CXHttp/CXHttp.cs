namespace CXHttpNS
{
    public class CXHttp
    {
        /// <summary>
        /// Construct a request
        /// </summary>
        /// <param name="url">Request URL</param>
        /// <returns><c cref="CXRequest">Request</c></returns>
        public static CXRequest Connect(string url)
        {
            var req = new CXRequest(url);
            return req;
        }

        /// <summary>
        /// Construct a anonymous session
        /// </summary>
        /// <returns><see cref="CXSession"/></returns>
        public static CXSession Session()
        {
            return new CXSession();
        }

        /// <summary>
        /// Construct or get a named session
        /// </summary>
        /// <param name="id">Session id (name)</param>
        /// <returns><see cref="CXSession"/></returns>
        public static CXSession Session(string id)
        {
            if (CXSession.sessions.ContainsKey(id))
                return CXSession.sessions[id];
            return new CXSession(id);
        }

    }
}
