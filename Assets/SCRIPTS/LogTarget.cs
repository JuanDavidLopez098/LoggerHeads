using UnityEngine;

public class LogTarget : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip hitSound; // Sonido al golpear el tronco
    [Range(0, 1)] public float hitVolume = 0.5f; // Control de volumen
    public float soundPitchVariation = 0.1f; // Variación de tono aleatoria
    
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
            PlayHitSound(); // Reproducir sonido antes de notificar al GameManager
            GameManager.instance.LogHit(player);
        }
    }

    private void PlayHitSound()
    {
        if (hitSound != null)
        {
            // Crea un AudioSource temporal para el sonido
            GameObject soundEmitter = new GameObject("LogHitSound");
            soundEmitter.transform.position = transform.position;
            AudioSource audioSource = soundEmitter.AddComponent<AudioSource>();
            
            audioSource.clip = hitSound;
            audioSource.volume = hitVolume;
            audioSource.pitch = 1f + Random.Range(-soundPitchVariation, soundPitchVariation); // Variación aleatoria
            audioSource.spatialBlend = 0.8f; // Mezcla entre 2D y 3D
            audioSource.Play();
            
            // Destruye el objeto después de que termine el sonido
            Destroy(soundEmitter, hitSound.length + 0.1f);
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