using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select DepartmentId, DepartmentName from dbo.Department";
            
            DataTable table = new ();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader dataReader;
            using (SqlConnection _connection = new SqlConnection(sqlDataSource))
            {
                _connection.Open();
                using (SqlCommand _command = new SqlCommand(query, _connection))
                {
                    dataReader = _command.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    _connection.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Department department)
        {
            string query = @"insert into dbo.Department values(@DepartmentName)";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader dataReader;
            using (SqlConnection _connection = new SqlConnection(sqlDataSource))
            {
                _connection.Open();
                using (SqlCommand _command = new SqlCommand(query, _connection))
                {
                    _command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    dataReader = _command.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    _connection.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(Department department)
        {
            string query = @"update dbo.Department
                            set DepartmentName=@DepartmentName
                            where DepartmentId=@DepartmentId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader dataReader;
            using (SqlConnection _connection = new SqlConnection(sqlDataSource))
            {
                _connection.Open();
                using (SqlCommand _command = new SqlCommand(query, _connection))
                {
                    _command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                    _command.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
                    dataReader = _command.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    _connection.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"delete from dbo.Department
                            where DepartmentId=@DepartmentId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader dataReader;
            using (SqlConnection _connection = new SqlConnection(sqlDataSource))
            {
                _connection.Open();
                using (SqlCommand _command = new SqlCommand(query, _connection))
                {
                    _command.Parameters.AddWithValue("@DepartmentId", id);
                    dataReader = _command.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    _connection.Close();
                }
            }

            return new JsonResult(table);
        }
    }
}
