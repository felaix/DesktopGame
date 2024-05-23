
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace TDS
{
    public class AP_ItemSpawner : AbstractPickup
    {

        public List<Item> ItemsToSpawn;
        public Vector3 ItemSpawnOffset;
        public Vector3 OffsetModifier = new Vector3(2f, 0f, 0f);

        public override void PickUp(GameObject obj, Item item)
        {
            base.PickUp(obj, item);

            foreach (Item buyable in ItemsToSpawn)
            {
                ItemSpawnOffset += OffsetModifier;
                SpawnManager.Instance.SpawnItem(buyable, transform.position + ItemSpawnOffset + OffsetModifier);
            }

            ItemsToSpawn.Clear();
        }
    }

}
