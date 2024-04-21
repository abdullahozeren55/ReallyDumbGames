using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootsCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float minRotatingSpeed = 240f;
    [SerializeField] private float maxRotatingSpeed = 480f;

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
        GameManager.Instance.MultiplySpeedOfKnight(isCollected, speedMultiplier);
        Destroy(gameObject);
    }
}
