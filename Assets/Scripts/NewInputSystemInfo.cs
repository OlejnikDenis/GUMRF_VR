using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class NewInputSystemInfo : MonoBehaviour
{
    [SerializeField] private InputActionReference _inputActionReference;
    
    // Start is called before the first frame update
    void Start()
    {
        _inputActionReference.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
