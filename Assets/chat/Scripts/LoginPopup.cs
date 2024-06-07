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
    internal InputField Input_UserName;

    [SerializeField]
    internal Button Btn_StartAsHostServer;
    [SerializeField]
    internal Button Btn_StartAsClient;

    [SerializeField]
    internal TextMeshProUGUI Text_Error;

    [SerializeField] NetworkManager _netManager;
    
    public static LoginPopup instance { get; private set; }

    private string _originNetworkAddress;

    private void Awake()
    {
        instance = this;
        Text_Error.gameObject.SetActive(false);
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
    public void SetUIClientDisconnected()
    {
        this.gameObject.SetActive(true);
        Input_UserName.text = string.Empty;
        Input_UserName.ActivateInputField();
    }
    public void SetUIOnAuthValueChanged()
    {
        Text_Error.text = string.Empty;
        Text_Error.gameObject.SetActive(false);
    }
    public void SetUIOnAuthError(string msg)
    {
        Text_Error.text = msg;
        Text_Error.gameObject.SetActive(true);
    }

    public void OnValueChanged_ToggleButton(string userName)
    {
        bool isUserNameValid = !string.IsNullOrWhiteSpace(userName);
        Btn_StartAsHostServer.interactable = isUserNameValid;
        Btn_StartAsClient.interactable = isUserNameValid;

    }

    public void OnClick_StartHost()
    {
        if (_netManager == null)
            return;

        _netManager.StartHost();
        this.gameObject.SetActive(false);
    }

    public void OnClick_StartAsClient()
    {
        if( _netManager == null) 
            return;
        _netManager.StartClient();
        this.gameObject.SetActive(false);
    }
   
}