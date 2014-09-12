using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Render.MobileCore
{
    [DataContract]
    public class AppState
    {
        [DataMember]
        public Guid SavedGuid { get; set; }

        public AppState()
        {
            SavedGuid = Guid.NewGuid();
        }
    }
}
