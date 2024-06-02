using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public partial class NetworkingAuthenticator
{
    [SerializeField] LoginPopup _loginPopup;

    [Header("Client Username")]
    public string _playerName;

    public void OnInputValueChanged_SetPlayerName(string username)
    {
        _playerName = username;
        _loginPopup.SetUIOnAuthValueChanged();
    }

    public override void OnStartClient()
    {
        NetworkClient.RegisterHandler<AuthResMsg>(OnAuthResponseMessage, false);
    }

    public override void OnStopClient()
    {
        NetworkClient.UnregisterHandler<AuthResMsg>();
    }

    // Ŭ�󿡼� ���� ��û �� �ҷ���
    public override void OnClientAuthenticate()
    {
        NetworkClient.Send(new AuthReqMsg { authUserName = _playerName });
    }

    public void OnAuthResponseMessage(AuthResMsg msg)
    {
        if (msg.code == 100) // ����
        {
            Debug.Log($"Auth Response:{msg.code} {msg.message}");
            ClientAccept();
        }
        else
        {
            Debug.LogError($"Auth Response: {msg.code} {msg.message}");
            NetworkManager.singleton.StopHost();

            _loginPopup.SetUIOnAuthError(msg.message);
        }
    }
}
