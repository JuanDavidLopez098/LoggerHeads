using UnityEngine;

public class BombTarget : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip explosionSound; // Sonido de explosión (asignar en el Inspector)
    [Range(0, 1)] public float explosionVolume = 0.7f; // Control de volumen

     [Header("Visual Effect")]
    public GameObject explosionEffectPrefab; // Efecto visual (asignar en el Inspector)
    
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
            PlayExplosionSound(); // Reproducir sonido antes de notificar al GameManager

        if (explosionEffectPrefab != null)
                Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity); // EFECTO VISUAL
    
            GameManager.instance.BombHit(player);
        }
    }

    private void PlayExplosionSound()
    {
        if (explosionSound != null)
        {
            // Crea un AudioSource temporal para el sonido 3D
            GameObject soundEmitter = new GameObject("ExplosionSound");
            soundEmitter.transform.position = transform.position;
            AudioSource audioSource = soundEmitter.AddComponent<AudioSource>();
            
            audioSource.clip = explosionSound;
            audioSource.volume = explosionVolume;
            audioSource.spatialBlend = 1f; // Sonido 3D completo
            audioSource.Play();
            
            // Destruye el objeto después de que termine el sonido
            Destroy(soundEmitter, explosionSound.length + 0.1f);
        }
    }
}