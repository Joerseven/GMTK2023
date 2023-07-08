using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class PopupItem : IQueueItem
{
    float totaltime;
    float elapsed;
    Animator info;

    public PopupItem(Animator animator)
    {
        info = animator;
    }

    public bool IsFinished()
    {
        var stateInfo = info.GetCurrentAnimatorStateInfo(0);
        bool isFinshed = stateInfo.IsName("Mole_Idle");

        return isFinshed;
    }

    public void Update(float dt)
    {
        
    }
}

public class Mole : MonoBehaviour, ITurnTaker
{
    private bool chaseable = false;
    [SerializeField]
    private int popupCounter = 3;
    private Animator animator;
    public IQueueItem[] TakeTurn()
    {
        popupCounter -= 1;
        if (popupCounter == 0)
        {

        }

        return new IQueueItem[] { new EmptyItem() };
    }

    public bool ShouldChase()
    {
        return chaseable;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
