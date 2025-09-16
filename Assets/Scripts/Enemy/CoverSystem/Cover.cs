using System;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    private Transform playerTransform;

    [Header("Cover points")]
    [SerializeField]
    private GameObject coverPointPrefab;

    [SerializeField]
    private List<CoverPoint> coverPoints = new();

    [SerializeField]
    private float xOffset = 1.25f;

    [SerializeField]
    private float yOffset = .2f;

    [SerializeField]
    private float zOffset = 1f;

    private void Start()
    {
        GenerateCoverPoints();

        playerTransform = FindFirstObjectByType<Player>().transform;
    }

    private void GenerateCoverPoints()
    {
        Vector3[] localCoverPoints =
        {
            new(0, yOffset, zOffset),
            new(0, yOffset, -zOffset),
            new(xOffset, yOffset, 0),
            new(-xOffset, yOffset, 0),
        };

        foreach (Vector3 localPoint in localCoverPoints)
        {
            Vector3 worldPoint = transform.TransformPoint(localPoint);

            CoverPoint coverPoint = Instantiate(
                    coverPointPrefab,
                    worldPoint,
                    Quaternion.identity,
                    transform
                )
                .GetComponent<CoverPoint>();

            coverPoints.Add(coverPoint);
        }
    }

    public List<CoverPoint> GetValidCoverPoints(Transform enemyTransform)
    {
        List<CoverPoint> validCoverPoints = new();

        foreach (CoverPoint coverPoint in coverPoints)
        {
            if (IsValidCoverPoint(coverPoint, enemyTransform))
            {
                validCoverPoints.Add(coverPoint);
            }
        }

        return validCoverPoints;
    }

    private bool IsValidCoverPoint(CoverPoint coverPoint, Transform enemyTransform)
    {
        if (coverPoint.occupied)
        {
            return false;
        }

        if (!IsFurthestFromPlayer(coverPoint))
        {
            return false;
        }

        if (IsCoverCloseToPlayer(coverPoint))
        {
            return false;
        }

        if (IsCoverBehindPlayer(coverPoint, enemyTransform))
        {
            return false;
        }

        if (IsCoverCloseToLastCover(coverPoint, enemyTransform))
        {
            return false;
        }

        return true;
    }

    private bool IsCoverBehindPlayer(CoverPoint coverPoint, Transform enemyTransform)
    {
        float distanceToPlayer = Vector3.Distance(
            coverPoint.transform.position,
            playerTransform.position
        );
        float distanceToEnemy = Vector3.Distance(
            coverPoint.transform.position,
            enemyTransform.position
        );

        return distanceToPlayer < distanceToEnemy;
    }

    private bool IsCoverCloseToPlayer(CoverPoint coverPoint)
    {
        return Vector3.Distance(coverPoint.transform.position, playerTransform.position) < 2f;
    }

    private bool IsCoverCloseToLastCover(CoverPoint coverPoint, Transform enemy)
    {
        CoverPoint lastCover = enemy.GetComponent<EnemyRange>().currentCover;

        return lastCover != null
            && Vector3.Distance(coverPoint.transform.position, lastCover.transform.position) < 3f;
    }

    private bool IsFurthestFromPlayer(CoverPoint coverPoint)
    {
        CoverPoint furthestPoint = null;

        float furthestDistance = 0;

        foreach (CoverPoint point in coverPoints)
        {
            float distance = Vector3.Distance(point.transform.position, playerTransform.position);

            if (distance > furthestDistance)
            {
                furthestDistance = distance;
                furthestPoint = point;
            }
        }
        return furthestPoint == coverPoint;
    }
}
