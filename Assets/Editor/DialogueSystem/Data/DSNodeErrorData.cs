using DS.Elements;
using System.Collections.Generic;

namespace DS.Data.Error
{
    public class DSNodeErrorData
    {
        public DSErrorData ErrorData { get; set; }
        public List<DSNode> Nodes { get; set; }

        public DSNodeErrorData() { ErrorData = new DSErrorData(); Nodes = new List<DSNode>(); }
    }

}
