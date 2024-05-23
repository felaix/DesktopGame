using DG.Tweening;
using System.Threading.Tasks;
using TDS;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent (typeof(Collider2D))]
public abstract class AbstractPickup : Item
{
    private Transform playerTransform;
    public bool AutoDestroy = true;

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
        if (AutoDestroy)
        {
            transform.DOMove(playerTransform.position, .5f);
            transform.DOScale(0f, .5f);
            if (gameObject != null) Destroy(gameObject, 1f);
        }

        if (item != null) { item.OnPickUp(item); }
        //await Task.Delay(1000);
    }
}

public abstract class Item : MonoBehaviour
{
    public ItemType itemType;
    public Sprite Sprite;
    public Vector2 SpawnOffset = new(0f, 0f);

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.sprite = Sprite;
        }

        if (TryGetComponent<Transform>(out Transform t))
        {
            t.DOJump(new Vector3((transform.position.x + SpawnOffset.x), (transform.position.y + SpawnOffset.y), 0), .5f, 2, .35f);
        }

        SpawnManager.Instance.AddItem(this);
    }

    private void OnDisable()
    {
        SpawnManager.Instance.RemoveItem(this);
        Destroy(gameObject);
    }

    public void OnPickUp(Item item) 
    {
        TDSManager.Instance.AddItem(item);
        SpawnManager.Instance.RemoveItem(item);

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

        if (item.itemType == ItemType.BulletUpgrade)
        {
            CanvasManager.Instance.UpdateItems(this);
        }

        if (item.itemType == ItemType.Seller)
        {

        }
    }
}

public enum ItemType
{
    None,
    Coin,
    Shoes,
    BulletUpgrade,
    Heart,
    Life,
    Explore,
    Seller
} 
