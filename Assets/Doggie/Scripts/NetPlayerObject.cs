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

        Vector3 moveVec = new Vector3(-hor, 0, -ver).normalized;

        transform.position += moveVec * NavAgent_Player.speed * Time.deltaTime;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        NavAgent_Player.velocity = forward * Mathf.Max(ver, 0) * NavAgent_Player.speed;
        Animator_Player.SetBool("isrun", moveVec != Vector3.zero);

        transform.LookAt(transform.position + moveVec);


        //if (isLocalPlayer == false)
        //    return;

        //// 회전
        //float horizontal = Input.GetAxis("Horizontal");
        //transform.Rotate(0, horizontal * _rotationSpeed * Time.deltaTime, 0);

        //// 이동
        //float vertical = Input.GetAxis("Vertical");
        //Vector3 forward = transform.TransformDirection(Vector3.forward);
        //NavAgent_Player.velocity = forward * Mathf.Max(vertical, 0) * NavAgent_Player.speed;
        //Animator_Player.SetBool("Moving", NavAgent_Player.velocity != Vector3.zero);

        //RotateLocalPlayer();

    }

    //void RotateLocalPlayer()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out RaycastHit hit, 100))
    //    {
    //        Debug.DrawLine(ray.origin, hit.point);
    //        Vector3 lookRotate = new Vector3(hit.point.x, Transform_Player.position.y, hit.point.z);
    //        Transform_Player.LookAt(lookRotate);
    //    }
    //}
}