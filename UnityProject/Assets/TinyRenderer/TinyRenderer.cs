using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{
    

    void Start()
    {
        m_rawImg.texture= new Texture2D(this.m_width, this.m_height, TextureFormat.RGBA32, false);
        var texture=m_rawImg.texture as Texture2D;
        texture.SetPixel(52,41,Color.red);
        texture.Apply();


    }

    void Update()
    {
        
    }

    [SerializeField]
    private int m_width;
    [SerializeField]
    private int m_height;
    [SerializeField]
    private RawImage m_rawImg;
}
