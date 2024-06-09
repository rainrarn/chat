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
        public bool correctAnswerIsO; // true면 O가 정답, false면 X가 정답
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

        // 질문과 답을 추가합니다.
        questionAnswerSets.Add(new QuestionAnswerSet { question = "인류의 최조의 조미료는 소금이다?", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "무게로 따졌을 때 지구에서 가장 무거운 생물군은 곤충이다.", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "인간의 뇌의 무게는 평균 2kg 이상이다", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "안드로메다 은하는 우리 은하와 가장 가까운 은하이다", correctAnswerIsO = true });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "지구상 두번째로 큰 대륙은 남극이다", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "바다의 평균수온은 약 16도이다.", correctAnswerIsO = true });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "코카콜라가 만들어 진 시기는 19세기이다", correctAnswerIsO = true });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "세계에서 가장 긴 강은 아마존강이다.", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "사이비는 영어이다.", correctAnswerIsO = false });
        questionAnswerSets.Add(new QuestionAnswerSet { question = "코뿔소는 소보다 말에 가깝다.", correctAnswerIsO = true });

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
        // 랜덤하게 문제와 정답 세트 선택
        if (remainingQuestions.Count == 0)
        {
            // 모든 문제가 출제되었을 경우 초기화
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
        int countdown = 10; // 게임 카운트다운 시간
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
        StartGame(); // 다음 문제를 출제
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
