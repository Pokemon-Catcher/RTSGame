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
        exploredMap.Apply();
        minimapUpdateRun();
    }

    public void visionUpdateRun(List<Vector3> points, Vector3 sender, float viewDistance)
    {
        StartCoroutine(visionUpdate(points, sender, viewDistance));
    }

        public IEnumerator visionUpdate(List<Vector3> points, Vector3 sender, float viewDistance)
    {
        Color[] pixels = visibilityMap.GetPixels();
        List<Vector2> angleDistance = new List<Vector2>();
        for (int j = 0; j < points.Count; j++)
        {
            angleDistance.Add(new Vector2(Vector2.Angle(sender, points[j]),Vector2.Distance(sender, points[j])));
        }
        int row = 0;
        for (int i = 0; i < pixels.Length; i++)
        {
            row++;
            Color pixel=white;
            int x = i % visibilityMap.width;
            int y = i / visibilityMap.width;
            Vector2 minPoint = new Vector2(x, y);
            float distance = Vector3.Distance(MinimapToWorldPoint(minPoint, sender.y), sender);
            if (distance > viewDistance) {
                pixel = black;
            } else 
            {
                Vector2 senderOnMinimap = WorldToMinimapPoint(sender);
                float angle = Vector2.Angle(senderOnMinimap, minPoint);
                float difference =  angleDistance[0].x - angle;
                Vector2 first;
                Vector2 second;
                int direction = (int)Mathf.Sign(difference);
                int j = 0;
                do
                {
                    j = j+direction;
                    if (j < 0) j = angleDistance.Count + j;
                    difference = angleDistance[j].x- angle;
                    direction= (int)Mathf.Sign(difference); 
                    if(direction>=0)
                    {
                        first = angleDistance[j];
                        second = angleDistance[(j+1)%angleDistance.Count];
                        break;
                    }
                    yield return null;
                } while (true);
                if (distance < Mathf.Lerp(first.y, second.y, first.x != second.x ? (Mathf.Abs((angle - second.x) / (first.x - second.x))) : 0))
                {
                    pixel = white;
                }
                else
                {
                    pixel = black;
                    Debug.DrawLine(sender, points[j], Color.blue, 1f);
                    Debug.DrawLine(sender, points[(j + 1) % angleDistance.Count], Color.red, 1f);
                }
            }
            visibilityMap.SetPixel(x, y, pixel);
            if (row >= visibilityMap.width) {
                row = 0;
                yield return null;
            }
        }
        visibilityMap.Apply();
    }


    public Vector2 WorldToMinimapPoint(Vector3 worldPosition)
    {
        return new Vector2(worldPosition.x *  visibilityMap.width / 100 + visibilityMap.width/2, worldPosition.z * visibilityMap.height / 100+ visibilityMap.height / 2);
    }

    public Vector3 MinimapToWorldPoint(Vector2 minimapPoint, float y)
    {
        return new Vector3((minimapPoint.x - visibilityMap.width / 2) * 100/visibilityMap.width, y, (minimapPoint.y- visibilityMap.height / 2) * 100 / visibilityMap.height);
    }
}
