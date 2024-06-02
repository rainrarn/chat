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

    public override void OnStartServer()
    {

    }

    public override void OnStartClient()
    {
        
    }






}
