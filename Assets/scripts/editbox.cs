using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class editbox : MonoBehaviour {

    static Transform m_objItem;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    static public void set_item(Transform TrObj)
    {
        m_objItem = TrObj;

        GameObject obj = GameObject.Find("editbox");
        if (obj == null)
            return;

        _fill_input_data(m_objItem, obj.transform);
    }

    public void click_button_ok()
    {
        if (m_objItem == null)
            return;

        GameObject obj = GameObject.Find("editbox");
        if (obj == null)
            return;

        _fill_item_data(obj.transform, m_objItem);
        if (obj != null)
            obj.SetActive(false);
    }

    public void click_button_cancel()
    {
        GameObject obj = GameObject.Find("editbox");
        if (obj != null)
            obj.SetActive(false);
    }

    static private void _fill_input_data(Transform objItem, Transform objEdit)
    {
        _set_input_data(objItem, "account", objEdit, "background/account/InputField");
        _set_input_data(objItem, "password", objEdit, "background/password/InputField");
        _set_input_data(objItem, "secondpwd", objEdit, "background/secondpwd/InputField");
    }

    static private void _set_input_data(Transform objItem, string fieldname, Transform objEdit, string inputname )
    {
        string value = button_event._get_field_text(objItem, fieldname);

        Transform obj = objEdit.FindChild(inputname);
        if (obj == null)
            return;

        InputField field = obj.GetComponent<InputField>();
        if (field == null)
            return;

        field.text = value;
    }

    static private void _fill_item_data(Transform objEdit, Transform objItem)
    {
        _set_item_data(objEdit, "background/account/InputField", objItem, "account");
        _set_item_data(objEdit, "background/password/InputField", objItem, "password");
        _set_item_data(objEdit, "background/secondpwd/InputField", objItem, "secondpwd");

		Transform obj = objItem.FindChild("Toggle");
		Toggle toggle = obj.GetComponent<Toggle>();
		toggle.isOn = false;
    }

    static private void _set_item_data(Transform objEdit, string inputname, Transform objItem, string fieldname)
    {
        Transform obj = objEdit.FindChild(inputname);
        if (obj == null)
            return;

        InputField field = obj.GetComponent<InputField>();
        if (field == null)
            return;

        obj = objItem.FindChild(fieldname);
        if (obj == null)
            return;

        Text text = obj.GetComponent<Text>();
        if (text == null)
            return;

        text.text = field.text.Trim();
    }
}
