using UnityEngine;

public class GrabHandler : Singleton<GrabHandler>
{
    bool isHolding;
    Particle holdingParticle;
    Ray cameraRay;
    Plane gamePlane;
    float rayDist = 1000f;
    Composite mouseGrabComp;
    Particle mouseGrabParticle;
    Camera cam;

    public Vector2 MouseGamePos { get; private set; }
    public Vector3 MouseGlobalPos { get; private set; }
    
    void Awake()
    {
        cam = GetComponent<Camera>();
        gamePlane = new Plane(Vector3.forward, Vector3.zero);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isHolding)
        {
            RaycastHit info;
            if (Physics.SphereCast(cameraRay, 5f, out info, 1000f))
            {
                var vPoint = info.collider.gameObject.GetComponent<VerletPoint>();
                if (vPoint != null)
                {
                    holdingParticle = vPoint.particle;
                    isHolding = true;
                    mouseGrabComp = new Composite();
                    mouseGrabComp.particles.Add(new Particle(MouseGamePos));
                    mouseGrabComp.constraints.Add(new DistanceConstraint(holdingParticle, mouseGrabComp.particles[0], 0.02f));
                    VerletHandler.Instance.CreateBody(mouseGrabComp);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit info;
            if (Physics.SphereCast(cameraRay, 5f, out info, 1000f))
            {
                var vPoint = info.collider.gameObject.GetComponent<VerletPoint>();
                if (vPoint != null)
                {
                    VerletHandler.instance.DestroyBody(vPoint.parentComp);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && holdingParticle != null)
        {
            isHolding = false;
            holdingParticle = null;

            if (mouseGrabComp != null)
            {
                VerletHandler.Instance.DestroyBody(mouseGrabComp);
                mouseGrabComp = null;
            }
        }

        cameraRay = cam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        gamePlane.Raycast(cameraRay, out rayDist);
        MouseGlobalPos = cameraRay.GetPoint(rayDist);
        MouseGamePos = new Vector2(MouseGlobalPos.x, -MouseGlobalPos.y);

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
