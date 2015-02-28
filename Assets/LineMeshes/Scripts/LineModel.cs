using UnityEngine;

public class LineModel : ScriptableObject
{
    [SerializeField]
    public Vector3[] vertices;
    [SerializeField]
    public int[] lineSegments;
}
