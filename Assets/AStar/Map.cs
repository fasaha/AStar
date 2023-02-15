using System;
using System.Collections.Generic;

public class Map
{
    public int rowCount { get; private set; }
    public int colCount { get; private set; }
    public int totalCount { get; private set; }
    private byte[] data;
    private Dictionary<int, Node> nodeDic;
    public Node GetNode(int x, int y)
    {
        if(x >= short.MaxValue || y >= short.MaxValue)
        {
            throw new System.Exception("error map is to large");
        }
        int key = (x << 16) + y;
        bool find = nodeDic.TryGetValue(key,out Node node);
        if(!find)
        {
            node = new Node(x, y);
            nodeDic.Add(key, node);
        }
        return node;
    }

    public Map(UnityEngine.Texture2D t2d)
    {
        rowCount = t2d.height;
        colCount = t2d.width;
        totalCount = colCount * rowCount;
        data = new byte[totalCount];
        nodeDic = new Dictionary<int, Node>(totalCount >> 8);
        var color32Arr = t2d.GetPixels32();
        for(int i = 0; i < totalCount; i++)
        {
            var color32 = color32Arr[i];
            data[i] = (byte)(255 - color32.r);// 灰度图，颜色越深越难通过，白色的为无障碍区域
        }
    }
    // 是否可通过，并且返回这个位置的阻力
    public bool CanPass(int x, int y, out byte resistance)
    {
        bool canPass;
        int index = y * colCount + x;
        if(index >= totalCount)
        {
            resistance = byte.MaxValue;
            canPass = false;
            throw new IndexOutOfRangeException($"x={x}, y={y} 不在地图范围内");
        }
        else
        {
            resistance = data[index];
            if (resistance < byte.MaxValue)
            {
                canPass = true;
            }
            else
            {
                canPass = false;
            }
        }
        
        return canPass;
    }

}
