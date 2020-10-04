using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TriangleBlock : MonoBehaviour
{
    private void OnEnable()
    {
        float angle = Random.Range(0, 4) * 90f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        

        TextMeshPro text = gameObject.GetComponentInChildren<TextMeshPro>();
        text.rectTransform.localRotation = Quaternion.Euler(0f, 0f, -angle);
        switch (angle)
        {
            case 0f:
                text.alignment = TextAlignmentOptions.BottomRight;
                break;
            case 90f:
                text.alignment = TextAlignmentOptions.TopRight;
                break;
            case 180f:
                text.alignment = TextAlignmentOptions.TopLeft;
                break;
            case 270f:
                text.alignment = TextAlignmentOptions.BottomLeft;
                break;
        }
    }
}
