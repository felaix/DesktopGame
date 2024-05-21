using UnityEngine;

namespace TDS
{
    public class AP_Shoes : AbstractPickup
    {

        public float AmountToIncrease = 2f;
        public override void PickUp(GameObject obj)
        {
            base.PickUp(obj);

            if (obj.CompareTag("Player"))
            {
                obj.GetComponent<Player>().IncreaseSpeed(AmountToIncrease);
            }
        }
    }

}
