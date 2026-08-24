using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StartGameLogic : MonoBehaviour
{
    public CameraMovement cameraMovement;
    public Transform pointBCamera;
    public SpriteRenderer backgroundBlack;
    public GameObject meteoGameobject;
    private Coroutine _fadeCoroutine;


    public void MoveCameraToTarget()
    {
        cameraMovement.MoveCamera(null, pointBCamera, 2f, ()=>
        {
            FadeOut(backgroundBlack, 2f, () =>
            {
                backgroundBlack.gameObject.SetActive(false);
                meteoGameobject.SetActive(true);
            });
        });
    }


    public void FadeOut(SpriteRenderer spriteRenderer, float duration, Action onComplete = null)
    {
        if (spriteRenderer == null)
        {
            Debug.LogError("[SpriteFader] SpriteRenderer không được để null!");
            return;
        }

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(FadeOutRoutine(spriteRenderer, duration, onComplete));
    }

    private IEnumerator FadeOutRoutine(SpriteRenderer spriteRenderer, float duration, Action onComplete)
    {
        Color color = spriteRenderer.color;
        float startAlpha = 1f; // Tương ứng 255 trong 255-byte Alpha
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Tính Alpha giảm dần từ 1.0 về 0.0
            float currentAlpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);

            color.a = currentAlpha;
            spriteRenderer.color = color;

            yield return null;
        }

        // Đảm bảo Alpha đạt chính xác bằng 0 khi kết thúc
        color.a = 0f;
        spriteRenderer.color = color;

        _fadeCoroutine = null;

        // Gọi action khi đã giảm về 0
        onComplete?.Invoke();
    }
}
