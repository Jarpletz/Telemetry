using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketRespawn : MonoBehaviour
{
    [SerializeField] ParticleSystem secondaryThrustParticles;
    [SerializeField] AudioSource thrustAmbience;
    GameManager gm;
    RocketMovement movement;
    RocketFuel fuel;
    Rigidbody2D rb;
    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        rb=GetComponent<Rigidbody2D>();
        fuel = GetComponent<RocketFuel>();
        movement = GetComponent<RocketMovement>();

    }

    public void respawnRocket()
    {
        gm.isDead = false;
        gm.hasRespawned = true;


        // reset Rigidbody and Rotation
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = 0;
        transform.rotation = Quaternion.identity;
        rb.rotation = 0;
        rb.drag = 0;

        //re-enable sprite
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color spriteColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1);
        sprite.color = spriteColor;

        //Refill fuel and power
        fuel.fuel = 100;
        fuel.power = 100;

        //Reset Particles and sound
        secondaryThrustParticles.Play();
        thrustAmbience.Play();

        PlanetBehavior startPlanet = findNearestPlanet();

        movement.initilaize(startPlanet);

        startPlanet.GetComponentInChildren<OnOrbit>().hasAchievedPerfectOrbit = true;//prevent planet it respawns on from awarding a perfect orbit.
    }
    PlanetBehavior findNearestPlanet()
    {
        PlanetBehavior nearestPlanet=gm.planets[0];
        float smallestDist = Vector3.Distance(transform.position, gm.planets[0].transform.position);

        foreach (PlanetBehavior p in gm.planets){
           if(Vector3.Distance(transform.position, p.transform.position) < smallestDist)
            {
                nearestPlanet = p;
                smallestDist = Vector3.Distance(transform.position, p.transform.position);
            }
        }


        return nearestPlanet;
    }

}
