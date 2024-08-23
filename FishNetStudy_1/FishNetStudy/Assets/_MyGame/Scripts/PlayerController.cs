using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
public class PlayerController : NetworkBehaviour
{
    public float rotateSpeed = 2f;
    public float moveSpeed = 10f;

    public GameObject playerCamera;
    public GameObject BulletPrefab;
    public Transform FirePoint;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
            return;
        playerCamera.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!base.IsOwner)
            return;

        float h = Input.GetAxis("Horizontal");
        transform.Rotate(0, h * rotateSpeed, 0);

        float v = Input.GetAxis("Vertical");
        transform.position += transform.forward * v * moveSpeed * Time.deltaTime;
    
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    [ServerRpc]
    private void Fire()
    {
        GameObject go = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        ServerManager.Spawn(go);
    }
}
