using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{
    
    void Render()
    {
        SetColor(50,50,Color.green);
    }

    void SetColor(int x,int y,Color color)
    {
       this.m_texture.SetPixel(x,y,color); 
    }

    void DrawLine()
    {

    }

    void Start()
    {
        m_rawImg.texture= new Texture2D(this.m_width, this.m_height, TextureFormat.RGBA32, false);
        m_texture = m_rawImg.texture as Texture2D;

    }

    void Update()
    {
        Render();
        this.m_texture.Apply();
        
    }


    [SerializeField]
    private int m_width;
    [SerializeField]
    private int m_height;
    [SerializeField]
    private RawImage m_rawImg;

    private Texture2D m_texture;
}
