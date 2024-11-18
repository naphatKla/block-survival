using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    protected SerializedProperty enemyName;
    protected SerializedProperty enemyType;
    protected SerializedProperty bulletPrefab;
    protected SerializedProperty bulletOffset;
    protected SerializedProperty fireRate;
    protected SerializedProperty attackRange;
    protected SerializedProperty maxHp;
    protected SerializedProperty attackDamage;
    protected SerializedProperty damageText;
    protected SerializedProperty expDrop;
    protected SerializedProperty maxSpeed;
    protected SerializedProperty minSpeed;
    protected SerializedProperty turnDirectionDamp;
    protected SerializedProperty distanceThreshold;
    protected SerializedProperty deadParticleSystem;
    protected SerializedProperty lootChest;
    
    protected virtual void OnEnable()
    {
        enemyName = serializedObject.FindProperty("enemyName");
        enemyType = serializedObject.FindProperty("enemyType");
        maxHp = serializedObject.FindProperty("maxHp");
        bulletPrefab = serializedObject.FindProperty("bulletPrefab");
        bulletOffset = serializedObject.FindProperty("bulletOffset");
        fireRate = serializedObject.FindProperty("fireRate");
        attackRange = serializedObject.FindProperty("attackRange");
        attackDamage = serializedObject.FindProperty("attackDamage");
        damageText = serializedObject.FindProperty("damageText");
        expDrop = serializedObject.FindProperty("expDrop");
        maxSpeed = serializedObject.FindProperty("maxSpeed");
        minSpeed = serializedObject.FindProperty("minSpeed");
        turnDirectionDamp = serializedObject.FindProperty("turnDirectionDamp");
        distanceThreshold = serializedObject.FindProperty("distanceThreshold");
        deadParticleSystem = serializedObject.FindProperty("deadParticleSystem");
        lootChest = serializedObject.FindProperty("lootChest");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUILayout.PropertyField(enemyName);
        EditorGUILayout.PropertyField(enemyType);  
        
        if (enemyType.enumValueIndex == (int)Enemy.EnemyType.Range)
        {
           EditorGUILayout.PropertyField(bulletPrefab);
           EditorGUILayout.PropertyField(bulletOffset);
           EditorGUILayout.PropertyField(fireRate);
           EditorGUILayout.PropertyField(attackRange);
        }
        
        EditorGUILayout.PropertyField(maxHp);   
        EditorGUILayout.PropertyField(attackDamage);
        EditorGUILayout.PropertyField(damageText);
        EditorGUILayout.PropertyField(expDrop);
        EditorGUILayout.PropertyField(maxSpeed);
        EditorGUILayout.PropertyField(minSpeed);
        EditorGUILayout.PropertyField(turnDirectionDamp);
        EditorGUILayout.PropertyField(distanceThreshold);
        EditorGUILayout.PropertyField(deadParticleSystem);
        EditorGUILayout.PropertyField(lootChest);
        serializedObject.ApplyModifiedProperties();
    }
}
