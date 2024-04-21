using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissingPoint : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        ICollectable collectable = other.GetComponent<ICollectable>();

        if(collectable != null)
        {
            collectable.Collect(false);
        }
    }
}
