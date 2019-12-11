using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ICSB.Business.Services;
using ICSB.Business.Models;
using System.Linq;
using Microsoft.AspNetCore.Cors;

namespace WebAPI.Controllers
{

    [ApiController]
    public class EmployeeController : ControllerBase
    {        
        // GET api/values
        [HttpGet]
        [Route("api/Employee/AllEmployeeDetails")]
        public IList<UserModel> AllEmployeeDetails()
        {
            try
            {
                using (var userService = new UserServices())
                {
                    IList<UserModel> userList = userService.GetUserList(0, "first_name", "Descending");
                    return userList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // GET api/values/5
        [HttpGet]
        [Route("api/Employee/GetEmployeeDetailsById/{id}")]
        public ActionResult<UserModel> GetEmployeeDetailsById(int id)
        {
            try
            {
                using (var userService = new UserServices())
                {
                    List<UserModel> userList = userService.GetUserList(0, null, null, id);
                    if (userList.Count > 0)
                        return userList.FirstOrDefault();
                    else
                    {                     
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET api/values
        [HttpGet]
        [Route("api/Employee/GetEducationList")]
        public IList<EducationModel> GetEducationList()
        {
            try
            {
                using (var userService = new UserServices())
                {
                    return userService.GetEducationList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        // POST api/values
        [HttpPost]
        [Route("api/Employee/InsertEmployeeDetails")]
        public int InsertEmployeeDetails(UserModel model)
        {           
            try
            {
                using (var userService = new UserServices())
                {
                    return userService.AddEditUser(model);
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        // PUT api/values
        [HttpPut]
        [Route("api/Employee/UpdateEmployeeDetails")]
        public int UpdateEmployeeDetails(UserModel model)
        {     
            try
            {
                using (var userService = new UserServices())
                {
                    return userService.AddEditUser(model);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("api/Employee/DeleteEmployee")]
        public bool DeleteEmaployee(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                try
                {
                    using (var userService = new UserServices())
                    {
                        userService.DeleteUserById(ids);
                        return true;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
                return false;
        }
    }
}