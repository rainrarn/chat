using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.AI;
public class NetPlayerObject : NetworkBehaviour
{
    [Header("Components")]
    public NavMeshAgent NavAgent_Player;
    public Animator Animator_Player;
    public Transform Transform_Player;
    
    // 플레이어의 준비 상태
    [SyncVar]
    public bool isReady = false;

    private void Update()
    {
        if (CheckIsFocusedOnUpdate() == false)
            return;

        CheckIsLocalPlayerOnUpdate();
    }

    private bool CheckIsFocusedOnUpdate()
    {
        return Application.isFocused;
    }

    //이동
    private void CheckIsLocalPlayerOnUpdate()
    {
        if (isLocalPlayer == false)
            return;

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 moveVec = new Vector3(-hor, 0, -ver).normalized;

        transform.position += moveVec * NavAgent_Player.speed * Time.deltaTime;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        NavAgent_Player.velocity = forward * Mathf.Max(ver, 0) * NavAgent_Player.speed;
        Animator_Player.SetBool("isrun", moveVec != Vector3.zero);

        transform.LookAt(transform.position + moveVec);
    }

    // 준비 버튼을 클릭했을 때 호출되는 메서드
    [Command]
    public void CmdSetReady(bool ready)
    {
        isReady = ready;
        GameManager.Instance.CheckAllPlayersReady(); // 모든 플레이어가 준비되었는지 확인
    }
}