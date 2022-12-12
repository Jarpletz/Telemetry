using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOrbit : MonoBehaviour
{

    [SerializeField] float fuelLeft = 100f;
    [SerializeField] float closeCallThreshold;

    [Header("Points Info")]



    [Header("Perfect Orbit info")]
    [SerializeField] float radiusRange;
    [SerializeField] Vector2 speedRange;
    [SerializeField] float dotProductRange;
    [SerializeField] ParticleSystem confetti;
    [SerializeField] float timeBetweenOrbits;
    [SerializeField] float minTimeInOrbit;

    [Header("Planet Color info")]
    [SerializeField] Color normalColor;
    [SerializeField] float colorLerpSpeed;
    [SerializeField] float atmosphereOpacity;

    [Header("Text Color info")]
    [SerializeField] Color enterOrbitTextColor;
    [SerializeField] Color closeCallTextColor;

    bool hasAwardedPoint = false;
    public bool hasAchievedPerfectOrbit = false;

    GameManager gm;
    RocketFuel rocketFuel;
    RocketMovement movement;
    Transform rocketPos;
    textSpawner textSpawner;

    soundManager sound;
    SpriteRenderer sprite;
    PlanetBehavior behavior;

    float orbitVelocity;
    float orbitAngularVelocity;
    float timeTillOrbit = 0;
    float timeInOrbit = 0;

    float Zrotation;

    private void Start()
    {
        behavior = GetComponentInParent<PlanetBehavior>();
        sprite = GetComponent<SpriteRenderer>();
        sound = GetComponent<soundManager>();
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        rocketFuel = GameObject.FindWithTag("Player").GetComponent<RocketFuel>();
        movement = GameObject.FindWithTag("Player").GetComponent<RocketMovement>();
        rocketPos = GameObject.FindWithTag("Player").transform;
        textSpawner = GameObject.FindWithTag("Player").GetComponent<textSpawner>();
        Zrotation = transform.parent.rotation.z;
    }
    private void Update()
    {
        if (!movement.inOrbit)
        {
            sprite.color = Color.Lerp(sprite.color, normalColor, colorLerpSpeed);
        }
        if (timeTillOrbit > 0)
        {
            timeTillOrbit -= Time.deltaTime;
        }
        if (movement.inOrbit)
        {
            timeInOrbit += Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Planet")
        {
            while (transform.parent.position.y > collision.transform.parent.transform.position.y)
            {
                transform.parent.position = new Vector3(transform.parent.position.x, transform.parent.position.y + 0.3f, transform.parent.position.z);
                Debug.LogWarning("Shifted Touching Planet Up!");
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (gm.isTutorial) return;
        if (!hasAchievedPerfectOrbit && movement.inOrbit) return;

        if (collision.tag == "Player")
        {

            if (!hasAwardedPoint && !gm.isDead)
            {
                hasAwardedPoint = true;
                if (gm.score >= 0)
                {
                    gm.planetsReached++;
                    if (rocketFuel.power < closeCallThreshold)
                    {
                        textSpawner.spawnText("Close Call!", closeCallTextColor, 3 * gm.multiplier);
                        sound.playSound("Score", 0.5f, 1 + (gm.multiplierPower / 4) + Random.Range(0, 0.3f));

                    }//if almost out of power when enters orbit, award extra points!
                    else { textSpawner.spawnText("", enterOrbitTextColor, 1 * gm.multiplier);
                        sound.playSound("Score", 0.5f, 1 + (gm.multiplierPower / 4) + Random.Range(-0.2f, 0.2f));

                    }
                }
                else gm.score = 0;



            }//award point for entering orbit
             ////////////////////Score///////////////

            if (fuelLeft > 0)
            {
                rocketFuel.fuel += rocketFuel.fuelRefillRate * Time.deltaTime;
                fuelLeft -= rocketFuel.fuelRefillRate * Time.deltaTime;
            }// the longer in orbit, the more fuel recieved (Max of full tank)

            rocketFuel.power += rocketFuel.powerRefillRate * Time.deltaTime;//Refill Power
            rocketFuel.usePower = false;//power consumption turned off
                                        ///////////////Fuel and Power///////////



            if (!movement.inOrbit && gm.orbitAssist)
            {
                findOrbitVelocity();
                findOrbitAngularVelocity();//calculate vels

                if (canDoOrbitAssist())
                {
                    enterOrbitAssist();
                }
            }

            if (movement.inOrbit && gm.orbitAssist)
            {
                doOrbitAssist();
            }
            ///////////////Orbit Assist////////////////


        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (gm.isTutorial) return;

        if (collision.tag == "Player")
        {
            rocketFuel.usePower = true;//re-enable power consumption
          //  if (movement.inOrbit && gm.orbitAssist) exitOrbitAssist();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, GetComponent<CircleCollider2D>().radius * transform.parent.transform.localScale.x);
    }

    void findOrbitVelocity()
    {
        orbitVelocity = Mathf.Sqrt(gm.G * behavior.mass / Vector3.Distance(transform.parent.position, rocketPos.position));
        // V = sqt( G * Mp / R )
    }
    void findOrbitAngularVelocity()
    {
        Vector3 crossProduct = Vector3.Cross((transform.position - rocketPos.position).normalized, rocketPos.transform.up);
        float angularSign = crossProduct.normalized.z;
        //determines the direction it spins. 1 for clockwise, -1 for counter-clockwise

        float omega = orbitVelocity / Vector3.Distance(transform.parent.position, rocketPos.position);
        //Determines the magnitude of the angular velocity

        orbitAngularVelocity = angularSign * omega * Mathf.Rad2Deg;
        //w= v/r
    }
    bool canDoOrbitAssist()
    {
        float dotProduct = Vector2.Dot((transform.position - rocketPos.position).normalized, rocketPos.transform.up);

        if (timeTillOrbit > 0)
        {//if too soon since this planet's last perfect orbit, return false.
            return false;
        }

        if (Mathf.Abs(dotProduct) > dotProductRange)
        {// if dot product of radius vector and rocket transform.up is too big, return false
            return false;
        }
        if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && gm.startedGame)
        {//if pressing a mouse button, return false
            return false;
        }

        if (Vector3.Distance(rocketPos.position, transform.position) < transform.parent.localScale.x * radiusRange)
        {// if the radius is too small, return false.
            return false;
        }

        float rocketSpeed = rocketPos.GetComponent<Rigidbody2D>().velocity.magnitude;
        if (rocketSpeed < orbitVelocity * speedRange.x || rocketSpeed > orbitVelocity * speedRange.y)
        {//if too fast or too slow, return false
            return false;
        }
        return true;
    }
    void enterOrbitAssist()
    {
        if (GlobalInfo.settings.lockOrbit|| !gm.startedGame)
        {
            movement.inOrbit = true;
            rocketPos.SetParent(transform.parent);//parent rocket to planet
            rocketPos.GetComponent<Rigidbody2D>().velocity = Vector3.zero;//zero rocket external velocity

            timeTillOrbit = timeBetweenOrbits;
            timeInOrbit = 0;//timerStuff
        }

        if (!hasAchievedPerfectOrbit)
        {
            if (gm.multiplier >= 2) { gm.multiplier += 2; }
            else gm.multiplier++;
            gm.multiplierPower++;
            gm.numbPerfectOrbits++;//update multiplier values

            if (gm.multiplierPower - 1 >= gm.muliplierColors.Count)
            {
                gm.muliplierColors.Add(Random.ColorHSV(0f, 1f, 0.6f, 1f, 0.9f, 1f));
            }//create new multiplier colors, if necissary

            var main = confetti.main;
            main.startColor = gm.muliplierColors[Mathf.RoundToInt(gm.multiplier/2f) - 1];
            confetti.Play();
            textSpawner.spawnText("Perfect Orbit!", gm.muliplierColors[Mathf.RoundToInt(gm.multiplier / 2f) - 1], 5 * gm.multiplier);
            sound.playSound("Combo", 0.5f, 1 + (gm.multiplierPower / 4));
            hasAchievedPerfectOrbit = true;
        }
    }
    void doOrbitAssist()
    {
        if (gm.startedGame)
        {

            if (gm.multiplierPower > 0)
            {
                Color newColor = new Color(gm.muliplierColors[gm.multiplierPower - 1].r, gm.muliplierColors[gm.multiplierPower - 1].g, gm.muliplierColors[gm.multiplierPower - 1].b, atmosphereOpacity);

                sprite.color = Color.Lerp(sprite.color, newColor, colorLerpSpeed);
            }
            else {
                Color newColor = new Color(gm.muliplierColors[0].r, gm.muliplierColors[0].g, gm.muliplierColors[0].b, atmosphereOpacity);
                sprite.color = Color.Lerp(sprite.color, newColor, colorLerpSpeed); 
            }
        }

        if (GlobalInfo.settings.lockOrbit || gm.score<=1)
        {
            Zrotation -= orbitAngularVelocity * Time.deltaTime;
            transform.parent.rotation = Quaternion.Euler(new Vector3(0, 0, Zrotation));
            //rotate planet

            movement.currentVelocity = orbitVelocity * rocketPos.up;//set movement current velocity var (for trajectory prediction)

            if ((Input.GetMouseButton(0) || Input.GetMouseButton(1)) && gm.startedGame && timeInOrbit > minTimeInOrbit)
            {
                exitOrbitAssist();
            }
        }
    }
    void exitOrbitAssist()
    {
        movement.inOrbit = false;
        rocketPos.GetComponent<Rigidbody2D>().velocity = orbitVelocity * rocketPos.up;
        rocketPos.SetParent(null);
    }

}
