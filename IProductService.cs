using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace AFGetStarted {
    public interface IProductService : IDisposable {
        Task AddProduct (string name, string description);
    }

    public class ProductService : IProductService {
        private readonly string _connectionString;
        private readonly IDbConnection _dbConnection;
        public ProductService (string connectionString) {
            _connectionString = connectionString;
            _dbConnection = new SqlConnection (_connectionString);
        }
        public async Task AddProduct (string name, string description) {
            _dbConnection.Open ();
            await _dbConnection.ExecuteAsync ($"Insert into ProductDetail values('{name}', '{description}')");
            _dbConnection.Close ();
        }

        public void Dispose () {
            _dbConnection?.Dispose ();
        }
    }
}