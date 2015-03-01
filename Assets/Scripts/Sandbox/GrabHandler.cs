using UnityEngine;

public class GrabHandler : Singleton<GrabHandler>
{
    bool isHolding;
    Ray cameraRay;
    Plane gamePlane;
    float rayDist = 1000f;
    Composite mouseGrabComp;
    Particle mouseGrabParticle;
    Camera cam;
    Particle interactingParticle;

    public Vector2 MouseGamePos { get; private set; }
    public Vector3 MouseGlobalPos { get; private set; }
    
    void Awake()
    {
        cam = GetComponent<Camera>();
        gamePlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            foreach (Composite c in VerletHandler.instance.World.composites)
            {
                foreach (Particle p in c.particles)
                {
                    if (Vector2.Distance(MouseGamePos, p.pos) < 200f)
                    {
                        interactingParticle = p;
                        break;
                    }
                }
            }
        }

        if (interactingParticle != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHolding = true;
                mouseGrabComp = new Composite();
                mouseGrabComp.particles.Add(new Particle(MouseGamePos));
                mouseGrabComp.constraints.Add(new DistanceConstraint(interactingParticle, mouseGrabComp.particles[0],
                    0.02f));
                VerletHandler.Instance.CreateComposite(mouseGrabComp);
            }

            if (Input.GetMouseButtonDown(1))
            {
                for (int i = 0; i < VerletHandler.instance.World.composites.Count; i++)
                {
                    Composite c = VerletHandler.instance.World.composites[i];
                    if (c.particles.Contains(interactingParticle))
                    {
                        VerletHandler.instance.DestroyComposite(c);
                        interactingParticle = null;
                        break;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isHolding && interactingParticle != null)
        {
            isHolding = false;
            interactingParticle = null;

            if (mouseGrabComp != null)
            {
                VerletHandler.Instance.DestroyComposite(mouseGrabComp);
                mouseGrabComp = null;
            }
        }

        cameraRay = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        gamePlane.Raycast(cameraRay, out rayDist);
        MouseGlobalPos = cameraRay.GetPoint(rayDist);
        MouseGamePos = new Vector2(MouseGlobalPos.x, MouseGlobalPos.y);

        if (isHolding)
        {
            mouseGrabComp.particles[0].pos = MouseGamePos;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(MouseGlobalPos, Vector3.one * 100f);
    }

    void OnGUI()
    {
        //GUI.Label(new Rect(0, 0, 1000, 200), MouseGamePos.ToString());

        if (mouseGrabComp != null)
        {
            //GUI.Label(new Rect(0, 18, 1000, 200), mouseGrabComp.particles[0].pos.ToString());
        }
    }
}
