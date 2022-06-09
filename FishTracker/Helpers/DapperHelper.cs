using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace FishTracker.Helpers
{
    internal class DapperHelper
    {
        /// <summary>
        /// 执行指定代码.
        /// </summary>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <returns>int.</returns>
        public static int ExecuteSql(string ConnectionString, string sql)
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.Execute(sql);
        }

        /// <summary>
        /// 添加.
        /// </summary>
        /// <typeparam name="T">实体类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="t">传入实体类型.</param>
        /// <returns>int.</returns>
        public static int Add<T>(string ConnectionString, string sql, T t)
            where T : class
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.Execute(sql, t);
        }

        /// <summary>
        /// 批量添加.
        /// </summary>
        /// <typeparam name="T">实体类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="t">传入泛型类.</param>
        /// <returns>int.</returns>
        public static int Add<T>(string ConnectionString, string sql, List<T> t)
            where T : class
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.Execute(sql, t);
        }

        /// <summary>
        /// 删除.
        /// </summary>
        /// <typeparam name="T">实体类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="t">传入实体类型.</param>
        /// <returns>int.</returns>
        public static int Delete<T>(string ConnectionString, string sql, T t)
              where T : class
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.Execute(sql, t);
        }

        /// <summary>
        /// 批量删除.
        /// </summary>
        /// <typeparam name="T">实体类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="t">传入泛型类.</param>
        /// <returns>int.</returns>
        public static int Delete<T>(string ConnectionString, string sql, List<T> t)
              where T : class
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.Execute(sql, t);
        }

        /// <summary>
        /// 修改.
        /// </summary>
        /// <typeparam name="T">实体类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="t">传入实体类型.</param>
        /// <returns>int.</returns>
        public static int Update<T>(string ConnectionString, string sql, T t)
              where T : class
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.Execute(sql, t);
        }

        /// <summary>
        /// 批量修改.
        /// </summary>
        /// <typeparam name="T">实体类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="t">传入泛型类.</param>
        /// <returns>int.</returns>
        public static int Update<T>(string ConnectionString, string sql, List<T> t)
              where T : class
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.Execute(sql, t);
        }

        /// <summary>
        /// 查询列表.
        /// </summary>
        /// <typeparam name="T">实体类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="param">查询参数.</param>
        /// <returns>泛型类.</returns>
        public static List<T> QueryList<T>(string ConnectionString, string sql, object? param = null)
             where T : class
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.Query<T>(sql, param).ToList();
        }

        /// <summary>
        /// 查询指定数据.
        /// </summary>
        /// <typeparam name="T">实体类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="param">查询参数.</param>
        /// <returns>类.</returns>
        public static T QueryFirst<T>(string ConnectionString, string sql, object param)
             where T : class
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.QueryFirst<T>(sql, param);
        }

        /// <summary>
        /// 查询指定数据,不存在则返回NULL.
        /// </summary>
        /// <typeparam name="T">返回值类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="param">查询参数.</param>
        /// <returns>类.</returns>
        public static T ExecuteScalar<T>(string ConnectionString, string sql, object? param = null)
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.ExecuteScalar<T>(sql, param);
        }

        /// <summary>
        /// 查询指定数据,不存在则返回默认值.
        /// </summary>
        /// <typeparam name="T">实体类型.</typeparam>
        /// <param name="ConnectionString">连接字符串。</param>
        /// <param name="sql">传入sql执行语句.</param>
        /// <param name="param">查询参数.</param>
        /// <returns>类.</returns>
        public static T QueryFirstOrDefault<T>(string ConnectionString, string sql, object param)
             where T : class
        {
            using IDbConnection connection = new MySqlConnection(ConnectionString);
            return connection.QuerySingleOrDefault<T>(sql, param);
        }

        ///// <summary>
        ///// 查询的in操作.
        ///// </summary>
        ///// <typeparam name="T">实体类型.</typeparam>
        ///// <param name="sql">传入sql执行语句.</param>
        ///// <returns>泛型类.</returns>
        //public static List<T> Query<T>(string ConnectionString, string sql, int[] ids)
        //    where T : class
        //{
        //    using (IDbConnection connection = new SqlConnection(ConnectionString))
        //    {
        //        return connection.Query<T>(sql, new { ids }).ToList();
        //    }
        //}

        ///// <summary>
        ///// 多语句操作.
        ///// </summary>
        ///// <typeparam name="T">实体类型.</typeparam>
        ///// <param name="sql">传入sql执行语句.</param>
        //public static void QueryMultiple(string ConnectionString, string sql)
        //{
        //    using (IDbConnection connection = new SqlConnection(ConnectionString))
        //    {
        //        var multiReader = connection.QueryMultiple(sql);
        //        var userInfo = multiReader.Read<UserInfo>();
        //        var student = multiReader.Read<Student>();

        //        multiReader.Dispose();
        //    }
        //}
    }
}
