using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Animation anim = gameObject.GetComponent<Animation>();
        if(!anim.isPlaying)
        {
            anim.Play();
        }
	}//upd
}
