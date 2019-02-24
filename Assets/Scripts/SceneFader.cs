using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    public Image img;
    public AnimationCurve curve;
    

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeTo (string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn ()
    {
        float t = 1f;

        while (t > 0f)
        {
            //decreased t by the generic time.deltatime
            t -= Time.deltaTime;

            //this a is used to apply the curve graph on t our time variable, in our case this is an exponential function
            float a = curve.Evaluate(t);


            img.color = new Color (0f, 0f, 0f, a);

            //wait a single frame
            yield return 0;
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t < 1f)
        {
            //decreased t by the generic time.deltatime
            t += Time.deltaTime;

            //this a is used to apply the curve graph on t our time variable, in our case this is an exponential function
            float a = curve.Evaluate(t);


            img.color = new Color(0f, 0f, 0f, a);

            //wait a single frame
            yield return 0;
        }

        SceneManager.LoadScene(scene);
    }
}
