using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class HighlightSelf : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Sprite plainSprite;
    [SerializeField]
    private Sprite highlightedSprite;

    private void Start()
    {
        // Fetch the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        plainSprite = spriteRenderer.sprite;

        // Subscribe to the event system
        MapEvents.current.onLocationSelected += Highlight;
    }

    private void Highlight(GameObject location)
    {
        // If this is the location that was clicked on, highlight self
        // Otherwise remove highlights
        spriteRenderer.sprite = (location.name.Equals(this.name)) ? highlightedSprite : plainSprite;
    }
}
