using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BruteForce.Core.Http
{
    /// <summary>
    /// Performs a request for brute forcing, it usually has a payload
    /// or a url and should replace TOKENS expressed with ${TOKEN}.
    /// </summary>
    public class HttpRequester : IRequester
    {
        private readonly IHttpRequestEngine _engine;
        private readonly string _url;
        private readonly string _body;

        public HttpMethod Method { get; set; }
        
        /// <summary>
        /// Initialize a new requester.
        /// </summary>
        /// <param name="engine">IoC</param>
        /// <param name="url">Url to brute force</param>
        /// <param name="body">Eventual payload, it is null if you issue a GET request because
        /// it usually has no body</param>
        public HttpRequester(
            IHttpRequestEngine engine,
            string url,
            string body)
        {
            _engine = engine ?? throw new ArgumentNullException(nameof(engine));
            _url = url ?? throw new ArgumentNullException(nameof(url));
            _body = body;
            if (body == null)
            {
                Method = HttpMethod.Get;
            }
        }

        public Task<GenericResponse> RunAsync(IDictionary<string, string> tokens)
        {
            string url = _url;
            foreach (var token in tokens)
            {
                url = url.Replace($"${{{token.Key}}}", token.Value);
            }

            return _engine.GetAsync(url);
        }
    }
}