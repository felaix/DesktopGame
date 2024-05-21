using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public abstract class AbstractPickup : MonoBehaviour
{

    private Transform playerTransform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            PickUp(collision.gameObject);
        }           

    }
    public virtual void PickUp(GameObject obj)
    {
        transform.DOMove(playerTransform.position, .5f);
        transform.DOScale(0f, .5f);
        //await Task.Delay(1000);
        if (gameObject != null) Destroy(gameObject, 1f);
    }
}
