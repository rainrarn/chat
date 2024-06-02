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

    // 클라에서 인증 요청 시 불려짐
    public override void OnClientAuthenticate()
    {
    }

    public void OnAuthResponseMessage(AuthResMsg msg)
    {

    }
}
