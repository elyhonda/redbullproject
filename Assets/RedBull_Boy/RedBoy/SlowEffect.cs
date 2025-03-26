using UnityEngine;
using System.Collections;

public class SlowEffect : MonoBehaviour
{
    public float slowMultiplier = 0.5f; // Multiplicador da velocidade (0.5 = 50% de lentidão)
    public float slowDuration = 2f; // Duração do efeito de lentidão
    public float normalSpeedDuration = 2f; // Duração para voltar a velocidade normal

    private PlayerMovement playerMovement;
    public float originalMaxSpeed;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement não encontrado no GameObject!");
        }
        else
        {
            originalMaxSpeed = playerMovement.MaxSpeed;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slow"))
        {
            if (playerMovement != null)
            {
                playerMovement.MaxSpeed *= slowMultiplier;
                StartCoroutine(ResetSpeed());
            }
        }
    }

    IEnumerator ResetSpeed()
    {
        yield return new WaitForSeconds(slowDuration);
        yield return new WaitForSeconds(normalSpeedDuration);
        if (playerMovement != null)
        {
            playerMovement.MaxSpeed = originalMaxSpeed;
        }
    }
}