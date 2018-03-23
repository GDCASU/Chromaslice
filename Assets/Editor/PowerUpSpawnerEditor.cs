using UnityEditor;
using UnityEngine;

/*
 * Description: This script creates a Custom Inspector window for the PowerUpSpawner script.
 * It allows designers to easily create power ups with varying data amounts and spawning methods
 * Version: 1.0.0
 * Author: Zachary Schmalz
 * Date: January 26, 2018
 */

[CustomEditor(typeof(PowerUpSpawner)), CanEditMultipleObjects]
public class PowerUpSpawnerEditor : Editor
{
    // Properties that need to be saved in the inspector window
    public SerializedProperty
        methodProp,
        areaProp,

        speedProp,
        ropeProp,
        knockbackProp,
        invincibilityProp,

        prefabProp,
        speedStructProp,
        ropeStructProp,
        knockbackStructProp,
        invincibilityStructProp,

        selectionProp,
        intervalProp,
        continuousSpawnProp,
        spawnBoxProp,
        spawnRadiusProp;

    // Serialize the properties
    void OnEnable()
    {
        methodProp = serializedObject.FindProperty("method");
        areaProp = serializedObject.FindProperty("area");
        speedProp = serializedObject.FindProperty("speedSelect");
        ropeProp = serializedObject.FindProperty("ropeSelect");
        knockbackProp = serializedObject.FindProperty("knockbackSelect");
        invincibilityProp = serializedObject.FindProperty("invincibilitySelect");

        prefabProp = serializedObject.FindProperty("powerUpPrefabs");
        speedStructProp = serializedObject.FindProperty("speedStruct");
        ropeStructProp = serializedObject.FindProperty("ropeStruct");
        knockbackStructProp = serializedObject.FindProperty("knockbackStruct");
        invincibilityStructProp = serializedObject.FindProperty("invincibilityStruct");

        selectionProp = serializedObject.FindProperty("selection");
        intervalProp = serializedObject.FindProperty("spawnInterval");
        continuousSpawnProp = serializedObject.FindProperty("continuousSpawn");
        spawnBoxProp = serializedObject.FindProperty("spawnBox");
        spawnRadiusProp = serializedObject.FindProperty("spawnRadius");
    }

    // Update the inspector window with these properties
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Prefab's property
        EditorGUILayout.PropertyField(prefabProp, new GUIContent("Power-Up Prefabs"), true);
        EditorGUILayout.Space();

        // Show/Hide spawning interval field depending if power-ups with continuously spawn
        EditorGUILayout.PropertyField(continuousSpawnProp, new GUIContent("Continuously Spawn"));
        if(continuousSpawnProp.boolValue)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(intervalProp, new GUIContent("Spawning Interval"));
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.Space();

        //Spawn Radius
        EditorGUILayout.PropertyField(areaProp);
        PowerUpSpawner.SpawningArea area = (PowerUpSpawner.SpawningArea)areaProp.enumValueIndex;

        switch (area)
        {
            // Random Spawning Method
            case PowerUpSpawner.SpawningArea.Circle:
                EditorGUI.indentLevel++;
                EditorGUIUtility.labelWidth = 150;

                EditorGUILayout.PropertyField(spawnRadiusProp, new GUIContent("Spawn Raduis"), true);

                EditorGUI.indentLevel--;
                break;
            case PowerUpSpawner.SpawningArea.Rectangle:
                EditorGUI.indentLevel++;

                EditorGUILayout.PropertyField(spawnBoxProp, new GUIContent("Spawn Box"), true);

                EditorGUI.indentLevel++;
                break;
        }
        EditorGUILayout.Space();

        // Spawning method property
        EditorGUILayout.PropertyField(methodProp);

        // Choose the spawning method
        PowerUpSpawner.SpawningMethod method = (PowerUpSpawner.SpawningMethod)methodProp.enumValueIndex;
        switch (method)
        {
            // Random Spawning Method
            case PowerUpSpawner.SpawningMethod.Random :
                EditorGUI.indentLevel++;
                EditorGUIUtility.labelWidth = 150;

                // Designers may choose which power-up's to randomly spawn

                // Speed Power-Up Data
                EditorGUILayout.PropertyField(speedProp, new GUIContent("Speed PowerUp"));
                speedStructProp.FindPropertyRelative("flag").boolValue = speedProp.boolValue;
                if (speedProp.boolValue)
                    AddSpeed();

                // Rope Power-Up Data
                EditorGUILayout.PropertyField(ropeProp, new GUIContent("Rope PowerUp"));
                ropeStructProp.FindPropertyRelative("flag").boolValue = ropeProp.boolValue;
                if (ropeProp.boolValue)
                    AddRope();

                // Knockback Power-Up Data
                EditorGUILayout.PropertyField(knockbackProp, new GUIContent("Knockback PowerUp"));
                knockbackStructProp.FindPropertyRelative("flag").boolValue = knockbackProp.boolValue;
                if (knockbackProp.boolValue)
                    AddKnockback();

                // Invincibility Power-Up Data
                EditorGUILayout.PropertyField(invincibilityProp, new GUIContent("Invincibility PowerUp"));
                invincibilityStructProp.FindPropertyRelative("flag").boolValue = knockbackProp.boolValue;
                if (invincibilityProp.boolValue)
                    AddInvincibility();

                EditorGUI.indentLevel--;
                break;

            // Single Power-Up Selection mode
            case PowerUpSpawner.SpawningMethod.Single :
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(selectionProp, new GUIContent("Power-Up"));

                // Get the PowerUpType selected and display the property data
                PowerUpSpawner.PowerUpType selectionType = (PowerUpSpawner.PowerUpType)selectionProp.enumValueIndex;
                switch (selectionType)
                {
                    case PowerUpSpawner.PowerUpType.Speed :
                        AddSpeed();
                        break;

                    case PowerUpSpawner.PowerUpType.Rope :
                        AddRope();
                        break;

                    case PowerUpSpawner.PowerUpType.Knockback :
                        AddKnockback();
                        break;

                    case PowerUpSpawner.PowerUpType.Incibility :
                        AddInvincibility();
                        break;
                }

                EditorGUI.indentLevel--;
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

    // Adds Speed property data
    private void AddSpeed()
    {
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(speedStructProp.FindPropertyRelative("param1"), new GUIContent("Spawn Delay"));
        EditorGUILayout.PropertyField(speedStructProp.FindPropertyRelative("param2"), new GUIContent("Boost Duration"));
        EditorGUILayout.PropertyField(speedStructProp.FindPropertyRelative("param3"), new GUIContent("Percent Boost"));
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
    }

    // Adds Rope property data
    private void AddRope()
    {
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(ropeStructProp.FindPropertyRelative("param1"), new GUIContent("Spawn Delay"));
        EditorGUILayout.PropertyField(ropeStructProp.FindPropertyRelative("param2"), new GUIContent("Boost Duration"));
        EditorGUILayout.PropertyField(ropeStructProp.FindPropertyRelative("param3"), new GUIContent("Percent Boost"));
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
    }

    // Adds Knockback property data
    private void AddKnockback()
    {
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(knockbackStructProp.FindPropertyRelative("param1"), new GUIContent("Spawn Delay"));
        EditorGUILayout.PropertyField(knockbackStructProp.FindPropertyRelative("param2"), new GUIContent("Knockback Radius"));
        EditorGUILayout.PropertyField(knockbackStructProp.FindPropertyRelative("param3"), new GUIContent("Knockback Intensity"));
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
    }

    // Adds Invincibility property data
    private void AddInvincibility()
    {
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(invincibilityStructProp.FindPropertyRelative("param1"), new GUIContent("Spawn Delay"));
        EditorGUILayout.PropertyField(invincibilityStructProp.FindPropertyRelative("param2"), new GUIContent("Boost Duration"));
        EditorGUI.indentLevel--;
        EditorGUILayout.Space();
    }
}