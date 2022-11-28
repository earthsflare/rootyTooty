using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFieldFormat : FieldFormat
{
    [SerializeField] private RectTransform button;
    private RectTransform rectTransform;

    [SerializeField] private float buttonOffset = 1f;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public override void FormatFields()
    {
        if(above != null)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
                above.GetBottom() - (button.sizeDelta.y * 0.5f) - buttonOffset);
        }

        if (below != null)
                below.FormatFields();
    }

    public override float GetBottom()
    {
        float height = rectTransform.anchoredPosition.y;

        if (button != null)
            if (button.gameObject.activeSelf)
                return height + button.anchoredPosition.y - (button.sizeDelta.y * 0.5f);

        return height;
    }
}
