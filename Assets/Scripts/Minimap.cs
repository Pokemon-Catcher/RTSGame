using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
       
    public Texture2D visibilityMap;
    public Texture2D exploredMap;
    public RawImage rawImage;
    private Color black = new Color(0, 0, 0, 0);
    private Color white = new Color(1, 1, 1, 1);
    public int minimapSize = 128;
    
    [SerializeField]
    private RTSPlayer player;
    List<UnitVisionInfo> unitsVisionInfo = new List<UnitVisionInfo>();
    [SerializeField]
    private float minimapUpdateFrequency = 0.1f;
    private float minimapUpdateTimer = 0f;
    [SerializeField]
    private float visionUpdateFrequency = 0.1f;
    private float visionUpdateTimer = 0f;

    private void Awake()
    {
        visibilityMap = new Texture2D(minimapSize, minimapSize);
        exploredMap = new Texture2D(minimapSize, minimapSize);
        MinimapClear();
        ApplyMinimapChanges();
        rawImage.material.SetTexture("_exploredTexture", exploredMap);
        rawImage.material.SetTexture("_visibleTexture", visibilityMap);
        ExploredMinimapUpdateRun();
    }


    private async void ExploredMinimapUpdateRun()
    {
        await Task.Run(()=>ExploredMinimapUpdate());
    }

    private void MinimapClear()
    {
        for (int i = 0; i < minimapSize; i++)
        {
            for (int j = 0; j < minimapSize; j++)
            {
                visibilityMap.SetPixel(i, j, black);
                exploredMap.SetPixel(i, j, black);
            }
        }
    }

    private void ApplyMinimapChanges()
    {
        visibilityMap.Apply();
        exploredMap.Apply();
    }

    private void ExploredMinimapUpdate()
    {
        Color[] pixels = visibilityMap.GetPixels();
        for (int i = 0; i < pixels.Length; i++)
        {
            Color pixel = exploredMap.GetPixel(i % visibilityMap.width, i / visibilityMap.width);
            exploredMap.SetPixel(i % visibilityMap.width, i / visibilityMap.width, pixel.r < pixels[i].r ? pixel : pixels[i]);
            Debug.Log(pixel.r < pixels[i].r ? pixel : pixels[i]);
        }
    }

    public async void VisionUpdateAsync()
    {
        Debug.Log("VisionUpdateAsyncBEGIN");
        List<Task<UnitVisionInfo>> unitVisionGetters = new List<Task<UnitVisionInfo>>();
        foreach (Unit unit in player.units) {
            Task<UnitVisionInfo> unitVisionGetter = Task.Run(unit.GetVisionInfo);
            unitVisionGetters.Add(unitVisionGetter);
        }
        Debug.Log("unitVisionGetters");
        await Task.WhenAll(unitVisionGetters);
        Debug.Log("unitVisionGettersEND");
        List<Task> visionUpdates = new List<Task>();
        Debug.Log("visionUpdates");
        foreach (Task<UnitVisionInfo> unitVisionInfo in unitVisionGetters)
        {
            visionUpdates.Add(Task.Run(()=>VisionUpdate(unitVisionInfo.Result)));
        }
        await Task.WhenAll(visionUpdates);
        Debug.Log("visionUpdatesENDS");
        Debug.Log("VisionUpdateAsyncEND");
    }

    public void VisionUpdate(UnitVisionInfo info)
    {
        Debug.Log("VisionUpdateBEGIN");
        Color[] pixels = visibilityMap.GetPixels();
        int row = 0;
        Vector3 begin = WorldToMinimapPoint(new Vector3(info.unitPosition.x - info.viewDistance, 0, info.unitPosition.z - info.viewDistance));
        Vector3 end = WorldToMinimapPoint(new Vector3(info.unitPosition.x + info.viewDistance, 0, info.unitPosition.z + info.viewDistance));
        for (int i = (int)(begin.x + begin.y * visibilityMap.width); i < (int)(end.x + end.y * visibilityMap.width); i++)
        {
            row++;
            Color pixel = black;
            int x = i % visibilityMap.width;
            int y = i / visibilityMap.width;
            Vector2 minPoint = new Vector2(x, y);
            float distance = Vector3.Distance(MinimapToWorldPoint(minPoint, info.unitPosition.y), info.unitPosition);
            if (distance < info.viewDistance)
            {
                for (int j = 0; j < info.borderPoints.Count; j++)
                {
                    if (IsPointInTri(MinimapToWorldPoint(minPoint, 0), info.unitPosition, info.borderPoints[j], info.borderPoints[(j + 1) % info.borderPoints.Count]))
                    {
                        pixel = white;
                        visibilityMap.SetPixel(x, y, pixel);
                        break;
                    }
                }
            }
        }
        Debug.Log("VisionUpdateEND");
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
        return new Vector2(worldPosition.x * visibilityMap.width / 100 + visibilityMap.width / 2, worldPosition.z * visibilityMap.height / 100 + visibilityMap.height / 2);
    }

    public Vector3 MinimapToWorldPoint(Vector2 minimapPoint, float y)
    {
        return new Vector3((minimapPoint.x - visibilityMap.width / 2) * 100 / visibilityMap.width, y, (minimapPoint.y - visibilityMap.height / 2) * 100 / visibilityMap.height);
    }

    private void Update()
    {
        minimapUpdateTimer += Time.deltaTime;
        if (minimapUpdateTimer > minimapUpdateFrequency)
        {
            MinimapClear();
            VisionUpdateAsync();
        }
    }
}
