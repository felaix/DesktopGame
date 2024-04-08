using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    public class DSDialogueContainerSO : ScriptableObject
    {
       [field: SerializeField] public string FileName { get; set; }
        [field: SerializeField] public List<DSDialogueSO> Dialogues { get; set; }

        public void Initialize(string fileName)
        {
            FileName = fileName;
            Dialogues = new List<DSDialogueSO>();
        }
    }

}
