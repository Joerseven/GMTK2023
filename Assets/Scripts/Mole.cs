using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour, ITurnTaker
{
    private bool chaseable = true;
    public IQueueItem[] TakeTurn()
    {
        throw new System.NotImplementedException();
    }

    public bool ShouldChase()
    {
        return chaseable;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
