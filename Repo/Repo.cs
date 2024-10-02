using System.Data;
using System.Text;
using System.Threading.Tasks;
using Project.ConnectionLogic;
using Project.Models;
using MySqlConnector;


namespace Project
{
    public class Repo
    {
        private readonly MySQLConnectionLogic _logic;
        private readonly MySQLConnectionLogic _connectionLogic;

        // Constructor to inject MySQLConnectionLogic
        public Repo(MySQLConnectionLogic logic, MySQLConnectionLogic connectionLogic)
        {
            _logic = logic;
            _connectionLogic = connectionLogic;
        }

        public async Task<bool> TestCon(TestData temp)
        {
            string sqlQuery = "INSERT INTO Signup (Id, Firstname, Lastname, DOB, Age, PhoneNumber, City, state, ZipCode, gender, Email, password) " +
                              "VALUES (@Id, @Firstname, @Lastname, @DOB, @Age, @PhoneNumber, @City, @State, @ZipCode, @Gender, @Email, @Password)";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Id", temp.Id),
                new MySqlParameter("@Firstname", temp.Firstname),
                new MySqlParameter("@Lastname", temp.Lastname),
                new MySqlParameter("@DOB", temp.DOB),
                new MySqlParameter("@Age", temp.Age),
                new MySqlParameter("@PhoneNumber", temp.PhoneNumber),
                new MySqlParameter("@City", temp.City),
                new MySqlParameter("@State", temp.state),
                new MySqlParameter("@ZipCode", temp.ZipCode),
                new MySqlParameter("@Gender", temp.gender),
                new MySqlParameter("@Email", temp.Email),
                new MySqlParameter("@Password", temp.password)
            };

            bool result = await _logic.DMLSingleLineQuery(sqlQuery, parameters);
            return result;
        }


        public async Task<string> fetchorderid()
        {
            var orderid = string.Empty;
            StringBuilder sb = new StringBuilder();
            DataTable dt = null;
            sb.Append("select * from ecommerce.Orders ORDER BY OrderID DESC LIMIT 1;");

            orderid = await _logic.DQLSingleLineQueryForString(sb.ToString());

            return orderid;
        }

        public async Task<bool> Login(string email, string password)
        {
            string sqlQuery = "SELECT COUNT(1) FROM Signup WHERE Email = @Email AND password = @Password";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Email", email),
                new MySqlParameter("@Password", password)
            };

            DataTable result = await _logic.DQLSingleLineQuery(sqlQuery, parameters);

            if (result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0][0]) > 0;
            }
            return false;
        }
        public async Task<int> SaveOrder(Order order)
        {
            string sqlQuery = "INSERT INTO Orders (CustomerName, PhoneNumber, ShippingAddress, TotalPrice) VALUES (@CustomerName, @PhoneNumber, @ShippingAddress, @TotalPrice); SELECT LAST_INSERT_ID();";
            MySqlParameter[] parameters = {
                new MySqlParameter("@CustomerName", order.CustomerName),
                new MySqlParameter("@PhoneNumber", order.PhoneNumber),
                new MySqlParameter("@ShippingAddress", order.ShippingAddress),
                new MySqlParameter("@TotalPrice", order.TotalPrice)
            };

            DataTable result = await _logic.DQLSingleLineQuery(sqlQuery, parameters);
            if (result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0][0]);
            }
            return 0;
        }
    }
}