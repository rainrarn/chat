using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

    public Text countdownText;
    public Text questionText;
    public Text answerText;

    [System.Serializable]
    public class QuestionAnswerSet
    {
        public string question;
        public bool correctAnswerIsO; // true�� O�� ����, false�� X�� ����
    }

    public List<QuestionAnswerSet> questionAnswerSets;

    private List<QuestionAnswerSet> remainingQuestions;
    private QuestionAnswerSet currentSet;
    private Coroutine countdownCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // ������ ���� �߰��մϴ�.
        questionAnswerSets.Add(new QuestionAnswerSet { question = "�η��� ������ ���̷�� �ұ��̴�?", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "���Է� ������ �� �������� ���� ���ſ� �������� �����̴�.", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "�ΰ��� ���� ���Դ� ��� 2kg �̻��̴�", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "�ȵ�θ޴� ���ϴ� �츮 ���Ͽ� ���� ����� �����̴�", correctAnswerIsO = true });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "������ �ι�°�� ū ����� �����̴�", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "�ٴ��� ��ռ����� �� 16���̴�.", correctAnswerIsO = true });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "��ī�ݶ� ����� �� �ñ�� 19�����̴�", correctAnswerIsO = true });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "���迡�� ���� �� ���� �Ƹ������̴�.", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "���̺�� �����̴�.", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "�ڻԼҴ� �Һ��� ���� ������.", correctAnswerIsO = true });

        remainingQuestions = new List<QuestionAnswerSet>(questionAnswerSets);
    }

    public void CheckAllPlayersReady()
    {
        foreach (NetPlayerObject player in FindObjectsOfType<NetPlayerObject>())
        {
            if (!player.isReady)
            {
                if (countdownCoroutine != null)
                {
                    StopCoroutine(countdownCoroutine);
                    countdownCoroutine = null;
                    RpcStopCountdown();
                }
                return; // ���� �غ���� ���� �÷��̾ ����
            }
        }

        if (countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(StartReadyCountdown());
        }
    }

    private IEnumerator StartReadyCountdown()
    {
        int countdown = 5; // �غ� ī��Ʈ�ٿ� �ð�
        while (countdown > 0)
        {
            RpcUpdateCountdown(countdown, "");
            yield return new WaitForSeconds(1);
            countdown--;
        }

        RpcStopCountdown();
        StartGame();
    }

    private void StartGame()
    {
        // �����ϰ� ������ ���� ��Ʈ ����
        if (remainingQuestions.Count == 0)
        {
            // ��� ������ �����Ǿ��� ��� �ʱ�ȭ
            remainingQuestions = new List<QuestionAnswerSet>(questionAnswerSets);
        }

        int randomIndex = Random.Range(0, remainingQuestions.Count);
        currentSet = remainingQuestions[randomIndex];
        remainingQuestions.RemoveAt(randomIndex);

        RpcShowQuestion(currentSet.question);
        countdownCoroutine = StartCoroutine(StartGameCountdown());
    }

    private IEnumerator StartGameCountdown()
    {
        int countdown = 10; // ���� ī��Ʈ�ٿ� �ð�
        while (countdown > 0)
        {
            RpcUpdateCountdown(countdown, "");
            yield return new WaitForSeconds(1);
            countdown--;
        }

        RpcStopCountdown();
        RpcShowAnswer(currentSet.correctAnswerIsO);
        yield return new WaitForSeconds(3);

        RpcHideQuestionAndAnswer();
        StartGame(); // ���� ������ ����
    }

    [ClientRpc]
    private void RpcUpdateCountdown(int countdown, string prefix)
    {
        countdownText.text = prefix + countdown.ToString();
        countdownText.gameObject.SetActive(true);
    }

    [ClientRpc]
    private void RpcStopCountdown()
    {
        countdownText.gameObject.SetActive(false);
    }

    [ClientRpc]
    private void RpcShowQuestion(string question)
    {
        questionText.text = question;
        questionText.gameObject.SetActive(true);
    }

    [ClientRpc]
    private void RpcShowAnswer(bool correctAnswerIsO)
    {
        answerText.text = correctAnswerIsO ? "���� : O" : "���� : X";
        answerText.gameObject.SetActive(true);

        foreach (NetPlayerObject player in FindObjectsOfType<NetPlayerObject>())
        {
            player.CheckAnswer(correctAnswerIsO);
        }
    }

    [ClientRpc]
    private void RpcHideQuestionAndAnswer()
    {
        questionText.gameObject.SetActive(false);
        answerText.gameObject.SetActive(false);
    }
}
