using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Data.Save
{
    public class DSGraphSaveDataSO : ScriptableObject
    {
        public string FileName { get; set; }
        public List<DSNodeSaveData> Nodes { get; set; }
        public List<string> OldNodeNames { get; set; }

    }

}
