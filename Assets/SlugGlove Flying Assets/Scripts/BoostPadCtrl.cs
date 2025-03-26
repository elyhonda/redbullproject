using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPadCtrl : MonoBehaviour
{
    public float Boost;
    public GameObject FX;
    public GameObject redBull;
    public bool isBull;
    private void OnTriggerEnter(Collider other)
    {
        PlayerCollisionSphere Player = other.GetComponent<PlayerCollisionSphere>();

        if (!Player)
            return;

        if(!isBull)
        Player.PlayerMov.SpeedBoost(Boost);
        else
        Player.PlayerMov.SpeedBoostBull(Boost);

        if (FX)
        {
            GameObject CRFX = Instantiate(FX, transform.position, transform.rotation);
            CRFX.SetActive(true);
        }

        if(redBull.active)
        {
            redBull.SetActive(false);
            GameManager gm = FindObjectOfType<GameManager>(); // Encontra o GameManager na cena
            if (gm != null)
            {
                gm.CollectRedbull();
            }
        }

    }
}
