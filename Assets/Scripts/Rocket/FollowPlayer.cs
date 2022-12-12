using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] float lerpSpeed;
    [SerializeField] float zoomLerpSpeed;
    [SerializeField] float cameraOffset;
    [SerializeField] float maxDistBack;
    [SerializeField] float orbitFOV;
    public float normalFOV;
    float greatestY;
    Transform player;
    GameManager gm;
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.startedGame) return;

        if (greatestY - player.position.y > maxDistBack) return;

        if (player.position.y > greatestY) greatestY = player.position.y;

            transform.position = new Vector3(0, Mathf.Lerp(transform.position.y, player.position.y + Camera.main.orthographicSize * cameraOffset, lerpSpeed), -1f);

        if (player.GetComponent<RocketMovement>().inOrbit)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,orbitFOV, zoomLerpSpeed * Time.deltaTime);
        }
        else
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, normalFOV, zoomLerpSpeed*Time.deltaTime);
        }

    }
    private void OnDisable()
    {
       transform.position = new Vector3(0, 0, -1);
    }
}
