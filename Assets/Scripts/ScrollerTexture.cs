using UnityEngine;
using UnityEngine.UI;


public class ScrollerTexture : MonoBehaviour
{
    [SerializeField] private RawImage img;
    [SerializeField] private float _x, _y;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        img.uvRect = new Rect(img.uvRect.position + new Vector2(_x, _y) * Time.unscaledDeltaTime, img.uvRect.size);
    }
}
