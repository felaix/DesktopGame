using UnityEngine;

namespace TDS
{
    public class AP_Item : AbstractPickup
    {
        public float AmountToIncrease = 1;
        public override void PickUp(GameObject obj, Item item)
        {
            base.PickUp(obj, item);

            if (obj.CompareTag("Player"))
            {
                Player player = obj.GetComponent<Player>();

                if (item.itemType == ItemType.Shoes) player.IncreaseSpeed(AmountToIncrease);
                if (item.itemType == ItemType.Heart) player.Shield((int)AmountToIncrease);
                if (item.itemType == ItemType.Life) { player.IncreaseMaxHP((int)AmountToIncrease); player.Heal(99); }
                if (item.itemType == ItemType.BulletUpgrade) { player.IncreaseBullets((int)AmountToIncrease); }
            }

        }
    }

}
