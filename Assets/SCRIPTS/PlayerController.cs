using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [Tooltip("Tecla para golpear (configurable por jugador)")]
    public KeyCode hitKey = KeyCode.Space;
    
    [Header("Components")]
    [Tooltip("Animator del jugador (se auto-asigna si está en el mismo GameObject)")]
    public Animator animator;
    [Tooltip("Collider del arma (opcional para detección de golpes)")]
    public Collider weaponCollider;

    [Header("Feedback")]
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private AudioClip hitSound;
    
    private bool canHit = true;
    private AudioSource audioSource;

    private void Awake()
    {
        // Auto-asignar componentes si no están establecidos
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (weaponCollider != null)
            weaponCollider.enabled = false;
            
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (canHit && Input.GetKeyDown(hitKey))
        {
            PerformHit();
        }
    }

    private void PerformHit()
    {
        canHit = false;
        
        // Activar animación
        animator.SetTrigger("Hit");
        
        // Activar collider del arma (si existe)
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            Invoke("DisableWeaponCollider", 0.2f); // Desactiva después de 0.2 segundos
        }
        
        // Feedback visual/auditivo
        if (hitParticles != null)
            hitParticles.Play();
            
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }

    private void DisableWeaponCollider()
    {
        if (weaponCollider != null)
            weaponCollider.enabled = false;
    }

    public void EnableHit()
    {
        canHit = true;
    }

    public void PlayWinAnimation()
    {
        animator.SetTrigger("Win");
    }

    public void PlayLoseAnimation()
    {
        animator.SetTrigger("Lose");
    }

    public void PlayIdleAnimation()
    {
        animator.SetTrigger("Idle");
    }

    // Método para cambiar la tecla dinámicamente (opcional)
    public void SetHitKey(KeyCode newKey)
    {
        hitKey = newKey;
    }
}