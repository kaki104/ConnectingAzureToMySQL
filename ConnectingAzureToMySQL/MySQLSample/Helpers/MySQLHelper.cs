using System;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySQLSample.Helpers
{
    /// <summary>
    ///     MySQL 헬퍼 클래스
    /// </summary>
    public class MySQLHelper
    {
        private static MySQLHelper _instance;

        private MySqlConnection _connection;

        /// <summary>
        ///     생성자
        /// </summary>
        public MySQLHelper()
        {
            Initialize();
        }

        /// <summary>
        ///     인스턴스
        /// </summary>
        public static MySQLHelper Instance => _instance ?? (_instance = new MySQLHelper());

        /// <summary>
        ///     초기화
        /// </summary>
        private void Initialize()
        {
            var server = "NAS 외부접속 아이피";
            var database = "northwind";
            var uid = "kakiadmin";
            var password = "계정 비밀번호를 입력합니다.";
            var connectionString =
                $"SERVER={server};DATABASE={database};UID={uid};Pwd={password};SslMode=none;";

            _connection = new MySqlConnection(connectionString);
        }

        /// <summary>
        ///     연결
        /// </summary>
        /// <returns></returns>
        private Tuple<bool, string> OpenConnection()
        {
            try
            {
                _connection.Open();
                return new Tuple<bool, string>(true, null);
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Debug.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Debug.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return new Tuple<bool, string>(false, ex.Message);
            }
        }

        /// <summary>
        ///     연결 종료
        /// </summary>
        /// <returns></returns>
        private bool CloseConnection()
        {
            try
            {
                _connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        ///     연결 테스트
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetConnectTestAsync()
        {
            var ds = new DataSet();
            var query = "SELECT * FROM Employees";
            var result = OpenConnection();
            if (result.Item1 == false) return result.Item2;
            var adpt = new MySqlDataAdapter(query, _connection);
            //var cmd = new MySqlCommand(query, _connection);
            await adpt.FillAsync(ds);
            CloseConnection();
            return "success connecting";
        }

        /// <summary>
        ///     결과 반환하지 않는 쿼리 실행, Insert, Update, Delete
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<string> ExecuteNonQueryAsync(string query)
        {
            if (string.IsNullOrEmpty(query)) return "Empty strings are not supported.";
            var result = OpenConnection();
            if (result.Item1 == false) return result.Item2;
            var cmd = new MySqlCommand(query, _connection);
            var resultExecute = await cmd.ExecuteNonQueryAsync();
            CloseConnection();
            return resultExecute.ToString();
        }

        /// <summary>
        ///     결과 반환하는 쿼리 실행 Select
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<DataSet> ExecuteAsync(string query)
        {
            if (string.IsNullOrEmpty(query)) return null;
            var ds = new DataSet();
            var result = OpenConnection();
            if (result.Item1 == false) return null;
            var adpt = new MySqlDataAdapter(query, _connection);
            await adpt.FillAsync(ds);
            CloseConnection();
            return ds;
        }
    }
}