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

    private float timeElapsed;
    public AudioSource au;
    void Awake()
    {
        // Singleton
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        record = PlayerPrefs.GetInt("Record", 0);
        UpdateUI();
    }

    void Update()
    {
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
        timeElapsed += Time.deltaTime;

        int seconds = Mathf.FloorToInt(timeElapsed);

        timerText.text = seconds.ToString();
    }


    public void ResetTimer()
    {
        timeElapsed = 0f;
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
