using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularCollectable : MonoBehaviour, ICollectable
{
    [SerializeField] private int collectingPoint = 10;
    [SerializeField] private float minRotatingSpeed = 180f;
    [SerializeField] private float maxRotatingSpeed = 360f;

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
        if(isCollected)
        {
            GameManager.Instance.AddToScore(collectingPoint);
        }
        else
        {
            GameManager.Instance.AddToScore(-collectingPoint);
        }
        Destroy(gameObject);
    }
}
