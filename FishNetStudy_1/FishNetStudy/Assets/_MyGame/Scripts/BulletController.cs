using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
public class BulletController : NetworkBehaviour
{
    private Rigidbody rig;
    public float force = 500;
    public float lifeTime = 5f;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        rig.AddForce(transform.forward * force);
    }

    // Update is called once per frame
    void Update()
    {
        if(base.IsServer)
        {
            lifeTime -= Time.deltaTime;
            if(lifeTime < 0)
            { 
                Destroyself(); 
            }    
        }
    }
    [Server]
    private void Destroyself()
    {
        Despawn();
    }
    [Server]
    private void OnTriggerEnter(Collider other)
    {
        Despawn();
    }
}
