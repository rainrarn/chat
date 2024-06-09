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
    private bool correctAnswerIsO = true; // 정답이 O 구역인지 X 구역인지 표시

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
                return; // 아직 준비되지 않은 플레이어가 있음
            }
        }

        if (countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(StartReadyCountdown());
        }
    }

    private IEnumerator StartReadyCountdown()
    {
        int countdown = 5; // 준비 카운트다운 시간
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
        int countdown = 3; // 게임 카운트다운 시간
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
        // 다음 문제를 설정하거나 게임 상태를 변경하는 로직 추가
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
        answerText.text = correctAnswerIsO ? "정답 : O" : "정답 : X";
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
   

