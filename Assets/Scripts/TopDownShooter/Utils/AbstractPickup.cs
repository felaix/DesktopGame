using DG.Tweening;
using TDS;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public abstract class AbstractPickup : Item
{
    private Transform playerTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            PickUp(collision.gameObject, (Item)this);
        }           

    }
    public virtual void PickUp(GameObject obj, Item item)
    {
        transform.DOMove(playerTransform.position, .5f);
        transform.DOScale(0f, .5f);

        if (item != null) { item.OnPickUp(item); }
        //await Task.Delay(1000);
        if (gameObject != null) Destroy(gameObject, 1f);
    }
}

public abstract class Item : MonoBehaviour
{
    public ItemType itemType;
    public Sprite Sprite;

    public void OnPickUp(Item item) 
    {
        TDSManager.Instance.AddItem(item);

        if (item.itemType == ItemType.Coin)
        {
            CanvasManager.Instance.UpdateCoinTMP();
        }

        if (item.itemType == ItemType.Shoes)
        {
            CanvasManager.Instance.UpdateItems(this);
        }

        if (item.itemType == ItemType.Heart)
        {
            CanvasManager.Instance.UpdateItems(this);
        }
    }
}

public enum ItemType
{
    None,
    Coin,
    Shoes,
    Shotgun,
    Heart,
    Life,
    Explore
} 
