using DG.Tweening;
using System;
using TDS;
using TMPro;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public abstract class Collectable : Item
{
    private Transform playerTransform;
    public bool AutoDestroy = true;

    private bool _pickedUp;

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
        if (_pickedUp) return;

        if (AutoDestroy)
        {
            transform.DOMove(playerTransform.position, .5f);
            transform.DOScale(0f, .5f);
            if (gameObject != null) Destroy(gameObject, 1f);
        }

        if (item != null) { item.OnPickUp(item); }
        _pickedUp = true;
        //await Task.Delay(1000);
    }
}

[Serializable]
public struct ItemData
{
    //public ItemType ItemType;
    public Sprite Sprite;
    public int Cost;
}

public abstract class Item : MonoBehaviour
{

    public ItemData ItemData;
    //public ItemType ItemType;
    //public Sprite Sprite;
    public Vector2 SpawnOffset = new(0f, 0f);
    private TMP_Text _costTMP;

    private void Start()
    {
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer sr))
        {
            sr.sprite = ItemData.Sprite;
        }

        if (TryGetComponent<Transform>(out Transform t))
        {
            t.DOJump(new Vector3((transform.position.x + SpawnOffset.x), (transform.position.y + SpawnOffset.y), 0), .5f, 2, .35f);


            if (ItemData.Cost > 0)
            {
                // Get coin TMP
                Transform _canvas = t.GetChild(1);
                if (_canvas == null) { Debug.Log("no _canvas found"); return; }
                _costTMP = _canvas.GetChild(0).GetComponentInChildren<TMP_Text>();
                if (_costTMP == null) {Debug.Log("no _costTMP found"); return; }
                _costTMP.text = ItemData.Cost.ToString();

                _canvas.gameObject.SetActive(true);
            }

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

        CanvasManager.Instance.UpdateItems(this);

        //if (item.ItemData.ItemType == ItemType.Coin)
        //{
        //    CanvasManager.Instance.UpdateCoinTMP();
        //}

        //if (item.ItemData.ItemType == ItemType.Shoes)
        //{
        //    CanvasManager.Instance.UpdateItems(this);
        //}

        //if (item.ItemData.ItemType == ItemType.Heart)
        //{
        //    CanvasManager.Instance.UpdateItems(this);
        //}

        //if (item.ItemData.ItemType == ItemType.BulletUpgrade)
        //{
        //    CanvasManager.Instance.UpdateItems(this);
        //}

        //if (item.ItemData.ItemType == ItemType.Seller)
        //{

        //}
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
