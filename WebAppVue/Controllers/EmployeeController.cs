using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using System.Data.SqlClient;

using System.Collections.ObjectModel;
using System.Data.Common;
using WebAppVUE.Models;
using System.Reflection.PortableExecutable;

using Newtonsoft.Json;
using System.Security.Principal;

//using System.Web.Http.Cors;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography.Xml;
using Microsoft.VisualBasic;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;


namespace WebAppVUE.Controllers
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
        //[HttpGet("{id}")]
        public JsonResult Get() 
        //public List<Employee> Get()
        {
            List<Employee> emplist = new List<Employee>();

            // sql 
            string query = @"
                            select EmployeeId, EmployeeName, Department, photoFileName, 
                            convert(varchar(10),DateOfJoining,120) as DateOfJoining   
                            from dbo.Employee 
                            ";

            try
            {
                DataTable dt = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConn");
                SqlDataReader myReader;
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        //myCommand.Parameters.AddWithValue("@EmployeeId", id);
                        myReader = myCommand.ExecuteReader();

                        dt.Load(myReader);
                        myReader.Close();
                        myConn.Close();

                        foreach (DataRow row in dt.Rows)
                        {
                            Employee emp = new Employee();
                            //Console.WriteLine(row["EmployeeId"].ToString());
                            int empid = 0;
                            if (int.TryParse(row["EmployeeId"].ToString(), out empid))
                            {
                                emp.EmployeeId = empid;
                            }

                            emp.EmployeeName = row["EmployeeName"].ToString();
                            emp.Department = row["Department"].ToString();
                            
                            emp.DateOfJoining = Convert.ToDateTime(row["DateOfJoining"].ToString()); 

                            emp.photoFileName = row["photoFileName"].ToString();

                            emplist.Add(emp);
                        }

                    }
                    //myReader.Close();
                    //myConn.Close();
                    return new JsonResult(dt);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: \n" + ex);
                return new JsonResult(ex.ToString());
            }
            //return emplist;
        }


        [HttpPost]
        public JsonResult Post(Employee emp)
        //public string Post(Employee emp)
        {
            // sql 
            string query = @"
                           insert into dbo.Employee (EmployeeName, Department, photoFileName, DateOfJoining ) 
                            values (@EmployeeName, @Department, @photoFileName, @DateOfJoining )
                            ";

            try
            {
                DataTable dt = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConn");
                SqlDataReader myReader;
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                        myCommand.Parameters.AddWithValue("@Department", emp.Department);
                        myCommand.Parameters.AddWithValue("@photoFileName", emp.photoFileName);
                        myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);

                        //myReader = myCommand.ExecuteReader();

                        //It is passed a string that is a Transact-SQL statement (such as UPDATE, INSERT, or DELETE)
                        myCommand.ExecuteNonQuery();
                        myCommand.Dispose();

                        //dt.Load(myReader);
                        //myReader.Close();

                        myConn.Close();
                    }
                }  
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: \n" + ex);
            }
            return new JsonResult("Added successfully");

            // convert Department to json 
            //string jsonResult = JsonConvert.SerializeObject("added successfully", Newtonsoft.Json.Formatting.Indented);
            //return jsonResult;

        }


        [HttpPut]
        public JsonResult Put(Employee emp)
        //public string Put(Employee emp)
        {
            // sql 
            string query = @"
                            update  dbo.Employee  
                            set 
                            EmployeeName = @EmployeeName, 
                            Department = @Department,
                            photoFileName = @photoFileName,
                            DateOfJoining = @DateOfJoining   
                            where EmployeeId = @EmployeeId ; 
                            ";

            try
            {
                DataTable dt = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConn");
                SqlDataReader myReader;
                using (SqlConnection myConn = new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myConn))
                    {
                        myCommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                        myCommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                        myCommand.Parameters.AddWithValue("@Department", emp.Department);
                        myCommand.Parameters.AddWithValue("@photoFileName", emp.photoFileName);
                        myCommand.Parameters.AddWithValue("@DateOfJoining", emp.DateOfJoining);

                        //myReader = myCommand.ExecuteReader();

                        myCommand.ExecuteNonQuery();
                        myCommand.Dispose();

                        //dt.Load(myReader);
                        //myReader.Close();
                        myConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception: \n" + ex);
            }
            return new JsonResult("updated successfully");

            // convert Department to json 
            //string jsonResult = JsonConvert.SerializeObject("updated successfully", Newtonsoft.Json.Formatting.Indented);
            //return jsonResult;

        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        //public string Delete(int id)
        {
            string query = @"
                            delete from dbo.Employee
                            where EmployeeId = @EmployeeId 
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {

                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@EmployeeId", id);

                    //myReader = myCommand.ExecuteReader();
                    //table.Load(myReader);
                    //myReader.Close();

                    myCommand.ExecuteNonQuery();
                    myCommand.Dispose(); 

                    myConn.Close();
                }
            }
            return new JsonResult("deleted successfully");

            //string jsonResult = JsonConvert.SerializeObject("deleted successfully", Newtonsoft.Json.Formatting.Indented);
            //return jsonResult;
        }


        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        //public string SaveFile()
        {
            string defaultPFileName = "anonymous.png"; 

            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/photos/" + filename;

                Console.WriteLine(_env.ContentRootPath);

                using (var stream=new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);

                //string jsonResult = JsonConvert.SerializeObject(physicalPath.ToString(), Newtonsoft.Json.Formatting.Indented);
                //string jsonResult = JsonConvert.SerializeObject(filename, Newtonsoft.Json.Formatting.Indented);
                //return jsonResult;
            }
            catch (Exception ex)
            {
                return new JsonResult(defaultPFileName);
            }
        }


    }   
}


