using UnityEngine;

public class AStarDemo : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.RawImage rawImage;
    [SerializeField]
    private Color openListColor = new Color(0.9f, 0.9f, 0.9f);
    [SerializeField]
    private Color closeListColor = new Color(0.5f, 0.5f, 0.5f);
    [SerializeField]
    private Color pathListColor = new Color(1.0f, 0.0f, 0.0f);

    private Texture2D pathT2D;
    private Texture2D mapT2D;
    private AStar aStar;
    private Map map;

    private void Start()
    {
        mapT2D = rawImage.mainTexture as Texture2D;
        pathT2D = new Texture2D(mapT2D.width, mapT2D.height, mapT2D.format, false, false);
        pathT2D.filterMode = FilterMode.Point;
        map = new Map(mapT2D);
        aStar = new AStar(map);
    }

    public void FindPath()
    {
        UnityEngine.Profiling.Profiler.BeginSample("___FindPathTest___");
        float startTime = UnityEngine.Time.realtimeSinceStartup;
        bool findPathResult = aStar.FindPath(map.GetNode(0, 0), map.GetNode(map.colCount - 1, map.rowCount  - 1));
        float costTime = UnityEngine.Time.realtimeSinceStartup - startTime;
        UnityEngine.Profiling.Profiler.EndSample();
        UnityEngine.Debug.Log($"findPathResult={findPathResult}, costTime=[{costTime}]sec");
        pathT2D.SetPixels32(mapT2D.GetPixels32());



        for (int i = 0, count = aStar.openList.Count; i < count; i++)
        {
            Node pathNode = aStar.openList[i];
            pathT2D.SetPixel(pathNode.x, pathNode.y, openListColor);
        }
        foreach (var pathNode in aStar.closeList)
        {
            pathT2D.SetPixel(pathNode.x, pathNode.y, closeListColor);
        }

        foreach (var pathNode in aStar.path)
        {
            pathT2D.SetPixel(pathNode.x, pathNode.y, pathListColor);
        }
        pathT2D.Apply();
        rawImage.texture = pathT2D;
    }


}
