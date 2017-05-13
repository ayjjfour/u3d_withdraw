using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Net;  
using System.IO;  
using System.IO.Compression;
using System.Text;
using System.Xml;
using HtmlAgilityPack;

public class working_thread {

	public enum ThreadEvent{
		TEVT_NONE = 0,
		TEVT_RUN = 1,
		TEVT_STOP = 2,
		TEVT_EXIT = 3,
	};

	static public Queue<ThreadEvent> m_evt_queue;
	static public int m_monitor_flag = 0;
	static public int m_work_flag = 0;
	static public Object m_evt_lock;

	static public Thread m_monitor_thread = null;
	static public Thread m_work_thread = null;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log ("working_thread update");
	}
		
	static public void send_event(ThreadEvent evt)
	{
		lock (m_evt_lock) {
			m_evt_queue.Enqueue (evt);
		}
	}

	static public void start()
	{
		m_evt_queue = new Queue<ThreadEvent> ();
		m_monitor_flag = 1;
		m_evt_lock = new Object ();
		m_monitor_thread = new Thread (_monitor_thread);
		m_monitor_thread.Start ();
		m_monitor_thread.IsBackground = true;

		// test
		/*
		if (m_evt_queue.Count <= 0)
			Debug.Log ("evt = null");
		else {
			ThreadEvent evt = m_evt_queue.Dequeue();
			Debug.Log ("evt = " + evt);
		}
		*/
	}

	static public void stop()
	{
		if (m_monitor_thread != null) {
			m_monitor_thread.Abort ();
			m_monitor_thread = null;
		}

		if (m_work_thread != null) {
			m_work_thread.Abort ();
			m_work_thread = null;
		}
	}

	static private void _start_work_thread()
	{
		if (m_work_thread != null && m_work_flag != 0)
			return;
		
		m_work_flag = 1;
		m_work_thread = new Thread (_work_thread);
		m_work_thread.Start ();
		m_work_thread.IsBackground = true;
	}

	static private void _monitor_thread()
	{
		ThreadEvent	evt;
		while (m_monitor_flag != 0) {
			evt = _get_event ();
			if (evt == ThreadEvent.TEVT_NONE) {
				//Debug.Log ("_monitor_thread TEVT_NONE");
				_check_work_thread();
				Thread.Sleep (100);
				continue;
			}

			switch (evt) {
			case ThreadEvent.TEVT_RUN:
				_start_work_thread ();
				break;
			case ThreadEvent.TEVT_STOP:
				m_work_flag = 0;
				break;
			case ThreadEvent.TEVT_EXIT:
				m_monitor_flag = 0;
				m_work_flag = 0;
				break;
			}
		}

		m_monitor_flag = 0;
		m_monitor_thread = null;
	}

	static private ThreadEvent _get_event()
	{
		ThreadEvent	evt = ThreadEvent.TEVT_NONE;
		lock (m_evt_lock) {
			if (m_evt_queue.Count > 0)
				evt = m_evt_queue.Dequeue ();
		}

		return evt;
	}

	static private void _check_work_thread()
	{
		if (m_work_thread == null) {
			return;
		}

		if (m_work_thread.IsAlive == false) {
			Debug.Log ("m_work_thread has been exit!");
			userdata.set_event (userdata.UserEvent.UEVT_FINISH);
			m_work_thread = null;
			m_work_flag = 0;
		}
	}

	static private void _work_thread()
	{
		foreach (KeyValuePair<string, userdata.AccountInfo> kvp in userdata.m_mapAccount)
		{
			if (m_work_flag == 0)
				break;
			
			_fetch_user_money(kvp.Value);
		}

		userdata.set_event (userdata.UserEvent.UEVT_FINISH);

		m_work_flag = 0;
		m_work_thread = null;
	}

	static private void _fetch_user_money(userdata.AccountInfo info)
	{
		Debug.Log ("_fetch_user_money account = " + info.name);

		string url = "http://www.sjhy2016.com";
		CookieContainer m_Cookie = new CookieContainer();
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
		request.Method = "GET";
		request.ContentType = "text/html;charset=UTF-8";

		HttpWebResponse response = (HttpWebResponse)request.GetResponse();
		Stream myResponseStream = response.GetResponseStream();
		StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
		string retString = myStreamReader.ReadToEnd();
		myStreamReader.Close();
		myResponseStream.Close();

		Debug.Log("ret text = " + retString);

		// retString = "<transforms>\n  <rotation id=\"1000\" name=\"momo\">\n    <x>0</x>\n    <y>1</y>\n    <z id=\"1\">update00000</z>\n  </rotation>\n  <rotation id=\"1\" name=\"yusong\">\n    <x id=\"1\">0</x>\n    <y>1</y>\n    <z>2</z>\n  </rotation>\n</transforms>";

		HtmlDocument doc = new HtmlDocument();
		Debug.Log ("new xml ok");
		doc.LoadHtml (retString);

		Debug.Log ("load xml ok");


	}
}
