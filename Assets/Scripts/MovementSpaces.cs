using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo
{
    GameObject item;

    public GridInfo()
    {
        item = null;
    }

    public GridInfo(GameObject thing)
    {
        item = thing;
    }

    public bool HasItem()
    {
        if (item == null) return false;
        return true;
    }

    public GameObject GetItem()
    {
        return item;
    }

    public void SetItem(GameObject thing)
    {
        item = thing;
    }

    public void ClearItem()
    {
        item = null;
    }

}

public class MovementSpaces : MonoBehaviour
{
    GridInfo[,] grid;

    void Awake()
    {
        grid = new GridInfo[8, 5];
        for (int i = 0; i < grid.GetLength(0); i++)
            for (int j = 0; j < grid.GetLength(1); j++)
                grid[i, j] = new GridInfo();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public GridInfo CheckGrid(int x, int y)
    {
        return grid[x,y];
    }

    public void NewPlace(GameObject thing, int x, int y)
    {

        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (!grid[i, j].HasItem()) continue;
                if (grid[i, j].GetItem() == thing)
                {
                    grid[i, j].ClearItem();
                }
            }
        }

        grid[x, y].SetItem(thing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
