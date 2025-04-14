using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<PlayerController> players;
    private bool hitRegistered = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            hitRegistered = false;

            foreach (PlayerController player in players)
            {
                player.animator.SetTrigger("Idle");
            }

            Debug.Log("Ronda reiniciada");
        }
    }

    public void RegisterHit(PlayerController player)
    {
        if (hitRegistered) return;

        hitRegistered = true;
        Debug.Log($"{player.playerName} GOLPEÓ PRIMERO");

        player.PlayWin();

        foreach (PlayerController p in players)
        {
            if (p != player)
            {
                p.PlayLose();
            }
        }
    }
}