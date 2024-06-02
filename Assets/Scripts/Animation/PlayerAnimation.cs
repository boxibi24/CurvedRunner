using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private string IS_DEAD = "IsDead";
    private string JUMP = "Jumping";
    private string IS_RUNNING = "IsRunning";
    [SerializeField] Player player;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;
    }
    private void Start()
    {
        player.OnJump += Player_OnJump;
        GameManager.Instance.OnStateChange += GameManager_OnStateChange;
    }

    private void GameManager_OnStateChange(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            animator.SetBool(IS_DEAD, true);
        }
        if (GameManager.Instance.IsGamePlaying())
        {
            animator.SetBool(IS_RUNNING, true);
        }
        if (GameManager.Instance.IsCountdownToStartActive())
        {
            animator.SetBool(IS_DEAD, false);
            animator.SetBool(IS_RUNNING, false);
        }
    }

    private void Player_OnJump(object sender, System.EventArgs e)
    {
        animator.Play(JUMP);
    }
}
