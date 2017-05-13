using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System.Collections.Generic;

public class dataif {
	dbaccess m_db = null;

    ~dataif()
    {
        if (m_db != null)
            m_db.CloseSqlConnection();
    }

	// Use this for initialization
    public dataif()
    {
		bool ret = true;
		do {
            ret = _create_db();
            if (ret == false)
                break;

			ret = _create_table ();
			if (ret == false)
				break;

			ret = _load_user_info();
			if (ret == false)
				break;
		} while(false);
	
		if (!ret)
			Application.Quit ();
	}

    public void save_user_info()
    {
        m_db.ExecuteQuery("delete from user_info");

        foreach (KeyValuePair<string, userdata.AccountInfo> kvp in userdata.m_mapAccount)
        {
            //Console.WriteLine("Key={0},Value={1}", kvp.Key, kvp.Value);
            //m_db.InsertInto("user");
            string strsql = string.Format("insert into user_info (nickname, password, secondpwd, fetch_flag) values('{0}', '{1}', '{2}', '{3}')",
                                             kvp.Value.name, kvp.Value.password, kvp.Value.secondpwd, kvp.Value.flag);
            Debug.Log(strsql);
            m_db.ExecuteQuery(strsql);
        }
    }

    private bool _create_db()
    {
        string dbpath = Application.persistentDataPath + "/withdraw.db";

        Debug.Log("dbpath = " + dbpath);

        //创建数据库名称为xuanyusong.db
        m_db = new dbaccess(@"data source=" + dbpath);
        if (m_db == null)
            return false;

        return true;
    }

	private bool _create_table()
	{
        // 创建表
		string strsql = @"CREATE TABLE IF NOT EXISTS `user_info` (`nickname` VARCHAR(64) NOT NULL,`password` VARCHAR(45) NOT NULL,`secondpwd` VARCHAR(45) NOT NULL,
							`fetch_date` DATE NULL,`fetch_flag` INT NOT NULL DEFAULT 0,PRIMARY KEY (`nickname`) )";
		SqliteDataReader sqReader = m_db.ExecuteQuery (strsql);
		if (sqReader == null) {
			m_db.CloseSqlConnection ();
			return false;
		}

		Debug.Log ("_create_table " + sqReader);

		return true;
	}

	private bool _load_user_info()
	{
		string strsql = @"select nickname, password, secondpwd, fetch_flag from user_info";

		SqliteDataReader sqReader = m_db.ExecuteQuery (strsql);
		if (sqReader == null) {
			m_db.CloseSqlConnection ();
			return false;
		}

		userdata.m_mapAccount.Clear ();
        while (sqReader.Read())
        {
            userdata.AccountInfo info;
            info.name = sqReader.GetString(sqReader.GetOrdinal("nickname"));
            info.password = sqReader.GetString(sqReader.GetOrdinal("password"));
            info.secondpwd = sqReader.GetString(sqReader.GetOrdinal("secondpwd"));
            info.flag = sqReader.GetInt32(sqReader.GetOrdinal("fetch_flag"));
			info.item = null;

            userdata.m_mapAccount.Add(info.name, info);
        } 
        
		return true;
	}
}