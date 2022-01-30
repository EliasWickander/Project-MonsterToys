using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraFade : MonoBehaviour
{
    public enum FadeState
    {
        Idle,
        FadeIn,
        FadeOut,
    }
    
    [SerializeField] 
    private Image m_blackScreenImage;

    [SerializeField] 
    private float m_fadeInTime = 1;
    
    [SerializeField] 
    private float m_fadeOutTime = 1;
    
    private FadeState m_currentState;

    public FadeState CurrentState => m_currentState;

    public event Action OnCameraFadeInStarted; 
    public event Action OnCameraFadeInEnded; 
    public event Action OnCameraFadeOutStarted; 
    public event Action OnCameraFadeOutEnded;

    public void FadeIn()
    {
        StartCoroutine(FadeIn_Internal());
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOut_Internal());
    }

    private IEnumerator FadeIn_Internal()
    {
        m_currentState = FadeState.FadeIn;
        OnCameraFadeInStarted?.Invoke();

        Color blackScreenColor = m_blackScreenImage.color;

        float startAlpha = blackScreenColor.a;
        float timeToFadeOut = m_fadeInTime * startAlpha;
        float lerpTimer = 0;
        while (lerpTimer < timeToFadeOut)
        {
            yield return new WaitForEndOfFrame();

            float currentAlpha = Mathf.Lerp(startAlpha, 0, lerpTimer / timeToFadeOut);
            blackScreenColor.a = currentAlpha;

            m_blackScreenImage.color = blackScreenColor;
            lerpTimer += Time.deltaTime;
        }

        m_currentState = FadeState.Idle;
        OnCameraFadeInEnded?.Invoke();
    }

    private IEnumerator FadeOut_Internal()
    {
        m_currentState = FadeState.FadeOut;
        OnCameraFadeOutStarted?.Invoke();

        Color blackScreenColor = m_blackScreenImage.color;

        float startAlpha = blackScreenColor.a;
        float timeToFadeOut = m_fadeOutTime * (1 - startAlpha);
        float lerpTimer = 0;
        while (lerpTimer < timeToFadeOut)
        {
            yield return new WaitForEndOfFrame();

            float currentAlpha = Mathf.Lerp(startAlpha, 1, lerpTimer / timeToFadeOut);
            blackScreenColor.a = currentAlpha;

            m_blackScreenImage.color = blackScreenColor;
            lerpTimer += Time.deltaTime;
        }

        m_currentState = FadeState.Idle;
        OnCameraFadeOutEnded?.Invoke();
    }
}
