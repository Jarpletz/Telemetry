using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenShake : MonoBehaviour
{
    public IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 OriginalPos = transform.localPosition;

        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            float x = OriginalPos.x + (magnitude * Random.Range(-1f,1f));
            float y = OriginalPos.y + (magnitude * Random.Range(-1f, 1f));

            transform.localPosition = new Vector3(x, y, OriginalPos.z);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = OriginalPos;
    }
}
