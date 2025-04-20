using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameFlowManager : MonoBehaviour
{
    public GameObject welcomeCanvas;
    public GameObject questionInputCanvas;
    public GameObject questionCanvas;
    public GameObject victoryCanvas;
    public GameObject hero1;
    public GameObject[] hearts;
    public GameObject gameOverCanvas;
    public GameObject director;

    private int defeatedBosses = 0;
    private const int TOTAL_BOSSES = 5;
    private int lives = 3;

    void Start()
    {
        Debug.Log("GameFlowManager Start: Initializing canvases...");
        welcomeCanvas.SetActive(true);
        questionInputCanvas.SetActive(false);
        questionCanvas.SetActive(false);
        victoryCanvas.SetActive(false);
        if (gameOverCanvas != null) gameOverCanvas.SetActive(false);
        if (director != null) director.SetActive(false);

        lives = 3;
        UpdateLivesUI();

        if (welcomeCanvas != null)
        {
            Button startButton = welcomeCanvas.GetComponentInChildren<Button>();
            if (startButton != null)
            {
                startButton.onClick.RemoveAllListeners();
                startButton.onClick.AddListener(OnWelcomeContinue);
                Debug.Log("Start button on WelcomeCanvas found and assigned to OnWelcomeContinue.");
            }
            else
            {
                Debug.LogWarning("Start button not found on WelcomeCanvas!");
            }
        }
        else
        {
            Debug.LogWarning("WelcomeCanvas is not assigned in GameFlowManager!");
        }

        if (hero1 != null)
        {
            hero1.SetActive(true);
            Debug.Log("Hero1 activated at start.");
        }
        else
        {
            Debug.LogWarning("Hero1 is not assigned in GameFlowManager!");
        }

        if (questionCanvas != null)
        {
            var qd = questionCanvas.GetComponent<QuestionDisplay>();
            if (qd == null)
            {
                Debug.LogWarning("QuestionCanvas does not have QuestionDisplay component!");
            }
            else
            {
                Debug.Log("QuestionDisplay found on QuestionCanvas at start.");
            }
        }
        else
        {
            Debug.LogWarning("QuestionCanvas is not assigned in GameFlowManager!");
        }
    }

    public void OnWelcomeContinue()
    {
        Debug.Log("OnWelcomeContinue called.");
        Debug.Log($"welcomeCanvas: {(welcomeCanvas != null ? welcomeCanvas.name : "null")}");
        Debug.Log($"questionInputCanvas: {(questionInputCanvas != null ? questionInputCanvas.name : "null")}");

        if (welcomeCanvas != null)
        {
            welcomeCanvas.SetActive(false);
            Debug.Log("WelcomeCanvas deactivated.");
        }
        else
        {
            Debug.LogWarning("WelcomeCanvas is not assigned!");
        }

        if (questionInputCanvas != null)
        {
            questionInputCanvas.SetActive(true);
            Debug.Log("QuestionInputCanvas activated.");
            var qim = questionInputCanvas.GetComponent<QuestionInputManager>();
            if (qim == null)
            {
                Debug.LogWarning("QuestionInputCanvas does not have QuestionInputManager component!");
            }
            else
            {
                Debug.Log("QuestionInputManager found on QuestionInputCanvas.");
            }
        }
        else
        {
            Debug.LogWarning("QuestionInputCanvas is not assigned!");
        }
    }

    public void OnQuestionSubmitted()
    {
        Debug.Log("OnQuestionSubmitted called.");
        if (questionInputCanvas != null)
        {
            questionInputCanvas.SetActive(false);
            Debug.Log("QuestionInputCanvas deactivated.");
        }
        if (questionCanvas != null)
        {
            questionCanvas.SetActive(false);
            Debug.Log("QuestionCanvas set to inactive.");
        }
        Debug.Log("All questions submitted, starting game.");
    }

    public void OnBossDefeated()
    {
        defeatedBosses++;
        Debug.Log($"Boss defeated! Total defeated bosses: {defeatedBosses}/{TOTAL_BOSSES}");

        if (questionCanvas != null)
        {
            questionCanvas.SetActive(false);
            Debug.Log("QuestionCanvas deactivated after boss defeat.");
        }

        if (defeatedBosses >= TOTAL_BOSSES)
        {
            Debug.Log("All bosses defeated! Activating Director.");
            if (director != null)
            {
                director.SetActive(true);
                Debug.Log("Director activated.");
            }
            else
            {
                Debug.LogWarning("Director is not assigned in GameFlowManager!");
            }
        }
        else
        {
            Debug.Log("More bosses to defeat!");
        }
    }

    public void OnDirectorDefeated()
    {
        Debug.Log("Director defeated! Player wins!");
        if (victoryCanvas != null)
        {
            victoryCanvas.SetActive(true);
            AudioManager.Instance.PlayVictorySound(); // Звук победы
            Debug.Log("Showing VictoryCanvas with default text.");
        }
        else
        {
            Debug.LogWarning("VictoryCanvas is not assigned!");
        }
    }

    public void LoseLife()
    {
        lives--;
        Debug.Log($"Player lost a life! Lives remaining: {lives}");
        UpdateLivesUI();

        if (lives <= 0)
        {
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        if (hearts != null && hearts.Length == 3)
        {
            for (int i = 0; i < hearts.Length; i++)
            {
                hearts[i].SetActive(i < lives);
            }
            Debug.Log($"Updated lives UI: Lives: {lives}");
        }
        else
        {
            Debug.LogWarning("Hearts array is not assigned or does not contain exactly 3 elements!");
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Player ran out of lives or failed Director's quiz.");
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
            var gameOverText = gameOverCanvas.GetComponentInChildren<TMP_Text>();
            if (gameOverText != null)
            {
                gameOverText.text = "Вы проиграли";
            }
            Debug.Log("Showing GameOverCanvas.");
        }
        else
        {
            Debug.LogWarning("GameOverCanvas is not assigned! Using VictoryCanvas as fallback.");
            if (victoryCanvas != null)
            {
                victoryCanvas.SetActive(true);
                var victoryText = victoryCanvas.GetComponentInChildren<TMP_Text>();
                if (victoryText != null)
                {
                    victoryText.text = "Вы проиграли";
                }
                Debug.Log("Showing VictoryCanvas with 'Вы проиграли'.");
            }
        }

        if (hero1 != null)
        {
            hero1.SetActive(false);
            Debug.Log("Hero1 deactivated due to Game Over.");
        }
    }
}