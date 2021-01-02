using System.Collections.Generic;
using System.Threading.Tasks;

namespace BruteForce.Core
{
    /// <summary>
    /// Generic behavior of a simple requester, a component that is
    /// capable to do a request to a target system and then return
    /// a <see cref="GenericResponse"/>
    /// </summary>
    public interface IRequester
    {
        Task<GenericResponse> RunAsync(IDictionary<string, string> tokens);
    }
}