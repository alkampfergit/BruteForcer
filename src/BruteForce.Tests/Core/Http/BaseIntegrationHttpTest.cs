using System.Threading.Tasks;
using BruteForce.Core;
using BruteForce.Core.Http;
using BruteForce.SampleTestWebapp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NUnit.Framework;

namespace BruteForce.Tests.Core.Http
{
    public class BaseIntegrationHttpTest
    {
        protected readonly TestServer _testServer;
        protected IHttpRequestEngine _testRequestEngine;

        public BaseIntegrationHttpTest()
        {
            var webBuilder = new WebHostBuilder();
            webBuilder.UseStartup<Startup>();
            _testServer = new TestServer(webBuilder);
            _testRequestEngine = new TestRequestEngine(_testServer);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _testServer.Dispose();
        }
        
        protected class TestRequestEngine : IHttpRequestEngine
        {
            private readonly TestServer _testServer;

            public TestRequestEngine(TestServer testServer)
            {
                _testServer = testServer;
            }
            
            public async Task<GenericResponse> GetAsync(string url)
            {
                var result = await _testServer.CreateRequest(url).GetAsync();
                var content = await result.Content.ReadAsStringAsync();
                return new GenericResponse(content);
            }
        }
    }
}