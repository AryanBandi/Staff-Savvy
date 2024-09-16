using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawnScript : MonoBehaviour
{
    public GameObject note;
    public GameObject trebleClef;
    public GameObject altoClef;
    public GameObject bassClef;
    public Canvas canvas;
    private float spawnRate = 0.5f;
    private int curCount = 0;
    private float timer = 0;
    private List<KeyCode> letterNames = new List<KeyCode> { KeyCode.D, KeyCode.E, KeyCode.F, KeyCode.G, KeyCode.A, KeyCode.B, KeyCode.C };
    private int noteNameDiff = 0;
    private int currentClefIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentClefIndex = PlayerPrefs.GetInt("ActiveClefIndex", 0);
        trebleClef.SetActive(currentClefIndex == 0);
        altoClef.SetActive(currentClefIndex == 1);
        bassClef.SetActive(currentClefIndex == 2);
        noteNameDiff = currentClefIndex;
    }

    // Update is called once per frame
    void Update()
    {
        int randint = Random.Range(0, 11);

        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            if (curCount < 8)
            {
                // Instantiate the note prefab under the Canvas as a UI element
                GameObject newNote = Instantiate(note, canvas.transform);
                newNote.transform.SetParent(canvas.transform, false);

                // Set the anchored position for the new note
                RectTransform noteRectTransform = newNote.GetComponent<RectTransform>();
                noteRectTransform.anchoredPosition = new Vector2(1100, -72 + randint * 39);

                // Assign the key for this note (for gameplay input)
                NoteScript script = newNote.GetComponent<NoteScript>();
                script.noteName = letterNames[(randint + noteNameDiff) % letterNames.Count];

                // Increment the count of spawned notes
                curCount++;
                timer = 0;

                // Handle upward stem rotation if needed
                if (randint > 5)
                {
                    // Adjust the note position and rotate it by 180 degrees for upward stems
                    noteRectTransform.anchoredPosition += new Vector2(0, -221);
                    noteRectTransform.localRotation = Quaternion.Euler(0, 0, 180);
                }
            }
        }
    }

    // Decrement the count of notes when one is removed
    public void removeFromCount()
    {
        curCount--;
        NoteScript[] notes = FindObjectsOfType<NoteScript>();
        foreach (NoteScript note in notes)
        {
            if (note.curIndex > 1)
            {
                note.curIndex--;
            }
        }
    }

    // Get the current number of notes
    public int getCount()
    {
        return curCount;
    }
}
