using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    private int hitsRemaining = 10;
    private SpriteRenderer spriteRenderer;
    private TextMeshPro text;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        text.SetText(hitsRemaining.ToString());
        //Color change system
        //spriteRenderer.color = GetBlockColor(hitsRemaining);
        spriteRenderer.color = Color.Lerp(Color.yellow, Color.red, hitsRemaining / 10f);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        hitsRemaining--;
        if (hitsRemaining > 0)
        {
            UpdateVisualState();
        }
        else
        {
            GameController.instance.CheckWin();
            GameController.instance.PlayBlockDestroyedEffect(transform.position);
            Destroy(gameObject);
        }
    }

    internal void SetHits(int hits)
    {
        hitsRemaining = hits;
        UpdateVisualState();
    }
}
