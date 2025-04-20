using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestionInputManager : MonoBehaviour
{
    public TMP_InputField questionInput; // QuestionInput
    public TMP_InputField correctAnswerInput; // CorrectAnswerInput
    public TMP_InputField wrongAnswer1Input; // WrongAnswer1Input
    public TMP_InputField wrongAnswer2Input; // WrongAnswer2Input
    public Button addQuestionButton; // AddQuestionButton (����� ������ ��� ���������� �������)
    public Button finishInputButton; // FinishInputButton (��� ���������� �����)
    public GameObject questionCanvas; // QuestionCanvas

    public GameFlowManager gameFlowManager;

    private string[] questions = new string[5]; // ��� 5 ������
    private string[][] answers = new string[5][]; // ��� 5 ������, ������ � 3 ��������
    private int[] correctAnswerIndices = new int[5]; // ������� ���������� �������
    private int currentBossIndex = 0; // ������� ����, ��� �������� ������ ������
    private const int REQUIRED_QUESTIONS = 5; // ��������� ����� 5 ��������

    void Awake()
    {
        Debug.Log("QuestionInputManager Awake on " + gameObject.name);
        for (int i = 0; i < REQUIRED_QUESTIONS; i++)
        {
            answers[i] = new string[3]; // ������������� ������� ������� ��� ������� �����
        }
    }

    void Start()
    {
        addQuestionButton.onClick.AddListener(AddQuestion); // ����������� ������ ��� ���������� �������
        finishInputButton.onClick.AddListener(FinishInput); // ����������� ������ ��� ���������� �����
        finishInputButton.interactable = false; // ���������� ������ "��������� ����" ���������
        Debug.Log("QuestionInputManager initialized on " + gameObject.name);
    }

    void AddQuestion()
    {
        if (string.IsNullOrEmpty(questionInput.text) ||
            string.IsNullOrEmpty(correctAnswerInput.text) ||
            string.IsNullOrEmpty(wrongAnswer1Input.text) ||
            string.IsNullOrEmpty(wrongAnswer2Input.text))
        {
            Debug.LogWarning("��������� ��� ����!");
            return;
        }

        if (currentBossIndex >= REQUIRED_QUESTIONS)
        {
            Debug.LogWarning("��� ������� 5 ��������! ������� '��������� ����'.");
            return;
        }

        questions[currentBossIndex] = questionInput.text;
        answers[currentBossIndex][0] = correctAnswerInput.text;
        answers[currentBossIndex][1] = wrongAnswer1Input.text;
        answers[currentBossIndex][2] = wrongAnswer2Input.text;
        correctAnswerIndices[currentBossIndex] = 0;

        Debug.Log($"Question for Boss {currentBossIndex + 1} saved: {questions[currentBossIndex]}, Correct answer: {answers[currentBossIndex][correctAnswerIndices[currentBossIndex]]}");

        currentBossIndex++;
        if (currentBossIndex < REQUIRED_QUESTIONS)
        {
            // ������� ���� ��� ����� ���������� �������
            questionInput.text = "";
            correctAnswerInput.text = "";
            wrongAnswer1Input.text = "";
            wrongAnswer2Input.text = "";
            Debug.Log($"Ready to input question for Boss {currentBossIndex + 1}");
        }

        // ���������� ������ "��������� ����", ������ ���� ������� ����� 5 ��������
        if (currentBossIndex == REQUIRED_QUESTIONS)
        {
            finishInputButton.interactable = true;
            Debug.Log("All 5 questions entered! You can now finish input.");
        }
    }

    void FinishInput()
    {
        if (currentBossIndex < REQUIRED_QUESTIONS)
        {
            Debug.LogWarning($"������� ������ {currentBossIndex} ��������! ����� ������ ����� 5 ��������.");
            return;
        }

        Debug.Log("All questions submitted, starting game.");
        gameFlowManager.OnQuestionSubmitted();
    }

    public void ShowQuestion(int bossIndex, System.Action<string> callback)
    {
        Debug.Log($"ShowQuestion called for Boss {bossIndex + 1}. Accessing QuestionCanvas...");
        if (questionCanvas != null)
        {
            var questionDisplay = questionCanvas.GetComponent<QuestionDisplay>();
            if (questionDisplay != null)
            {
                Debug.Log($"QuestionDisplay found. Showing question for Boss {bossIndex + 1}...");
                questionDisplay.ShowQuestion(questions[bossIndex], answers[bossIndex], callback);
            }
            else
            {
                Debug.LogWarning("QuestionDisplay component not found on QuestionCanvas!");
            }
        }
        else
        {
            Debug.LogWarning("QuestionCanvas is not assigned in QuestionInputManager!");
        }
    }

    public bool CheckAnswer(int bossIndex, string selectedAnswer)
    {
        bool isCorrect = answers[bossIndex][correctAnswerIndices[bossIndex]] == selectedAnswer;
        Debug.Log($"CheckAnswer for Boss {bossIndex + 1}: Selected = {selectedAnswer}, Correct = {answers[bossIndex][correctAnswerIndices[bossIndex]]}, Result = {isCorrect}");
        return isCorrect;
    }

    public string GetQuestion(int bossIndex) => questions[bossIndex];
    public string[] GetAnswers(int bossIndex) => answers[bossIndex];
    public int GetCorrectAnswerIndex(int bossIndex) => correctAnswerIndices[bossIndex];
}