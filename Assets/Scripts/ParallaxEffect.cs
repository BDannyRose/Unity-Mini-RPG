using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    public Vector2 startingPosition;
    public float startingZ;
    public Vector2 cameraMoveSinceStart => (Vector2)cam.transform.position - startingPosition;
    public float zDistanceFromTarget => transform.position.z - followTarget.position.z;
    public float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));
    public float parallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    private void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    private void Update()
    {
        Vector2 newPosition = startingPosition + cameraMoveSinceStart * parallaxFactor;

        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
