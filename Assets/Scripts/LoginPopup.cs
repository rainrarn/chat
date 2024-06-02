using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Mirror;
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

    private string _originNetworkAddress;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetDefaultNetworkAddress();
    }

    private void OnEnable()
    {
        Input_UserName.onValueChanged.AddListener(OnValueChanged_ToggleButton);
    }

    private void OnDisable()
    {
        Input_UserName.onValueChanged.RemoveListener(OnValueChanged_ToggleButton);
    }
    private void Update()
    {
        CheckNetworkAddressNaledOnUpdate();
    }

    private void SetDefaultNetworkAddress()
    {
        //��Ʈ��ũ �ּ� ���°��, ����Ʈ ����
        if(string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = "localhost";
        }

        //��Ʈ��ũ �ּ� �������� ����� ����� ���� ��Ʈ��ũ �ּ� ����
        _originNetworkAddress = NetworkManager.singleton.networkAddress;
    }

    private void CheckNetworkAddressNaledOnUpdate()
    {
        if(string.IsNullOrWhiteSpace(NetworkManager.singleton.networkAddress))
        {
            NetworkManager.singleton.networkAddress = _originNetworkAddress;
        }

        if(Input_NetworkAddress.text != NetworkManager.singleton.networkAddress)
        {
            Input_NetworkAddress.text = NetworkManager.singleton.networkAddress;
        }
    }

    public void OnValueChanged_ToggleButton(string userName)
    {
        bool isUserNameValid = !string.IsNullOrWhiteSpace(userName);
        Btn_StartAsHostServer.interactable = isUserNameValid;
        Btn_StartAsClient.interactable = isUserNameValid;
    }


}
