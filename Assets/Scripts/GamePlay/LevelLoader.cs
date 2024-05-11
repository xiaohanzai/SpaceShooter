using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private float speedToCreateThePlayer;

    public static int difficulty;

    public Player player1;
    public Enemy normalEnemy;
    public ExplodingEnemy explodingEnemy;
    // Start is called before the first frame update
    void Start()
    {
        //creating OBJECTS and assigning them to variables
        //player1 = new Player(speedToCreateThePlayer); //creating a player with a constructor that has one parameter

        normalEnemy = new Enemy(); //creating an enemy with empty constructor
        explodingEnemy = new ExplodingEnemy(); //an exploding enemy is just as same as the enemy, all the methods and attributes but it will have additional features

        explodingEnemy.Attack();
    }

    // Update is called once per frame
    void Update()
    {
        //player1.Move();
    }
}


