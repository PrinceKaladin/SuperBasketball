using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TMP_Text scoreText;
    public TMP_Text recordText;
    public TMP_Text timerText;

    private int score;
    private int record;

    [Header("Timer")]
    public float startTime = 8f;
    private float timeLeft;
    private bool timerRunning;

    public AudioSource au;
    public LoseManager meneGameOver;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        record = PlayerPrefs.GetInt("Record", 0);
        ResetTimer();
        UpdateUI();
    }


    private void OnEnable()
    {
        ResetTimer();
       
    }
    void Update()
    {
        if (timerRunning)
            UpdateTimer();
    }

    // --------------------
    // SCORE
    // --------------------

    public void AddScore(int amount)
    {
        score += amount;

        if (score > record)
        {
            record = score;
            PlayerPrefs.SetInt("Record", record);
            PlayerPrefs.Save();
        }

        au.Play();
        UpdateUI();
        ResetTimer();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateUI();
    }

    // --------------------
    // TIMER
    // --------------------

    void UpdateTimer()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            timerRunning = false;
            OnTimerEnd();
        }

        timerText.text = Mathf.CeilToInt(timeLeft).ToString();
    }

    public void ResetTimer()
    {
        timeLeft = startTime;
        timerRunning = true;
        timerText.text = Mathf.CeilToInt(timeLeft).ToString();
    }

    void OnTimerEnd()
    {
        meneGameOver.makeMenu(1);
        score = 0;
        UpdateUI();
    }

    // --------------------
    // UI
    // --------------------

    void UpdateUI()
    {
        scoreText.text = score.ToString();
        recordText.text = record.ToString();
    }
}
