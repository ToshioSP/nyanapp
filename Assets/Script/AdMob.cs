﻿using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class AdMob : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        RequestBanner();
    }

    private void RequestBanner()
    {
        // 広告ユニット ID を記述します
        string adUnitId = "ca-app-pub-8456666231982348/5128178318";

        // Create a 320x50 banner at the top of the screen.
        BannerView bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);
    }
}
