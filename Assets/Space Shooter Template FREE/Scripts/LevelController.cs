using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Serializable classes
[System.Serializable]
public class EnemyWaves 
{
    [Tooltip("time for wave generation from the moment the game started")]
    public float timeToStart;

    [Tooltip("Enemy wave's prefab")]
    public GameObject wave;
}

#endregion

public class LevelController : MonoBehaviour {

    //Serializable classes implements
    public EnemyWaves[] enemyWaves; 

    public GameObject powerUp;
    public float timeForNewPowerup;
    public GameObject[] planets;
    public float timeBetweenPlanets;
    public float planetsSpeed;
    List<GameObject> planetsList = new List<GameObject>();

    Camera mainCamera;   

    private void Start()
    {
        mainCamera = Camera.main;
        //for each element in 'enemyWaves' array creating coroutine which generates the wave
        for (int i = 0; i<enemyWaves.Length; i++) 
        {
            StartCoroutine(CreateEnemyWave(enemyWaves[i].timeToStart, enemyWaves[i].wave));
        }
        /*StartCoroutine(PowerupBonusCreation());*/
        StartCoroutine(PlanetsCreation());
    }
    
    //Create a new wave after a delay
    IEnumerator CreateEnemyWave(float delay, GameObject Wave) 
    {
        if (delay != 0)
            yield return new WaitForSeconds(delay);
        if (Player.instance != null)
            Instantiate(Wave);
    }

    //endless coroutine generating 'levelUp' bonuses. 
    /*IEnumerator PowerupBonusCreation() 
    {
        while (true) 
        {
            yield return new WaitForSeconds(timeForNewPowerup);
            Instantiate(
                powerUp,
                //Set the position for the new bonus: for X-axis - random position between the borders of 'Player's' movement; for Y-axis - right above the upper screen border 
                new Vector2(
                    Random.Range(PlayerMoving.instance.borders.minX, PlayerMoving.instance.borders.maxX), 
                    mainCamera.ViewportToWorldPoint(Vector2.up).y + powerUp.GetComponent<Renderer>().bounds.size.y / 2), 
                Quaternion.identity
                );
        }
    }*/

    IEnumerator PlanetsCreation()
    {
        for (int i = 0; i < planets.Length; i++)
        {
            planetsList.Add(planets[i]);
        }

        yield return new WaitForSeconds(10);

        while (true)
        {
            int randomIndex = Random.Range(0, planetsList.Count);
            // Spawn planet di posisi tetap (18.61, 5.3, 0)
            GameObject newPlanet = Instantiate(planetsList[randomIndex], new Vector3(18.61f, 5.3f, 0f), Quaternion.identity);
            planetsList.RemoveAt(randomIndex);

            if (planetsList.Count == 0)
            {
                for (int i = 0; i < planets.Length; i++)
                {
                    planetsList.Add(planets[i]);
                }
            }

            newPlanet.GetComponent<DirectMoving>().speed = planetsSpeed;
            yield return new WaitForSeconds(timeBetweenPlanets);
        }
    }
}
