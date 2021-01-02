namespace BruteForce.Core.Storage
{
    /// <summary>
    /// Record saved in some storage for further analysis.
    /// </summary>
    public record ResultRecord
    {
        public ResultRecord(string id, GenericResponse response)
        {
            Id = id;
            Response = response;
        }

        /// <summary>
        /// An identifier that uniquely identify the record
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Raw response from the responder
        /// </summary>
        public GenericResponse Response { get; set; }
    }
}