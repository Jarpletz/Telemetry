using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketMovement : MonoBehaviour
{
    [Header("Rocket")]
    public bool inOrbit;
    [SerializeField] bool isTutorial;
    [SerializeField] float thrustForce;
    [SerializeField] float sideThrustForce;
    [SerializeField] KeyCode rightKey;
    [SerializeField] KeyCode leftKey;
    [SerializeField] bool useAllPlanets;
    public Vector2 currentVelocity;// just rb.velocity, unless in perfect orbit, in which case it is determined by planet onOrbit script. Used for the initial velocity of the trajectory prediction/

    float mass;
    [SerializeField]float startSpeed;
    Vector2 totalForce;


    [Header("Graphics")]
    [SerializeField] ParticleSystem thrustParticles;
    [Header("Audio")]
    [SerializeField] AudioSource thrustAudio;
    [SerializeField] float thrustAudioLerpSpeed;
    [SerializeField] float pitchIncrease;
    [SerializeField] float backThrustVolume;

    [Header("Screen Shake")]
    [SerializeField] float shakeMagnitude;

    [Header("Trajectory")]
    [SerializeField] float trajectoryDeltaTime;
    [SerializeField] LayerMask planetLayer;

    [System.Serializable]
    class trajectoryPoint
    {
        public Vector2 pos;
        public Vector2 vel;
        public Vector2 acc;
        public trajectoryPoint()
        {
            pos = Vector2.zero;
            vel = Vector2.zero;
            acc = Vector2.zero;
        }
    }
    [SerializeField]trajectoryPoint[] points = new trajectoryPoint[100];

    RocketFuel rocketFuel;
    Rigidbody2D rb;
    GameManager gm;
    LineRenderer trajectoryLine;
    screenShake shake;

    bool up = false;
    bool down = false;
    float leftTouchID=-1;
    float rightTouchID=-1;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rocketFuel = GetComponent<RocketFuel>();
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        trajectoryLine= GetComponentInChildren<LineRenderer>();
        shake = Camera.main.GetComponent<screenShake>();//find all them components

        if (!gm.isTutorial)
        {
            initilaize(gm.planets[0]);
        }
        rb.velocity = startSpeed * transform.up;
        mass = rb.mass;
        trajectoryLine.positionCount=points.Length;//set all initial values
    }

    private void Update()
    {

        if(gm.isTutorial && !isTutorial)
        {
            rb.position = new Vector2(0, rb.position.y);
        }//if rocket is for tutorial but is interactible, lock it in the center of the screen.
        if (isTutorial) return;

        if (gm.isDead|| !gm.startedGame || rocketFuel.fuel<=0)
        {
            return;
        }

        checkInputs();

    }
    private void FixedUpdate()
    {

        if (!gm.startedGame ) return;

        if (inOrbit || gm.isDead)
        {
            updateTrajectory();//update the trajectory prediction
            thrustAudio.volume = Mathf.Lerp(thrustAudio.volume, 0, thrustAudioLerpSpeed);
            return;
        }

        currentVelocity = rb.velocity;
        totalForce = returnGravitationalForce((Vector2)transform.position);//finds gravitational force on rocket

        if (up && rocketFuel.fuel > 0 && !isTutorial)
        {
            totalForce += (Vector2)transform.up * thrustForce;
            if(!gm.isTutorial)rocketFuel.fuel -= rocketFuel.fuelConsumeRate * Time.deltaTime;
         //   StartCoroutine(shake.ScreenShake(Time.deltaTime, shakeMagnitude));
            thrustAudio.volume = Mathf.Lerp(thrustAudio.volume, 1*GlobalInfo.settings.sfxVolume, thrustAudioLerpSpeed);
            thrustAudio.pitch = 1;
        }//if left mouse button, give forward thrust and do everthing related
        else thrustAudio.volume = Mathf.Lerp(thrustAudio.volume, 0, thrustAudioLerpSpeed);
        

        if (down && rocketFuel.fuel > 0 && !isTutorial)
        {
           totalForce += -(Vector2)transform.up * thrustForce;
            if (!gm.isTutorial) rocketFuel.fuel -= rocketFuel.fuelConsumeRate * Time.deltaTime; 
           //StartCoroutine(shake.ScreenShake(Time.deltaTime, shakeMagnitude));
            thrustAudio.pitch = pitchIncrease;
           thrustAudio.volume = Mathf.Lerp(thrustAudio.volume, backThrustVolume * GlobalInfo.settings.sfxVolume, thrustAudioLerpSpeed);
        }//if right mouse button, give backward thrust and do everthing related
        else thrustAudio.volume = Mathf.Lerp(thrustAudio.volume, 0, thrustAudioLerpSpeed);


        float horizontalAxis = Input.GetAxis("Horizontal");
        if (horizontalAxis != 0)
        {
            totalForce +=(Vector2)transform.right * horizontalAxis*sideThrustForce * Time.deltaTime;
            Debug.Log((Vector2)transform.right * horizontalAxis * sideThrustForce * Time.deltaTime);
            rocketFuel.fuel -= rocketFuel.fuelConsumeRate * Time.deltaTime;
        }//add side force

        rb.AddForce(totalForce);//add net force to rocket
        transform.up = Vector2.Lerp(transform.up,rb.velocity, 1);//rotate rocket to face direction of movement
        if (rb.velocity.magnitude < 0.2) rb.velocity = 0.2f * rb.velocity.normalized;
        updateTrajectory();//update the trajectory prediction
    }
    void updateTrajectory()
    {
        if (!GlobalInfo.settings.predictTrajectory)
        {
            trajectoryLine.enabled = false;
            return;
        }

        float dt = trajectoryDeltaTime*Time.fixedDeltaTime;

        points[0].pos = transform.position;
        points[0].vel = currentVelocity;
        points[0].acc = totalForce / mass;
        trajectoryLine.SetPosition(0, points[0].pos);//set kinematics for first point

        for (int i = 1; i < points.Length; i++)
        {
            if (gm.isDead)
            {
                points[i].acc = Vector2.zero;
                //A=Fg at that point/m
                points[i].vel = Vector2.zero;
                points[i].pos = points[0].pos;
            }
            else
            {
                points[i].acc = (returnGravitationalForce(points[i - 1].pos) / mass);
                //A=Fg at that point/m
                points[i].vel = points[i - 1].vel + points[i].acc * dt;
                //V = Vprev. + A*dt 
                points[i].pos = 0.5f * points[i].acc * Mathf.Pow(dt, 2) + points[i - 1].vel * dt + points[i - 1].pos;
                //D = 0.5* A * dt^2  +  Vprev. * dt  +  Dprev.
                //calculates kinematics for other points

                if (Physics2D.OverlapCircle(points[i-1].pos, 0.2f, planetLayer) != null)
                {
                    points[i].pos = points[i - 1].pos;
                    
                }//checks if inside of a planet (BROKEN)
            }
            trajectoryLine.SetPosition(i, points[i].pos);
        }
    }

 
    Vector2 returnGravitationalForce(Vector2 position)
    {
        Vector2 netForce=Vector2.zero;
            for (int i = 0; i < gm.planets.Count; i++)
            {
            if (!gm.planets[i].gameObject.activeSelf) continue;

                Vector2 dist = (Vector2)gm.planets[i].transform.position - position;
                Vector2 direction = dist.normalized;
                float magnitude= gm.G * mass * gm.planets[i].mass / Mathf.Pow(dist.magnitude, 2);
                //Fg= k * ship mass * plant's mass / distance^2
                Vector2 newF = direction * magnitude;
                netForce += newF;
            }
        return netForce;
    }

    void checkInputs()
    {
        if (isTutorial) return;
        if (gm.isMobile)
        {
            
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Vector3 touchPos = Camera.main.ScreenToViewportPoint(Input.GetTouch(i).position);

                    //end thust
                    if (Input.GetTouch(i).fingerId == leftTouchID && Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        
                        down = false;
                        leftTouchID = -1;
                    }//if is the left touch, and is ending, end the back thrust
                    else if (Input.GetTouch(i).fingerId == rightTouchID && Input.GetTouch(i).phase == TouchPhase.Ended)
                    {
                        thrustParticles.Stop();
                        up = false;
                        rightTouchID = -1;
                    }//if is the left touch, and is ending, end the forward thrust


                    //begin thrust
                    else if(leftTouchID==-1 && touchPos.x <= 0.5)
                    {
                        if (!up)
                            down = true;
                        leftTouchID = Input.GetTouch(i).fingerId;
                    }//if no left touch and touch is on the left half, start forward thrust
                    else if (rightTouchID == -1 && touchPos.x > 0.5)
                    {
                       
                        if (!down) 
                            up = true;
                        thrustParticles.Play();
                        rightTouchID = Input.GetTouch(i).fingerId;

                    }//if no right touch and touch is on the right half, start backward thrust
                }
            }



            return;
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
            {//if ending forward thrust 
                thrustParticles.Stop();
                up = false;
            }
            if (Input.GetMouseButtonUp(1))
            {//if ending backward thrust 
                down = false;
            }
            if (Input.GetMouseButtonDown(0))
            {//if beginning to add forward thrust
                if (!down) up = true;
                thrustParticles.Play();
            }
            if (Input.GetMouseButtonDown(1))
            {//if beginning to add backward thrust
                if (!up) down = true;
            }


        }



    }

    public void initilaize(PlanetBehavior planet)
    {
        float startX = planet.transform.position.x + planet.transform.localScale.x;
        transform.position = new Vector3(startX, planet.transform.position.y, 0);
        //find and set optimal start position for rocket


        startSpeed = Mathf.Sqrt(gm.G * planet.mass / Mathf.Abs(planet.transform.position.x - transform.position.x));
        // V = sqt( G * Mp / R )
        //find optimal start speed for rocket

        rb.velocity = startSpeed * transform.up;

    }

}
