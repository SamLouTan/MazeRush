using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.UI;

public class NbEnemy : MonoBehaviour
{
    private GameObject[] enemies;
    public Text enemyCountText;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            enemyCountText.text = "Level Complete ! ";
        }
        else
        {
            enemyCountText.text = "Enemies : " + enemies.Length.ToString();
        }
    }
}
