using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    public GameObject Title;
    public GameObject VV;
    public GameObject background;

    // Start is called before the first frame update
    void Start()
    {
        Utilities.ResizeAndPositionSprite(Title.gameObject);
        Utilities.ResizeAndPositionSprite(VV.gameObject);
        Utilities.ResizeSpriteToFullScreen(background.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeMenuScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
