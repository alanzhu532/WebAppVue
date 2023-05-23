using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Cors;

namespace WebAppVUE.Controllers
{
    //[EnableCors("http://localhost:5241", ForwardedHeadersExtensions: "*", methods: "*")]

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
        //[HttpGet("{id}")]
        public JsonResult Get() 
        //public List<Department> Get() 
        {
            List<Department> deplist = new List<Department>();

            // sql 
            string query = @"
                            select DepartmentId, DepartmentName from 
                            dbo.Department 
                            ";

            try
            {
                DataTable dt = new DataTable();
                string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConn");
                SqlDataReader myReader;
                using (SqlConnection myConn=new SqlConnection(sqlDataSource))
                {
                    myConn.Open();
                    using(SqlCommand myCommand=new SqlCommand(query, myConn)) 
                    {
                        //myCommand.Parameters.AddWithValue("@DepartmentId", id);
                        myReader = myCommand.ExecuteReader();

                        dt.Load(myReader);
                        myReader.Close();
                        myConn.Close();

                        //int rcount = table.Rows.Count;
                        //int tnum = 0;

                        //if (rcount > 0)
                        //{
                        //    depq = new Department[rcount];

                        //    while (rcount > tnum)
                        //    {
                        //        //
                        //        tnum++;
                        //    }
                        //}

                        foreach (DataRow row in dt.Rows)
                        {
                            Department dep = new Department();
                            //Console.WriteLine(row["DepartmentId"].ToString());
                            int depid = 0;
                            if (int.TryParse(row["DepartmentId"].ToString(), out depid))
                            {
                                dep.DepartmentId = depid;
                            }

                            dep.DepartmentName = row["DepartmentName"].ToString();

                            //dep.logoFileName = row["logoFileName"].ToString();

                            deplist.Add(dep);
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
            //return deplist;
        }


        [HttpPost]
        public JsonResult Post(Department dep)
        //public string Post(Department dep)
        {
            string query = @"
                            insert into dbo.Department (DepartmentName) 
                            values (@DepartmentName)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {   
                    myCommand.Parameters.AddWithValue("@DepartmentName",dep.DepartmentName);

                    //myReader = myCommand.ExecuteReader();
                    //table.Load(myReader);
                    //myReader.Close();

                    myCommand.ExecuteNonQuery();
                    myCommand.Dispose();   

                    myConn.Close();
                }
            }
            return new JsonResult("added successfully");

            // convert Department to json 
            //string jsonResult = JsonConvert.SerializeObject("added successfully", Newtonsoft.Json.Formatting.Indented);
            //return jsonResult;
        }


        [HttpPut]
        public JsonResult Put(Department dep)
        //public string Put(Department dep)
        {
            string query = @"
                            update dbo.Department 
                            set DepartmentName = @DepartmentName 
                            where DepartmentId = @DepartmentId 
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {

                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    myCommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);

                    //myReader = myCommand.ExecuteReader();
                    //table.Load(myReader);
                    //myReader.Close();

                    myCommand.ExecuteNonQuery();
                    myCommand.Dispose();

                    myConn.Close();
                }
            }
            return new JsonResult("updated successfully");

            //string jsonResult = JsonConvert.SerializeObject("updated successfully", Newtonsoft.Json.Formatting.Indented);
            //return jsonResult; 
        }


        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        //public string Delete(int id)
        {
            string query = @"
                            delete from dbo.Department 
                            where DepartmentId = @DepartmentId 
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeAppConn");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {

                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@DepartmentId", id);

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

    }

}
