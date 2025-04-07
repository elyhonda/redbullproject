using UnityEngine;

public class DeathObject : MonoBehaviour
{
    public GameManager gm;
    public CameraFollow cameraFollow;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerCollisionSphere Player = other.GetComponent<PlayerCollisionSphere>();
        if (!Player)
            return;
        cameraFollow.StopFollowing();
        gm.GameOver();
    }
}
