using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class TinyRenderer : MonoBehaviour
{
    void Render()
    {
        DrawMesh();

        foreach (var tri in m_triangles)
        {
            DrawTri(tri);
        }

    }

    void SetColor(int x, int y, Color color)
    {
        this.m_texture.SetPixel(x, y, color);
    }

    void DrawMesh()
    {
        var faces = this.m_africanHeadMesh.triangles;
        for (int i = 0; i + 2 < faces.Length; i += 3)
        {
            for (int j = 0; j < 3; j++)
            {
                var v1 = this.m_africanHeadMesh.vertices[faces[i + j]];
                var v2 = this.m_africanHeadMesh.vertices[faces[i + ((j + 1) % 3)]];
                int x0 = (int)((v1.x + 1) * this.m_width / 2);
                int y0 = (int)((v1.y + 1) * this.m_height / 2);
                int x1 = (int)((v2.x + 1) * this.m_width / 2);
                int y1 = (int)((v2.y + 1) * this.m_height / 2);
                DrawLine(x0, y0, x1, y1, Color.black);

            }
        }
    }

    void DrawTri(Triangle tri)
    {
        DrawTri(tri.m_v1, tri.m_v2, tri.m_v3, tri.m_color);
    }

    /// <summary>
    ///
    ///todo
    ///Sort vertices of the triangle by their y-coordinates;
    ///Rasterize simultaneously the left and the right sides of the triangle;
    ///Draw a horizontal line segment between the left and the right boundary points.
    ///
    ///  
    /// </summary>
    void DrawTri(Vector2Int t0, Vector2Int t1, Vector2Int t2, Color color)
    {
        DrawLine(t0.x, t0.y, t1.x, t1.y, color);
        DrawLine(t1.x, t1.y, t2.x, t2.y, color);
        DrawLine(t2.x, t2.y, t0.x, t0.y, color);
    }


    /// <summary>
    /// more optimization here:
    /// https://github.com/ssloy/tinyrenderer/wiki/Lesson-1:-Bresenham%E2%80%99s-Line-Drawing-Algorithm
    /// </summary>
    void DrawLine(int x0, int y0, int x1, int y1, Color color)
    {
        //handle sparse points
        int h = Math.Abs(y0 - y1);
        int w = Math.Abs(x0 - x1);
        bool steep = h > w;
        if (steep)
        {
            (x0, y0) = (y0, x0);
            (x1, y1) = (y1, x1);
        }


        //make x0 always smaller than x1; for symmetry
        if (x0 > x1)
        {
            (x0, x1) = (x1, x0);
            (y0, y1) = (y1, y0);
        }

        float k = (y1 - y0) / (float)(x1 - x0);
        for (int x = x0; x <= x1; x++)
        {
            int y = (int)(k * (x - x0)) + y0;
            if (steep)
            {
                SetColor(y, x, color);
            }
            else
            {
                SetColor(x, y, color);
            }
        }
    }

    void LoadModel()
    {
        //https://en.wikipedia.org/wiki/Wavefront_.obj_file more about file format here;
        var africanHead = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TinyRenderer/Models/african_head/african_head.obj");
        m_africanHeadMesh = africanHead.GetComponentInChildren<MeshFilter>().sharedMesh;



    }

    [Serializable]
    struct Triangle
    {
        public Color m_color;
        public Vector2Int m_v1;
        public Vector2Int m_v2;
        public Vector2Int m_v3;
    }





    void Start()
    {
        m_rawImg.texture = new Texture2D(this.m_width, this.m_height, TextureFormat.RGBA32, false);
        m_texture = m_rawImg.texture as Texture2D;
        m_rawImg.SetNativeSize();

        LoadModel();

        this.m_btn.onClick.AddListener(Save);


    }

    void Save()
    {
        var bytes = this.m_texture.EncodeToPNG();
        var dirPath = string.Format($"{Application.dataPath}/../SaveImages/");
        Directory.CreateDirectory(dirPath);
        File.WriteAllBytes(string.Format($"{dirPath}/{DateTime.Now:HHmmss}.png"), bytes);
    }

    void Update()
    {
        Render();
        this.m_texture.Apply();

    }


    [SerializeField]
    private List<Triangle> m_triangles;



    [SerializeField]
    private Button m_btn;

    private Mesh m_africanHeadMesh;

    [SerializeField]
    private int m_width;
    [SerializeField]
    private int m_height;
    [SerializeField]
    private RawImage m_rawImg;

    private Texture2D m_texture;
}
