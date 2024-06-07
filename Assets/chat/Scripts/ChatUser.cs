using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class ChatUser : NetworkBehaviour
{
    // SyncVar - ���� ������ ��� Ŭ�� �ڵ� ����ȭ�ϴµ� ����
    // Ŭ�� ���� �����ϸ� �ȵǰ�, �������� �����ؾ� ��
    [SyncVar]
    public string PlayerName;

    // ȣ��Ʈ �Ǵ� ���������� ȣ��Ǵ� �Լ�
    public override void OnStartServer()
    {
        PlayerName = (string)connectionToClient.authenticationData;
    }

    public override void OnStartLocalPlayer()
    {
        var objChatUI = GameObject.Find("ChattingUI");
        if (objChatUI != null)
        {
            var chattingUI = objChatUI.GetComponent<ChattingUI>();
            if (chattingUI != null)
            {
                chattingUI.SetLocalPlayerName(PlayerName);
            }
        }

    }
}
