using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AlienPathController : MonoBehaviour
{
    [Header("DOTween Settings")]
    [SerializeField] private List<Transform> waypoints = new List<Transform>();

    [Header("Speed Controls")]
    [SerializeField] private bool useSpeedInsteadOfDuration = true;
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float duration = 10f;

    [Header("Path Settings")]
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private PathType pathType = PathType.Linear;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationOffsetAngle = 0f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Visual Settings")]
    [SerializeField] private GameObject spriteObject; // Keo GameObject/Transform chua Sprite con sau vao day

    private Tween pathTween;

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Alpha3)) StartPathMovement();
    //    if (Input.GetKeyDown(KeyCode.Alpha4)) StopMovement();
    //}

    private void Start()
    {
        StartPathMovement();
    }

    public void StartPathMovement()
    {
        if (waypoints == null || waypoints.Count < 2) return;

        StopMovement();

        Vector3[] pathPositions = new Vector3[waypoints.Count];
        for (int i = 0; i < waypoints.Count; i++)
        {
            pathPositions[i] = waypoints[i].position;
        }

        // 2. Dat vi tri va xoay dau ve Waypoint 2 ngay lap tuc
        transform.position = pathPositions[0];

        Vector3 initialDir = pathPositions[1] - pathPositions[0];
        if (initialDir.sqrMagnitude > 0.0001f)
        {
            float targetAngle = Mathf.Atan2(initialDir.y, initialDir.x) * Mathf.Rad2Deg + rotationOffsetAngle;
            transform.rotation = Quaternion.Euler(0, 0, targetAngle);
        }

        // 3. Bat dau di chuyen Path
        pathTween = transform.DOPath(pathPositions, useSpeedInsteadOfDuration ? moveSpeed : duration, pathType)
            .SetSpeedBased(useSpeedInsteadOfDuration)
            .SetEase(easeType)
            .OnUpdate(() =>
            {
                Vector3 lookTarget = pathTween.PathGetPoint(Mathf.Clamp01(pathTween.ElapsedPercentage() + 0.05f));
                Vector3 dir = lookTarget - transform.position;

                if (dir.sqrMagnitude > 0.0001f)
                {
                    float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + rotationOffsetAngle;
                    Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                }
            })
            .OnComplete(() =>
            {
                // Khi toi diem cuoi (Point 3) thi an Sprite đi
                if (spriteObject != null)
                {
                    spriteObject.SetActive(true);
                    gameObject.SetActive(false);
                }
            });
    }

    public void StopMovement()
    {
        pathTween?.Kill();
    }

    private void OnDestroy()
    {
        StopMovement();
    }
}