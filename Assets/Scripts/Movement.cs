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

    public MoveQueueItem(Transform item, Vector3 to, float time)
    {
        elapsed = 0;
        totaltime = time;
        finished = false;
        target = to;
        subject = item;
    }

    private void Start()
    {
        start = subject.position;
    }

    public bool IsFinished()
    {
        return finished;
    }

    public void Update(float dt)
    {

        if (elapsed == 0)
        {
            Start();
        }

        elapsed += dt;

        if (elapsed >= totaltime)
        {
            finished = true;
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
    }
    public void SnapTo(int x, int y)
    {
        transform.position = thisgrid.GetCellCenterLocal(new Vector3Int(x, y, 0));
    }

    public MoveQueueItem MoveTo(int x, int y)
    {
        return new MoveQueueItem(transform, thisgrid.GetCellCenterLocal(new Vector3Int(x, y, 0)), 1);
    }

    public Vector3Int GetPosition()
    {
        return thisgrid.LocalToCell(transform.position);
    }

    void LoadLevel()
    {

    }
}
