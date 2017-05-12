using UnityEngine;
using System.Collections;

public class messagebox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void click_button_ok()
    {
        GameObject obj = GameObject.Find("messagebox");
        if (obj != null)
            obj.SetActive(false);
    }
}
