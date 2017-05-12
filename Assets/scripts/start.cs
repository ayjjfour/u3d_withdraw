using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class start : MonoBehaviour {

       static Dictionary<int, string> m_errcode = new Dictionary<int,string>{
            {-9999, "未执行"},
            {-2001, "二级密码错误"},
            {-3001, "提取金额不正确"},
            {-1002, "校验码错误"},
            {-1001, "密码错误"},
            {0, "完成"},
            {1001, "已经提取完毕"},
            {1002, "公司正在处理业务，请稍后再试"},
            {3001, "已经提取完毕"}
    };

    dataif m_dataif;
	// Use this for initialization
	void Start () {
        if (m_dataif == null)
            m_dataif = new dataif();

        button_event.m_dataif = m_dataif;

        _init_ui();
	}

    private void _init_ui()
    {
        var gObject = GameObject.Find("scroll_list/Viewport/panel_grid/item");
        if (gObject == null)
        {
            Debug.Log("Button Add: find [scroll_list/Viewport/panel_grid/item] failed!");
            return;
        }

        foreach (KeyValuePair<string, userdata.AccountInfo> kvp in userdata.m_mapAccount)
        {
            // 创建一个新的“item”对象
            var gameObject = GameObject.Instantiate(gObject);
            if (gameObject == null)
            {
                Debug.Log("_init_ui: Create item failed!");
                continue;
            }

            // 设置新对象的参数
            userdata.m_index++;
            gameObject.transform.SetParent(gObject.transform.parent);
            gameObject.name = "item" + userdata.m_index;
            gameObject.transform.localScale = Vector3.one;
            gameObject.transform.localPosition = Vector3.zero;

            _set_item_info(gameObject, kvp.Value);

            gameObject.SetActive(true);
        }
    }

    private void _set_item_info(GameObject objItem, userdata.AccountInfo info)
    {
        _set_toggle_status(objItem, "Toggle", false);
        _set_field_info(objItem, "account", info.name);
        _set_field_info(objItem, "password", info.password);
        _set_field_info(objItem, "secondpwd", info.secondpwd);
        _set_field_info(objItem, "status", _get_status(info.flag));
        _set_field_info(objItem, "code", string.Format("{0}",info.flag));
    }

    private void _set_toggle_status(GameObject objItem, string name, bool status)
    {
        Transform trObj = objItem.transform;
        var obj = trObj.FindChild(name);
        if (obj == null)
        {
            Debug.LogError("_set_toggle_status find obj [" + name + "] failed!");
            return;
        }

        Toggle field = obj.GetComponent<Toggle>();
        if (field == null)
        {
            Debug.LogError("_set_toggle_status Get Component [" + name + "] failed!");
            return;
        }

        field.isOn = status;
    }

    private void _set_field_info(GameObject objItem, string name, string value)
    {
        Transform trObj = objItem.transform;
        var obj = trObj.FindChild(name);
        if (obj == null)
        {
            Debug.LogError("_set_field_info find obj [" + name + "] failed!");
            return;
        }

        Text field = obj.GetComponent<Text>();
        if (field == null)
        {
            Debug.LogError("_set_field_info Get Component [" + name + "] failed!");
            return;
        }

        field.text = value;
    }

    private string _get_status(int flag)
    {
        int localflag = flag;
        string errmsg;
        if (!m_errcode.ContainsKey(localflag))
            localflag = -9999;

        if (localflag < 0)
            errmsg = string.Format("<color=#ff0000>{0}</color>", m_errcode[localflag]);
        else
            errmsg = string.Format("<color=#00ff00>{0}</color>", m_errcode[localflag]);

        return errmsg;
    }
}
