using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using GoogleMobileAds.Api;

/*
 * code from https://developers.google.com/admob/unity/test-ads#enable_test_devices
 * */

public class AdManager : MonoBehaviour
{
    #region Singleton
    private static AdManager _instance;
    public static AdManager Instance => _instance;


    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize(initStatus => { });
        }
    }
    #endregion

    private BannerView bannerView;

    private string adUnitId = "ca-app-pub-3940256099942544/6300978111";

    public void Start()
    {

    }

    public void RequestBanner(AdPosition adPosition)
    {
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, adPosition);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    public void DestroyBanner()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
}

