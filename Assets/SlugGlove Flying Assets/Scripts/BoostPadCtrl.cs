using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPadCtrl : MonoBehaviour
{
    public float Boost;
    public GameObject FX;
    public GameObject redBull;
    private void OnTriggerEnter(Collider other)
    {
        PlayerCollisionSphere Player = other.GetComponent<PlayerCollisionSphere>();

        if (!Player)
            return;

        Player.PlayerMov.SpeedBoost(Boost);

        if (FX)
        {
            GameObject CRFX = Instantiate(FX, transform.position, transform.rotation);
            CRFX.SetActive(true);
        }

        if(redBull.active)
        {
            redBull.SetActive(false);
            GameManager.redbullCan += 1;
        }

    }
}
