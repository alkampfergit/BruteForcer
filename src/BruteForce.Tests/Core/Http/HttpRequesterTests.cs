using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BruteForce.Core;
using BruteForce.Core.Http;
using NSubstitute;
using NUnit.Framework;

namespace BruteForce.Tests.Core.Http
{
    [TestFixture]
    public class HttpRequesterTests
    {
        [Test]
        public void Verify_null_url_validation()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpRequester(Substitute.For<IHttpRequestEngine>(), null, null));
        }
        
        [Test]
        public void Verify_null_request_engine_validation()
        {
            Assert.Throws<ArgumentNullException>(() => new HttpRequester(null, "http://www.test.com", null));
        }
        
        [Test]
        public void Null_body_force_get_request()
        {
            var sut = new HttpRequester(Substitute.For<IHttpRequestEngine>(), "http://www.test.it", null);
            Assert.That(sut.Method, Is.EqualTo(HttpMethod.Get));
        }
        
        [Test]
        public async Task Basic_get_substitution()
        {
            var httpEngine = Substitute.For<IHttpRequestEngine>();
            var sut = new HttpRequester(
                httpEngine, 
                "http://www.test.it/login.aspx?username=${USERNAME}&password=${PASSWORD}",
                null);

            var tokens = CreateTokens(httpEngine);
            var result = await sut.RunAsync(tokens);
            Assert.That(result.Content, Is.EqualTo("login failed"));
        }

        private static Dictionary<string, string> CreateTokens(IHttpRequestEngine httpEngine)
        {
            var retValue = new GenericResponse("login failed");
            httpEngine
                .GetAsync("http://www.test.it/login.aspx?username=fred&password=smith")
                .Returns(retValue);
            var tokens = new Dictionary<string, string>()
            {
                ["USERNAME"] = "fred",
                ["PASSWORD"] = "smith"
            };
            return tokens;
        }
    }
}