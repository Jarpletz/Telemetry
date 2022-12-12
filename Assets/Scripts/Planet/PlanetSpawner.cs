using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawner : MonoBehaviour
{
    [SerializeField] GameObject planet;
    [SerializeField] Vector2 yRange;
    [SerializeField] Vector2 firstPos;
    [SerializeField] float DistTillSpawn;

    Camera cam;
    float screenHeight;
    float screenWidth;
    public float planetsSpawned=0;

    GameManager gm;
    float xRange;
    Vector2 nextPos;
    private void Awake()
    {
        gm = GetComponent<GameManager>();
        cam = Camera.main;
       // if(gm.isMobile) yRange = new Vector2(cam.orthographicSize*3/4, 5f / 4f * cam.orthographicSize);
       // else
        yRange = new Vector2(cam.orthographicSize, 13f/8f*cam.orthographicSize);
        
        nextPos = firstPos;
        SpawnPlanet();
    }
    
    private void Update()
    {
        screenHeight = cam.orthographicSize;
        screenWidth= screenHeight * Screen.width / Screen.height;
        if (nextPos.y - cam.transform.position.y < DistTillSpawn) 
        {
            SpawnPlanet();
        }
    }
    void SpawnPlanet()
    {
        GameObject newPlanet = Instantiate(planet, (Vector3)nextPos,Quaternion.identity);
        //gm.planets.Add(newPlanet.GetComponent<PlanetBehavior>());

        nextPos.x = Random.Range(-screenWidth * 2 / 3, screenWidth * 2 / 3);
        nextPos.y +=Random.Range(yRange.x, yRange.y);
        planetsSpawned++;

    }
}
