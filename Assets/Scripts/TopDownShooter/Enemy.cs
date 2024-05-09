using DG.Tweening;
using TDS;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private Animator animator;
    private SpriteRenderer sr;
    [SerializeField] private float speed = 1f;

    [SerializeField] private GameObject deathFX;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (sr == null) Debugger.Instance.CreateErrorLog("SpriteRenderer not found");
        if (animator == null) Debugger.Instance.CreateErrorLog("Animator not found!");
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 directionToPlayer = (player.position - transform.position);
            transform.position += directionToPlayer * speed * Time.smoothDeltaTime;
        }
    }


    private Vector3 GetRandomPositionNearPlayer()
    {

        float randomOffsetX = Random.Range(0, .5f);
        float randomOffsetY = Random.Range(0, .5f);
        Vector3 randomPos = new Vector3(player.position.x + randomOffsetX, player.position.y + randomOffsetY, 1f);

        return randomPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log("trigger entered");

        if (collision.CompareTag("Bullet"))
        {
            Instantiate(deathFX, transform.position, Quaternion.identity);
            Debugger.Instance.CreateLog("Enemy die!");
            sr.DOBlendableColor(Color.red, .1f);
            animator.Play("Enemy_Death");
            Destroy(this.gameObject, .25f);
        }

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>().DealDamage(1);
        }
    }

}
