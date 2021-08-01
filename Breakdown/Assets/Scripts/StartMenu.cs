using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    public GameObject Title;
    public GameObject VV;
    public GameObject background;
    public GameObject loading;

    // Start is called before the first frame update
    void Start()
    {
        loading.SetActive(false);

        Utilities.ResizeAndPositionSprite(Title.gameObject);
        Utilities.ResizeAndPositionSprite(VV.gameObject);
        Utilities.ResizeAndPositionSprite(loading.gameObject);
        Utilities.ResizeSpriteToFullScreen(background.gameObject);

        AdManager.Instance.RequestBanner(GoogleMobileAds.Api.AdPosition.Bottom);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeMenuScene(string sceneName)
    {
        AdManager.Instance.DestroyBanner();
        loading.SetActive(true);
        SceneManager.LoadScene(sceneName);
    }
}
