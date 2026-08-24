using UnityEngine;
using System;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    private Coroutine _moveCoroutine;

    public void MoveCamera(Transform targetA, Transform targetB, float duration = 1.0f, Action onComplete = null)
    {
        if (targetB == null)
        {
            Debug.LogError("[CameraMovement] Target B không được để null!");
            return;
        }

        // Xác định vị trí bắt đầu
        Vector3 startPosition = (targetA != null) ? targetA.position : transform.position;
        Quaternion startRotation = (targetA != null) ? targetA.rotation : transform.rotation;

        // Xác định vị trí đích
        Vector3 endPosition = targetB.position;
        Quaternion endRotation = targetB.rotation;

        // Ép cứng trục Z về -10
        startPosition.z = -10f;
        endPosition.z = -10f;

        // Dừng di chuyển cũ nếu đang chạy
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }

        _moveCoroutine = StartCoroutine(MoveRoutine(startPosition, endPosition, startRotation, endRotation, duration, onComplete));
    }

    private IEnumerator MoveRoutine(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float duration, Action onComplete)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Làm mượt chuyển động (SmoothStep)
            t = Mathf.SmoothStep(0f, 1f, t);

            // Cập nhật vị trí và góc quay
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, t);
            currentPos.z = -10f; // Bảo đảm trục Z luôn luôn là -10 trong từng frame

            transform.position = currentPos;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            yield return null;
        }

        // Gắn chính xác vị trí cuối cùng với Z = -10
        endPos.z = -10f;
        transform.position = endPos;
        transform.rotation = endRot;

        _moveCoroutine = null;

        // Gọi callback onComplete
        onComplete?.Invoke();
    }
}