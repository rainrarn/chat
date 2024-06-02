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
    [SerializeField] TMP_InputField Input_ChatMsg;
    [SerializeField] Button Btn_Send;


    //서버 온리 - 연결된 플레이어들 이름
    internal static readonly Dictionary<NetworkConnectionToClient, string> _connectedNameDic = new Dictionary<NetworkConnectionToClient, string>();
    public override void OnStartServer()
    {
        _connectedNameDic.Clear();
    }

    public override void OnStartClient()
    {
        Text_ChatHistory.text = string.Empty;
    }

    [Command(requiresAuthority =false)]
    void CommandSendMsg(string msg, NetworkConnectionToClient sender =null)
    {
        if(!_connectedNameDic.ContainsKey(sender))
        {
            var player = sender.identity.GetComponent<Player>();
            var playerName = player.playerName;
            _connectedNameDic.Add(sender, playerName);
        }
    }

    public void OnClick_SendMsg()
    {
        var currentChatMsg = Input_ChatMsg.text;
        if(!string.IsNullOrWhiteSpace(currentChatMsg))
        {
            CommandSendMsg(currentChatMsg.Trim());
        }
    }




}
