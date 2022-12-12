using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class textSpawner : MonoBehaviour
{
    [SerializeField] float spawnDist;
    [SerializeField] GameObject textEffect;
    [SerializeField] LayerMask planetLayer;
    GameManager gm;

    Vector2[] spawnDirections = { new Vector2(1, 1), new Vector2(1,-1),new Vector2(-1,-1), new Vector2(-1,1), new Vector2(0,1), new Vector2(0,-1) };//all i+j vectors except +i+0j and -i+0j

    private void Start()
    {
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    public void spawnText(string text, Color color, float points = 0)
    {
        Vector3 spawnPoint = transform.position + (Vector3)spawnDirections[Random.Range(0,spawnDirections.Length)] * spawnDist;
        float attempts = 0;
        while (!Physics2D.CircleCast((Vector2)spawnPoint, 1,Vector2.up)|| attempts<5)
        {
            spawnPoint = transform.position + (Vector3)spawnDirections[Random.Range(0, spawnDirections.Length)] * spawnDist;
            attempts++;
        }

        GameObject newText = Instantiate(textEffect, spawnPoint, Quaternion.identity);
        textEffectInfo info = newText.GetComponent<textEffectInfo>();

        if (points > 0)
        {
            points *=10;
            text += " +" + points.ToString("0");
        }

        info.text = text;
        info.color = color;
        info.points = points;

    }
}
