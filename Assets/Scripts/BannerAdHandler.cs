using GoogleMobileAds.Api;
using System;
using Unity.Services.RemoteConfig;
using UnityEngine;

public class BannerAdHandler : MonoBehaviour
{
    BannerView _bannerView;


    private void Start()
    {
        if (MainMenuManager.isBannerAdEnable)
        {
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                CreateBannerView();
            });
        }
    }
    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyAd();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(MainMenuManager.bannerAd_Id, AdSize.Banner, AdPosition.Top);

    }

    public void LoadAd()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }
        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        Debug.Log("Loading banner ad.");
        _bannerView.LoadAd(adRequest);
    }



    /// <summary>
    /// Destroys the ad.
    /// </summary>
    public void DestroyAd()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner ad.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    private void OnDestroy()
    {
        DestroyAd();
    }
    private void OnEnable()
    {
        Player.onGameoverLoadBannerAd += LoadAd;
    }

    private void OnDisable()
    {
        Player.onGameoverLoadBannerAd -= LoadAd;
    }

}