using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public partial class NetworkingAuthenticator
{
    public void SetPlayerName(string username)
    {
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
    }

    public void OnAuthResponseMessage(AuthResMsg msg)
    {

    }
}
