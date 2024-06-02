using TDS;
using UnityEngine;

namespace TDS
{
    public class AP_Coin : Collectable
    {
        public override void PickUp(GameObject obj, Item item)
        {
            base.PickUp(obj, item);

            TDSManager.Instance.AddCoins(1);
            TDSCanvasManager.Instance.UpdateCoinTMP();
        }
    }

}
