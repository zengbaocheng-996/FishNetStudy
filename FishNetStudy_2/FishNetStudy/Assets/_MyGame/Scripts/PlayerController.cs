using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;
using System.Drawing;
public class PlayerController : NetworkBehaviour
{
    public float rotateSpeed = 2f;
    public float moveSpeed = 10f;

    public GameObject playerCamera;
    public GameObject BulletPrefab;
    public Transform FirePoint;
    private Animator anim;

    public Transform _groundCheck;
    public float _groundCheckRadius = 0.3f;
    public LayerMask _groundMask;
    public float force = 300f;

    private bool isGrounded = false;
    private Rigidbody rig;

    private bool isWalking;

    public float FireIntervalTime = 0.5f;
    private float _time = 0;

    public GameObject Body;
    public GameObject Leg;
    public GameObject Arm;

    private HealthManager healthManager;
    //*使用SyncVar修改Player颜色
    //[SyncVar(OnChange = nameof(UpdatePlayerColor))]
    //public Color32 PlayerColor;
    //private readonly SyncVar<Color32> PlayerColor = new SyncVar<Color32>();
    /// <summary>
    /// 修改Player的颜色
    /// </summary>
    /// <param name="oldColor"></param>
    /// <param name="newColor"></param>
    /// <param name="asServer"></param>
    //private void UpdatePlayerColor(Color32 oldColor, Color32 newColor, bool asServer)
    //{
    //    Body.GetComponent<Renderer>().material.SetColor("_Color",newColor);
    //    Leg.GetComponent<Renderer>().material.SetColor("_Color",newColor);
    //    Arm.GetComponent<Renderer>().material.SetColor("_Color",newColor);
    //}
    public override void OnStartClient()
    {
        base.OnStartClient();
        rig = GetComponent<Rigidbody>();
        healthManager = GetComponent<HealthManager>();
        //PlayerColor.OnChange += UpdatePlayerColor;
        if (base.IsOwner)
        {
            playerCamera.SetActive(true);
            anim = GetComponent<Animator>();
            //随机初始化Player的颜色
            Color32 _color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255),255);
            SetPlayerColor(_color);
        }
        else
        {
            rig.useGravity = false;
            rig.isKinematic = true;
        }
    }
    [ServerRpc]
    private void SetPlayerColor(Color32 color) //在Server端运行
    {
        //*使用SyncVar修改Player颜色
        //PlayerColor.Value = color;
        //*使用RPC修改Player的颜色
        UpdateClientPlayerColor(color);
    }

    [ObserversRpc(BufferLast =true)]
    private void UpdateClientPlayerColor(Color32 newColor) //在Client端运行
    {
        Body.GetComponent<Renderer>().material.SetColor("_Color", newColor);
        Leg.GetComponent<Renderer>().material.SetColor("_Color",newColor);
        Arm.GetComponent<Renderer>().material.SetColor("_Color",newColor);
    }

    // Update is called once per frame
    void Update()
    {
        if (!base.IsOwner || healthManager.isDie)
            return;

        _time += Time.deltaTime;
        isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundMask);
        anim.SetBool("Jump", !isGrounded);

        float h = Input.GetAxis("Horizontal");
   

        float v = Input.GetAxis("Vertical");
        if (v > 0) {
            isWalking = true;
            anim.SetBool("Walk",true);
            transform.position += transform.forward * v * moveSpeed * Time.deltaTime;
        }
        else
        {
            isWalking = false;
            anim.SetBool("Walk", false);
        }
        if (h != 0f)
        {
            anim.SetBool("Walk", true);
            transform.Rotate(0, h * rotateSpeed, 0);
        }
        else
        {
            if(isWalking==false)
                anim.SetBool("Walk", false);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(isGrounded==true)
                Jump();
        }
        if(Input.GetMouseButton(0))
        {
            if(_time > FireIntervalTime)
            {
                Fire();
                _time = 0;
            }           
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Color32 _color = new Color32((byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255), (byte)UnityEngine.Random.Range(0, 255),255);
            SetPlayerColor(_color);
        }
    }
    private void Jump()
    {
        rig.AddForce(Vector3.up * force);
    }
    [ServerRpc]
    private void Fire()
    {
        GameObject go = Instantiate(BulletPrefab, FirePoint.position, FirePoint.rotation);
        ServerManager.Spawn(go);
    }
}
