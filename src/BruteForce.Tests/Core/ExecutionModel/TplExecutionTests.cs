using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BruteForce.Core;
using BruteForce.Core.ExecutionModels;
using BruteForce.Core.Storage;
using BruteForce.Tests.Core.Support;
using NSubstitute;
using NSubstitute.Core;
using NUnit.Framework;

namespace BruteForce.Tests.Core.ExecutionModel
{
    [TestFixture]
    public class TplExecutionTests
    {
        [SetUp]
        public void SetUp()
        {
            _responses.Clear();
        }

        [Test]
        public async Task Verify_basic_tpl_capabilities_single_request()
        {
            var requester = CreateGenericRequester();

            var sut = new TplExecution()
                .WithRequester(requester)
                .SetCallback(CallBack)
                .Init();

            await sut.QueueAsync(new Dictionary<string, string>()
            {
                ["key"] = "test"
            });

            await sut.Finish();
            Assert.That(_responses.Select(r => r.Content), Is.EquivalentTo(new string[] {"key/test"}));
        }
        
        [Test]
        public async Task Verify_1000_requests()
        {
            var requester = CreateGenericRequester();

            var sut = new TplExecution()
                .WithRequester(requester)
                .SetCallback(CallBack)
                .Init();

            for (int i = 0; i < 1000; i++)
            {
                await sut.QueueAsync(new Dictionary<string, string>()
                {
                    ["key"] = "test"
                });
            }
            
            await sut.Finish();
            Assert.That(_responses.Count, Is.EqualTo(1000));
        }

        [Test]
        public async Task Storage_is_honored()
        {
            var requester = CreateGenericRequester();
            var storage = new InMemoryStorage();
            
            var sut = new TplExecution()
                .WithRequester(requester)
                .SetCallback(CallBack)
                .WriteToStorage(storage)
                .Init();

            await sut.QueueAsync(new Dictionary<string, string>()
            {
                ["key"] = "test"
            });

            await sut.Finish();
            Assert.That(storage.Storage.Count, Is.EqualTo(1));
        }
        
        [Test]
        public async Task Storage_is_honored_with_multiple_requests()
        {
            var requester = CreateGenericRequester();
            var storage = new InMemoryStorage();
            
            var sut = new TplExecution()
                .WithRequester(requester)
                .SetCallback(CallBack)
                .WriteToStorage(storage)
                .Init();

            for (int i = 0; i < 1094; i++)
            {
                await sut.QueueAsync(new Dictionary<string, string>()
                {
                    ["key"] = "test"
                });
            }
            
            await sut.Finish();
            Assert.That(storage.Storage.Count, Is.EqualTo(1094));
        }
        
        private IRequester CreateGenericRequester()
        {
            var requester = Substitute.For<IRequester>();
            requester
                .RunAsync(Arg.Any<IDictionary<string, string>>())
                .Returns(cinfo =>
                    new GenericResponse(CreateResponse(cinfo)));
            return requester;
        }

        private readonly List<GenericResponse> _responses = new List<GenericResponse>();
        private Task CallBack(GenericResponse arg)
        {
            _responses.Add(arg);
            return Task.CompletedTask;
        }

        private string CreateResponse(CallInfo cinfo)
        {
            var tokens = cinfo.Arg<IDictionary<string, string>>();
            StringBuilder sb = new StringBuilder();
            foreach (var token in tokens)
            {
                sb.AppendLine($"{token.Key}/{token.Value}");
            }

            return sb.ToString().Trim('\r', '\n');
        }
    }
}