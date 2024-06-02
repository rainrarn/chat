using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mirror;


public class ChattingUI : NetworkBehaviour
{
    [Header("UI")]
    [SerializeField] TextMeshProUGUI Text_ChatHistory;
    [SerializeField] Scrollbar Scrollbar_Chat;
    [SerializeField] TMP_InputField Input_ChatMsg;
    [SerializeField] Button Btn_Send;


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






}
