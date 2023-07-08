using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo
{
    Farmer farmer;
    Mole mole;
    GameObject terrain;

    public GridInfo()
    {
        farmer = null;
        mole = null;
        terrain = null;
    }

    public bool HasItem()
    {
        if (farmer == null && mole == null && terrain == null) return false;
        return true;
    }

    public GameObject GetItem()
    {
        if (farmer != null) return farmer.gameObject;
        else if (mole != null) return mole.gameObject;
        else if (terrain != null) return terrain;
        else return null;
    }

    public void SetItem(GameObject thing)
    {
        if (thing.TryGetComponent<Mole>(out var m))
        {
            mole = m;
        }

        if (thing.TryGetComponent<Farmer>(out var f))
        {
            farmer = f;
        }
    }

    public void ClearItem(GameObject thing)
    {
        if (thing.TryGetComponent<Farmer>(out var f))
        {
            if (farmer == f)
            {
                farmer = null;
            }
        } 
        
        else if (thing.TryGetComponent<Mole>(out var m)) {
            if (mole == m)
            {
                mole = null;
            }
        }
        
        else if (thing == terrain)
        {
            terrain = null;
        }
    }

    public GameObject GetTerrain()
    {
        return terrain;
    }

    public Mole GetMole()
    {
        return mole;
    }

    public Farmer GetFarmer()
    {
        return farmer;
    }

}

public class MovementSpaces : MonoBehaviour
{
    GridInfo[,] grid;
    Mole[] shelf;

    void Awake()
    {
       
    }

    public void BeginLevel()
    {
        int columns = 8;
        int rows = 5;
        grid = new GridInfo[columns, rows];
        for (int i = 0; i < grid.GetLength(0); i++)
            for (int j = 0; j < grid.GetLength(1); j++)
                grid[i, j] = new GridInfo();

        shelf = new Mole[columns];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void ShelfMole(Mole mole)
    {
        ClearOldPlace(mole.gameObject);
        int i = 0;
        foreach (var m in shelf)
        {
            if (m == null)
            {
                shelf[i] = mole;
                mole.transform.position = GetComponent<Grid>().GetCellCenterLocal(new Vector3Int(-1, i, 0));
                return;
            }

            i++;
        }
    }

    public bool CheckShelfEmpty()
    {
        foreach (var m in shelf)
        {
            if (m != null)
            {
                return false;
            }
        }

        return true;
    }

    public void DeShelfMole(Mole mole)
    {
        for (int i=0,j=0; i<shelf.Length; i++,j++)
        {
            shelf[j] = shelf[i];
            
            if (shelf[j] != null)
            {
                shelf[j].transform.position = GetComponent<Grid>().GetCellCenterLocal(new Vector3Int(-1, j, 0));
            }

            if (shelf[i] == mole)
            {
                shelf[i] = null;
                j--;
            };

        }
    }

    public GridInfo CheckGrid(int x, int y)
    {
        return grid[x,y];
    }

    public bool ValidPlacement(int x, int y)
    {
        if (x < 0 || x >= grid.GetLength(0)) return false;
        if (y < 0 || y >= grid.GetLength(1)) return false;
        if (grid[x, y].GetItem() != null) return false;
        return true;
    }

    public void ClearOldPlace(GameObject thing)
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                if (!grid[i, j].HasItem()) continue;
                if (grid[i, j].GetItem() == thing)
                {
                    grid[i, j].ClearItem(thing);
                }
            }
        }
    }

    public void NewPlace(GameObject thing, int x, int y)
    {

        ClearOldPlace(thing);
        grid[x, y].SetItem(thing);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
