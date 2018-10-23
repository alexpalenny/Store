using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Aya.Core.FilterQuery
{
    /// <summary>
    /// Represents a sort expression of DataSource.
    /// </summary>
    [DataContract]
    public class Sort
    {

        public static Sort Ascending(string fieldName)
        {
            return new Sort() { Dir = "asc", Field = fieldName };
        }

        public static Sort Descending(string fieldName)
        {
            return new Sort() { Dir = "desc", Field = fieldName };
        }

        /// <summary>
        /// Gets or sets the name of the sorted field (property).
        /// </summary>
        [DataMember(Name = "field")]
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets the sort direction. Should be either "asc" or "desc".
        /// </summary>
        [DataMember(Name = "dir")]
        public string Dir { get; set; }

        /// <summary>
        /// Converts to form required by Dynamic Linq e.g. "Field1 desc"
        /// </summary>
        public string ToExpression()
        {
            if (Field.Contains(","))
                return string.Join(",", Field.Split(',').Select(f => f + " " + Dir));
            return Field + " " + Dir;
        }
    }
}
