using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script;
using UnityEngine.SceneManagement;
using UnityEngine;
namespace Assets.Script
{
    public class CareHistory:MonoBehaviour
    {
        public static string strMoveSceneName = "Main";
        public static string strData = null;
        public void Start()
        {
            strMoveSceneName = "Main";
        }

        public void MoveScene()
        {
            SceneManager.LoadScene(strMoveSceneName);
        }

    }
}
