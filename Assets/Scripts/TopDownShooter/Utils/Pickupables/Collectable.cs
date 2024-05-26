using DG.Tweening;
using System;
using TDS;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public abstract class Collectable : Item
{
    private Transform playerTransform;
    public bool AutoDestroy = true;
    public bool AutoDisable = false;

    private bool _pickedUp = false;

    private void OnEnable()
    {
        _pickedUp = false;
    }

    private void OnDisable()
    {
        _pickedUp = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            if (!_pickedUp) PickUp(collision.gameObject, (Item)this);
        }           
    }

    public virtual void PickUp(GameObject obj, Item item)
    {
        if (_pickedUp) return;
        _pickedUp = true;

        if (item != null) { item.OnPickUp(item); }

        if (!AutoDestroy && !AutoDisable) return;

        if (AutoDestroy)
        {
            transform.DOMove(playerTransform.position, .5f);
            transform.DOScale(0f, .5f);
            if (gameObject != null) Destroy(gameObject, 1f);
        }else if (AutoDisable)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (gameObject != null) gameObject.SetActive(false);
        }

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
        //Destroy(gameObject);
    }

    public void OnPickUp(Item item) 
    {
        TDSManager.Instance.AddItem(item);

        // Remove because it isn't in the scene anymore
        SpawnManager.Instance.RemoveItem(item);

        // UI Animation in the right upper corner.
        CanvasManager.Instance.UpdateItems(this);
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
