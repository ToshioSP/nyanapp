//  MovieAdManager.cs
//  http://kan-kikuchi.hatenablog.com/entry/UnityAds
//
//  Created by kang05057 on 2015.08.31.

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;

/// <summary>
/// 動画広告を管理するクラス
/// </summary>
public class MovieAdManager : SingletonMonoBehaviour<MovieAdManager>
{

    //Unityの広告ID
    [SerializeField]
    private string _unityAdsiosID, _unityAdsAndroidID;

    //=================================================================================
    //初期化
    //=================================================================================

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        //プラットフォームが対応しているか判定し、初期化
        if (Advertisement.isSupported)
        {
            Advertisement.allowPrecache = true;

#if UNITY_IOS
      Advertisement.Initialize (_unityAdsiosID);
#elif UNITY_ANDROID
            Advertisement.Initialize(_unityAdsAndroidID);
#endif
        }
        else {
            Debug.Log("プラットフォームがUnityAdsに対応していません");
        }
    }

    //=================================================================================
    //判定、取得
    //=================================================================================

    /// <summary>
    /// 動画を再生する事ができるか
    /// </summary>
    public bool CanPlay()
    {
        //プラットフォームが対応しているかつ準備が完了している時だけtrueを返す
        return Advertisement.isSupported && Advertisement.isReady();
    }

    //=================================================================================
    //動画再生
    //=================================================================================

    /// <summary>
    /// 動画再生
    /// </summary>
    public void Play(Action OnFinished = null, Action OnFailed = null, Action OnSkipped = null)
    {

        //コールバック用メソッド作成、Result の値は Finished、Failed、Skipped
        Action<ShowResult> callBack = (result) => {

            if (result == ShowResult.Finished && OnFinished != null)
            {
                OnFinished();
            }
            else if (result == ShowResult.Failed && OnFailed != null)
            {
                OnFailed();
            }
            else if (result == ShowResult.Skipped && OnSkipped != null)
            {
                OnSkipped();
            }

        };

        //動画再生
        Advertisement.Show(null, new ShowOptions
        {
            //trueだとUnityが止まり、音もミュートになる
            pause = true,
            //広告が表示された後のコールバック設定
            resultCallback = callBack
        });

    }

}