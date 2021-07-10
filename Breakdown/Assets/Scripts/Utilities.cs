using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Utilities : MonoBehaviour
{
    #region Singleton
    private static Utilities _instance;
    public static Utilities Instance => _instance;

    private float widthFactor;
    private float heightFactor;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;

            widthFactor = Screen.width / 1920.0f;
            heightFactor = Screen.height / 1080.0f;

        }
    }
    #endregion

    public void Resize(GameObject gameObject)
    {
        float objectWidth;
        float objectHeight;
        RectTransform rt;

        rt = gameObject.GetComponent<RectTransform>();
        objectWidth = rt.rect.width;
        objectHeight = rt.rect.height;

        objectWidth = objectWidth * widthFactor;
        objectHeight = objectHeight * heightFactor;

        rt.sizeDelta = new Vector2(objectWidth, objectHeight);
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x * widthFactor, rt.anchoredPosition.y * heightFactor);
    }

    public void ResizeText(GameObject gameObject)
    {
        Text t = gameObject.GetComponent<Text>();
        t.fontSize = (t.fontSize * Screen.width) / 1920;
    }

    public void ResizeSpriteRendered(GameObject gameObject)
    {
        float objectWidth;
        float objectHeight;
        SpriteRenderer sr;

        sr = gameObject.GetComponent<SpriteRenderer>();
        objectWidth = sr.transform.localScale.x;
        objectHeight = sr.transform.localScale.y;

        objectWidth = objectWidth * widthFactor;
        objectHeight = objectHeight * heightFactor;

        sr.transform.localScale = new Vector2(objectWidth, objectHeight);
    }
}
