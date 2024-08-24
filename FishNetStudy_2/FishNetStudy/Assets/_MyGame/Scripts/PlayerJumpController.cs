using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpController : MonoBehaviour
{
    public float Gravity = -9.81f;
    public float StartJumpSpeed = 10;
    private float JumpSpeed;
    private bool IsJumping;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpSpeed = StartJumpSpeed;
            IsJumping = true;
        }
        if (IsJumping == true)
        {
            JumpSpeed = JumpSpeed + Gravity * Time.deltaTime;
            transform.position = transform.position + Vector3.up *JumpSpeed*Time.deltaTime;
            if(transform.position.y<=1)
            {
                transform.position = new Vector3(transform.position.x, 1, transform.position.z);
                IsJumping= false;
            }
        }

    }
}
