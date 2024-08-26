using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;
using FishNet.Object.Prediction;

public class Prediction4NetworkTransform : NetworkBehaviour
{
    public float _moveRate = 6f;
    private void Awake()
    {
        InstanceFinder.TimeManager.OnTick += TimeManager_OnTick;
    }

    private void OnDestroy()
    {
        if (InstanceFinder.TimeManager != null)
        {
            InstanceFinder.TimeManager.OnTick -= TimeManager_OnTick;
        }
    }
    private void TimeManager_OnTick()
    {
        if(base.IsOwner)
        {
            Reconciliation(default, false);//调整数据。按照服务器端返回的数据调整
            CheckInput(out MoveData md);//当前客户端输入数据
            Move(md, false);//客户端按照输入移动并将输入传递给服务器端
        }
        if (base.IsServer)
        {
            Move(default, true);//移动服务器端对象
            ReconcileData rd = new ReconcileData(transform.position, transform.rotation);//获得服务器端对象移动后的对象
            Reconciliation(rd, true);//将服务器端对象移动后的数据传递给客户端
        }
    }
    /// <summary>
    /// 客户端输入数据结构，传输给服务端
    /// </summary>
    public struct MoveData
    {
        public Vector2 Movement;
        public float Horizontal;
        public float Vertical;
        private uint _tick;

        public MoveData(float horizontal, float vertical)
        {
            Movement = new Vector2(horizontal, vertical);
            Horizontal = horizontal;
            Vertical = vertical;
            _tick = 0;
        }
    }

    /// <summary>
    /// 服务器端生成的数据结构。是服务器端对象运行后的结果数据。
    /// 数据将会传递回客户端。
    /// </summary>
    public struct ReconcileData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public ReconcileData(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
    private void CheckInput(out MoveData md)
    {
        md = default;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //No input to send.
        if (horizontal == 0f && vertical == 0f)
            return;

        //Make movedata with input.
        md = new MoveData()
        {
            Horizontal = horizontal,
            Vertical = vertical
        };
    }
    [Replicate]
    private void Move(MoveData md, bool asServer, bool replaying = false)
    {
        /* You can check if being run as server to
         * add security checks such as normalizing
         * the inputs. */
        if (asServer)
        {
            //Sanity check!
        }
        /* You may also use replaying to know
         * if a client is replaying inputs rather
         * than running them for the first time. This can
         * be useful because you may only want to run
         * VFX during the first input and not during
         * replayed inputs. */
        if (!replaying)
        {
            //VFX!
        }

        Vector3 move = new Vector3(md.Horizontal, 0f, md.Vertical);
        transform.position += (move * _moveRate * (float)base.TimeManager.TickDelta);
    }
    [Reconcile]
    private void Reconciliation(ReconcileData rd, bool asServer)
    {
        transform.position = rd.Position;
        transform.rotation = rd.Rotation;
    }
}
