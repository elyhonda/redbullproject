using UnityEngine;

public class WingTrailController : MonoBehaviour
{
    [Header("References")]
    public PlayerController playerController; // Referência ao PlayerController
    public TrailRenderer[] wingTrails;        // Array com os TrailRenderers das asas

    [Header("Settings")]
    public float minFlySpeedToEnable = 0.1f;   // Velocidade mínima para ativar trail

    void Update()
    {
        if (playerController != null)
        {
            // Verifica se está voando e se a velocidade é maior que mínimo
            bool enableTrail = playerController.IsFlying() && playerController.GetCurrentFlySpeed() > minFlySpeedToEnable;

            foreach (TrailRenderer trail in wingTrails)
            {
                if (trail != null)
                    trail.emitting = enableTrail;
            }
        }
    }
}
