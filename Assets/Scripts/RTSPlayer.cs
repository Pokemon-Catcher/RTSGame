using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RTSPlayer : MonoBehaviour
{
    public int id = 0;
    public string playerName="Player";
    public int gold=0;
    public int wood=0;
    public int food=0;
    public Camera playerCamera;
    public Texture2D visibilityMap;
    public Texture2D exploredMap;
    public RawImage rawImage;
    private Color black= new Color(0, 0, 0, 0);
    private Color white = new Color(1, 1, 1, 1);
    public int minimapSize=128;
    private float minimapUpdateFrequency=0.1f;
    private float minimapUpdateTimer = 0f;
    void Awake()
    { 
        visibilityMap = new Texture2D(minimapSize, minimapSize);
        exploredMap = new Texture2D(minimapSize, minimapSize);
        for (int i = 0; i < minimapSize; i++)
        {
            for (int j = 0; j < minimapSize; j++)
            {
                visibilityMap.SetPixel(i, j, black);
                exploredMap.SetPixel(i, j, black);
            }
        }
        visibilityMap.Apply();
        exploredMap.Apply();
        rawImage.texture = visibilityMap;
        minimapUpdateRun();
    }

    void minimapUpdateRun()
    {
        StopCoroutine(minimapUpdate());
        StartCoroutine(minimapUpdate());
    }

    private IEnumerator minimapUpdate()
    {
        Color[] pixels = visibilityMap.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = exploredMap.GetPixel(i%visibilityMap.width,i/visibilityMap.width);
            exploredMap.SetPixel(i % visibilityMap.width, i / visibilityMap.width, pixel.a < pixels[i].a ? pixel : pixels[i]);
            yield return null;
        }
        minimapUpdateRun();
    }

    public void visionUpdateRun(List<Vector3> points, Vector3 sender, float viewDistance)
    {
        StartCoroutine(visionUpdate(points, sender, viewDistance));
    }

        public IEnumerator visionUpdate(List<Vector3> points, Vector3 sender, float viewDistance)
    {
        Color[] pixels = visibilityMap.GetPixels();
        int row = 0;
        for (int i = 0; i < pixels.Length; i++)
        {
            row++;
            Color pixel=black;
            int x = i % visibilityMap.width;
            int y = i / visibilityMap.width;
            Vector2 minPoint = new Vector2(x, y);
            float distance = Vector3.Distance(MinimapToWorldPoint(minPoint, sender.y), sender);
            if (distance < viewDistance) {
                for (int j = 0; j < points.Count; j ++)
                {
                    if (IsPointInTri(MinimapToWorldPoint(minPoint, 0), sender, points[j], points[(j + 1) % points.Count])) {
                        Color color = new Color(j % 2, (j % 3) / 3, (j % 5) / 5);
                        Debug.DrawLine(points[j], points[(j + 1) % points.Count], color, 1);
                        Debug.DrawLine(points[(j + 1) % points.Count], sender, color, 1);
                        Debug.DrawLine(points[j], sender, color, 1);
                        pixel = white;
                        break;
                    }
                } 
            }
            visibilityMap.SetPixel(x, y, pixel);
            if (row >= visibilityMap.width) {
                row = 0;
                yield return null;
            }
        }
    }

    float Sign(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        return (p1.x - p3.x) * (p2.z - p3.z) - (p2.x - p3.x) * (p1.z - p3.z);
    }

    bool IsPointInTri(Vector3 pt, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        bool b1, b2, b3;

        b1 = Sign(pt, v1, v2) < 0.0f;
        b2 = Sign(pt, v2, v3) < 0.0f;
        b3 = Sign(pt, v3, v1) < 0.0f;

        return ((b1 == b2) && (b2 == b3));
    }


    public Vector2 WorldToMinimapPoint(Vector3 worldPosition)
    {
        return new Vector2(worldPosition.x *  visibilityMap.width / 100 + visibilityMap.width/2, worldPosition.z * visibilityMap.height / 100+ visibilityMap.height / 2);
    }

    public Vector3 MinimapToWorldPoint(Vector2 minimapPoint, float y)
    {
        return new Vector3((minimapPoint.x - visibilityMap.width / 2) * 100/visibilityMap.width, y, (minimapPoint.y- visibilityMap.height / 2) * 100 / visibilityMap.height);
    }

    private void Update()
    {
        minimapUpdateTimer += Time.deltaTime;
        if (minimapUpdateTimer > minimapUpdateFrequency)
        {
            visibilityMap.Apply();
            exploredMap.Apply();
            minimapUpdateTimer = 0;
        }
    }
}
