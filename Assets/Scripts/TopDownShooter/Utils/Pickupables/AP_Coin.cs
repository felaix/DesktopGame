using TDS;
using UnityEngine;

public class AP_Coin : AbstractPickup
{
    public override void PickUp(GameObject obj, Item item)
    {
        base.PickUp(obj, item);

        TDSManager.Instance.AddCoins(1);
    }
}
