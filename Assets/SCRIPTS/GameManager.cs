using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController[] players;
    public TargetSpawner spawner;

    [Header("Victory UI")]
    public GameObject victoryPanel;
    public TMP_Text victoryMessageText;
    public AudioClip victorySound;

    [Header("Music Settings")] 
    public AudioSource backgroundMusic;
    public AudioClip gameMusic;
    
    private GameObject currentTarget;
    private bool gameActive = true;

    private void Awake()
    {
        // Manejo de singleton modificado para permitir reinicio completo
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        // Eliminado DontDestroyOnLoad para permitir reinicio completo
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        victoryPanel.SetActive(false);
        Time.timeScale = 1f;
        gameActive = true;
        
        if (spawner != null)
        {
            spawner.SpawnNewObject();
        }
        
        PlayBackgroundMusic();
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && gameMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.clip = gameMusic;
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    private void StopBackgroundMusic()
    {
        if (backgroundMusic != null && backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop();
        }
    }

    public void PlayerWins(PlayerController winner)
    {
        if (!gameActive) return;

        gameActive = false;
        victoryPanel.SetActive(true);
        victoryMessageText.text = $"GANADOR: {winner.hitKey.ToString().ToUpper()}";
        
        if (victorySound != null)
            AudioSource.PlayClipAtPoint(victorySound, Camera.main.transform.position);
        
        StopBackgroundMusic();
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        // Destruye el GameManager actual antes de recargar
        Destroy(gameObject);
        
        // Reinicia completamente la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetCurrentTarget(GameObject target)
    {
        currentTarget = target;
    }

    public GameObject GetCurrentTarget()
    {
        return currentTarget;
    }

    public void LogHit(PlayerController hitter)
    {
        if (!gameActive) return;

        hitter.PlayWinAnimation();
        hitter.AddLogHit();

        foreach (var player in players)
        {
            if (player != hitter)
                player.PlayLoseAnimation();
        }

        DestroyCurrentObject();
        spawner.LogWasHit();
        Invoke("ResetPlayersToIdle", 1.5f);
    }

    public void BombHit(PlayerController hitter)
    {
        if (!gameActive) return;

        hitter.PlayLoseAnimation();

        foreach (var player in players)
        {
            if (player != hitter)
                player.PlayWinAnimation();
        }

        DestroyCurrentObject();
        Invoke("ResetPlayersToIdle", 1.5f);
        spawner.StartCoroutine(spawner.DelayedSpawn());
    }

    public void TargetDestroyedWithoutHit()
    {
        if (!gameActive) return;

        foreach (var player in players)
        {
            player.PlayWinAnimation();
        }
        
        Invoke("ResetPlayersToIdle", 1.5f);
    }

    private void DestroyCurrentObject()
    {
        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }
    }

    private void ResetPlayersToIdle()
    {
        foreach (var player in players)
        {
            if (player != null)
            {
                player.PlayIdleAnimation();
                player.EnableHit();
            }
        }
    }
}