using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.Data
{
    public class DSDialogueChoiceData
    {
        [field: SerializeField] public string Text { get; set; }
        [field: SerializeField] public DSDialogueSO NextDialogue { get; set; }
    }

}
