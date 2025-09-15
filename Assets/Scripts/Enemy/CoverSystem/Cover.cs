using System;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
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

    public List<CoverPoint> GetCoverPoints()
    {
        return coverPoints;
    }
}
