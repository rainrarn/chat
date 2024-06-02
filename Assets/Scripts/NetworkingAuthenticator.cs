using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkingAuthenticator : NetworkAuthenticator
{
    public struct AuthReqMsg : NetworkMessage
    {
        // ������ ���� ���
        // OAuth ���� �� ��� �� �� �κп� ������ ��ū ���� ������ �߰��ϸ� ��
        public string authUserName;
    }

    public struct AuthResMsg : NetworkMessage
    {
        public byte code;
        public string message;
    }

    #region Sereverside
    [UnityEngine.RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {

    }
    public override void OnStartServer()
    {
        //Ŭ��κ��� ���� ��û ó���� ���� �ڵ鷯 ����
        NetworkServer.RegisterHandler<AuthReqMsg>(OnAuthRequestMessage, false);
    }

    public override void OnStopServer()
    {
        NetworkServer.UnregisterHandler<AuthReqMsg>();
    }

    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {
    }

    public void OnAuthRequestMessage(NetworkConnectionToClient conn, AuthReqMsg msg)
    {
    }

    IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        yield return null;
    }


    #endregion
}
