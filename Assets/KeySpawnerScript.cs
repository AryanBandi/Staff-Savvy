using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeySpawnerScript : MonoBehaviour
{
    public GameObject sharp; // Prefab for sharp (UI Image or UI element)
    public GameObject flat;  // Prefab for flat (UI Image or UI element)
    public Canvas canvas;    // Reference to the canvas
    public Dictionary<KeyCode, bool> keySignature = new Dictionary<KeyCode, bool>();    // holds the sharp (true) and flat (false) notes
    private float distApart = 50;
    private float offset;
    private List<KeyCode> notes = new List<KeyCode> { KeyCode.F, KeyCode.C, KeyCode.G, KeyCode.D, KeyCode.A, KeyCode.E, KeyCode.B };
    private float[] sharpPos = new float[] { 163, 50, 197, 88, -29, 127 };
    private float[] flatPos = new float[] { 75, 192, 40, 153, -8, 111 };
    private int key;

    void Start()
    {
        // Get offset based on clef index
        offset = PlayerPrefs.GetInt("ActiveClefIndex", 0) * 39;
        key = PlayerPrefs.GetInt("Key", 0);

        if (key > 5)
        {
            // Instantiate sharp notes as UI elements
            for (int i = 0; i < 12 - key; i++)
            {
                GameObject newSharp = Instantiate(sharp, canvas.transform);
                RectTransform sharpRect = newSharp.GetComponent<RectTransform>();

                // Adjust the position of the sharp note in UI space
                sharpRect.anchoredPosition = new Vector2(-775 + (i * distApart), sharpPos[i] - offset);
                keySignature[notes[i]] = true;
            }
        }
        else
        {
            // Instantiate flat notes as UI elements
            for (int i = 0; i < key; i++)
            {
                GameObject newFlat = Instantiate(flat, canvas.transform);
                RectTransform flatRect = newFlat.GetComponent<RectTransform>();

                // Adjust the position of the flat note in UI space
                flatRect.anchoredPosition = new Vector2(-775 + (i * distApart), flatPos[i] - offset);
                keySignature[notes[notes.Count - i - 1]] = false;
            }
        }
    }
}
