using UnityEngine;

public class VerletPoint : MonoBehaviour
{
    public Composite parentComp;
    public Particle particle;
    public bool Anchored;

    public void LateUpdate()
    {
        transform.position = new Vector3(particle.pos.x, particle.pos.y, 0);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = !Anchored ? new Color(0.5f, 1f, 0.5f) : new Color(1f, 0.5f, 0.5f);
        Gizmos.DrawWireCube(transform.position, Vector3.one * 40f);

        if (particle != null)
        {
            Vector2 delta = particle.pos - particle.lastPos;
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position,
                new Vector3(particle.pos.x, particle.pos.y, 0) + new Vector3(delta.x, delta.y, 0));
        }
    }
}
