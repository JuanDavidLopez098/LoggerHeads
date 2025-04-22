using TMPro;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public KeyCode hitKey = KeyCode.Space;
    
    [Header("Components")]
    public Animator animator;
    public Collider weaponCollider;

    [Header("Feedback")]
    [SerializeField] private ParticleSystem hitParticles;
    [SerializeField] private AudioClip hitSound;

    [Header("Score")]
    public TMP_Text scoreText;
    public int logsHit = 0;
    
    [Header("Victory")]
    public int winThreshold = 3;

    // Sistema de voces
    public enum CharacterGender { Male, Female }

    [Header("Voice Settings")]
    public CharacterGender gender = CharacterGender.Male;
    public AudioClip[] victoryVoices;
    public AudioClip[] defeatVoices;
    [Range(0.1f, 1f)] public float voiceVolume = 0.7f;
    
    private bool canHit = true;
    private AudioSource audioSource;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
            
        if (weaponCollider != null)
            weaponCollider.enabled = false;
            
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateScoreText();
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
        animator.SetTrigger("Hit");
        
        if (weaponCollider != null)
        {
            weaponCollider.enabled = true;
            Invoke("DisableWeaponCollider", 0.2f);
        }
        
        if (hitParticles != null)
            hitParticles.Play();
            
        if (hitSound != null)
            audioSource.PlayOneShot(hitSound);
    }

    public void AddLogHit()
    {
        logsHit++;
        UpdateScoreText();
        
        if(logsHit >= winThreshold)
        {
            GameManager.instance.PlayerWins(this);
        }
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = $"{hitKey}: {logsHit}";
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
        PlayRandomVoice(victoryVoices);
    }

    public void PlayLoseAnimation()
    {
        animator.SetTrigger("Lose");
        PlayRandomVoice(defeatVoices);
    }

    public void PlayIdleAnimation()
    {
        animator.SetTrigger("Idle");
    }

    public void SetHitKey(KeyCode newKey)
    {
        hitKey = newKey;
        UpdateScoreText();
    }

    private void PlayRandomVoice(AudioClip[] voiceClips)
    {
        if (voiceClips == null || voiceClips.Length == 0) return;

        AudioClip randomVoice = voiceClips[Random.Range(0, voiceClips.Length)];
        
        GameObject voiceObject = new GameObject("TempVoice");
        voiceObject.transform.position = transform.position;
        AudioSource voiceSource = voiceObject.AddComponent<AudioSource>();
        
        voiceSource.clip = randomVoice;
        voiceSource.volume = voiceVolume;
        voiceSource.spatialBlend = 0.8f;
        
        voiceSource.pitch = gender == CharacterGender.Female ? 
            Random.Range(1.1f, 1.3f) : 
            Random.Range(0.9f, 1.1f);
        
        voiceSource.Play();
        Destroy(voiceObject, randomVoice.length + 0.1f);
    }
}