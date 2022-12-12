using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialPlanetSpawner : MonoBehaviour
{
    [SerializeField] Transform thrustParent;
    [SerializeField] GameObject planet;
    [SerializeField] Vector2 yRange;
    [SerializeField] Vector2 firstPos;
    [SerializeField] float DistTillSpawn;

    Camera cam;
    float screenHeight;
    float screenWidth;
    public float planetsSpawned = 0;

    float xRange;
    Vector2 nextPos;
    private void Awake()
    {
        cam = Camera.main;

        yRange = new Vector2(cam.orthographicSize, 13f / 8f * cam.orthographicSize);

        screenHeight = cam.orthographicSize;
        screenWidth = screenHeight * Screen.width / Screen.height;

        nextPos = firstPos;
        nextPos.x = screenWidth * 2 / 3;
        SpawnPlanet();
    }

    private void LateUpdate()
    {
        screenHeight = cam.orthographicSize;
        screenWidth = screenHeight * Screen.width / Screen.height;
        if (nextPos.y - cam.transform.position.y < DistTillSpawn)
        {
            SpawnPlanet();
        }
    }
    void SpawnPlanet()
    {
        GameObject newPlanet1 = Instantiate(planet, (Vector3)nextPos, Quaternion.identity);
        GameObject newPlanet2 = Instantiate(planet, new Vector3(-nextPos.x,nextPos.y,0), Quaternion.identity);
        newPlanet1.transform.SetParent(thrustParent);
        newPlanet2.transform.SetParent(thrustParent);

        nextPos.x = screenWidth * 2 / 3;
        nextPos.y += Random.Range(yRange.x, yRange.y);

    }
}
