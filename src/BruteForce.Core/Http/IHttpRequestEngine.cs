using System.Threading.Tasks;

namespace BruteForce.Core.Http
{
    /// <summary>
    /// In progress.
    /// </summary>
    public interface IHttpRequestEngine
    {
        /// <summary>
        /// Allow performing a simple get request against an url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<GenericResponse> GetAsync(string url);
    }
}