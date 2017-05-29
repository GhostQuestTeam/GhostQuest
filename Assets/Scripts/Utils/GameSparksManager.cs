using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Networking
{
    public class GameSparksManager : MonoBehaviour
    {
        private static GameSparksManager _instance = null;
        void Awake() {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
            } else
            {
                Destroy(this.gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}