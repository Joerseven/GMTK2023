using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

public class MoveQueueItem : IQueueItem
{

    Transform subject;
    Vector3 start;
    Vector3 target;
    float elapsed;
    float totaltime;
    bool finished;
    Animator animator;

    public MoveQueueItem(Transform item, Vector3 to, float time, Animator anim)
    {
        elapsed = 0;
        totaltime = time;
        finished = false;
        target = to;
        subject = item;
        animator = anim;
    }

    private void Start()
    {
        start = subject.position;
        animator.Play("Farmer_Walk");
    }

    public bool IsFinished()
    {
        return finished;
    }

    public void Update(float dt)
    {
        if (finished)
        {
            return;
        }

        if (elapsed == 0)
        {
            Start();
        }

        elapsed += dt;

        if (elapsed >= totaltime)
        {
            finished = true;
            animator.Play("Farmer_Idle");
            elapsed = totaltime;
        }

        subject.position = Vector3.Lerp(start, target, elapsed / totaltime);
    }
}

public class Movement : MonoBehaviour
{
    public Grid thisgrid;
    public MovementSpaces spaces;

    private void Start()
    {
        thisgrid = GetComponentInParent<Grid>();
        spaces = GetComponentInParent<MovementSpaces>();
    }
    public void SnapTo(int x, int y)
    {
        spaces.NewPlace(this.gameObject, x, y);
        transform.position = thisgrid.GetCellCenterLocal(new Vector3Int(x, y, 0));
    }

    public MoveQueueItem MoveTo(int x, int y, Animator anim)
    {
        spaces.NewPlace(gameObject, x, y);
        return new MoveQueueItem(transform, thisgrid.GetCellCenterLocal(new Vector3Int(x, y, 0)), 3, anim);
    }

    public Vector3Int GetPosition()
    {
        return thisgrid.LocalToCell(transform.position);
    }

    void LoadLevel()
    {

    }
}
