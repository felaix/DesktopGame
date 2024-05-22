using DG.Tweening;
using UnityEngine;

public class ExploreArea : MonoBehaviour
{
    private void OnEnable()
    {
        Debug.Log("Explore area activated");
        transform.DOMove(new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), 1f).SetEase(Ease.InOutBack);
    }
}
