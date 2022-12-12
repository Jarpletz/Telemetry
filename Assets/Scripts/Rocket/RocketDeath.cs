using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketDeath : MonoBehaviour
{
    [SerializeField] float drag;
    [SerializeField] float fadeSpeed;
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeMagnitude;

    [SerializeField] ParticleSystem[] thrustParticles;
    [SerializeField] ParticleSystem explosionParticles;

    float xRange;
    bool fadeSprites = false;

    Rigidbody2D rb;
    RocketFuel fuel;
    DeathUI deathUI;
    GameManager gm;
    screenShake shake;
    soundManager sound;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        fuel = GetComponent<RocketFuel>();
        sound = GetComponent<soundManager>();
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        if (!gm.isTutorial)deathUI = GameObject.FindWithTag("UI").GetComponent<DeathUI>();
        shake = Camera.main.GetComponent<screenShake>();

        xRange=Camera.main.orthographicSize* Screen.width / Screen.height * 1.5f;
    }
    private void Update()
    {
        if (!gm.isDead) fadeSprites = false;//For respawn, easier than callig a script reference just for this little var I forget about

        if (gm.isDead || gm.isTutorial)
        {
            if (fadeSprites)
            {
                SpriteRenderer sprite = GetComponent<SpriteRenderer>();
                Color spriteColor = new Color(sprite.color.r, sprite.color.g, sprite.color.b, Mathf.Lerp(sprite.color.a, 0, fadeSpeed));
                sprite.color = spriteColor;
            }
            return;
        }

        if (transform.position.x > xRange || transform.position.x < -xRange)
        {
            onDeath(DeathUI.deathCauses.outOfBounds);
            sound.playSound("OutOfPower");
        }
        //if out of bounds, Die.

        if (fuel.power <= 0)
        {
            GlobalInfo.info.powerOutages++;
            onDeath(DeathUI.deathCauses.outOfPower);
            sound.playSound("OutOfPower");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Planet" && !fadeSprites)
        {
            if (!gm.isTutorial && !gm.isDead)
            {
                GlobalInfo.info.crashes++;
                onDeath(DeathUI.deathCauses.hitPlanet);
            }
            explosionParticles.Play();

            sound.playSound("Explosion");
            fadeSprites = true;
            rb.drag = 100f;
            StartCoroutine(shake.ScreenShake(shakeDuration, shakeMagnitude));
            foreach (ParticleSystem p in thrustParticles)
            {
                p.Stop();
            }
        }
    }

    void onDeath(DeathUI.deathCauses cause)
    {
        
        gm.isDead = true;
        deathUI.causeOfDeath = cause;
        deathUI.onDeath();//ui death stuff
        gm.updateSavedData();//update save data
        rb.drag = drag;

    }

    
}
