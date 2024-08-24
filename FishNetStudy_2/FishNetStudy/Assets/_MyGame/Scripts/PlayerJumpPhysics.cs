using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpPhysics : MonoBehaviour
{
    private Rigidbody rig;
    public float force = 300f;
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rig.AddForce(Vector3.up*force);
        }
    }
}
