using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CompositeCreator
{
    public static Composite CreateBox(Vector2 position, bool pinRandomCorner = true)
    {
        Composite comp = new Composite();
        Vector2 center = position;

        float stiffness = 0.25f;
        float size = 200f;

        Particle c1 = new Particle(center + new Vector2(-size, size));
        Particle c2 = new Particle(center + new Vector2(size, size));
        Particle c3 = new Particle(center + new Vector2(size, -size));
        Particle c4 = new Particle(center + new Vector2(-size, -size));
        Constraint e1 = new DistanceConstraint(c1, c2, stiffness);
        Constraint e2 = new DistanceConstraint(c2, c3, stiffness);
        Constraint e3 = new DistanceConstraint(c3, c4, stiffness);
        Constraint e4 = new DistanceConstraint(c4, c1, stiffness);
        Constraint e5 = new DistanceConstraint(c1, c3, stiffness);
        Constraint e6 = new DistanceConstraint(c2, c4, stiffness);

        List<Particle> p = new[] { c1, c2, c3, c4 }.ToList();
        List<Constraint> c = new[] { e1, e2, e3, e4, e5, e6 }.ToList();

        if (pinRandomCorner)
        {
            Constraint pin = new PinConstraint(p[Random.Range(0, p.Count())]);
            c.Add(pin);
        }

        comp.particles.AddRange(p);
        comp.constraints.AddRange(c);

        VerletHandler.Instance.CreateBody(comp);

        return comp;
    }

    public static Composite CreateRope(Vector2 position)
    {
        Composite comp = new Composite();
        Vector2 center = position;

        float stiffness = 0.25f;
        float segSize = 200f;
        int segs = 12;

        Particle lastParticle = null;
        for (int i = 0; i < segs; i++)
        {
            Particle currentParticle = new Particle(center + new Vector2(segSize * i, 0));
            comp.particles.Add(currentParticle);

            if (lastParticle != null)
            {
                comp.constraints.Add(new DistanceConstraint(lastParticle, currentParticle, stiffness));
            }
            else
            {
                comp.constraints.Add(new PinConstraint(currentParticle));
            }

            lastParticle = currentParticle;
        }

        VerletHandler.Instance.CreateBody(comp);

        return comp;
    }
}
