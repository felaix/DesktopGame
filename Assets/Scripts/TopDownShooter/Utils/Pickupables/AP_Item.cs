using UnityEngine;

namespace TDS
{
    public class AP_Item : Collectable
    {
        public Stats StatToIncrease = new Stats();
        public string CollectText = "";
        public override void PickUp(GameObject obj, Item item)
        {
            base.PickUp(obj, item);

            if (obj.CompareTag("Player"))
            {
                Player player = obj.GetComponent<Player>();

                //CanvasManager.Instance.CreateItemIcon(item);
                CanvasManager.Instance.CreatePlayerTMP(CollectText);

                player.IncreaseStats(StatToIncrease);


                //if (item.ItemData.ItemType == ItemType.Shoes) player.IncreaseSpeed(AmountToIncrease);
                //if (item.ItemData.ItemType == ItemType.Heart) player.Shield((int)AmountToIncrease);
                //if (item.ItemData.ItemType == ItemType.Life) { player.IncreaseMaxHP((int)AmountToIncrease); player.Heal(99); }
                //if (item.ItemData.ItemType == ItemType.BulletUpgrade) { player.IncreaseBullets((int)AmountToIncrease); }
            }

        }
    }

}
