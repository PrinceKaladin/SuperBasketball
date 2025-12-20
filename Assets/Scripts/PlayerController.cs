using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 7f;

    [Header("Jump Cooldown")]
    public float jumpCooldown = 3f;

    [Header("Spawn")]
    public GameObject prefabToSpawn;
    public float spawnInterval = 3f;

    private Rigidbody2D rb;
    private float moveInput;

    private bool canJump = true;

    private Vector3 originalScale;
    public AudioSource au;

    // 👇 ВАЖНО: ссылка на корутину спавна
    private Coroutine spawnCoroutine;

    void OnEnable()
    {
        var d = FindAnyObjectByType<BallController>();
        if (d != null)
            Destroy(d.gameObject);

        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;

        canJump = true;
        SpawnObject();
    }

    void Update()
    {
        // Поворот персонажа
        if (moveInput > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    // --------------------
    // СЕНСОРНЫЕ КНОПКИ
    // --------------------

    public void MoveLeftDown()
    {
        moveInput = -1f;
        animator.Play("PlayerGo");
    }

    public void MoveRightDown()
    {
        moveInput = 1f;
        animator.Play("PlayerGo");
    }

    public void MoveStop()
    {
        moveInput = 0f;
        animator.Play("PlayerIdle");
    }

    public void JumpButton()
    {
        if (!canJump) return;

        canJump = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        StartCoroutine(JumpCooldownRoutine());

        if (au != null)
            au.Play();
    }

    IEnumerator JumpCooldownRoutine()
    {
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    // --------------------
    // SPAWN
    // --------------------

    public void SpawnObject()
    {
        // ❌ НЕ StopAllCoroutines
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        spawnCoroutine = StartCoroutine(spawnMake());
    }

    IEnumerator spawnMake()
    {
        yield return new WaitForSeconds(spawnInterval);

        if (prefabToSpawn != null)
        {
            Vector2 pos = new Vector2(
                transform.position.x,
                transform.position.y + 50
            );

            Instantiate(prefabToSpawn, pos, Quaternion.identity, transform.parent);
        }
    }
}
