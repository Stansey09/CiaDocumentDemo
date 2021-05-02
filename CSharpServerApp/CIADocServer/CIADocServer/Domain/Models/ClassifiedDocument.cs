using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace CIADocServer.Domain.Models
{
    [DataContract]
    public class ClassifiedDocument
    {
        public ClassifiedDocument()
        {

        }

        [DataMember]
        public string DocumentTitle { get; set; }

        [DataMember]
        public string DocumentText { get; set; }
    }
}
