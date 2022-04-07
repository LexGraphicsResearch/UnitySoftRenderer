using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{

    void Render()
    {
        DrawLine(13,20,80,40,Color.white);
        DrawLine(20,13,40,80,Color.red);
        DrawLine(80, 40, 13, 20, Color.red);
    }

    void SetColor(int x, int y, Color color)
    {
        this.m_texture.SetPixel(x, y, color);
    }

    void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        if (x0 > x1)
        {
            var (tempx,tempy)=(x0,y0);
            (x0,y0)=(x1,y1);
            (x1, y1) = (tempx,tempy);
        }

        for (int x = x0; x <=x1 ; x++)
        {
           float k=(y1-y0)/(float)(x1-x0);
           int y=(int) (k * (x - x0));
           SetColor(x,y,color);
        }
    }

    void Start()
    {
        m_rawImg.texture = new Texture2D(this.m_width, this.m_height, TextureFormat.RGBA32, false);
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
