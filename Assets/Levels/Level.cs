using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    public Vector2Int FarmerStart;
    public List<int> MolesStart;
    public List<bool> MolesGround;
    public List<Vector2Int> Obstacles;
}
