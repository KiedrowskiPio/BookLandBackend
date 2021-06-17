using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using BookLand.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookLand.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public BooksController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            
            string query = @"select BookId, BookTitle, BookAuthor, BookRelese,PhotoFileName from dbo.Books";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BooklandConnection");
            SqlDataReader reader;

                using (SqlConnection con = new SqlConnection(sqlDataSource))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        reader = command.ExecuteReader();
                        table.Load(reader); ;

                        reader.Close();
                        con.Close();
                    }
                }
            

            return new JsonResult(table);

        }

        [HttpPost]
        public JsonResult Post(Books mt)
        {
            
            string query = @"insert into dbo.Books values ('"+mt.BookTitle+@"', '"+mt.BookAuthor+@"','" + mt.BookRelese + @"','" + mt.PhotoFileName + @"')";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BookLandConnection");
            SqlDataReader reader;

            try
            {
                using (SqlConnection con = new SqlConnection(sqlDataSource))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        reader = command.ExecuteReader();
                        table.Load(reader); ;

                        reader.Close();
                        con.Close();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Problem occured during loading SQL: " + e);
            }
            return new JsonResult("Book Added");
        }

        [HttpPut]
        public JsonResult Put(Books mt)
        {
            string query = @"update dbo.Books set BookTitle ='" + mt.BookTitle + @"', 
                            BookAuthor = '" + mt.BookAuthor + @"', 
                            BookRelese = '" + mt.BookRelese + @"',
                            PhotoFileName = '" + mt.PhotoFileName + @"' from dbo.Books where '" + mt.BookId+@"' = BookId";


            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BookLandConnection");
            SqlDataReader reader;

            try
            {
                using (SqlConnection con = new SqlConnection(sqlDataSource))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        reader = command.ExecuteReader();
                        table.Load(reader); ;

                        reader.Close();
                        con.Close();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Problem occured during loading SQL: " + e);
            }

            return new JsonResult("Book Updated");
        }

        [HttpDelete("{BookId}")]
        public JsonResult Delete(int BookId)
        {
            string query = @"delete from dbo.Books where '" + BookId + @"' = BookId";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BookLandConnection");
            SqlDataReader reader;

            try
            {
                using (SqlConnection con = new SqlConnection(sqlDataSource))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        reader = command.ExecuteReader();
                        table.Load(reader); ;

                        reader.Close();
                        con.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem occured during loading SQL: " + e);
            }
            return new JsonResult("Book Deleted");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
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
                return new JsonResult(filename);
            }
            catch (Exception)
            {
                return new JsonResult("generic.png");
            }
        }

        [Route("GetBookNames")]
        public JsonResult GetBookNames()
        {
            string query = @"select BookTitle from dbo.Books";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("BookLandConnection");
            SqlDataReader reader;

            try
            {
                using (SqlConnection con = new SqlConnection(sqlDataSource))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        reader = command.ExecuteReader();
                        table.Load(reader); ;

                        reader.Close();
                        con.Close();
                    }
                }
            }
             catch (Exception e)
            {
                Console.WriteLine("Problem occured during loading SQL: " + e);
            }
            return new JsonResult(table);
        }
    }
}
