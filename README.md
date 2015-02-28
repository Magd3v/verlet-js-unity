verlet-js-unity by <a href="http://twitter.com/magdvv">@magdvv</a>

a C# port of <a href="http://subprotocol.com/system/introducing-verlet-js.html">verlet-js</a> with some components to make it work in unity3d

includes a line renderer made using a <a href="https://github.com/bzgeb/UnityLineMeshes">rad technique</a> by 
<a href="http://twitter.com/bzgeb">@bzgeb</a><br>

<a href="http://whybotherplayinggameswhenyouwilljustdieandforgetthem.nfshost.com/verlet/">web player demo</a><br>

<b>things to note:</b>
- there's no collision between bodies. verlet-js didn't have this and as a result everything is 10x simpler and faster.
it wouldn't have been much use unless there was a bunch of optimzation on it e.g. quad trees.<br>
get creative! I came up with lots of cool game ideas that didn't involve collision.

<br>

<b>todo:</b>
- nicer, cleaner frontend code
- fix inconsistent naming conventions
- more transparent api
- optimize the line renderer. place vertices by shader? the current shader is weirdly slow.
- 3d support

<br>

<b>bugs:</b>
- if you make a boxrope and delete the rope some weird stuff happens. weird issue with the delete code maybe?

<br>
current issue with frontend:<br>
````c#
//grab a particle of a composite that exists in the game
Composite firstComposite = VerletHandler.Instance.World.composites[0];
Particle firstParticle = firstComposite.particles[0];

//okay cool, now add a particle to the existing composite, and constrain it to one of its particles
Particle otherParticle = new Particle(firstParticle.pos + new Vector2(32f, 0f));
firstComposite.constraints.Add(new DistanceConstraint(firstParticle, otherParticle, 0.5f));
firstComposite.particles.Add(otherParticle);

//huh??? it acts as if it's constrained, but it's not drawing the new line ingame!
//oh yeah forgot to call this method to rebuild the correlating GameObjects
VerletHandler.Instance.RebuildBody(firstComposite);

//this is bad!! requiring a function call will be error prone
//possible fix: use ObservableLists for the particle/composite lists
//then have it automatically call RebuildBody when a new particle/composite is added or removed?
````
<br>

Guide:<br>
````
particle:           a single point in the verlet simulation
````
````
constraint:         a... constraint. a tether between two particles
````
````
composition:        a collection of particles and constraints.
                    particles and constraints are not known by the physics simulation 
                    until they are part of a composition registered by the simulation
````
````
DistanceConstraint: elastic (if stiffness < 1) link between two particles
AngleConstraint:    I haven't tried this yet so idk lol
PinConstraint:      an "anchor". sticks a particle to a position
````
<br>
Example:
````c#
float stiffness = 0.25f;
float segSize = 200f;
int segs = 12;
Vector2 position = new Vector2(100, 100);

Composite comp = new Composite();

//create a string of particles constrained in a line
Particle lastParticle = null;
for (int i = 0; i < segs; i++)
{
  Particle currentParticle = new Particle(position + new Vector2(segSize * i, 0));
  comp.particles.Add(currentParticle);

  if (lastParticle != null) 
    comp.constraints.Add(new DistanceConstraint(lastParticle, currentParticle, stiffness));
  else
    comp.constraints.Add(new PinConstraint(currentParticle));
  
  lastParticle = currentParticle;
}

//tell the VerletHandler to build a GameObject model out of the composition so you can see it ingame
VerletHandler.Instance.CreateBody(comp);

````

that's how to generate bodies by code. you can also build them by hand in the editor if you wanted. heirarchy looks like:
````
body [VerletBody component]
  >pointa [VerletPoint component]
  >pointb [VerletPoint component]
  >line   [VerletConstraint component]
````
