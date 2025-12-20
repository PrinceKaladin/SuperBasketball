using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("Target")]
    public string basketTag = "Basket";

    [Header("Movement")]
    public float force = 8f;
    public float randomOffset = 1.5f;

    [Header("Stop Check")]
    public float checkDelay = 1f;       
    public float stopVelocity = 0.05f;   

    private Rigidbody2D rb;
    private bool canCheckStop = false;
    private bool stopped = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        FlyToBasket();

        Invoke(nameof(EnableStopCheck), checkDelay);
    }

    void FlyToBasket()
    {
        GameObject basket = GameObject.FindGameObjectWithTag(basketTag);
        if (basket == null) return;

        Vector2 startPos = transform.position;
        Vector2 targetPos = basket.transform.position;

        // горизонтальная случайность
        targetPos.x += Random.Range(-randomOffset, randomOffset);

        // вертикальная случайность (подлёт выше корзины)
        float heightOffset = Random.Range(1f, 2f);
        targetPos.y += heightOffset;

        // расстояние до цели
        Vector2 distance = targetPos - startPos;

        // случайное время полёта для вариативности
        float time = Random.Range(0.6f, 1.2f);

        // расчёт скорости с учётом гравитации
        float vx = distance.x / time;
        float vy = distance.y / time + 0.5f * Mathf.Abs(Physics2D.gravity.y) * rb.gravityScale * time;

        // назначаем скорость
        rb.velocity = new Vector2(vx, vy);
    }




    void EnableStopCheck()
    {
        canCheckStop = true;
    }

    void FixedUpdate()
    {
        if (!canCheckStop || stopped) return;

        if (rb.velocity.magnitude <= stopVelocity)
        {
            stopped = true;
            OnBallStopped();
        }
    }

    void OnBallStopped()
    {

        FindAnyObjectByType<PlayerController>().SpawnObject();
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Basket")
        {
            Debug.Log("Collide");
            OnBallStopped();
            PlusScore();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Basket")
        { Debug.Log("Collide");
            Debug.Log("Collide");
            OnBallStopped();
            PlusScore();
        }


    
    }


    public void PlusScore()
    {
        FindObjectOfType<GameManager>().AddScore(1);
        GameObject.FindGameObjectWithTag("Basket").GetComponent<Animator>().Play("BasketGoal"); ;
    }
}
