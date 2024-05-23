using DG.Tweening;
using System.Threading.Tasks;
using TDS;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private Animator animator;
    private SpriteRenderer sr;
    [SerializeField] private float speed = 20f;

    [SerializeField] private GameObject deathFX;
    [SerializeField] private Vector3 radius;

    [SerializeField] private Item _dropItem;
    private bool canMove = true;

    private void Start()
    {
        GetPlayer();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        if (sr == null) Debugger.Instance.CreateErrorLog("SpriteRenderer not found");
        if (animator == null) Debugger.Instance.CreateErrorLog("Animator not found!");
    }

    private async void GetPlayer()
    {
        await Task.Delay(100);
        player = SpawnManager.Instance.GetPlayer().transform;
    }

    private void LateUpdate()
    {
        if (player != null && DistanceToPlayer() > -2f && canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, GetRandomPositionNearPlayer(), (speed * Time.deltaTime));
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private float DistanceToPlayer() => Vector3.Distance(transform.position, player.position);

    private Vector3 GetRandomPositionNearPlayer()
    {

        if (player == null) GetPlayer();

        float randomOffsetX = Random.Range(0, radius.x);
        float randomOffsetY = Random.Range(0, radius.y);
        Vector3 randomPos = new Vector3(player.position.x + randomOffsetX, player.position.y + randomOffsetY, 0f);

        return randomPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log("trigger entered");

        if (collision.CompareTag("Bullet"))
        {
            Death();
        }

        else if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>()?.DealDamage(1);
            Death();
        }
    }

    private void DropItem()
    {

        SpawnManager.Instance.SpawnItem(_dropItem, transform.position);
        //Transform item = Instantiate(_dropItem, transform.position, Quaternion.identity, transform.GetComponentInParent<Transform>()).transform;
        //item.DOJump(new Vector3((transform.position.x + .1f), (transform.position.y + .1f), 0),.5f, 2, .35f);
    }

    public void Death()
    {
        SpawnManager.Instance.OnEnemyDeath(this);

        DropItem();

        canMove = false;

        CreateDeathFX();

        //sr.DOBlendableColor(Color.red, .1f);
        sr.DOBlendableColor(Color.white, .2f);

        if (animator != null) animator.Play("Enemy_Death");

        this.GetComponent<Collider2D>().enabled = false;

        Destroy(this.gameObject, 1f);
    }

    private void CreateDeathFX() {if (deathFX != null) Instantiate(deathFX, transform.position, Quaternion.identity, transform);
}}
