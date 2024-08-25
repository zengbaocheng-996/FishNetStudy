using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
public class PlayerMove : NetworkBehaviour
{
    public float Speed = 6f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float H = Input.GetAxis("Horizontal");
        float V = Input.GetAxis("Vertical");
        if (IsOwner) 
        {
            Move(H, V);
            //RpcMove(H, V);
        }
    }
    private void Move(float h, float v)
    {
        transform.Translate(new Vector3(h, 0, v) * Speed * Time.deltaTime);
    }

    [ServerRpc]
    private void RpcMove(float h, float v)
    {
        transform.Translate(new Vector3(h, 0, v) * Speed * Time.deltaTime);
    }
}
