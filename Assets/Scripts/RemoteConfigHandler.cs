using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;
using Unity.Services.RemoteConfig;

public class RemoteConfigHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!MainMenuManager.isSessionActive)
        {
            SetRemoteConfigDefaults();
            MainMenuManager.isSessionActive = true;
        }
    }

    private void SetRemoteConfigDefaults()
    {
        Dictionary<string, object> defaults = new Dictionary<string, object>();
        defaults.Add("isBannerAdEnable", true);
        defaults.Add("bannerAd_Id", "ca-app-pub-3940256099942544/6300978111");
        defaults.Add("isInterstitialAdEnable", true);
        defaults.Add("interstitialAd_Id", "ca-app-pub-3940256099942544/1033173712");
        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        remoteConfig.SetDefaultsAsync(defaults).ContinueWithOnMainThread(
           previousTask =>
           {
               //set default values to main manager
               MainMenuManager.isBannerAdEnable = remoteConfig.GetValue("isBannerAdEnable").BooleanValue;
               MainMenuManager.bannerAd_Id = remoteConfig.GetValue("bannerAd_Id").StringValue;
               MainMenuManager.isInterstitialAdEnable = remoteConfig.GetValue("isInterstitialAdEnable").BooleanValue;
               MainMenuManager.interstitialAd_Id = remoteConfig.GetValue("interstitialAd_Id").StringValue;
               FetchDataAsync();
           }
        );
    }

    public Task FetchDataAsync()
    {
        Debug.Log("Fetching data...");
        Task fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        return fetchTask.ContinueWithOnMainThread(FetchComplete);
    }

    private void FetchComplete(Task fetchTask)
    {
        if (!fetchTask.IsCompleted)
        {
            Debug.LogError("Retrieval hasn't finished.");
            return;
        }

        var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
        var info = remoteConfig.Info;
        if (info.LastFetchStatus != LastFetchStatus.Success)
        {
            Debug.LogError($"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
            return;
        }

        // Fetch successful. Parameter values must be activated to use.
        remoteConfig.ActivateAsync()
          .ContinueWithOnMainThread(
            task => {
                Debug.Log($"Remote data loaded and ready for use. Last fetch time {info.FetchTime}.");
                
                MainMenuManager.isBannerAdEnable = remoteConfig.GetValue("isBannerAdEnable").BooleanValue;
                MainMenuManager.bannerAd_Id = remoteConfig.GetValue("bannerAd_Id").StringValue;
                MainMenuManager.isInterstitialAdEnable = remoteConfig.GetValue("isInterstitialAdEnable").BooleanValue;
                MainMenuManager.interstitialAd_Id = remoteConfig.GetValue("interstitialAd_Id").StringValue;
            });
    }

}
