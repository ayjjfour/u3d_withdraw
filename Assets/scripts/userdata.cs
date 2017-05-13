using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class userdata {

	public enum UserEvent
	{
		UEVT_NONE = 0,
		UEVT_FINISH = 1,
	}

	static Queue<UserEvent> m_queue = null;
    static public int m_index = 0;
	static public Object m_queue_lock = null;

	public struct AccountInfo{
		public string name;
		public string password;
		public string secondpwd;
        public int flag;
		public Transform item;
    };

	static public void init()
	{
		m_index = 0;
		m_queue_lock = new Object ();
		m_queue = new Queue<UserEvent>();
	}

	static public void set_event(UserEvent evt)
	{
		lock (m_queue_lock) {
			m_queue.Enqueue (evt);
		}
	}

	static public UserEvent get_event()
	{
		UserEvent evt = UserEvent.UEVT_NONE;
		lock (m_queue_lock) {
			if (m_queue.Count > 0)
				evt = m_queue.Dequeue ();
		}

		return evt;
	}

	static public Dictionary<string, AccountInfo> 	m_mapAccount = new Dictionary<string, AccountInfo>();
}
