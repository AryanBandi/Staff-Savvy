using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Import for UI elements

public class NoteScript : MonoBehaviour
{
    public GameObject note;
    private KeySpawnerScript KeySpawner;  // Reference to the KeySpawnerScript
    public NoteSpawnScript NoteSpawner;
    public float stopPos;
    public int curIndex;
    public KeyCode noteName;
    private float movementSpeed = 3000f;
    private float distApart = 150f;
    private bool isAnimating;
    private float animationDuration = 1f;
    private float animationTimer = 0f;
    private float initialY;
    private Color initialColor;

    private RectTransform rectTransform; // To replace Transform for UI elements
    private Image image; // To replace SpriteRenderer for UI

    public bool isSharp = false;
    public bool isFlat = false;

    // Start is called before the first frame update
    void Start()
    {
        // Get RectTransform component for UI manipulation
        rectTransform = GetComponent<RectTransform>();

        // Get Image component for the UI sprite
        image = GetComponent<Image>();

        NoteSpawner = GameObject.FindObjectOfType<NoteSpawnScript>();
        KeySpawner = GameObject.FindObjectOfType<KeySpawnerScript>();
        if (KeySpawner == null)
        {
            print("KeySpawnerScript not found in the scene.");
        }
        curIndex = NoteSpawner.getCount();
        initialY = rectTransform.anchoredPosition.y; // Use RectTransform's anchoredPosition for Canvas UI

        // Store initial color of the Image (alpha manipulation for fade out)
        initialColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        stopPos = getStopPos(curIndex);

        if (!isAnimating)
        {
            if (rectTransform.anchoredPosition.x > stopPos) // Use anchoredPosition for UI movement
            {
                rectTransform.anchoredPosition += Vector2.left * movementSpeed * Time.deltaTime; // Move in UI space
            }
            else
            {
                if (Input.GetKeyDown(noteName))
                {
                    bool shiftCondition = false;

                    if (KeySpawner != null && KeySpawner.keySignature.ContainsKey(noteName))
                    {
                        bool isSharp = KeySpawner.keySignature[noteName]; // true for sharp, false for flat

                        // If the note is sharp, check for Right Shift
                        if (isSharp)
                        {
                            shiftCondition = Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift);
                        }
                        // If the note is flat, check for Left Shift
                        else
                        {
                            shiftCondition = !Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.LeftShift);
                        }
                    }
                    else
                    {
                        // If the key isn't in the dictionary, the note is not sharp or flat, and no shift is required
                        shiftCondition = !Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift);
                    }


                    if (shiftCondition && curIndex == 1)
                    {
                        NoteSpawner.removeFromCount();
                        isAnimating = true;
                        animationTimer = 0f; // Reset the animation timer
                    }
                }
            }
        }
        else
        {
            KillSprite();
        }
    }

    void KillSprite()
    {
        animationTimer += Time.deltaTime;

        // Calculate the animation progress
        float progress = animationTimer / animationDuration;

        // Move upwards in UI space
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, initialY + progress * 5f);

        // Rotate clockwise by 60 degrees over the duration (UI space uses RectTransform rotation)
        rectTransform.rotation = Quaternion.Euler(0, 0, progress * 60f);

        // Fade out by adjusting the alpha of the Image component
        Color color = image.color;
        color.a = Mathf.Lerp(initialColor.a, 0, progress);
        image.color = color;

        // Check if the animation duration has been reached
        if (progress >= 1f)
        {
            Destroy(note); // Despawn the UI element
        }
    }

    float getStopPos(int curIndex)
    {
        return -475 + (curIndex * distApart);
    }
}
