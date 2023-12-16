using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnButtonAnimation : MonoBehaviour
{
    Image bar;
    [SerializeField] float barSpeed;
    [SerializeField] float scaleSpeed;

    [SerializeField]bool runAnim = false;

    private void Start()
    {
        bar = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (runAnim)
        {
            if (bar.fillAmount > 0)
            {
                bar.fillAmount -= barSpeed * Time.deltaTime;
            }
            else
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, transform.localScale, scaleSpeed*Time.deltaTime);
            }
            if (transform.localScale.x < 0.001) noMoreRespawns();

        }
    }
    private void OnEnable()
    {
        runAnim = true;
    }


    public void noMoreRespawns()
    {
        Destroy(gameObject);
    }
}
