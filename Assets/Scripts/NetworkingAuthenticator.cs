using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkingAuthenticator : NetworkAuthenticator
{
#region Sereverside
    [UnityEngine.RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {

    }
    public override void OnStartServer()
    {
    }

    public override void OnStopServer()
    {
    }

    public override void OnServerAuthenticate(NetworkConnectionToClient conn)
    {
    }

    public void OnAuthRequestMessage(NetworkConnectionToClient conn)
    {
    }

    IEnumerator DelayedDisconnect(NetworkConnectionToClient conn, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        yield return null;
    }


    #endregion
}
