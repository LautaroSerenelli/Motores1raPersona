using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 345;
    public float hitForce = 50f;
    public float destroyAfter = 3.5f;

    float currentTime = 0;
    Vector3 newPos;
    Vector3 oldPos;
    bool hasHit = false;

    float damagePoints;

    IEnumerator Start()
    {
        newPos = transform.position;
        oldPos = newPos;

        while (currentTime < destroyAfter && !hasHit)
        {
            Vector3 velocity = transform.forward * bulletSpeed;
            newPos += velocity * Time.deltaTime;
            Vector3 direction = newPos - oldPos;
            float distance = direction.magnitude;
            RaycastHit hit;

            // Check hit
            if (Physics.Raycast(oldPos, direction, out hit, distance))
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(direction * hitForce);

                    IEntity npc = hit.transform.GetComponent<IEntity>();
                    if (npc != null)
                    {
                        //Aplicar daño a npc
                        npc.ApplyDamage(damagePoints);
                    }
                }

                newPos = hit.point;
                StartCoroutine(DestroyBullet());
            }

            currentTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();

            transform.position = newPos;
            oldPos = newPos;
        }

        if (!hasHit)
        {
            StartCoroutine(DestroyBullet());
        }
    }

    IEnumerator DestroyBullet()
    {
        hasHit = true;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void SetDamage(float points)
    {
        damagePoints = points;
    }
}