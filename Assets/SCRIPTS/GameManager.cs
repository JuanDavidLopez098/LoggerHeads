using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController[] players;
    public TargetSpawner spawner;

    private GameObject currentTarget;
    private bool gameActive = true;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Validación básica de componentes
        if (players == null || players.Length == 0)
            Debug.LogError("No players assigned in GameManager!");

        if (spawner == null)
            Debug.LogError("No spawner assigned in GameManager!");
    }

    private void Start()
    {
        if (gameActive)
        {
            spawner.SpawnNewObject();
        }
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

        // Jugador que golpeó gana
        hitter.PlayWinAnimation();

        // Los demás jugadores pierden
        foreach (var player in players)
        {
            if (player != hitter)
                player.PlayLoseAnimation();
        }

        DestroyCurrentObject();
        spawner.LogWasHit(); // Notificar al spawner que el tronco fue golpeado
        Invoke("ResetPlayersToIdle", 1.5f);
    }

    public void BombHit(PlayerController hitter)
    {
        if (!gameActive) return;

        // Jugador que golpeó pierde
        hitter.PlayLoseAnimation();

        // Los demás jugadores ganan
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

        // Todos ganan cuando la bomba desaparece sola
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

    public void EndGame()
    {
        gameActive = false;
        DestroyCurrentObject();
        CancelInvoke();
    }
}