using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo
{
    GameObject item;

    GridInfo()
    {
        item = null;
    }

    GridInfo(GameObject thing)
    {
        item = thing;
    }

}

public class MovementSpaces : MonoBehaviour
{
    GridInfo[,] grid;
    
    // Start is called before the first frame update
    void Start()
    {
        grid = new GridInfo[4, 4];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
