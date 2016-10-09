using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TransparencyAnim : MonoBehaviour {

    [SerializeField]
    float duration = 1f;

    [SerializeField]
    AnimationCurve curve;

    [SerializeField]
    Image img;

    [SerializeField]
    GameManager gm;

    void OnEnable()
    {
        gm.OnNewNPCSet += Anim;
    }

    void OnDisable()
    {
        gm.OnNewNPCSet -= Anim;
    }

    public void Anim()
    {
        StartCoroutine(anim());
    }

    IEnumerator<WaitForSeconds> anim()
    {
        float startTime = Time.timeSinceLevelLoad;
        Debug.Log("Start");
        while (true)
        {
            float progress = Mathf.Clamp01((Time.timeSinceLevelLoad - startTime) / duration);

            Color color = img.color;
            color.a = curve.Evaluate(progress);
            img.color = color;
            if (progress < 1)
            {
                yield return new WaitForSeconds(0.02f);
            } else
            {
                break;
            }            
        }
        Debug.Log("End");
    }
}
