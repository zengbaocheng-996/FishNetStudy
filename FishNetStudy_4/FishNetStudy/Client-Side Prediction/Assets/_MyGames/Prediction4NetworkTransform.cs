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
            Reconciliation(default, false);//�������ݡ����շ������˷��ص����ݵ���
            CheckInput(out MoveData md);//��ǰ�ͻ�����������
            Move(md, false);//�ͻ��˰��������ƶ��������봫�ݸ���������
        }
        if (base.IsServer)
        {
            Move(default, true);//�ƶ��������˶���
            ReconcileData rd = new ReconcileData(transform.position, transform.rotation);//��÷������˶����ƶ���Ķ���
            Reconciliation(rd, true);//���������˶����ƶ�������ݴ��ݸ��ͻ���
        }
    }
    /// <summary>
    /// �ͻ����������ݽṹ������������
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
    /// �����������ɵ����ݽṹ���Ƿ������˶������к�Ľ�����ݡ�
    /// ���ݽ��ᴫ�ݻؿͻ��ˡ�
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
