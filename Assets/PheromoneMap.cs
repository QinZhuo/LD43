using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheromoneMap : MonoBehaviour {
    public bool showGizmos=true;
    private Pheromone[,] map;
    public static int mapSize=50;
	// Use this for initialization
	void Start () {
        map = new Pheromone[mapSize, mapSize] ;
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                map[x, y] = new Pheromone();
            }
        }
        InvokeRepeating("PheromoneDelete", 0, 1);
    }
    private void PheromoneDelete()
    {
        List<PheromoneType> pl = new List<PheromoneType>() { PheromoneType.Food, PheromoneType.Queen };
        foreach (var item in map)
        {
            for (int i = 0; i < pl.Count; i++)
            {
                if (item.scale[pl[i]] > 1)
                {
                    item.scale[pl[i]] *= 0.95f;
                    if (item.scale[pl[i]] <= 1)
                    {
                        item.scale[pl[i]]=0;
                    }
                }
            }
               
        }
    }
	public static int[] GetPos(Vector3 v3)
    {
        int x= Mathf.FloorToInt(v3.x)+ mapSize/2;
        if (v3.x - x > 0.5f)
        {
            x += 1;
        }
        int y = Mathf.FloorToInt(v3.z) + mapSize / 2;
        if (v3.z - y > 0.5f)
        {
            y += 1;
        }
        return new int[2] { x, y };
    }
    public static Vector3 GetVector3(params int[] pos)
    {
        return new Vector3(pos[0] - mapSize / 2,0, pos[1] - mapSize / 2);
    }
    public Pheromone GetPreomone(params int[] pos)
    {
      //  Debug.Log(pos[0]+","+ pos[1]);
        return map[pos[0],  pos[1]];
    }
	// Update is called once per frame
	void Update () {
		
	}
    private void OnDrawGizmos()
    {
        if (!showGizmos||map==null) return;
        
        for (int x = 0; x < mapSize; x++)
        {
            for (int y = 0; y < mapSize; y++)
            {
                if (map[x, y].scale[PheromoneType.Queen] > 0)
                {
                    Gizmos.color = new Color(0, 0, 1, map[x, y].scale[PheromoneType.Queen] / 300);
                    Gizmos.DrawSphere(GetVector3(x, y), 0.2f);
                    // Debug.Log("["+x+","+y+"] "+ GetVector3(x, y) + ":" + map[x, y].scale);
                }
               
                if (map[x, y].scale[PheromoneType.Food]>0)
                {
                    Gizmos.color = new Color(1, 0,0, map[x, y].scale[PheromoneType.Food] / 300);
                    Gizmos.DrawSphere(GetVector3(x, y), 0.3f);
                }
                //else
                //{
                //    Gizmos.color = new Color(0, 0, 0);
                //    Gizmos.DrawSphere(GetVector3(x, y), 0.5f);
                //}
            }
        }
    }
}
public class Pheromone
{
    public static float MaxScale=400;
    public Dictionary<PheromoneType,float> scale;
    public Pheromone()
    {
        scale = new Dictionary<PheromoneType, float>();
        scale.Add(PheromoneType.Food, 0);
        scale.Add(PheromoneType.Queen, 0);
    }
    
}
public enum PheromoneType
{
    none,
    Food,
    Queen
}