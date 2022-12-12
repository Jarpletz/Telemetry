using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketFuel : MonoBehaviour
{
    [SerializeField] Color outOfFuelColor;
    [Header("FUUUUUUEEEELLLLLLLL!")]
    public float fuel;
    public float power;
    public bool usePower;

    bool outOfFuel;

    [Header("Fuel/Power per second")]
    public float fuelConsumeRate;
    public float powerConsumeRate;
    public float fuelRefillRate;
    public float powerRefillRate;
    [Header("Secondary Booster effects")]
    [SerializeField]ParticleSystem thrustParticles;
    [SerializeField]AudioSource thrustAmbience;
    [SerializeField] float thrustAmbienceVolume;

    textSpawner textSpawner;

    private void Start()
    {
        textSpawner = GetComponent<textSpawner>();
    }

    private void Update()
    {
        thrustAmbience.volume = GlobalInfo.settings.sfxVolume * thrustAmbienceVolume;
        if (fuel  > 100) fuel  = 100;
        if (power > 100) power = 100;

        if (fuel  < 0)   fuel  = 0;
        if (power < 0)   power = 0;

        if (usePower)
        {
            power -= powerConsumeRate * Time.deltaTime;
        }

        if(fuel==0 && !outOfFuel)
        {
            outOfFuel = true;
            textSpawner.spawnText("Out Of Fuel", outOfFuelColor, 0);
            thrustParticles.Stop();
            thrustAmbience.Stop();
        }
        if(fuel>0 && thrustParticles.isStopped &&!GameObject.FindWithTag("GameController").GetComponent<GameManager>().isDead)
        {
            thrustParticles.Play();
            thrustAmbience.Play();
        }
        else if (GameObject.FindWithTag("GameController").GetComponent<GameManager>().isDead)
        {
            thrustParticles.Stop();
            thrustAmbience.Stop();
        }
        //deal with the secondary thrust particles and sound. idk why i do it here.

        if (fuel > 0 && outOfFuel) outOfFuel = false;
    }
}
