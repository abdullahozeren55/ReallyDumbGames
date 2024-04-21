using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomsCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] private float sizeMultiplier = 2f;
    [SerializeField] private float minRotatingSpeed = 360f;
    [SerializeField] private float maxRotatingSpeed = 720f;

    private float rotatingSpeed;

    void Start()
    {
        rotatingSpeed = Random.Range(minRotatingSpeed, maxRotatingSpeed);
    }


    void Update()
    {
        transform.Rotate(Vector3.forward * rotatingSpeed * Time.deltaTime);
    }

    public void Collect(bool isCollected)
    {
        GameManager.Instance.MultiplySizeOfKnight(isCollected, sizeMultiplier);
        Destroy(gameObject);
    }
}
