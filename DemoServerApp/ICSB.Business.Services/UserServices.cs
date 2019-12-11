using System;
using System.Collections.Generic;
using System.Linq;
using ICSB.Business.Models;
using ICSB.DataContext;

using System.Data;

using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ICSB.Business.Services
{
    /// <summary>
    /// Service Layer - User
    /// </summary>
    public class UserServices : ServiceContext
    {
        public UserServices()
        {
            this.PagingInformation = new Pagination() { PageSize = DefaultPageSize, PagerSize = DefaultPagerSize };
        }

        /// <summary>
        /// Fetch user list from database
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="userId"></param>     
        /// <returns></returns>
        public virtual List<UserModel> GetUserList(int? pageNo, string sortExpression, string sortDirection, int userId = 0)
        {
            Collection<DBParameters> parameters = new Collection<DBParameters>();
            if (userId > 0)
            {
                parameters.Add(new DBParameters() { Name = "user_id", Value = userId, DBType = DbType.Int32 });
                return this.ExecuteProcedure<UserModel>("[dbo].[user_detail_get]", parameters).ToList();
            }
            else
            {
                if (this.StartRowIndex(pageNo) > 0 && this.EndRowIndex(pageNo) > 0)
                {
                    parameters.Add(new DBParameters() { Name = "start_row_index", Value = this.StartRowIndex(pageNo), DBType = DbType.Int16 });
                    parameters.Add(new DBParameters() { Name = "end_row_index", Value = this.EndRowIndex(pageNo), DBType = DbType.Int16 });
                }

                if (!string.IsNullOrEmpty(sortExpression) && !string.IsNullOrEmpty(sortDirection))
                {
                    parameters.Add(new DBParameters() { Name = "sort_expression", Value = sortExpression, DBType = DbType.String });
                    parameters.Add(new DBParameters() { Name = "sort_direction", Value = sortDirection, DBType = DbType.String });
                }
                return this.ExecuteProcedure<UserModel>("[dbo].[user_list_get]", parameters).ToList();
            }
        }

        /// <summary>
        /// Add Or Update User Detail In Database.
        /// </summary>
        /// <param name="objUserModel"></param>    
        /// <returns></returns>
        public int AddEditUser(UserModel objUserModel)
        {
            System.Collections.ObjectModel.Collection<DBParameters> parameters = new System.Collections.ObjectModel.Collection<DBParameters>();

            if (objUserModel.UserId > 0)
                parameters.Add(new DBParameters() { Name = "@user_id", Value = objUserModel.UserId, DBType = DbType.Int32 });

            if (!string.IsNullOrEmpty(objUserModel.Profile_Picture))            
                parameters.Add(new DBParameters() { Name = "@profile_picture", Value = Convert.FromBase64String(objUserModel.Profile_Picture.Split(',')[1]), DBType = DbType.Binary });

            if (!string.IsNullOrEmpty(objUserModel.Document))
            {        
                parameters.Add(new DBParameters() { Name = "@document_name", Value = objUserModel.Document_Name, DBType = DbType.AnsiString });
                parameters.Add(new DBParameters() { Name = "@document", Value = objUserModel.Document.Contains("base64")? Convert.FromBase64String(objUserModel.Document.Split(',')[1]): Convert.FromBase64String(objUserModel.Document), DBType = DbType.Binary });
            }

            parameters.Add(new DBParameters() { Name = "@email", Value = objUserModel.Email, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@first_name", Value = objUserModel.First_name, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@last_name", Value = objUserModel.Last_name, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@mobile_number", Value = objUserModel.Mobile_number, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@education_id", Value = objUserModel.Education_Id, DBType = DbType.Int32 });
            parameters.Add(new DBParameters() { Name = "@salary", Value = objUserModel.Salary, DBType = DbType.Decimal });
            parameters.Add(new DBParameters() { Name = "@birth_date", Value = objUserModel.Birth_Date, DBType = DbType.Date });
            parameters.Add(new DBParameters() { Name = "@is_married", Value = objUserModel.Is_Married, DBType = DbType.Boolean });
            parameters.Add(new DBParameters() { Name = "@address", Value = objUserModel.Address, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@blog", Value = objUserModel.Blog, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@created_by", Value = objUserModel.Updated_by, DBType = DbType.Int32 });
            return Convert.ToInt32(this.ExecuteProcedure("dbo.add_edit_user", ExecuteType.ExecuteScalar, parameters));
        }


        /// <summary>
        /// Fetch user list from database
        /// </summary>       
        /// <param name="userId"></param>       
        /// <returns></returns>
        public virtual void DeleteUserById(string userIds)
        {
            Collection<DBParameters> parameters = new Collection<DBParameters>();
            parameters.Add(new DBParameters() { Name = "user_ids", Value = userIds, DBType = DbType.AnsiString });
            this.ExecuteProcedure<UserModel>("[dbo].[user_delete]", parameters);
        }

        /// <summary>
        /// checks whether email address exist in auth.logins table
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public virtual int isEmailAddressExist(string emailId)
        {
            System.Collections.ObjectModel.Collection<DBParameters> parameters = new System.Collections.ObjectModel.Collection<DBParameters>();
            parameters.Add(new DBParameters() { Name = "@email", Value = emailId, DBType = DbType.AnsiString });
            return Convert.ToInt32(this.ExecuteProcedure("dbo.user_email_check", ExecuteType.ExecuteScalar, parameters));
        }


        /// <summary>
        /// Get EducationList.
        /// </summary>
        /// <param name="objUserModel"></param>    
        /// <returns></returns>
        public IList<EducationModel> GetEducationList()
        {
            Collection<DBParameters> parameters = new Collection<DBParameters>();
            return this.ExecuteProcedureWithPerameterwithoutPagination<EducationModel>("[dbo].[education_dropdown_get]", parameters).ToList();
        }


    }
}
