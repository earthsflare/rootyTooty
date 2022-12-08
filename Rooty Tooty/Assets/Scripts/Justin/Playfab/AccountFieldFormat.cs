using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AccountFieldFormat : FieldFormat
{
    [Header("Account Field References")]
    [SerializeField] private TMP_Text label = null;
    [SerializeField] private TMP_InputField inputField = null;
    private RectTransform inputRectTransform;
    [SerializeField] private TMP_Text error = null;
    [SerializeField] private TMP_Text forgotButton = null;
    [SerializeField] private TMP_Text description = null;

    private RectTransform rectTransform;

    [Header("Offset Properties")]
    //Spacing between different Account Fields
    [SerializeField] private float accountFieldOffset = 10f;
    //Spacing between descriptions (forgotButton, error, description)
    private const float descriptionOffset = 0.4f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (inputField != null)
            inputRectTransform = inputField.GetComponent<RectTransform>();
    }

    private void Start()
    {
        if(above == null)
            StartCoroutine(Waiting());
    }

    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1);
        FormatFields();
    }

    //Make sure fields are spaced out based on spacing variables
    public override void FormatFields()
    {
        //Position inputRectTransform if it's 
        if(above != null)
        {
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,
                above.GetBottom() - (inputRectTransform.sizeDelta.y * 0.5f) - accountFieldOffset);
            label.rectTransform.anchoredPosition = new Vector2(label.rectTransform.anchoredPosition.x, 
                rectTransform.anchoredPosition.y);
        }

        float height = inputRectTransform.anchoredPosition.y - (inputRectTransform.sizeDelta.y * 0.5f);

        if (error != null)
        {
            height -= (error.rectTransform.sizeDelta.y * 0.5f);
            error.rectTransform.anchoredPosition = new Vector2(error.rectTransform.anchoredPosition.x,
                height);

            height -= ((error.rectTransform.sizeDelta.y * 0.5f) + descriptionOffset);
        }

        if (forgotButton != null)
        {
            height -= (forgotButton.rectTransform.sizeDelta.y * 0.5f);
            forgotButton.rectTransform.anchoredPosition = new Vector2(forgotButton.rectTransform.anchoredPosition.x,
                height);

            height -= ((forgotButton.rectTransform.sizeDelta.y * 0.5f) + descriptionOffset);
        }

        if (description != null)
        {
            height -= (description.rectTransform.sizeDelta.y * 0.5f);
            description.rectTransform.anchoredPosition = new Vector2(description.rectTransform.anchoredPosition.x,
                height);

            height -= ((description.rectTransform.sizeDelta.y * 0.5f) + descriptionOffset);
        }


        if (below != null)
            below.FormatFields();

    }
    public override float GetBottom()
    {
        float height = rectTransform.anchoredPosition.y;

        if (inputField != null)
            if (inputField.gameObject.activeSelf)
                return height + inputRectTransform.anchoredPosition.y - (inputRectTransform.sizeDelta.y * 0.5f);

        return height;
    }
}
