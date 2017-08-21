using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace HauntedCity.Utils
{
  
    public class DownloadObb : MonoBehaviour
    {
        private string expPath;
        private string logtxt;
        private bool alreadyLogged = false;
        private string nextScene = "start_scene";
        private bool downloadStarted;

    
        private void Awake()
        {
            #if UNITY_EDITOR
            SceneManager.LoadScene(nextScene);
            #else
            GooglePlayDownloader.FetchOBB();
            StartCoroutine(loadLevel());
            #endif
        }

        protected IEnumerator loadLevel()
        {
            string mainPath;
            do
            {
                yield return new WaitForSeconds(0.5f);
                mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
            } while (mainPath == null);

            if (downloadStarted == false)
            {
                downloadStarted = true;

                string uri = "file://" + mainPath;
                WWW www = WWW.LoadFromCacheOrDownload(uri, 0);

                // Wait for download to complete
                yield return www;

                if (www.error != null)
                {
                }
                else
                {
                    SceneManager.LoadScene(nextScene);
                }
            }
        }
    }
}