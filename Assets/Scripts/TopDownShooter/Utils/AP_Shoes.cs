using UnityEngine;

namespace TDS
{
    public class AP_Shoes : AbstractPickup
    {

        public float AmountToIncrease = 2f;
        public override void PickUp(GameObject obj, Item item)
        {
            base.PickUp(obj, item);

            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<Player>().IncreaseSpeed(AmountToIncrease);
            }

        }
    }

}
