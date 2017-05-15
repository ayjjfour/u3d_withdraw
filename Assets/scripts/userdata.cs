using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class userdata {

	public enum UserEvent
	{
		UEVT_NONE = 0,
        UEVT_LOG,
        UEVT_STATUS,
		UEVT_FINISH,
	}

    public struct AccountInfo
    {
        public string name;
        public string password;
        public string secondpwd;
        public int flag;
        public Transform item;
    };

    static public void _initialize_info(out AccountInfo info)
    {
        info.name = "";
        info.password = "";
        info.secondpwd = "";
        info.flag = -9999;
        info.item = null;
    }

    static public object m_evt_lock = null;
	static Queue<UserEvent> m_evt_queue = null;

    static public object m_log_lock = null;
    static Queue<string> m_log_queue = null;

    static public object m_info_lock = null;
    static Queue<AccountInfo> m_info_queue = null;

    static public int m_index = 0;
	
    static public void init()
	{
		m_index = 0;
        m_evt_lock = new object();
        m_evt_queue = new Queue<UserEvent>();

        m_log_lock = new object();
        m_log_queue = new Queue<string>();

        m_info_lock = new object();
        m_info_queue = new Queue<AccountInfo>();
	}

	static public void set_event(UserEvent evt)
	{
        lock (m_evt_lock)
        {
            m_evt_queue.Enqueue(evt);
		}
	}

    static public UserEvent get_event()
    {
        UserEvent evt = UserEvent.UEVT_NONE;
        lock (m_evt_lock)
        {
            if (m_evt_queue.Count > 0)
                evt = m_evt_queue.Dequeue();
        }

        return evt;
    }

    static public void set_log(string log)
    {
        lock (m_log_lock)
        {
            m_log_queue.Enqueue(log);
        }
    }

    static public string get_log()
    {
        string log = "";
        lock (m_log_lock)
        {
            if (m_log_queue.Count > 0)
                log = m_log_queue.Dequeue();
        }

        return log;
    }

    static public void set_info(AccountInfo info)
    {
        lock (m_info_lock)
        {
            m_info_queue.Enqueue(info);
        }
    }

    static public AccountInfo get_info()
    {
        AccountInfo info;
        _initialize_info(out info);        

        lock (m_info_lock)
        {
            if (m_info_queue.Count > 0)
                info = m_info_queue.Dequeue();
        }

        return info;
    }

	static public Dictionary<string, AccountInfo> 	m_mapAccount = new Dictionary<string, AccountInfo>();
}
