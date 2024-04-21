using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set;}
    public bool isGameOver;
    private int currentScore;
    private int maxScore;
    private string currentScoreAsString;
    private string maxScoreAsString;

    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text maxScoreText;
    [SerializeField] private Image timerImage;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private Knight knight;
    [SerializeField] private float maxTime;
    private float remainingTime;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        currentScore = 0;
        remainingTime = maxTime;
    }

    void Start()
    {
        isGameOver = false;

        UpdateScoreText();
        GetMaxScore();
    }
    
    void Update()
    {
        if(!isGameOver)
        {
            DecreaseTime();
        }
    }

    public void  AddToScore(int points)
    {
        currentScore  = (int)Mathf.Clamp(currentScore + points, 0, Mathf.Infinity);
        UpdateScoreText();

        if(currentScore > maxScore)
        {
            SetMaxScore();
        }
    }

    public void MultiplySpeedOfKnight(bool isIncreasing, float multipleAmount)
    {
        knight.MultiplySpeed(isIncreasing, multipleAmount);
    }

    public void MultiplySizeOfKnight(bool isIncreasing, float multipleAmount)
    {
        knight.MultiplySize(isIncreasing, multipleAmount);
    }

    private void UpdateScoreText()
    {
        currentScoreAsString = currentScore.ToString();
        scoreText.text = currentScoreAsString;
    }

    private void UpdateMaxScoreText()
    {
        maxScoreAsString = maxScore.ToString();
        maxScoreText.text = maxScoreAsString;
    }

    private void SetMaxScore()
    {
        PlayerPrefs.SetInt("Max Score", currentScore);
        GetMaxScore();
    }

    private void GetMaxScore()
    {
        maxScore = PlayerPrefs.GetInt("Max Score", 0);
        UpdateMaxScoreText();
    }

    private void DecreaseTime()
    {
        remainingTime -= Time.deltaTime;
        timerImage.fillAmount = remainingTime/maxTime;

        if(remainingTime <= 0f)
        {
            isGameOver = true;
            OpenPauseMenu(isGameOver);
        }
    }

    public void OpenPauseMenu(bool isGameOver)
    {
        Time.timeScale = 0f;
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);

        if(isGameOver)
        {
            resumeButton.SetActive(false);
        }
        else
        {
            resumeButton.SetActive(true);
        }

        //TODO Choose Between Sound Buttons Depends On If Its On Or Off
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
    }
}
