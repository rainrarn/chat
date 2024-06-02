using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LoginPopup : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    internal TMP_InputField Input_NetworkAddress;
    [SerializeField]
    internal TMP_InputField Input_UserName;

    [SerializeField]
    internal Button Btn_StartAsHostServer;
    [SerializeField]
    internal Button Btn_StartAsClient;

    [SerializeField]
    internal TextMeshProUGUI Text_Error;

    public static LoginPopup instance { get; private set; }


    private void Awake()
    {
        instance = this;
    }

}
