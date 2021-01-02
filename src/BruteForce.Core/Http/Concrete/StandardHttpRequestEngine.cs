using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BruteForce.Core.Http.Concrete
{
    public class StandardHttpRequestEngine : IHttpRequestEngine
    {
        public async Task<GenericResponse> GetAsync(string url)
        {
            using var request = new HttpClient();
            var retValue = await request.GetStringAsync(url);
            return new GenericResponse(retValue);
        }
    }
}