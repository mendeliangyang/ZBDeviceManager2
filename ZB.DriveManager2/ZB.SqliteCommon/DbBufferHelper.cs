using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ZB.SqliteCommon
{
    public class DbBufferHelper
    {
        private SQLiteConnection dbConnection;
        private SQLiteCommand dbCommand;
        private SQLiteDataReader dataReader;

        private string connString;

        public string ConnString
        {
            get { return connString; }
            set { connString = value; }
        }

        /// <summary>
        /// 对象构造器，连接默认的数据库
        /// </summary>
        public DbBufferHelper() : this("realdatamode.db")
        {
        }

        /// <summary>
        /// 连接参数指定的数据库
        /// </summary>
        /// <param name="connectionString">需要连接的数据库</param>
        public DbBufferHelper(string connectionString)
        {
            try
            {
                this.connString = connectionString;
                dbConnection = new SQLiteConnection(connectionString);
                dbConnection.Open();
            }
            catch (Exception ex)
            {
                //LogPrint.WriteLog("Alarm", string.Format("连接数据库\"{0}\"出错, ", connString) + ex.ToString(), 1);
            }
        }

        //public void InsertData(List<object> datas)
        //{
        //    using (SQLiteConnection conn = new SQLiteConnection(connString))
        //    {
        //        using (var command = conn.CreateCommand())
        //        {
        //            command.CommandText = "SELECT (Identification, Param) FROM AlarmBuffer";
        //            var reader = command.ExecuteReader();
        //            var identification = reader.GetFieldValue<int>(0);
        //            var param = reader.GetFieldValue<int>(1);
        //            command.CommandText = "INSERT INTO AlarmBuffer(Identification, Param, ParamValue) Values({0},{1},{2})";
        //            int count = command.ExecuteNonQuery();
        //        }
        //    }
        //}

        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <returns>The query.</returns>
        /// <param name="queryString">SQL命令字符串</param>
        public SQLiteDataReader ExecuteQuery(string queryString)
        {
            try
            {
                dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = queryString;
                dataReader = dbCommand.ExecuteReader();
            }
            catch (Exception ex)
            {
               // LogPrint.WriteLog("Alarm", string.Format("数据库操作\"{0}\"出错, ", connString) + ex.ToString(), 1);
            }

            return dataReader;
        }

        /// <summary>
        /// 读取整张数据表
        /// </summary>
        /// <returns>The full table.</returns>
        /// <param name="tableName">数据表名称</param>
        public SQLiteDataReader ReadFullTable(string tableName)
        {
            string queryString = "SELECT * FROM " + tableName;
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 读取整张数据表
        /// </summary>
        /// <returns>The full table.</returns>
        public SQLiteDataReader ReadFullTable()
        {
            string queryString = "SELECT * FROM AlarmBuffer";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 向指定数据表中插入数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="values">插入的数值</param>
        public SQLiteDataReader InsertValues(List<object> values, int port)
        {
            string queryString = string.Format("INSERT INTO AlarmBuffer(Identification, Param, ParamValue, Port) Values({0},{1},{2},{3})", values[0], values[1], values[2], port);
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 更新指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        /// <param name="key">关键字</param>
        /// <param name="value">关键字对应的值</param>
        /// <param name="operation">运算符：=,<,>,...，默认“=”</param>
        public SQLiteDataReader UpdateValues(string tableName, string[] colNames, string[] colValues, string key, string value, string operation = "=")
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length");
            }

            string queryString = "UPDATE " + tableName + " SET " + colNames[0] + "=" + "'" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += ", " + colNames[i] + "=" + "'" + colValues[i] + "'";
            }
            queryString += " WHERE " + key + operation + "'" + value + "'";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 更新指定数据表内的数据
        /// </summary>
        /// <param name="values">表内的数据对应的值</param>
        /// <returns></returns>
        public SQLiteDataReader UpdateValues(List<object> values)
        {
            string updateString = string.Format("UPDATE AlarmBuffer SET ParamValue={0} where Identification={1} AND Param={2}", values[2], values[0], values[1]);
            return ExecuteQuery(updateString);
        }

        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        public SQLiteDataReader DeleteValuesOR(string tableName, string[] colNames, string[] colValues, string[] operations)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + "'" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += "OR " + colNames[i] + operations[0] + "'" + colValues[i] + "'";
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 删除指定数据表内的数据
        /// </summary>
        /// <returns>The values.</returns>
        /// <param name="tableName">数据表名称</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colValues">字段名对应的数据</param>
        public SQLiteDataReader DeleteValuesAND(string tableName, string[] colNames, string[] colValues, string[] operations)
        {
            //当字段名称和字段数值不对应时引发异常
            if (colNames.Length != colValues.Length || operations.Length != colNames.Length || operations.Length != colValues.Length)
            {
                throw new SQLiteException("colNames.Length!=colValues.Length || operations.Length!=colNames.Length || operations.Length!=colValues.Length");
            }

            string queryString = "DELETE FROM " + tableName + " WHERE " + colNames[0] + operations[0] + "'" + colValues[0] + "'";
            for (int i = 1; i < colValues.Length; i++)
            {
                queryString += " AND " + colNames[i] + operations[i] + "'" + colValues[i] + "'";
            }
            return ExecuteQuery(queryString);
        }


        /// <summary>
        /// 创建数据表
        /// </summary> +
        /// <returns>The table.</returns>
        /// <param name="tableName">数据表名</param>
        /// <param name="colNames">字段名</param>
        /// <param name="colTypes">字段名类型</param>
        public SQLiteDataReader CreateTable(string tableName, string[] colNames, string[] colTypes)
        {
            string queryString = "CREATE TABLE IF NOT EXISTS " + tableName + "( " + colNames[0] + " " + colTypes[0];
            for (int i = 1; i < colNames.Length; i++)
            {
                queryString += ", " + colNames[i] + " " + colTypes[i];
            }
            queryString += "  ) ";
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <returns>The table.</returns>
        public SQLiteDataReader CreateTable()
        {
            string createString = @"CREATE TABLE IF NOT EXISTS AlarmBuffer(
                                            ID INTEGER PRIMARY KEY NOT NULL,
                                            Identification INTEGER NOT NULL,
                                            Param INTEGER NOT NULL,
                                            ParamValue INTEGER,
                                            Port INTEGER NOT NULL
                                    )";
            return ExecuteQuery(createString);
        }

        /// <summary>
        /// Reads the table.
        /// </summary>
        /// <returns>The table.</returns>
        /// <param name="tableName">Table name.</param>
        /// <param name="items">Items.</param>
        /// <param name="colNames">Col names.</param>
        /// <param name="operations">Operations.</param>
        /// <param name="colValues">Col values.</param>
        public SQLiteDataReader ReadTable(string tableName, string[] items, string[] colNames, string[] operations, string[] colValues)
        {
            string queryString = "SELECT " + items[0];
            for (int i = 1; i < items.Length; i++)
            {
                queryString += ", " + items[i];
            }
            queryString += " FROM " + tableName + " WHERE " + colNames[0] + " " + operations[0] + " " + colValues[0];
            for (int i = 0; i < colNames.Length; i++)
            {
                queryString += " AND " + colNames[i] + " " + operations[i] + " " + colValues[0] + " ";
            }
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 根据端口选择指定业态的数据
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public SQLiteDataReader SelectWithBuilding(int port)
        {
            string selectString = string.Format("SELECT * FROM AlarmBuffer WHERE Port={0}", port);
            return ExecuteQuery(selectString);
        }

        public SQLiteDataReader SelectWithIdentyParam(List<object> values)
        {
            string queryString = string.Format("SELECT * FROM AlarmBuffer WHERE Identification={0} AND Param={1}", values[0], values[1]);
            return ExecuteQuery(queryString);
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void CloseConnection()
        {
            //销毁Reader
            if (dataReader != null)
            {
                dataReader.Close();
            }
            dataReader = null;

            //销毁Commend
            if (dbCommand != null)
            {
                dbCommand.Cancel();
            }
            dbCommand = null;

            //销毁Connection
            if (dbConnection != null)
            {
                dbConnection.Close();
            }
            dbConnection = null;
        }
    }
}
