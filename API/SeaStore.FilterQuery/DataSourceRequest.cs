using System.Collections.Generic;

namespace Aya.Core.FilterQuery
{
    /// <summary>
    /// Describes a Datasource request.
    /// </summary>
    public class FilterRequest
    {
        /// <summary>
        /// Specifies how many items to take.
        /// </summary>
        public int Take { get; set; }

        /// <summary>
        /// Specifies how many items to skip.
        /// </summary>
        public int Skip { get; set; }

        /// <summary>
        /// Specifies the requested sort order.
        /// </summary>
        public IEnumerable<Sort> Sort { get; set; }

        /// <summary>
        /// Specifies the requested filter.
        /// </summary>
        public Filter Filter { get; set; }

        /// <summary>
        /// Specifies the security filters attached by the security service.
        /// </summary>
        public Filter SecurityFilter { get; set; }

        public bool CacheTotal { get; set; }

        public bool ResetCache { get; set; }
    }
}
