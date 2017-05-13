using UnityEngine;
using System.Collections;

public class messagebox : MonoBehaviour {

	public enum BTNPress{
		BTN_OK,
		BTN_CANCEL,
	};

	public delegate void OkDelegate();
	public delegate void CancelDelegate();

	public static messagebox.BTNPress 	m_btnPress;
	public static OkDelegate			m_funOk = null;
	public static CancelDelegate 		m_funCancel = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	static public void set_callback(OkDelegate funOk = null,  CancelDelegate funCancel = null)
	{
		m_funOk = funOk;
		m_funCancel = funCancel;
	}

    public void click_button_ok()
    {
        GameObject obj = GameObject.Find("messagebox");
        if (obj != null)
            obj.SetActive(false);

		m_btnPress = BTNPress.BTN_OK;

		if (m_funOk != null) {
			m_funOk ();
			m_funOk = null;
		}
		m_funCancel = null;
    }

	public void click_button_cancel()
	{
		GameObject obj = GameObject.Find("messagebox");
		if (obj != null)
			obj.SetActive(false);

		m_btnPress = BTNPress.BTN_CANCEL;
		if (m_funCancel != null) {
			m_funCancel ();
			m_funCancel = null;
		}
		m_funOk = null;
	}
}
