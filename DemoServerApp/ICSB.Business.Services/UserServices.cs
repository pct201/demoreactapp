using System;
using System.Collections.Generic;
using System.Linq;
using ICSB.Business.Models;
using ICSB.DataContext;

using System.Data;

using System.Collections.ObjectModel;


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
        /// Bind model to script's parameter and execute procedure for inserting new user data.
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <param name="isCompanyRegister"></param>
        /// <returns></returns>
        public int AddUser(UserModel objUserModel)
        {
            System.Collections.ObjectModel.Collection<DBParameters> parameters = new System.Collections.ObjectModel.Collection<DBParameters>();
            parameters.Add(new DBParameters() { Name = "@user_id", Value = objUserModel.UserId, DBType = DbType.Int32 });
            parameters.Add(new DBParameters() { Name = "@email_address", Value = objUserModel.Email, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@first_name", Value = objUserModel.First_name, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@last_name", Value = objUserModel.Last_name, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@mobile_phone_number", Value = objUserModel.Mobile_Number, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@education_id", Value = objUserModel.Education_Id, DBType = DbType.Int32 });
            parameters.Add(new DBParameters() { Name = "@salary", Value = objUserModel.Salary, DBType = DbType.Decimal });
            parameters.Add(new DBParameters() { Name = "@birth_date", Value = objUserModel.Birth_Date, DBType = DbType.DateTime });
            parameters.Add(new DBParameters() { Name = "@is_married", Value = objUserModel.Is_Married, DBType = DbType.Boolean });
            parameters.Add(new DBParameters() { Name = "@address", Value = objUserModel.Address, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@blog", Value = objUserModel.Blog, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@profile_picture", Value = objUserModel.Profile_Picture, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@document_name", Value = objUserModel.Document_Name, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@document", Value = objUserModel.Document, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@updated_by", Value = objUserModel.Updated_by, DBType = DbType.Int32 });
            return Convert.ToInt32(this.ExecuteProcedure("co.users_add", ExecuteType.ExecuteScalar, parameters));
        }

        /// <summary>
        /// Bind model to script's parameter and execute procedure for editing user data.
        /// </summary>
        /// <param name="objUserModel"></param>
        /// <returns></returns>
        public int EditUser(UserModel objUserModel)
        {
            System.Collections.ObjectModel.Collection<DBParameters> parameters = new System.Collections.ObjectModel.Collection<DBParameters>();
            parameters.Add(new DBParameters() { Name = "@user_id", Value = objUserModel.UserId, DBType = DbType.Int32 });   
            parameters.Add(new DBParameters() { Name = "@email_address", Value = objUserModel.Email, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@first_name", Value = objUserModel.First_name, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@last_name", Value = objUserModel.Last_name, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@mobile_phone_number", Value = objUserModel.Mobile_Number, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@education_id", Value = objUserModel.Education_Id, DBType = DbType.Int32 });
            parameters.Add(new DBParameters() { Name = "@salary", Value = objUserModel.Salary, DBType = DbType.Decimal });
            parameters.Add(new DBParameters() { Name = "@birth_date", Value = objUserModel.Birth_Date, DBType = DbType.DateTime });
            parameters.Add(new DBParameters() { Name = "@is_married", Value = objUserModel.Is_Married, DBType = DbType.Boolean });
            parameters.Add(new DBParameters() { Name = "@address", Value = objUserModel.Address, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@blog", Value = objUserModel.Blog, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@profile_picture", Value = objUserModel.Profile_Picture, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@document_name", Value = objUserModel.Document_Name, DBType = DbType.String });
            parameters.Add(new DBParameters() { Name = "@document", Value = objUserModel.Document, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@updated_by", Value = objUserModel.Updated_by, DBType = DbType.Int32 });
           this.ExecuteProcedure("co.users_update", ExecuteType.ExecuteScalar, parameters);            
            return 0;
        }

        /// <summary>
        /// checks whether email address exist in auth.logins table
        /// </summary>
        /// <param name="emailId"></param>
        /// <returns></returns>
        public virtual object isEmailAddressExist(string emailId)
        {
            System.Collections.ObjectModel.Collection<DBParameters> parameters = new System.Collections.ObjectModel.Collection<DBParameters>();            
            parameters.Add(new DBParameters() { Name = "@email_address", Value = emailId, DBType = DbType.AnsiString });
            return this.ExecuteProcedure("auth.login_find_by_email", ExecuteType.ExecuteScalar, parameters);
        }

       

        /// <summary>
        /// Fetch user list from database
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public virtual IList<UserModel> GetUserList(int? pageNo, string sortExpression, string sortDirection)
        {

            Collection<DBParameters> parameters = new Collection<DBParameters>();           

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

        /// <summary>
        /// Fetch user list from database
        /// </summary>     
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual UserModel GetUserByID(int userId)
        {
            Collection<DBParameters> parameters = new Collection<DBParameters>();
            parameters.Add(new DBParameters() { Name = "user_id", Value = userId, DBType = DbType.Int32 });
            return this.ExecuteProcedure<UserModel>("[co].[user_list_get]", parameters).FirstOrDefault();
        }


        /// <summary>
        /// Fetch user list from database
        /// </summary>       
        /// <param name="userId"></param>       
        /// <returns></returns>
        public virtual void DeleteUserById(int userId)
        {
            Collection<DBParameters> parameters = new Collection<DBParameters>();
            parameters.Add(new DBParameters() { Name = "user_id", Value = userId, DBType = DbType.Int32 });
            this.ExecuteProcedure<UserModel>("[co].[user_delete]", parameters);
        }


    }
}
