using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyController : MonoBehaviour, IEntity
{
    public float attackDistance = 3f;
    public float movementSpeed = 4f;
    public float npcHP = 100;
    public float npcDamage = 5;
    public float attackRate = 0.5f;
    public Transform firePoint;
    public GameObject npcDeadPrefab;

    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public EnemySpawner es;
    NavMeshAgent agent;
    float nextAttackTime = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;
        agent.speed = movementSpeed;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update()
    {
        if (agent.remainingDistance - attackDistance < 0.01f)
        {
            if(Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackRate;
                //Attack
                Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);

                RaycastHit hit;
                if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, attackDistance))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.DrawLine(firePoint.position, firePoint.position + firePoint.forward * attackDistance, Color.cyan);

                        IEntity player = hit.transform.GetComponent<IEntity>();
                        player.ApplyDamage(npcDamage);
                    }
                }
            }
        }
        agent.destination = playerTransform.position;
        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));
    }

    public void ApplyDamage(float points)
    {
        npcHP -= points;
        if(npcHP <= 0)
        {
            //Destroy NPC
            GameObject npcDead = Instantiate(npcDeadPrefab, transform.position, transform.rotation);
            npcDead.GetComponent<Rigidbody>().velocity = (-(playerTransform.position - transform.position).normalized * 8) + new Vector3(0, 5, 0);
            Destroy(npcDead, 10);
            es.EnemyEliminated(this);
            Destroy(gameObject);
        }
    }
}