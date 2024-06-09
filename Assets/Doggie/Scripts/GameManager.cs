using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;

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
                return;
            }
        }

        RpcStartGame();
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
