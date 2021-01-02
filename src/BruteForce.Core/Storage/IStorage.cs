using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BruteForce.Core.Storage
{
    /// <summary>
    /// Simple interface to storage response value of the requests
    /// it can be really interesting to further analysis without the
    /// need to always look at the UI.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Append a single record in the underling storage.
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        Task AppendRecordAsync(ResultRecord record);
        
        /// <summary>
        /// Appends multiple records at once
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        Task AppendRecordsAsync(IEnumerable<ResultRecord> records);
    }
}