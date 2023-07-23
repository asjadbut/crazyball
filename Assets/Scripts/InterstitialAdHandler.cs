using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterstitialAdHandler : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    // Start is called before the first frame update
    void Start()
    {
        if (MainMenuManager.isInterstitialAdEnable)
        {
            LoadInterstitialAd();
        }
    }

    /// <summary>
    /// Loads the interstitial ad.
    /// </summary>
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        adRequest.Keywords.Add("unity-admob-sample");

        // send the request to load the ad.
        InterstitialAd.Load(MainMenuManager.interstitialAd_Id, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
            });
    }

    /// <summary>
    /// Shows the interstitial ad.
    /// </summary>
    public void ShowAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void OnDestroy()
    {
        if (MainMenuManager.isInterstitialAdEnable)
            interstitialAd.Destroy();
    }
    private void OnEnable()
    {
        GameManager.onGameoverLoadInterstitialAd += ShowAd;
    }
    private void OnDisable()
    {
        GameManager.onGameoverLoadInterstitialAd -= ShowAd;
    }
}
