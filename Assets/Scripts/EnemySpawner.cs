using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public DamageReceiver player;
    public Texture crosshairTexture;
    public float spawnInterval = 2;
    public int enemiesPerWave = 5;
    public int waveNumberTotal;
    public Transform[] spawnPoints;

    float nextSpawnTime = 0;
    int waveNumber = 1;
    bool waitingForWave = true;
    float newWaveTimer = 0;
    int enemiesToEliminate;
    int enemiesEliminated = 0;
    int totalEnemiesSpawned = 0;
    bool youWin = false;

    public int WaveNumber
    {
        get { return waveNumber; }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        newWaveTimer = 10;
        waitingForWave = true;
    }

    void Update()
    {
        if (waitingForWave)
        {
            if(newWaveTimer >= 0)
            {
                newWaveTimer -= Time.deltaTime;
            }
            else
            {
                enemiesToEliminate = waveNumber * enemiesPerWave;
                enemiesEliminated = 0;
                totalEnemiesSpawned = 0;
                waitingForWave = false;
            }
        }
        else
        {
            if(Time.time > nextSpawnTime)
            {
                nextSpawnTime = Time.time + spawnInterval;

                //Spawn enemy 
                if(totalEnemiesSpawned < enemiesToEliminate)
                {
                    Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length - 1)];

                    GameObject enemy = Instantiate(enemyPrefab, randomPoint.position, Quaternion.identity);
                    EnemyController npc = enemy.GetComponent<EnemyController>();
                    npc.playerTransform = player.transform;
                    npc.es = this;
                    totalEnemiesSpawned++;
                }
            }
        }

        if (player.playerHP <= 0 || youWin)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    void OnGUI()
    {
        GUI.Box(new Rect(10, Screen.height - 35, 100, 25), ((int)player.playerHP).ToString() + " HP");
        GUI.Box(new Rect(Screen.width / 2 - 35, Screen.height - 35, 70, 25), player.weaponManager.selectedWeapon.cargadorSize.ToString());

        if(player.playerHP <= 0)
        {
            GUI.Box(new Rect(Screen.width / 2 - 85, Screen.height / 2 - 20, 170, 40), "Game Over\n(Press 'Space' to Restart)");
        }
        else
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 - 3, Screen.height / 2 - 3, 6, 6), crosshairTexture);
        }

        GUI.Box(new Rect(Screen.width / 2 - 50, 10, 100, 25), (enemiesToEliminate - enemiesEliminated).ToString());

        if (waitingForWave)
        {
            if (waveNumber < waveNumberTotal)
            {
                GUI.Box(new Rect(Screen.width / 2 - 125, Screen.height / 4 - 12, 250, 25), "Waiting for Wave " + waveNumber.ToString() + " (" + ((int)newWaveTimer).ToString() + " seconds left...)");
            }
            else
            {
                GUI.Box(new Rect(Screen.width / 2 - 125, Screen.height / 4 - 12, 250, 25), "Waiting for final Wave " + " (" + ((int)newWaveTimer).ToString() + " seconds left...)");
            }
        }

        if (youWin)
        {
            GUI.Box(new Rect(Screen.width / 3, Screen.height / 2 - 20, 340, 80), "You Win!\n(Press 'Space' to Restart)");
        }
    }

    public void EnemyEliminated(EnemyController enemy)
    {
        enemiesEliminated++;

        if(enemiesToEliminate - enemiesEliminated <= 0)
        {
            if (waveNumber != waveNumberTotal)
            {
                //Start next wave
                newWaveTimer = 10;
                waitingForWave = true;
                waveNumber++;
            }
            else
            {
                ShowVictoryScreen();
            }
        }
    }

    void ShowVictoryScreen()
    {
        youWin = true;
    }
}