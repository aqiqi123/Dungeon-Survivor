using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene {
        MainMenuScene,
        GameScene,
        LoadingScene
    }

    private static Scene targetScene;

    public static Scene TargetScene => targetScene;

    public static void Load(Scene targetScene) {
        Loader.targetScene = targetScene;
        SceneTransition.Instance.StartTransition(() =>
        {
            SceneManager.LoadScene(Scene.LoadingScene.ToString());
        });
    }
}
