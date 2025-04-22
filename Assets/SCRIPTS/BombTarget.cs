using UnityEngine;

public class BombTarget : MonoBehaviour
{
    private bool canBeHit = false;
    private bool wasHit = false;
    
    private void Start()
    {
        Invoke("EnableHit", 0.5f);
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
            GameManager.instance.BombHit(player);
        }
    }
}