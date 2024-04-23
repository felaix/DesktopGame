using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] private bool destroyAtStart = true;
    [SerializeField] private float delay = .25f;

    void Start()
    {
        if (destroyAtStart) DestroyThis();
    }

    public void DestroyThis()
    {
        Destroy(gameObject, delay);
    }

}
