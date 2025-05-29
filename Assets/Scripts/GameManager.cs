using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    public Transform miniCharacter;
    public Transform[] checkpoints;
    public float baseTime = 60f; // Starting time per level
    public float minTime = 10f;  // Minimum time allowed

    [Header("UI Elements")]
    public TMP_Text timerText;
    public TMP_Text levelText;
    public GameObject victoryScreen;
    public GameObject failScreen;

    private int currentLevel = 0;
    private float timeLeft;
    private bool levelActive = false;

    private Transform currentCheckpoint;

    private Vector3 miniCharacterStartPoint; 

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        StartLevel(0);
    }

    void Update()
    {
        if (levelActive)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerUI();

            if (timeLeft <= 0)
            {
                levelActive = false;
                StartCoroutine(LevelFailedCoroutine());
            }
        }
    }

    public void StartLevel(int levelIndex)
    {
        currentLevel = levelIndex;

        if (currentLevel >= checkpoints.Length)
        {
            OnGameWon();
            return;
        }

        levelText.text = $"Level {currentLevel + 1}";
        timeLeft = CalculateTimeForLevel(currentLevel);
        levelActive = true;

        currentCheckpoint = checkpoints[currentLevel];
        currentCheckpoint.gameObject.SetActive(true);

        SaveCharacterStartTransform();
        UpdateTimerUI();

        failScreen.SetActive(false);
        victoryScreen.SetActive(false);
    }

    private void SaveCharacterStartTransform()
    {
        miniCharacterStartPoint = miniCharacter.position;
    }

    float CalculateTimeForLevel(int level)
    {
        // Reduce time by 10% each level, clamp at minTime
        return Mathf.Max(baseTime * Mathf.Pow(0.9f, level), minTime);
    }

    void UpdateTimerUI()
    {
        timerText.text = $"Time Left: {timeLeft:F1}s";
    }

    void ResetMiniCharacter()
    {
        miniCharacter.position = miniCharacterStartPoint;
    }

    public void OnCheckpointReached()
    {
        if (!levelActive)
            return;

        levelActive = false;
        currentCheckpoint.gameObject.SetActive(false);
        StartCoroutine(LevelCompleteRoutine());
    }

    IEnumerator LevelCompleteRoutine()
    {
        // Play success effects here (animations, sounds)

        victoryScreen.SetActive(true);
        yield return new WaitForSeconds(1f); // Pause before next level
        StartLevel(currentLevel + 1);
    }

    private IEnumerator LevelFailedCoroutine()
    {
        OnLevelFailed();

        yield return new WaitForSeconds(3f);
        RestartLevel();
    }

    void OnLevelFailed()
    {
        failScreen.SetActive(true);
        // Optionally provide restart button to call RestartLevel()
    }

    public void RestartLevel()
    {
        ResetMiniCharacter();
        failScreen.SetActive(false);
        StartLevel(currentLevel);
    }

    void OnGameWon()
    {
        victoryScreen.SetActive(true);
        levelActive = false;
    }
}
