using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ICSB.Business.Services;
using ICSB.Business.Models;
using Microsoft.AspNetCore.Cors;

namespace WebAPI.Controllers
{
   
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        //private readonly UserServices userServices;
        //public EmployeeController(UserServices _userServices)
        //{
        //   this.userServices = _userServices;
        //}

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
                if (id > 0)
                {
                    using (var userService = new UserServices())
                    {
                        return userService.GetUserByID(id);
                    }
                }
                else
                {
                    UserModel user = new UserModel();
                    return user;
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
            if (!ModelState.IsValid)
            {
                return 0;
            }
            try
            {
                using (var userService = new UserServices())
                {
                    return userService.AddUser(model);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // PUT api/values
        [HttpPut]
        [Route("api/Employee/UpdateEmployeeDetails")]
        public int UpdateEmployeeDetails(UserModel model)
        {
            if (!ModelState.IsValid)
            {
                return 0;
            }

            try
            {
                using (var userService = new UserServices())
                {
                    return userService.EditUser(model);
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("api/Employee/DeleteEmaployee/{id}")]
        public bool DeleteEmaployee(int id)
        {
            if (id > 0)
            {
                try
                {
                    using (var userService = new UserServices())
                    {
                        userService.DeleteUserById(id);
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