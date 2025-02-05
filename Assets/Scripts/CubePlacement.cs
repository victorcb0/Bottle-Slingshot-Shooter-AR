using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CubePlacement : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public GameObject cubePrefab;
    public Timer timer;

    private List<GameObject> cubes = new List<GameObject>();
    private int maxCubes = 3;
    public float heightOffset = 0.05f;
    public float minimumDistanceBetweenCubes = 0.3f;

    private bool hasStartedTimer = false;  // Asigurăm că pornim timerul o singură dată

    void Update()
    {
        ValidateCubeList();

        if (cubes.Count >= maxCubes)
        {
            DisablePlanes();
            return;
        }

        foreach (ARPlane plane in planeManager.trackables)
        {
            if (cubes.Count < maxCubes)
            {
                PlaceCubeAbovePlane(plane);
            }
        }

        UpdateCubePositions();
    }

    void PlaceCubeAbovePlane(ARPlane plane)
    {
        Vector3 planeCenter = plane.center;
        bool isPositionValid = false;
        Vector3 cubePosition = Vector3.zero;

        for (int i = 0; i < 10; i++)
        {
            float randomX = Random.Range(-plane.extents.x, plane.extents.x);
            float randomZ = Random.Range(-plane.extents.y, plane.extents.y);
            cubePosition = new Vector3(
                planeCenter.x + randomX,
                plane.center.y + heightOffset,
                planeCenter.z + randomZ
            );

            if (Mathf.Abs(randomX) > plane.extents.x || Mathf.Abs(randomZ) > plane.extents.y)
            {
                continue;
            }

            isPositionValid = true;
            foreach (GameObject existingCube in cubes)
            {
                float distanceFromOtherCube = Vector3.Distance(existingCube.transform.position, cubePosition);
                if (distanceFromOtherCube < minimumDistanceBetweenCubes)
                {
                    isPositionValid = false;
                    break;
                }
            }

            if (isPositionValid)
            {
                break;
            }
        }

        if (isPositionValid)
        {
            GameObject newCube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
            cubes.Add(newCube);
        }
    }

    void UpdateCubePositions()
    {
        foreach (GameObject cube in cubes)
        {
            ARPlane closestPlane = null;
            float closestDistance = float.MaxValue;

            foreach (ARPlane plane in planeManager.trackables)
            {
                float distance = Vector3.Distance(cube.transform.position, plane.center);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlane = plane;
                }
            }

            if (closestPlane != null)
            {
                Vector3 newPosition = cube.transform.position;
                newPosition.y = closestPlane.center.y + heightOffset;
                cube.transform.position = newPosition;
            }
        }
    }

    void ValidateCubeList()
    {
        cubes.RemoveAll(cube => cube == null);
    }

    void DisablePlanes()
    {
        planeManager.enabled = false;

        foreach (ARPlane plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }

        // Aici pornim timerul o singură dată
        if (!hasStartedTimer && timer != null)
        {
            hasStartedTimer = true;
            timer.StartTimer();
        }
    }
}
