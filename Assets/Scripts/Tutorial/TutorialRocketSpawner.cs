using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialRocketSpawner : MonoBehaviour
{
    [SerializeField] GameObject rocket;
    [SerializeField] float lifeTime;
    [SerializeField] float spawnRate;

    List<GameObject> rockets = new List<GameObject>();
    List<float> times = new List<float>();

    float timer;

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GameObject newRocket=Instantiate(rocket, transform.position, Quaternion.identity);
            newRocket.transform.SetParent(transform.parent);
            timer = spawnRate;
            rockets.Add(newRocket);
            times.Add(new int());
        }
        for(int i = 0; i < rockets.Count; i++)
        {
            times[i] += Time.deltaTime;
            if (times[i] > lifeTime)
            {
                Destroy(rockets[i]);
                rockets.RemoveAt(i);
                times.RemoveAt(i);
            }
        }
    }

}
