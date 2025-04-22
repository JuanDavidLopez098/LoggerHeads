using UnityEngine;

public class LogTarget : MonoBehaviour
{
    private bool canBeHit = false;
    private bool wasHit = false;
    
    private void Start()
    {
        Invoke("EnableHit", 0.5f);
        GameManager.instance.spawner.SetWaitingForLogHit(true);
    }
    
    private void EnableHit()
    {
        canBeHit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canBeHit || wasHit) return;
        
        PlayerController player = other.GetComponentInParent<PlayerController>();
        if (player != null)
        {
            wasHit = true;
            GameManager.instance.LogHit(player);
        }
    }

    private void OnDestroy()
    {
        if (!wasHit)
        {
            GameManager.instance.spawner.SetWaitingForLogHit(false);
        }
    }
}