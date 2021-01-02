namespace BruteForce.Core
{
    /// <summary>
    /// Generic response code for a brute force attempt
    /// </summary>
    public class GenericResponse
    {
        /// <summary>
        /// Generic text of the response, for HTTP is the response of the request.
        /// </summary>
        public string Content { get; private set; }

        public GenericResponse(string content)
        {
            Content = content;
        }
    }
}