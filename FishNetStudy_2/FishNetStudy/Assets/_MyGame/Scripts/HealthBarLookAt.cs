using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarLookAt : MonoBehaviour
{
    private GameObject _mainCamera;

    // Update is called once per frame
    void Update()
    {
        if(_mainCamera != null)
        {
            transform.rotation = _mainCamera.transform.rotation;
        }
        else
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
}
