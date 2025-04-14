using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public KeyCode hitKey;
    public Animator animator;
    public Collider axeCollider;
    public string playerName;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator.SetTrigger("Idle");
        axeCollider.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(hitKey))
        {
            Debug.Log($"{playerName} PRESSED {hitKey}");
            PerformHit();
        }
    }

    void PerformHit()
    {
        animator.SetTrigger("Hit");
        axeCollider.enabled = true;

        Invoke("DisableAxe", 0.3f);

        gameManager.RegisterHit(this); // Si querés que registre el primero
    }

    void DisableAxe()
    {
        axeCollider.enabled = false;
    }

    public void PlayWin()
    {
        animator.SetTrigger("Win");
    }

    public void PlayLose()
    {
        animator.SetTrigger("Lose");
    }
}