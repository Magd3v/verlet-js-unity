using System.Linq;
using UnityEngine;

public class SandboxController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Composite box = CompositeCreator.CreateBox(GrabHandler.Instance.MouseGamePos);
            VerletHandler.Instance.CreateComposite(box);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Composite rope = CompositeCreator.CreateRope(GrabHandler.Instance.MouseGamePos);
            VerletHandler.Instance.CreateComposite(rope);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Composite rope = CompositeCreator.CreateRope(GrabHandler.Instance.MouseGamePos);
            Particle lastRopeSegment = rope.particles.Last();
            
            Composite box = CompositeCreator.CreateBox(lastRopeSegment.pos, false);
            box.constraints.Add(new DistanceConstraint(box.particles[0], lastRopeSegment, 0.25f));
            
            VerletHandler.Instance.CreateComposite(box + rope);
        }
    }
}
