using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class button_event : MonoBehaviour {

	enum ButtonBit{
		BTN_ADD = 1,
		BTN_DEL = 2,
		BTN_SAVE = 4,
		BTN_RESET = 8,
		BTN_START = 16,
		BTN_STOP = 32,
	};

    public static dataif m_dataif = null;
    static GameObject m_errmsgBox = null;
    static GameObject m_editBox = null;
    static Button m_btnAdd = null;
    static Button m_btnDelete = null;
    static Button m_btnSave = null;
    static Button m_btnReset = null;
    static Button m_btnStart = null;
    static Button m_btnStop = null;

	public void click_button_add(){
		var gObject = GameObject.Find("scroll_list/Viewport/panel_grid/item");
		if (gObject == null) {
			Debug.Log ("Button Add: find [scroll_list/Viewport/panel_grid/item] failed!");
			return;
		}

		// 创建一个新的“item”对象
		var gameObject = GameObject.Instantiate (gObject);
		if (gameObject == null) {
			Debug.Log ("Button Add: Create item failed!");
			return;
		}

		// 设置新对象的参数
        userdata.m_index++;
		gameObject.transform.SetParent (gObject.transform.parent);
        gameObject.name = "item" + userdata.m_index;
		gameObject.transform.localScale = Vector3.one;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.SetActive (true);

		_set_buttons_enabled (ButtonBit.BTN_RESET | ButtonBit.BTN_START, false);
	}

	public void click_button_delete(){
		var gObject = GameObject.Find ("panel_list/scroll_list/Viewport/panel_grid");
		if (gObject == null) {
			Debug.Log ("Button Delete: find [scroll_list/Viewport/panel_grid] failed!");
			return;
		}

		foreach(Transform child in gObject.transform){
			if (child.name != "item") {
				bool need_del = _check_selected (child);
				if (need_del)
					Destroy (child.gameObject);
			}
		}
	}

	public void click_button_save(){
		var gObject = GameObject.Find ("panel_list/scroll_list/Viewport/panel_grid");
		if (gObject == null) {
			Debug.Log ("Button Save: find [scroll_list/Viewport/panel_grid] failed!");
			return;
		}

		userdata.m_mapAccount.Clear ();
		foreach(Transform child in gObject.transform){
			if (child.name == "item")
				continue;

			userdata.AccountInfo info;
			bool need_save = _check_empty_and_fetch (child, out info);
			if (!need_save) {
				Debug.Log ("Item [" + child.name + "] has empty fields!");
				userdata.m_mapAccount.Clear ();
                _show_errmsg("用户名，密码和二级密码不能为空");
				return;
			}

			if (userdata.m_mapAccount.ContainsKey (info.name)) {
				Debug.Log ("duplicate items!");
				userdata.m_mapAccount.Clear ();
                _show_errmsg(string.Format("用户名[{0}]，不能有重复", info.name));
				return;
			}

			userdata.m_mapAccount.Add (info.name, info);
		}

        m_dataif.save_user_info();
        _set_buttons_enabled(ButtonBit.BTN_RESET | ButtonBit.BTN_START, true);
    }

	public void click_button_reset(){
		Debug.Log("Button Reset");  
	}

	public void click_button_start(){
		Debug.Log("Button Start");  
	}

	public void click_button_stop(){
		Debug.Log("Button Stop");  
	}

    public void click_button_edit()
    {
        if (m_editBox == null)
        {
            GameObject box = Resources.Load<GameObject>("prefab/editbox");
            if (box != null)
            {
                m_editBox = Instantiate(box);
                Debug.Log("click_button_edit Create editbox successfully!");
            }
            else
                Debug.LogError("click_button_edit Create editbox failed!");
        }

        if (m_editBox != null)
        {
            GameObject Parent = GameObject.Find("UIMain");
            m_editBox.name = "editbox";
            //_set_field_text(m_editBox.transform, "background/errmsg", errmsg);
            editbox.set_item(this.transform.parent);
            m_editBox.layer = LayerMask.NameToLayer("Default");
            m_editBox.transform.SetParent(Parent.transform);
            m_editBox.transform.localScale = Vector3.one;
            m_editBox.transform.localPosition = Vector3.zero;
            m_editBox.SetActive(true);
        }

        Debug.Log("obj = " + this.transform.parent);
    }

    // 校验Item是否被选中
    private bool _check_selected(Transform trObj)
	{
		var toggle = trObj.GetComponentInChildren<Toggle>();
		//Debug.Log ("toggle = " + toggle);
		if (toggle && toggle.isOn)
			return true;
		else
			return false;
	}

	// 校验Item 的account，password和secondpwd字段是否为空
	private bool _check_empty_and_fetch(Transform trObj, out userdata.AccountInfo info)
	{
		info.name = "";
		info.password = "";
		info.secondpwd = "";
        info.flag = -9999;

		info.name = _get_field_text (trObj, "account");
		if (info.name == "")
			return false;
		
		info.password = _get_field_text (trObj, "password");
		if (info.password == "")
			return false;
		
		info.secondpwd = _get_field_text (trObj, "secondpwd");
		if (info.secondpwd == "")
			return false;

        string flag = _get_field_text(trObj, "code");
        if (flag != "")
            info.flag = int.Parse(flag);

		return true;
	}

	static public string _get_field_text(Transform trObj, string name)
	{
		Transform obj = trObj.FindChild (name);
		if (obj == null) {
			Debug.LogError ("_get_field_text find obj [" + name + "] failed!");
			return "";
		}

		Text field = obj.GetComponent<Text> ();
		if (field == null) {
			Debug.LogError ("_get_field_text Get Component [" + name + "] failed!");
			return "";
		}

		return field.text.Trim ();
	}

    private void _set_field_text(Transform trObj, string name, string value)
    {
        var obj = trObj.FindChild(name);
        if (obj == null)
        {
            Debug.LogError("_get_field_text find obj [" + name + "] failed!");
            return;
        }

        Text field = obj.GetComponent<Text>();
        if (field == null)
        {
            Debug.LogError("_get_field_text Get Component [" + name + "] failed!");
            return;
        }

        field.text = value;
    }

	private void _set_buttons_enabled(ButtonBit btnbit, bool enabled)
	{
		if ((btnbit & ButtonBit.BTN_ADD) != 0) {
			_set_button_enabled (ref m_btnAdd, "panel_top/button_add", enabled);
		}

		if ((btnbit & ButtonBit.BTN_DEL) != 0) {
			_set_button_enabled (ref m_btnDelete, "panel_top/button_delete", enabled);
		}

		if ((btnbit & ButtonBit.BTN_SAVE) != 0) {
			_set_button_enabled (ref m_btnSave, "panel_top/button_save", enabled);
		}

		if ((btnbit & ButtonBit.BTN_RESET) != 0) {
			_set_button_enabled (ref m_btnReset, "panel_top/button_reset", enabled);
		}

		if ((btnbit & ButtonBit.BTN_START) != 0) {
			_set_button_enabled (ref m_btnStart, "panel_top/button_start", enabled);
		}

		if ((btnbit & ButtonBit.BTN_STOP) != 0) {
			_set_button_enabled (ref m_btnStop, "panel_top/button_stop", enabled);
		}
	}

	private void _set_button_enabled(ref Button objbtn, string name, bool enabled)
	{
		if (objbtn == null) {
			var btn_object = GameObject.Find (name);
			if (btn_object == null)
				return;

			objbtn = btn_object.GetComponent<Button>();
		}
		if (objbtn != null)
			objbtn.interactable = enabled;
	}

    private void _show_errmsg(string errmsg)
    {
        if (m_errmsgBox == null)
        {
            GameObject msgbox = Resources.Load<GameObject>("prefab/messagebox");
            if (msgbox != null)
            {
                m_errmsgBox = Instantiate(msgbox);
                Debug.Log("_show_errmsg Create messagebox successfully!");
            }
            else
                Debug.LogError("_show_errmsg Create messagebox failed!");
        }

        if (m_errmsgBox != null)
        {
            GameObject Parent = GameObject.Find("UIMain");
            m_errmsgBox.name = "messagebox";
            _set_field_text(m_errmsgBox.transform, "background/errmsg", errmsg);
            m_errmsgBox.layer = LayerMask.NameToLayer("Default");
            m_errmsgBox.transform.SetParent(Parent.transform);
            m_errmsgBox.transform.localScale = Vector3.one;
            m_errmsgBox.transform.localPosition = Vector3.zero;
            m_errmsgBox.SetActive(true);
        }
    }
}