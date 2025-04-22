using UnityEngine;
using System.Collections;

public class TargetSpawner : MonoBehaviour
{
    public GameObject logPrefab;
    public GameObject bombPrefab;
    public Transform spawnPoint;
    
    [Header("Spawn Settings")]
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;
    public float bombLifetime = 3f; // Tiempo antes de que la bomba desaparezca

    private bool waitingForLogHit = false;

    public void SpawnNewObject()
    {
        if (waitingForLogHit) return;

        int random = Random.Range(0, 2); // 0 = tronco, 1 = bomba
        GameObject prefabToSpawn = (random == 0) ? logPrefab : bombPrefab;

        GameObject newObject = Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);
        GameManager.instance.SetCurrentTarget(newObject);

        if (random == 1) // Si es bomba
        {
            StartCoroutine(DestroyBombAfterTime(newObject, bombLifetime));
        }
    }

    private IEnumerator DestroyBombAfterTime(GameObject bomb, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (bomb != null && bomb == GameManager.instance.GetCurrentTarget())
        {
            Destroy(bomb);
            GameManager.instance.TargetDestroyedWithoutHit();
            StartCoroutine(DelayedSpawn());
        }
    }

    public void LogWasHit()
    {
        waitingForLogHit = false;
        StartCoroutine(DelayedSpawn());
    }

    public IEnumerator DelayedSpawn()
    {
        float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(delay);
        SpawnNewObject();
    }

    public void SetWaitingForLogHit(bool waiting)
    {
        waitingForLogHit = waiting;
    }
}