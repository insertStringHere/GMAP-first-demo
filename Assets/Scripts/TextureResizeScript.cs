using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureResizeScript : MonoBehaviour
{
    float resize = 4;
    float scale = 1;
    private Transform t;
    private Renderer r;
    private Material m;
    private void Start()
    {
        t = this.gameObject.GetComponent<Transform>();
        scale = t.localScale.x;
        r = this.gameObject.GetComponent<Renderer>();
        r.material.mainTextureScale = new Vector2(resize*scale, resize*scale);

    }
    
}
