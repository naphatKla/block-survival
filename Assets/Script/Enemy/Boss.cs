using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : Enemy
{
    private float originalTurnDirectionDamp;
    protected override void Start()
    {
        base.Start();
        originalTurnDirectionDamp = turnDirectionDamp;
        StartCoroutine(RandomSkill());

    }
    protected override void Update()
    {
        base.Update();
        
    }
    
    protected override void OnDestroy ()
    {
        try
        {
            LootChestSpawn();
            _level.enemyKill++;
            _level.LevelGain(expDrop);
            _gameManager.enemyLeft--;
        }
        catch (Exception e)
        {
            //Debug.Log(e);
        }
    }

    private IEnumerator RandomSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            if (Random.Range(0, 101) >= 50)
            {
                float time = 5f;
                float timeCount = 0;
                Debug.Log("Boss Skill");

                while (timeCount < time)
                {
                    timeCount += Time.deltaTime;
                    if (timeCount < 2.5)
                    {
                        _currentSpeed = 0;
                        
                    }
                    else
                    {
                        _currentSpeed = 30f;
                        turnDirectionDamp = 5f;
                    }
                    yield return null;
                }
                turnDirectionDamp = originalTurnDirectionDamp;
            }
        }
    }
}
