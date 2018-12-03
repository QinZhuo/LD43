using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : MonoBehaviour {
    private Vector3 nextPosition;
    public PheromoneMap pheromone;
    public PheromoneType type;
    public PheromoneType targetType;
    public Queen queen;
    public float pheromoneScale;
    public float foods;
    public int[] pos;
    public int[] nextPos;
    public int[] lastDirection;
    public LayerMask targetLayer;
    public readonly static int[][] diretion=new int[8][] {
        new int[] {1,0}, new int[] {0,1}, new int[] {-1,0}, new int[] {0,-1},
        new int[] {1,1}, new int[] {1,-1}, new int[] {-1,1}, new int[] {-1,-1}
    };
    private void Awake()
    {
        InvokeRepeating("GetNetPos", 0, 1);
        type = PheromoneType.Queen;
        targetType = PheromoneType.Food;
       // pheromoneScale = 100;
        pos = PheromoneMap.GetPos(this.transform.position);
        nextPos = pos;
        targetLayer =(1<< LayerMask.NameToLayer("Food"));
    }
    private void FixedUpdate()
    {
       
        //transform.position = nextPosition;
        transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime);
        transform.LookAt(nextPosition);
    }
    public void GetNetPos()
    {
        
        //nextPosition=transform.position+ new Vector3(diretion[Random.Range(0, diretion.Length)][0],0,diretion[ Random.Range(0, diretion.Length)][1]);
        float scale = 0;
        float[] slist = new float[9];
        pos = nextPos;
        SetPheromone();
        TargetCheck();
        for (int i = 0; i < diretion.Length; i++)
        {
            var p = pheromone.GetPreomone(pos[0] + diretion[i][0], pos[1] + diretion[i][1]);
            if (targetType == PheromoneType.none) break;
            if (p.scale[ targetType]>0)
            {
                scale += p.scale[targetType];
                slist[i+1] = scale;
            }
            else
            {
                slist[i+1] = scale;
            }
        }
        if (scale > 0)
        {
            float r = Random.Range(0, scale);
            for (int i = 0; i < slist.Length - 1; i++)
            {
                if (r >= slist[i] && r < slist[i + 1])
                {
                    nextPos = new int[] { pos[0] + diretion[i][0], pos[1] + diretion[i][1] };
                    nextPosition = PheromoneMap.GetVector3(nextPos[0], nextPos[1]);
                    
                    break;
                }
            }

        }
        else
        {
            int r = Random.Range(0,8);
            nextPos = new int[] { pos[0] + diretion[r][0], pos[1] + diretion[r][1] };
            nextPosition = PheromoneMap.GetVector3(nextPos[0],nextPos[1]);
           
            Debug.Log(diretion[r][0] + "," + diretion[r][1]);
        }
        
      
    }
   
    private void SetPheromone()
    {
        if (pheromoneScale <= 10f)
        {
            targetType = PheromoneType.Queen;
            type = PheromoneType.none;
            targetLayer = (1 << LayerMask.NameToLayer("Queen"));
            return;
        }
        if (type == PheromoneType.none) return;
        var p = pheromone.GetPreomone(pos);
        //Debug.Log(pos[0] + "," + pos[1]);
        p.scale[type] += pheromoneScale;
        if (p.scale[type] > Pheromone.MaxScale)
        {
            p.scale[type] = Pheromone.MaxScale;
        }
       
        pheromoneScale *=0.7f;
      
    }
    private void TargetCheck()
    {
        if (Physics.CheckSphere(transform.position,0.1f,targetLayer))
        {
            if (targetType == PheromoneType.Food)
            {
                targetType = PheromoneType.Queen;
                foods = 100;
                targetLayer =(1<< LayerMask.NameToLayer("Queen"));
                type = PheromoneType.Food;
                pheromoneScale = 200;
               
            }
            else if(targetType == PheromoneType.Queen)
            {
                targetType = PheromoneType.Food;
                foods = 0;
                type = PheromoneType.Queen;
                targetLayer =(1<< LayerMask.NameToLayer("Food"));
                pheromoneScale = 200;
                queen.food += 200;
            }
            SetPheromone();
        }
    }
    private void OnDrawGizmos()
    {
        if (!pheromone.showGizmos) return;
        switch (targetType)
        {
            case PheromoneType.none:
                break;
            case PheromoneType.Food:
                Gizmos.color = new Color(0, 0.5f, 0, 1);
                Gizmos.DrawWireCube(transform.position+Vector3.up, Vector3.one);
                break;
            case PheromoneType.Queen:
                Gizmos.color = new Color(0, 0, 0.5f, 1);
                Gizmos.DrawWireCube(transform.position + Vector3.up, Vector3.one);
                break;
            default:
                break;
        }
    }
}
