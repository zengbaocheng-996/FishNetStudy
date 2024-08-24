using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;
public class HealthManager : NetworkBehaviour
{
    private readonly SyncVar<int> HealthValue = new SyncVar<int>(100);
    public Image HealthBar;
    private Animator _animator;
    public bool isDie = false;
    private void Awake()
    {
        HealthValue.OnChange += UpdateHealthBar;
    }
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void UpdateHealthBar(int _oldValue, int _newValue, bool asServer)
    {
        HealthBar.fillAmount = (float)_newValue/100;
        if (_newValue <= 0)
        {
            isDie = true;
            //ËÀÍö¶¯»­
            _animator.SetBool("Die", true);
        }
    }
    [Server]
    public void Damage(int value)
    {
        if(HealthValue.Value > 0)
        {
            HealthValue.Value -= value;
            
        }
    }
}
