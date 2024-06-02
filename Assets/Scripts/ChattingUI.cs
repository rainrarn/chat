using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;
using Mirror.Examples.Chat;


public class ChattingUI : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI Text_ChatHistory;
    [SerializeField] Scrollbar Scrollbar_Chat;
    [SerializeField] InputField Input_ChatMsg;
    [SerializeField] Button Btn_Send;

    internal static string _localPlayerName;


    //���� �¸� - ����� �÷��̾�� �̸�
    internal static readonly Dictionary<NetworkConnectionToClient, string> _connectedNameDic = new Dictionary<NetworkConnectionToClient, string>();
    public override void OnStartServer()
    {
        _connectedNameDic.Clear();
    }

    public override void OnStartClient()
    {
        Text_ChatHistory.text = string.Empty;
    }

    [Command(requiresAuthority = false)]
    void CommandSendMsg(string msg, NetworkConnectionToClient sender = null)
    {
        if (!_connectedNameDic.ContainsKey(sender))
        {
            var player = sender.identity.GetComponent<Player>();
            var playerName = player.playerName;
            _connectedNameDic.Add(sender, playerName);
        }

        if (!string.IsNullOrWhiteSpace(msg))
        {
            var senderName = _connectedNameDic[sender];
            OnRecvMessage(senderName, msg.Trim());

        }
    }

    public void RemoveNameOnServerDisconnect(NetworkConnectionToClient conn)
    {
        _connectedNameDic.Remove(conn);
    }

    [ClientRpc]
    void OnRecvMessage(string senderName, string msg)
    {
        string formateMsg = (senderName == _localPlayerName) ?
            $"<color=red>{senderName}:</color>{msg}" :
            $"<color=blue>{senderName}:</color>{msg}";

        AppendMessage(formateMsg);
    }
    //==================[UI]=======================
    void AppendMessage(string msg)
    {
        StartCoroutine(AppendAndScroll(msg));
    }
    IEnumerator AppendAndScroll(string msg)
    {
        Text_ChatHistory.text += msg + "\n";

        yield return null;
        yield return null;

        Scrollbar_Chat.value = 0;
    }
    //=============================================

    public void OnClick_SendMsg()
    {
        var currentChatMsg = Input_ChatMsg.text;
        if(!string.IsNullOrWhiteSpace(currentChatMsg))
        {
            CommandSendMsg(currentChatMsg.Trim());
        }
    }

    public void OnClick_Exit()
    {
        NetworkManager.singleton.StopHost();
    }

    public void OnValueChanged_ToggleButton(string input)
    {
        Btn_Send.interactable = !string.IsNullOrWhiteSpace(input);
    }

    public void OnEndEdit_SendMsg(string input)
    {
        if(Input.GetKeyDown(KeyCode.Return)
            ||Input.GetKeyDown(KeyCode.KeypadEnter)
            ||Input.GetButtonDown("Summit"))
        {
            OnClick_SendMsg();
        }
    }


}
