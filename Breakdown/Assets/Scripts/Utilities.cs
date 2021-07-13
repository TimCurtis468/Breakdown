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

    static public void ResizeSprite(GameObject gameObject)
    {
        var topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float worldSpaceWidth = topRightCorner.x * 2f;
        float worldSpaceHeight = topRightCorner.y * 2f;

        // width and scale relavtive to 1080 x 1920 
        float worldWidthScale = worldSpaceWidth / 5.625f;
        float worldHeightScale = worldSpaceHeight / 10.0f;
        var spriteSize = gameObject.transform.localScale;

        float scaleFactorX = worldWidthScale * spriteSize.x;
        float scaleFactorY = worldHeightScale * spriteSize.y;
        gameObject.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
    }

    static public void ResizeAndPositionSprite(GameObject gameObject)
    {
        var topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float worldSpaceWidth = topRightCorner.x * 2f;
        float worldSpaceHeight = topRightCorner.y * 2f;

        // width and scale relavtive to 1080 x 1920 
        float worldWidthScale = worldSpaceWidth / 5.625f;
        float worldHeightScale = worldSpaceHeight / 10.0f;
        var spriteSize = gameObject.transform.localScale;

        float scaleFactorX = worldWidthScale * spriteSize.x;
        float scaleFactorY = worldHeightScale * spriteSize.y;
        gameObject.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);

        var spriteLocation = gameObject.transform.localPosition;
        float posFactorX = worldWidthScale * spriteLocation.x;
        float posFactorY = worldHeightScale * spriteLocation.y;
        gameObject.transform.localPosition = new Vector3(posFactorX, posFactorY, 1);

    }

    static public float ResizeXValue(float x)
    {
        var topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float worldSpaceWidth = topRightCorner.x * 2f;

        // width and scale relavtive to 1080 x 1920 
        float worldWidthScale = worldSpaceWidth / 5.625f;
        x = x * worldWidthScale;

        return x;
    }

    static public float ResizeYValue(float y)
    {
        var topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float worldSpaceHeight = topRightCorner.y * 2f;

        // width and scale relavtive to 1080 x 1920 
        float worldHeightScale = worldSpaceHeight / 10.0f;
        y = y * worldHeightScale;

        return y;
    }

    static public void ResizeAndPositionUI(GameObject gameObject)
    {
//#if (PI)
        var topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float worldSpaceWidth = topRightCorner.x * 2f;
        float worldSpaceHeight = topRightCorner.y * 2f;

        float worldWidthScale = worldSpaceWidth / 5.625f;
        float worldHeightScale = worldSpaceHeight / 10.0f;
        var spriteSize = gameObject.GetComponent<RectTransform>().rect;

        float scaleFactorX = worldWidthScale * spriteSize.x;
        float scaleFactorY = worldHeightScale * spriteSize.y;
        gameObject.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);

        var spriteLocation = gameObject.transform.localPosition;
        float posFactorX = worldWidthScale * spriteLocation.x;
        float posFactorY = worldHeightScale * spriteLocation.y;
        gameObject.transform.localPosition = new Vector3(posFactorX, posFactorY, 1);
//#endif

#if (PI)
        float objectWidth;
        float objectHeight;
        RectTransform rt;
        float widthFactor = Screen.width / 1920.0f;
        float heightFactor = Screen.height / 1080.0f;


        rt = gameObject.GetComponent<RectTransform>();
        objectWidth = rt.rect.width;
        objectHeight = rt.rect.height;

        objectWidth = objectWidth * widthFactor;
        objectHeight = objectHeight * heightFactor;

        rt.sizeDelta = new Vector2(objectWidth, objectHeight);
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x * widthFactor, rt.anchoredPosition.y * heightFactor);
#endif
    }

    static public void ResizeText(GameObject gameObject)
    {
        Text t = gameObject.GetComponent<Text>();
        t.fontSize = (t.fontSize * Screen.width) / 1080;
    }

    static public void ResizeSpriteToFullScreen(GameObject gameObject)
    {
        var topRightCorner = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        var worldSpaceWidth = topRightCorner.x * 2;
        var worldSpaceHeight = topRightCorner.y * 2;
        var spriteSize = gameObject.GetComponent<SpriteRenderer>().bounds.size;
        var scaleFactorX = worldSpaceWidth / spriteSize.x;
        var scaleFactorY = worldSpaceHeight / spriteSize.y;

        gameObject.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, 1);
    }
}
