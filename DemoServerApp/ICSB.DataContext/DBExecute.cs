using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Data;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Configuration;
namespace ICSB.DataContext
{
    public abstract class DBExecute
    {
        /// <summary>
        /// SQL server name 
        /// </summary>
        protected const string ClientDatabaseName = "EnterPriseEntities";
        protected const string MasterDatabaseName = "DBConnectionString";
               
        /// <summary>
        /// Gets Default Page Size
        /// </summary>
        public int DefaultPageSize
        {
            get
            {
                //return 5;
                return 10;
            }
        }

        /// <summary>
        /// Gets Default Page Size
        /// </summary>
        public int DefaultPagerSize
        {
            get
            {
                return 5;
            }
        }

        /// <summary>
        /// Gets or sets Pagination Information
        /// </summary>
        public virtual Pagination PagingInformation { get; set; }

        /// <summary>
        /// Execute Procedure for custom methods
        /// </summary>
        /// <param name="procedureName">procedure name</param>
        /// <param name="executeType">execute type</param>
        /// <param name="parameters">parameter list</param>
        /// <returns>return procedure output</returns>
        public virtual object ExecuteProcedure(string procedureName, ExecuteType executeType, System.Collections.ObjectModel.Collection<DBParameters> parameters, string productUid = null)
        {
            AppConfiguration appSettings = new AppConfiguration(productUid);
            return DBClient.ExecuteProcedure(procedureName, executeType, parameters, appSettings.GetConnectionString);
        }

        /// <summary>
        /// Executes the procedurewith out parameter.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="executeType">Type of the execute.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="dic">The dic.</param>
        /// <returns></returns>
        public virtual object ExecuteProcedurewithOutParameter(string procedureName, ExecuteType executeType, System.Collections.ObjectModel.Collection<DBParameters> parameters, out Dictionary<string, string> dic, string productUid = null)
        {
            AppConfiguration appSettings = new AppConfiguration(productUid);
            return DBClient.ExecuteProcedurewithOutParameter(procedureName, executeType, parameters, appSettings.GetConnectionString, out dic);
        }

        /// <summary>
        /// Execute Procedure for custom methods
        /// </summary>
        /// <typeparam name="TEntity">Entity Type which require to be return as list</typeparam>
        /// <param name="procedureName">procedure name</param>
        /// <param name="parameters">parameter list</param>
        /// <returns>return procedure output</returns>
        public virtual IList<TEntity> ExecuteProcedure<TEntity>(string procedureName, System.Collections.ObjectModel.Collection<DBParameters> parameters, string productUid = null)
        {
            AppConfiguration appSettings = new AppConfiguration(productUid);
            var list = DBClient.ExecuteProcedure<TEntity>(procedureName, parameters, appSettings.GetConnectionString);
            if (list != null && list.Any())
            {
                this.SetPaginationInformation(Convert.ToInt32(GetPropertyValue(list[0], "total_records"), CultureInfo.InvariantCulture));
            }

            return list;
        }
        
        public virtual IList<TEntity> ExecuteProcedureWithPerameterwithoutPagination<TEntity>(string procedureName, System.Collections.ObjectModel.Collection<DBParameters> parameters, string productUid = null)
        {
            AppConfiguration appSettings = new AppConfiguration(productUid);
            return DBClient.ExecuteProcedure<TEntity>(procedureName, parameters, appSettings.GetConnectionString);
        }


        /// <summary>
        /// Execute Procedure for custom methods without pagination
        /// </summary>
        /// <typeparam name="TEntity">Entity Type which require to be return as list</typeparam>
        /// <param name="procedureName">procedure name</param>
        /// <returns>return procedure output</returns>
        public virtual IList<TEntity> ExecuteProcedurewithoutPagination<TEntity>(string procedureName, string productUid = null)
        {
            AppConfiguration appSettings = new AppConfiguration(productUid);
            return DBClient.ExecuteProcedure<TEntity>(procedureName, null, appSettings.GetConnectionString);
        }

        /// <summary>
        /// Execute Procedure for custom methods without pagination
        /// </summary>
        /// <param name="procedureName"></param>
        /// <returns>returns data table</returns>
        public virtual DataTable ExecuteProcedurewithoutPaginationDatatable(string procedureName, System.Collections.ObjectModel.Collection<DBParameters> parameter, string productUid = null)
        {
            AppConfiguration appSettings = new AppConfiguration(productUid);
            return DBClient.ExecuteProcedureDatatable(procedureName, parameter, appSettings.GetConnectionString);
        }

        public virtual IList<TEntity> ExecuteProcedurewithOutParameter<TEntity>(string procedureName, System.Collections.ObjectModel.Collection<DBParameters> parameters, out Dictionary<string, string> dictionary, string productUid = null)
        {
            AppConfiguration appSettings = new AppConfiguration(productUid);
            return DBClient.ExecuteProcedurewithOutParameter<TEntity>(procedureName, parameters, appSettings.GetConnectionString, out dictionary);
        }

        public virtual IList<TEntity> CustomSPExecute<TEntity>(string procedureName, int? pageNo, string sortExpression, string sortDirection, Collection<DBParameters> parameters, string SearchValue = "")
        {
            /*Add Parameters*/

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

            if (!string.IsNullOrEmpty(SearchValue))
            {
                parameters.Add(new DBParameters() { Name = "SearchValue", Value = SearchValue, DBType = DbType.String });
            }

            /*Convert Dataset to Model List object*/
            return this.ExecuteProcedure<TEntity>(procedureName, parameters);
        }

        /// <summary>
        /// Get Property Value of given Property Name
        /// </summary>
        /// <typeparam name="TEntity">Entity Type</typeparam>
        /// <param name="entity">Model to Extract</param>
        /// <param name="propertyName">property name</param>
        /// <returns>Table Name</returns>
        protected static string GetPropertyValue<TEntity>(TEntity entity, string propertyName)
        {
            try
            {
                PropertyInfo info = entity.GetType().GetProperties().FirstOrDefault(prop => prop.Name.ToLower(System.Globalization.CultureInfo.CurrentCulture) == propertyName.ToLower(System.Globalization.CultureInfo.CurrentCulture));

                if (info != null)
                {
                    if (info.PropertyType == typeof(string))
                    {
                        return Convert.ToString(info.GetValue(entity, null), CultureInfo.InvariantCulture).Trim();
                    }

                    return Convert.ToString(info.GetValue(entity, null), CultureInfo.InvariantCulture);
                }

                return string.Empty;
            }
            catch (Exception)
            {
                Console.WriteLine("There is an error in getting property value for " + propertyName);
                throw;
            }
        }

        /// <summary>
        /// Gets or sets Start Row Index for Current Page
        /// </summary>
        /// <param name="pageNo">page no</param>
        /// <returns>return start row index</returns>
        public int StartRowIndex(int? pageNo)
        {
            if (pageNo.HasValue && pageNo > 0)
            {
                this.PagingInformation.PageNo = pageNo.Value;
                return ((pageNo.Value - 1) * this.PagingInformation.PageSize) + 1;
            }
            return 0;
        }

        /// <summary>
        /// End Row Index for Current Page
        /// </summary>
        /// <param name="pageNo">page no</param>
        /// <returns>returns end row index</returns>
        public int EndRowIndex(int? pageNo)
        {
            if (pageNo.HasValue && pageNo > 0)
            {
                return ((pageNo.Value - 1) * this.PagingInformation.PageSize) + this.PagingInformation.PageSize;
            }
            return 0;
        }

        /// <summary>
        /// Set Pagination Information
        /// </summary>
        /// <param name="totalRecords">total records</param>
        private void SetPaginationInformation(int totalRecords)
        {
            this.PagingInformation.total_records = totalRecords;
            this.PagingInformation.TotalPages = (this.PagingInformation.total_records / this.PagingInformation.PageSize) + ((this.PagingInformation.total_records % this.PagingInformation.PageSize > 0) ? 1 : 0);
            this.PagingInformation.HasPreviousPage = this.PagingInformation.PageNo > this.PagingInformation.PagerSize;
            int currentPagerNo = this.PagingInformation.PageNo / (this.PagingInformation.PagerSize + (this.PagingInformation.PageNo % this.PagingInformation.PagerSize > 0 ? 1 : 0));
            int currentPagerRecords = currentPagerNo * this.PagingInformation.PagerSize;
            this.PagingInformation.HasNextPage = this.PagingInformation.TotalPages > this.PagingInformation.PagerSize ? ((this.PagingInformation.TotalPages % this.PagingInformation.PagerSize) == 0 ? (currentPagerRecords < this.PagingInformation.TotalPages - (this.PagingInformation.TotalPages % this.PagingInformation.PagerSize)) : (currentPagerRecords <= this.PagingInformation.TotalPages - (this.PagingInformation.TotalPages % this.PagingInformation.PagerSize))) : false;
        }
    }
}
