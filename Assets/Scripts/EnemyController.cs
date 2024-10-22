using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    int currentWave;

    [HideInInspector]
    public Transform playerTransform;
    [HideInInspector]
    public EnemySpawner es;
    NavMeshAgent agent;
    float nextAttackTime = 0;
    bool isPlayerInRange = false;

    [SerializeField] private Image barImage;
    float npcHPTotal;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = attackDistance;

        currentWave = es.WaveNumber;
        movementSpeed += currentWave - 1;
        agent.speed = movementSpeed;

        npcHP += currentWave * currentWave * currentWave - 1;
        npcHPTotal = npcHP;

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    void Update()
    {
        agent.destination = playerTransform.position;
        transform.LookAt(new Vector3(playerTransform.transform.position.x, transform.position.y, playerTransform.position.z));

        if (isPlayerInRange && Time.time > nextAttackTime)
        {
            IEntity player = playerTransform.GetComponent<IEntity>();
            player.ApplyDamage(npcDamage + (currentWave * currentWave - 1));

            nextAttackTime = Time.time + attackRate;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            nextAttackTime = Time.time + attackRate;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            nextAttackTime = 0;
        }
    }

    public void ApplyDamage(float points)
    {
        npcHP -= points;
        UpdateHealthBar();
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

    public void UpdateHealthBar()
    {
        barImage.fillAmount = npcHP / npcHPTotal;
    }
}