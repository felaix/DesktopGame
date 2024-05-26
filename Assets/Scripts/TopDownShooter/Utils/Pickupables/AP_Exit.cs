using System.Collections.Generic;
using TDS;
using UnityEngine;

public class AP_Exit : Collectable
{

    [SerializeField] private List<GameObject> _objToDeactivate;
    [SerializeField] private List<GameObject> _objToActivate;
    public bool StopGameOnPickUp;
    public override void PickUp(GameObject obj, Item item)
    {
        base.PickUp(obj, item);

        if (StopGameOnPickUp) SpawnManager.Instance.StopGame();

        foreach (GameObject obj2 in _objToDeactivate)
        {
            obj2.SetActive(false);
        }

        foreach (GameObject obj3 in _objToActivate)
        {
            obj3.SetActive(true);
        }

    }
}
