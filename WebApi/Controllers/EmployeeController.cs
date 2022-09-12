using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"select EmployeeId, EmployeeName, Department, convert(varchar(10), DateOfJoining, 120) as DateOfJoining, PhotoFileName
                            from dbo.Employee";

            DataTable table = new();
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
        public JsonResult Post(Employee employee)
        {
            string query = @"insert into dbo.Employee values(@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader dataReader;
            using (SqlConnection _connection = new SqlConnection(sqlDataSource))
            {
                _connection.Open();
                using (SqlCommand _command = new SqlCommand(query, _connection))
                {
                    _command.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    _command.Parameters.AddWithValue("@Department", employee.Department);
                    _command.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    _command.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    dataReader = _command.ExecuteReader();
                    table.Load(dataReader);
                    dataReader.Close();
                    _connection.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string query = @"update dbo.Employee
                            set EmployeeName=@EmployeeName,
                            Department=@Department,
                            DateOfJoining=@DateOfJoining,
                            PhotoFileName=@PhotoFileName
                            where EmployeeId=@EmployeeId";

            DataTable table = new();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader dataReader;
            using (SqlConnection _connection = new SqlConnection(sqlDataSource))
            {
                _connection.Open();
                using (SqlCommand _command = new SqlCommand(query, _connection))
                {
                    _command.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    _command.Parameters.AddWithValue("@Department", employee.Department);
                    _command.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    _command.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    _command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
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
            string query = @"delete from dbo.Employee
                            where EmployeeId=@EmployeeId";

            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");

            SqlDataReader dataReader;
            using (SqlConnection _connection = new SqlConnection(sqlDataSource))
            {
                _connection.Open();
                using (SqlCommand _command = new SqlCommand(query, _connection))
                {
                    _command.Parameters.AddWithValue("@EmployeeId", id);
                    dataReader = _command.ExecuteReader();
                    dataReader.Close();
                    _connection.Close();
                }
            }

            var res = new
            {
                message= "Delete successfully"
            };

            return new JsonResult(res);
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SavePhoto()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(new { file_name = filename });
            }
            catch (Exception)
            {
                return new JsonResult(new { file_name="anonymous.png"});
            }
        }

    }
}
