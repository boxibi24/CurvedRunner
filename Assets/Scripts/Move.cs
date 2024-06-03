using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private float relativeMoveSpeed;
    [SerializeField] private bool isMoveable;
    private bool shouldMove = true;
    private void Start()
    {
        if (isMoveable)
        {
            if (transform.rotation == Quaternion.Euler(new Vector3(0, 180, 0))) 
            {
                relativeMoveSpeed = Player.Instance.GetPlayerRunSpeed() + moveSpeed;
            } 
            else
            {
                relativeMoveSpeed = Player.Instance.GetPlayerRunSpeed()  - moveSpeed;
            }
        }
        else
        {
            relativeMoveSpeed = Player.Instance.GetPlayerRunSpeed();
        }
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
    }

    /*private void OnDestroy()
    {
        GameManager.Instance.OnStateChange -= GameManager_OnStateChange;
    }
*/
    private void GameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            shouldMove = false;
        }
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            shouldMove = false;
        }
        if (GameManager.Instance.IsGamePlaying())
        { 
            shouldMove = true; 
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DeleteTrigger"))
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        if (shouldMove)
        {
            move();
        }
    }
    private void move()
    {   
        transform.position -= new Vector3(0, 0 , relativeMoveSpeed * Time.deltaTime);
    }
}
