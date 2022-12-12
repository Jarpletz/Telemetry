using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textEffectInfo : MonoBehaviour
{
    [SerializeField] float lerpSpeed;
    public Color color;
    public string text;
    public float points;
    bool lerpToScore = false;
    bool hasAwardedPoints=false;

    TextMeshPro textComponent;
    GameManager gm;

    void Start()
    {
        textComponent = GetComponentInChildren<TextMeshPro>();
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        textComponent.text = text;
        textComponent.color = color;

        if (points > 0)
        {
            Invoke("startLerping", 1f);
        }
    }

    private void Update()
    {
        if (lerpToScore)
        {
            Vector3 targetPos = Camera.main.ScreenToWorldPoint( GameObject.FindWithTag("UI").GetComponent<UI_Manager>().scoreText.transform.position);
            //A giant mess that determines the position of the ui score indicator in world space
            transform.position = Vector3.Lerp(transform.position, targetPos, lerpSpeed*Time.deltaTime);

            if ((textComponent.color.a == 0 ||gm.isDead)&&!hasAwardedPoints)
            {
                gm.score += points;
                hasAwardedPoints = true;
            }
            if (textComponent.color.a == 0)
            {
                Destroy(gameObject);
            }

        }
    }

    void startLerping()
    {
        lerpToScore = true;
    }
   
}
