using System.Collections.Generic;
using System.Threading.Tasks;
using BruteForce.Core.Storage;

namespace BruteForce.Tests.Core.Support
{
    public class InMemoryStorage : IStorage
    {
        public List<ResultRecord> Storage { get; } = new List<ResultRecord>();
        
        public Task AppendRecordAsync(ResultRecord record)
        {
            Storage.Add(record);
            return Task.CompletedTask;
        }

        public Task AppendRecordsAsync(IEnumerable<ResultRecord> records)
        {
            Storage.AddRange(records);
            return Task.CompletedTask;
        }
    }
}