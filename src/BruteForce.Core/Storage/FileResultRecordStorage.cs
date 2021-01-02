using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BruteForce.Core.Storage
{
    public class FileResultRecordStorage : IStorage
    {
        private readonly string _fileName;

        public FileResultRecordStorage(string fileName)
        {
            _fileName = fileName;
            if (!File.Exists(_fileName))
            {
                using (File.Create(_fileName));
            }
        }
        
        public Task AppendRecordAsync(ResultRecord record)
        {
            var logLine = CreateLogLineFromRecord(record);

            return AppendAsyncToLogfile(logLine);
        }

        public Task AppendRecordsAsync(IEnumerable<ResultRecord> records)
        {
            var logLines = new StringBuilder();
            foreach (var record in records)
            {
                logLines.AppendLine(CreateLogLineFromRecord(record));
            }

            if (logLines.Length > 0)
            {
                logLines.Length--;
            }

            return AppendAsyncToLogfile(logLines.ToString());
        }

        private static string CreateLogLineFromRecord(ResultRecord record)
        {
            return $"{record.Id}: {record.Response.Content}";
        }
        
        private async Task AppendAsyncToLogfile(string logLine)
        {
            await using var file = new FileStream(_fileName, FileMode.Open, FileAccess.Write, FileShare.Read);
            var buffer = Encoding.UTF8.GetBytes(logLine);
            await file.WriteAsync(buffer, 0, buffer.Length);
            return;
        }
    }
}