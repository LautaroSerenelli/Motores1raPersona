using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPCaida : MonoBehaviour
{
    public Transform playerTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = Vector3.zero;
            IEntity player = playerTransform.GetComponent<IEntity>();
            player.ApplyDamage(7);
        }
    }
}