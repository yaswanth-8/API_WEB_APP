using API_WEB_APP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API_WEB_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _config;
        List<EmployeeModel> _employees = new List<EmployeeModel>();



        public EmployeeController(IConfiguration config) { 
            _config = config;
        }




        // GET: api/<EmployeeController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from EmployeeDetails", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        EmployeeModel employeeModel = new EmployeeModel();
                        employeeModel.Id = reader.GetInt32(0);
                        employeeModel.Name = reader.GetString(1);
                        _employees.Add(employeeModel);
                    }
                    reader.Close();
                    con.Close();
                    return Ok(_employees);
                }

            }
            catch (Exception ex)
            {

            }
            return Ok(_employees);
        }






        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand($"select * from EmployeeDetails where employee_id={id}", con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    EmployeeModel employeeModel = new EmployeeModel();
                    while (reader.Read())
                    {
                        employeeModel.Id = reader.GetInt32(0);
                        employeeModel.Name = reader.GetString(1);
                    }
                    reader.Close();

                    con.Close();
                    return Ok(employeeModel);
                }

            }
            catch (Exception ex)
            {

            }
            return NotFound();
        }





        // POST api/<EmployeeController>
        [HttpPost]
        public IActionResult Post(EmployeeModel emp)
        {
            using (SqlConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                con.Open();
                SqlCommand _cmd = new SqlCommand("insertEmployee", con);
                _cmd.CommandType = System.Data.CommandType.StoredProcedure;
                _cmd.Parameters.AddWithValue("@id",emp.Id);
                _cmd.Parameters.AddWithValue("@name",emp.Name);
                _cmd.ExecuteNonQuery();
                con.Close();
                return Ok();
            }
            return StatusCode(400, "Insert Valid Data");
        }






        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
