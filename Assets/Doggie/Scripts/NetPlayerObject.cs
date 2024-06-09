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
    public TextMesh TextMesh_HealthBar;
    public Transform Transform_Player;

    [Header("Movement")]
    public float _moveSpeed = 1.0f;

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
    private void CheckIsLocalPlayerOnUpdate()
    {
        if (isLocalPlayer == false)
            return;

        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");

        Vector3 moveVec = new Vector3(hor, 0, ver).normalized;

        transform.position += moveVec * _moveSpeed * Time.deltaTime;
        Animator_Player.SetBool("isrun", moveVec != Vector3.zero);

        transform.LookAt(transform.position + moveVec);
    }
}
