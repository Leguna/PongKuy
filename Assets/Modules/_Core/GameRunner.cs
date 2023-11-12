using System;
using Constant;
using Cysharp.Threading.Tasks;
using LoadingModule;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GameRunner : SingletonMonoBehaviour<GameRunner>
{
    public LoadingManager LoadingManager { get; private set; }

    protected override async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await SceneManager.LoadSceneAsync((int)SceneConst.SceneName.LoadingScreen, LoadSceneMode.Additive);
            LoadingManager = FindObjectOfType<LoadingManager>();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}