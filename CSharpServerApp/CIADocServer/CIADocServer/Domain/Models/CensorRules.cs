using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIADocServer.Domain.Models
{
    /// <summary>
    /// Possible levels of general classification
    /// </summary>
    public enum ClearanceLevel
    {
        Unclassifed,
        Confidential,
        Secret,
        TopSecret,
        SpecialAuthorizationOnly
    }


    /// <summary>
    /// Describes a rule for censoring documents that are served to consumers of the document server
    /// </summary>
    public class CensorRules
    {
        /// <summary>
        /// A unique identifier for the censorship rule, so that it can be retrieved, removed, or modified by a system administrator.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// A list of terms that should be censored if the users access level if not sufficient.
        /// </summary>
        public List<string> ClassifiedTerms { get; set; }

        /// <summary>
        /// If the ClassifiedTerms need to be censored, they will be replaced with this string.
        /// </summary>
        public string ReplacementTerm { get; set; }

        /// <summary>
        /// If this level of clearance is not met or exceeded than the ClassifiedTerms will be censored
        /// </summary>
        public ClearanceLevel MinimumClearance { get; set; }

        /// <summary>
        /// If the user has any of these special authorizations, the ClassifiedTerms will not be censored regardless of MinumumClearance
        /// </summary>
        public List<string> SpecialAuthorizations { get; set; }
    }
}
