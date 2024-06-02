using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkingAuthenticator : NetworkAuthenticator
{
    readonly HashSet<NetworkConnection> _connectionsPendingDisconnect = new HashSet<NetworkConnection>();
    internal static readonly HashSet<string> _playerNames = new HashSet<string>();
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
        //Ŭ�� ���� ��û �޽��� ���� �� ó��
        Debug.Log($"���� ��û : {msg.authUserName}");

        if (_connectionsPendingDisconnect.Contains(conn)) return;

        //������, DB, PlayFab API���� ȣ���� ���� Ȯ��
        if (!_playerNames.Contains(msg.authUserName))
        {
            _playerNames.Add(msg.authUserName);

            //������ ���� ���� Player.OnStartServer �������� ����
            conn.authenticationData = msg.authUserName;

            AuthResMsg authResMsg = new AuthResMsg
            {
                code = 100,
                message = "Auth Sucess"
            };

            conn.Send(authResMsg);
            ServerAccept(conn);
        }
        else
        {

        }
    }

    IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        yield return null;
    }


    #endregion
}
