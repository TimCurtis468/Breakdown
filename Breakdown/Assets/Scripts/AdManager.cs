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
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;

    private bool interAdClosed = true;
    public bool rewardGiven = false;
    public bool rewardedAdClosed = false;
    public bool rewardedAdFailed = false;

#if (PI)
    private string bannerAdId = "ca-app-pub-3940256099942544/6300978111";
    private string interstitialAdId = "ca-app-pub-3940256099942544/1033173712";
    private string rewardedAdId = "ca-app-pub-3940256099942544/5224354917";
#else
    private string bannerAdId = "ca-app-pub-4187256387197374/6028382101";
    private string interstitialAdId = "ca-app-pub-4187256387197374/8313417055";
    private string rewardedAdId = "ca-app-pub-4187256387197374/1935904482";
#endif

    public void Start()
    {
        interAdClosed = true;
        rewardGiven = false;
        rewardedAdClosed = false;
        rewardedAdFailed = false;
    }

    /**********
     * BANNER *
     **********/
    public void RequestBanner(AdPosition adPosition)
    {
        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(bannerAdId, AdSize.Banner, adPosition);

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

    /****************
     * INTERSTITIAL *
     ****************/
    public void RequestInterstital()
    {
        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(interstitialAdId);

        // Called when an ad request has successfully loaded.
        this.interstitial.OnAdLoaded += HandleOnInterstitialAdLoaded;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnInterstitialAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);

        interAdClosed = false;
    }
    public void HandleOnInterstitialAdLoaded(object sender, EventArgs args)
    {
        this.interstitial.Show();
        MusicManager.Instance.PauseMusic();

    }
    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        interAdClosed = true;
        MusicManager.Instance.UnPauseMusic();
    }

    public bool isInterstialClosed()
    {
        return interAdClosed;
    }

    public void DestroyInterstitial()
    {
        if (interstitial != null)
        {
            this.interstitial.OnAdLoaded -= HandleOnInterstitialAdLoaded;
            interstitial.Destroy();
        }
    }

    /****************
     * REWARDED     *
     ****************/
    public void RequestRewarded()
    {
        // Rewarded ad
        this.rewardedAd = new RewardedAd(rewardedAdId);

        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += OnRewardedAdClosed;
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the ad with the request.
        this.rewardedAd.LoadAd(request);

        rewardGiven = false;
        rewardedAdClosed = false;
        rewardedAdFailed = false;
    }



    public void DestroyRewarded()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            this.rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
            this.rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
            this.rewardedAd.OnAdClosed -= OnRewardedAdClosed;
            this.rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;

        }
    }

    public void ShowRewardedAd()
    {
        this.rewardedAd.Show();
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
//        this.rewardedAd.Show();
    }
    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        rewardedAdFailed = true;
    }


    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //        string type = args.Type;
        //        double amount = args.Amount;
        //        MonoBehaviour.print(
        //            "HandleRewardedAdRewarded event received for "
        //                        + amount.ToString() + " " + type);
        //       GameManager.Instance.GameOverExtraLife();
        rewardGiven = true;
    }

    public void OnRewardedAdClosed(object sender, EventArgs args)
    {
        rewardedAdClosed = true;
    }


    public void OnAdClosed(object sender, EventArgs args)
    {
 //       GameManager.Instance.MoveToNextScene();
    }
}

