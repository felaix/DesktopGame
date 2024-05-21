
using TDS;
using UnityEngine;

public class AP_Coin : AbstractPickup
{
    public override void PickUp(GameObject obj)
    {
        base.PickUp(obj);

        TDSManager.Instance.AddCoins(1);
        CanvasManager.Instance.UpdateCoinTMP();

    }
}
