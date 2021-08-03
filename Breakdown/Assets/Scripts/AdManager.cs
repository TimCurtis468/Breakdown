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

    private string bannerAdId = "ca-app-pub-3940256099942544/6300978111";
    private string interstitialAdId = "ca-app-pub-3940256099942544/1033173712";
    private string rewardedAdId = "ca-app-pub-3940256099942544/5224354917";

    public void Start()
    {
        interAdClosed = true;
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
    }
    public void HandleOnInterstitialAdClosed(object sender, EventArgs args)
    {
        interAdClosed = true;
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


        // Called when an ad request has successfully loaded.
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd.OnAdClosed += OnAdClosed;

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the ad with the request.
        this.rewardedAd.LoadAd(request);

    }

    public void DestroyRewarded()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            this.rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        }
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        this.rewardedAd.Show();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //        string type = args.Type;
        //        double amount = args.Amount;
        //        MonoBehaviour.print(
        //            "HandleRewardedAdRewarded event received for "
        //                        + amount.ToString() + " " + type);
        GameManager.Instance.GameOverExtraLife();
    }

    public void OnAdClosed(object sender, EventArgs args)
    {
 //       GameManager.Instance.MoveToNextScene();
    }
}

