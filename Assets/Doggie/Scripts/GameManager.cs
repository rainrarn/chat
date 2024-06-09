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
    
    private string currentQuestion = "Is the sky blue?";
    private bool correctAnswerIsO = true; // ������ O �������� X �������� ǥ��

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
        RpcShowQuestion(currentQuestion);
        countdownCoroutine = StartCoroutine(StartGameCountdown());
    }

    private IEnumerator StartGameCountdown()
    {
        int countdown = 3; // ���� ī��Ʈ�ٿ� �ð�
        while (countdown > 0)
        {
            RpcUpdateCountdown(countdown, "");
            yield return new WaitForSeconds(1);
            countdown--;
        }

        RpcStopCountdown();
        RpcShowAnswer(correctAnswerIsO);
        yield return new WaitForSeconds(3);

        RpcHideQuestionAndAnswer();
        // ���� ������ �����ϰų� ���� ���¸� �����ϴ� ���� �߰�
        Debug.Log("Game segment ended!");
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
   

