using UnityEngine;

public class GameplayIntro : MonoBehaviour
{
    [Header("Sons")]
    public AudioClip roundSFX;
    public AudioClip startSFX;
    public AudioSource sfx;

    public GameManager gm;

    public void PlayRound()
    {
        sfx.PlayOneShot(roundSFX);
    }

    public void PlayStart()
    {
        sfx.PlayOneShot(startSFX);
        gm.PlayGame();
    }

}
