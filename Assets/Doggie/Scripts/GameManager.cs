using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;


    public Text countdownText; // ī��Ʈ�ٿ� �ؽ�Ʈ�� UI�� �߰�
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
            // ���� �غ���� ���� �÷��̾ ����
            if (!player.isReady)
            {
                if (countdownCoroutine != null)
                {
                    StopCoroutine(countdownCoroutine);
                    countdownCoroutine = null;
                    RpcStopCountdown();
                }
                return;
            }
        }
        if (countdownCoroutine == null)
        {
            countdownCoroutine = StartCoroutine(StartCountdown());
        }
    }


    private IEnumerator StartCountdown()
    {
        int countdown = 5;
        while (countdown > 0)
        {
            RpcUpdateCountdown(countdown);
            yield return new WaitForSeconds(1);
            countdown--;
        }

        RpcStartGame();
    }


    [ClientRpc]
    private void RpcUpdateCountdown(int countdown)
    {
        countdownText.text = countdown.ToString();
        countdownText.gameObject.SetActive(true);
    }

    [ClientRpc]
    private void RpcStopCountdown()
    {
        countdownText.gameObject.SetActive(false);
    }
    [ClientRpc]
    private void RpcStartGame()
    {
        
        Debug.Log("All players are ready. Starting the game!");
        
        StartOXGame();
    }

    private void StartOXGame()
    {
        
    }
}
