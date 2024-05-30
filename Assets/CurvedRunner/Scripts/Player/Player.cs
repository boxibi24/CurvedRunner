using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance {  get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {

    }
}
