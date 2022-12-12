using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class startButtonText : MonoBehaviour
{
    [SerializeField] float angularVelocity;
    [SerializeField] float textFadeSpeed;
    float currentRotation;
    RectTransform rect;
    TextMeshPro text;
    GameManager gm;
    private void Start()
    {
        rect = GetComponent<RectTransform>();
        gm = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        text = GetComponent<TextMeshPro>();
    }


    private void Update()
    {
        currentRotation += angularVelocity;
        rect.rotation = Quaternion.identity;
        rect.rotation = Quaternion.Euler(new Vector3(0, 0, currentRotation));

        if (gm.startedGame)
        {
            Color spriteColor = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(text.color.a, 0, textFadeSpeed));
            text.color = spriteColor;
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {

           if (EventSystem.current.IsPointerOverGameObject())
                return;

            //check touch
            if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return;
            }
           
            if (!gm.canStartGame) return;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = new Vector3(mousePos.x, mousePos.y, 0);
            if (Vector3.Distance(transform.position, mousePos) < transform.parent.localScale.x * 2)
            {
                GameObject.FindWithTag("UI").GetComponent<StartUI>().startGame();
            }

        }
    }
}
