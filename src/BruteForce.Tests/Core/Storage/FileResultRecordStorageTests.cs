using System.IO;
using System.Threading.Tasks;
using BruteForce.Core;
using BruteForce.Core.Storage;
using NUnit.Framework;

namespace BruteForce.Tests.Core.Storage
{
    [TestFixture]
    public class FileResultRecordStorageTests
    {
        private const string FileName = "out.txt";
        
        [SetUp]
        public void Setup()
        {
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
        }

        [Test]
        public async Task Verify_simple_append_log()
        {
            var sut = new FileResultRecordStorage("out.txt");
            await sut.AppendRecordAsync(new ResultRecord("bla", new GenericResponse("result")));

            var content = File.ReadAllText(FileName);
            Assert.That(content, Is.EqualTo("bla: result"));
        }
        
        [Test]
        public async Task Verify_multiple_append_log()
        {
            var sut = new FileResultRecordStorage("out.txt");
            await sut.AppendRecordsAsync(
                new[]
                {
                    new ResultRecord("bla", new GenericResponse("result")),
                    new ResultRecord("foo", new GenericResponse("fores")),
                    new ResultRecord("bar", new GenericResponse("bares")),
                });

            var content = File.ReadAllText(FileName);
            Assert.That(content, Is.EqualTo(
@"bla: result
foo: fores
bar: bares"));
        }
    }
}