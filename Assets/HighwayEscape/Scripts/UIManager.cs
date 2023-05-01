using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System;
using SgLib;

public class UIManager : MonoBehaviour
{
    [Header("Object References")]
    public PlayerController playerController;
    public GameObject mainCanvas;
    public GameObject characterSelectionUI;
    public GameObject header;
    public GameObject title;
    public GameObject tutorial;
    public Text score;
    public Text bestScore;
    public Text coinText;
    public GameObject newBestScore;
    public GameObject playBtn;
    public GameObject restartBtn;
    public GameObject menuButtons;
    public GameObject velocityBoard;
    public Text velocityText;
    public GameObject velocityNote;



    Animator scoreAnimator;
    float timeCount;
    float maxSpeed;

    void OnEnable()
    {
        GameManager.GameStateChanged += GameManager_GameStateChanged;
        ScoreManager.ScoreUpdated += OnScoreUpdated;
    }

    void OnDisable()
    {
        GameManager.GameStateChanged -= GameManager_GameStateChanged;
        ScoreManager.ScoreUpdated -= OnScoreUpdated;
    }

    // Use this for initialization
    void Start()
    {
        scoreAnimator = score.GetComponent<Animator>();
        Reset();
        ShowStartUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState == GameState.Playing)
        {
            timeCount += Time.deltaTime;
            if (timeCount > 0.25f)
            {
                velocityText.text = ((int)playerController.currentSpeed).ToString();
                if (maxSpeed < playerController.currentSpeed)
                    maxSpeed = playerController.currentSpeed;
                timeCount = 0;
            }
        }

        score.text = ScoreManager.Instance.Score.ToString();
        bestScore.text = ScoreManager.Instance.HighScore.ToString();
        coinText.text = CoinManager.Instance.Coins.ToString();

        if (Input.GetKeyDown(KeyCode.Space) && GameManager.Instance.GameState == GameState.GameOver)
        {
            RestartGame();
        }
    }

    void GameManager_GameStateChanged(GameState newState, GameState oldState)
    {
        if (newState == GameState.Playing)
        {
            ShowGameUI();
        }
        else if (newState == GameState.GameOver)
        {
            Invoke("ShowGameOverUI", 1.2f);
        }
    }

    void OnScoreUpdated(int newScore)
    {
        scoreAnimator.Play("NewScore");
    }

    void Reset()
    {
        mainCanvas.SetActive(true);
        header.SetActive(false);
        title.SetActive(false);
        tutorial.SetActive(false);
        score.gameObject.SetActive(false);
        newBestScore.SetActive(false);
        playBtn.SetActive(false);
        menuButtons.SetActive(false);
        velocityBoard.SetActive(false);
    }

    public void StartGame()
    {
        GameManager.Instance.StartGame();
    }

    public void EndGame()
    {
        GameManager.Instance.GameOver();
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartGame(0.2f);
    }

    public void ShowStartUI()
    {
        if (PlayerPrefs.GetInt("firstTime", 0) == 0)
        {
            PlayerPrefs.SetInt("firstTime", 1);
        }
        header.SetActive(true);
        title.SetActive(true);
        tutorial.SetActive(true);
        playBtn.SetActive(true);
        restartBtn.SetActive(false);
        menuButtons.SetActive(true);
        velocityBoard.SetActive(false);
    }

    public void ShowGameUI()
    {
        header.SetActive(true);
        title.SetActive(false);
        tutorial.SetActive(false);
        score.gameObject.SetActive(true);
        playBtn.SetActive(false);
        menuButtons.SetActive(false);
        velocityNote.SetActive(false);
        velocityBoard.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        header.SetActive(true);
        title.SetActive(false);
        tutorial.SetActive(false);
        score.gameObject.SetActive(true);
        newBestScore.SetActive(ScoreManager.Instance.HasNewHighScore);
        playBtn.SetActive(false);
        restartBtn.SetActive(true);
        menuButtons.SetActive(true);
        velocityText.text = ((int)maxSpeed).ToString();
        velocityNote.SetActive(true);
    }
}
