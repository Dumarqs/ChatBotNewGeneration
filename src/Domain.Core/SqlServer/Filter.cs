namespace Domain.Core.SqlServer
{
    /// <summary>
    /// Properties to filter result
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// How many rows to skip
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// How many takes to take
        /// </summary>
        public int Take { get; set; }
        /// <summary>
        /// Field to Search
        /// </summary>
        public string Field { get; set; }
        /// <summary>
        /// Search
        /// </summary>
        public string Search { get; set; }
    }
}
