using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bullet))]
public class BulletEditor : Editor
{
    private SerializedProperty _bulletType;
    private SerializedProperty _playerClassData;
    private SerializedProperty _bulletSpeed;
    private SerializedProperty _bulletDamage;
    private SerializedProperty _bulletOffSetScale;
    private SerializedProperty _destroyTime;
    private SerializedProperty _damageText;
    
    private void OnEnable()
    {
        _bulletType = serializedObject.FindProperty("bulletType");
        _playerClassData = serializedObject.FindProperty("playerClassData");
        _bulletSpeed = serializedObject.FindProperty("bulletSpeed");
        _bulletDamage = serializedObject.FindProperty("bulletDamage");
        _bulletOffSetScale = serializedObject.FindProperty("bulletOffSetScale");
        _destroyTime = serializedObject.FindProperty("destroyTime");
        _damageText = serializedObject.FindProperty("damageText");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(_bulletType);
        if (_bulletType.enumValueIndex.Equals((int)Bullet.BulletType.Enemy) || _bulletType.enumValueIndex.Equals((int)Bullet.BulletType.PlayerHelper))
        {
            EditorGUILayout.PropertyField(_bulletSpeed);
            EditorGUILayout.PropertyField(_bulletDamage);
            EditorGUILayout.PropertyField(_bulletOffSetScale);
            EditorGUILayout.PropertyField(_destroyTime);
        }
        
        if (_bulletType.enumValueIndex.Equals((int)Bullet.BulletType.Player))
            EditorGUILayout.PropertyField(_playerClassData);
        EditorGUILayout.ObjectField(_damageText);
        serializedObject.ApplyModifiedProperties();
    }
}
