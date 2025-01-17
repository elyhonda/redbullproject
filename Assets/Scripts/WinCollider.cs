using UnityEngine;

public class WinCollider : MonoBehaviour
{
    [SerializeField] private GameManager gm;

    private void OnTriggerEnter(Collider other)
    {
        gm.GameWin();
    }
}
