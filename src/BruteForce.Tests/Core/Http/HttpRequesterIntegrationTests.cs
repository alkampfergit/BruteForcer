using System.Collections.Generic;
using System.Threading.Tasks;
using BruteForce.Core;
using BruteForce.Core.Http;
using BruteForce.Core.Http.Concrete;
using NUnit.Framework;

namespace BruteForce.Tests.Core.Http
{
    [TestFixture]
    public class HttpRequesterIntegrationTests :  BaseIntegrationHttpTest
    {
        [TestCase("buongusto", "true")]
        [TestCase("pippo", "false")]
        public async Task Verify_simple_call(string password, string expected)
        {
            var sut = new HttpRequester(
                _testRequestEngine,
                $"/api/stupid-login/login-get?password=${{PASSWORD}}&userName=${{USERNAME}}",
                null);

            var result = await sut.RunAsync(new Dictionary<string, string>()
            {
                ["USERNAME"] = "fred",
                ["PASSWORD"] = password
            });
            Assert.That(result.Content.Contains(expected));
        }
    }
}