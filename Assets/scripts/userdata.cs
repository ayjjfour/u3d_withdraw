using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class userdata {

    static public int m_index;

	public struct AccountInfo{
		public string name;
		public string password;
		public string secondpwd;
        public int flag;
    };

    void Awake()
    {
        m_index = 0;
    }

	static public Dictionary<string, AccountInfo> 	m_mapAccount = new Dictionary<string, AccountInfo>();
}
