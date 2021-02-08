
using UnityEngine;
using UnityEngine.UI;
public class ScreenFadeController : MonoBehaviour
{

    public Image blackScreen;

    void Start()
    {
        blackScreen.canvasRenderer.SetAlpha(0.0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ActivateTeleportFade(1.0f);
            Debug.Log("fading");
        }
    }

    public void ActivateTeleportFade(float fadeDuration)
    {
        FadeOut(fadeDuration);
        FadeIn(fadeDuration);
    }

    private void FadeOut(float fadeDuration)
    {
        blackScreen.CrossFadeAlpha(1.0f, fadeDuration, false);
    }

    private void FadeIn(float fadeDuration)
    {
        blackScreen.CrossFadeAlpha(0.0f, fadeDuration, false);
    }
}
