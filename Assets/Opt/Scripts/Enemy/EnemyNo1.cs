using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyNo1 : EnemyController
{
    void Start()
    {
        moveSpeed = 3f;
        maxHp = 1000f;
        currentHp = 1000f;
        damage = 100f;
        enemyScore = 100f;
    }
    
    
}
