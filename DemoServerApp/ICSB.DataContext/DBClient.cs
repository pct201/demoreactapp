//-----------------------------------------------------------------------
// <copyright file="DBClient.cs" company="TatvaSoft">
//     Copyright (c) TatvaSoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ICSB.DataContext
{
    using Microsoft.Extensions.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Define the possible execution type for Procedure
    /// </summary>
    /// <CreatedBy></CreatedBy>
    /// <CreatedDate>09-Nov-2017</CreatedDate>
    /// <ModifiedBy></ModifiedBy>
    /// <ModifiedDate></ModifiedDate>
    /// <ReviewBy></ReviewBy>
    /// <ReviewDate></ReviewDate>
    public enum ExecuteType
    {
        /// <summary>
        /// Execute as Scalar
        /// </summary>
        ExecuteScalar,

        /// <summary>
        /// Execute as Dataset
        /// </summary>
        ExecuteDataSet,

        /// <summary>
        /// Execute as non query
        /// </summary>
        ExecuteNonQuery,

        /// <summary>
        /// Execute as Reader
        /// </summary>
        ExecuteReader
    }

    /// <summary>
    /// This class is to define common methods for do the DBCall using Stored Procedure with SQL Command 
    /// </summary>
    /// <CreatedBy>Tatvasoft</CreatedBy>
    /// <CreatedDate>09-Nov-2017</CreatedDate>
    /// <ModifiedBy></ModifiedBy>
    /// <ModifiedDate></ModifiedDate>
    /// <ReviewBy></ReviewBy>
    /// <ReviewDate></ReviewDate>
    internal sealed class DBClient
    {
        #region Property/Enum
        public static int _appSetting = 100;
        /// <summary>
        /// Prevents a default instance of the DBClient class from being created.
        /// </summary>
        private DBClient()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);
            var root = configurationBuilder.Build();
            _appSetting = Convert.ToInt32(root.GetSection("ApplicationSettings").GetSection("CommandTimeOut").Value);
        }


      
        #endregion

        #region Methods

        /// <summary>
        /// Execute the Stored Procedure with given Parameters
        /// </summary>
        /// <param name="procedureName">procedure name</param>
        /// <param name="executeType">execute type</param>
        /// <param name="parameters">procedure parameter</param>
        /// <param name="databaseConnection">Database Connection String</param>
        /// <returns>return execute procedure </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "This will only return predefined Procedure Name, not user inputs")]
        public static object ExecuteProcedure(string procedureName, ExecuteType executeType, System.Collections.ObjectModel.Collection<DBParameters> parameters, string databaseConnection)
        {
            object returnValue;
            //// Create a suitable command type and add the required parameter
            using (SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(databaseConnection))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection);                
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = _appSetting;
                /*Add different Parameter from Model object Property*/
                AddParameters(ref sqlCommand, parameters);

                /*Execute Procedure from supplied Execution type*/

                if (executeType == ExecuteType.ExecuteScalar)
                {
                   returnValue = sqlCommand.ExecuteScalar();
                }
                else if (executeType == ExecuteType.ExecuteDataSet)
                {
                    DataSet dataSet = new DataSet();
                    dataSet.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                    sqlAdapter.Fill(dataSet);
                    returnValue = dataSet;
                }
                else if (executeType == ExecuteType.ExecuteNonQuery)
                {
                    returnValue = sqlCommand.ExecuteNonQuery();
                }
                else if (executeType == ExecuteType.ExecuteReader)
                {
                    returnValue = sqlCommand.ExecuteReader();
                }
                else
                {
                    returnValue = "No Proper execute type provide";
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Execute the Stored Procedure with given Parameters
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="procedureName">procedure name</param>
        /// <param name="parameters">procedure parameter</param>
        /// <param name="databaseConnection">Database Connection String</param>
        /// <returns>return execute procedure </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "This will only return predefined Procedure Name, not user inputs")]
        public static IList<T> ExecuteProcedure<T>(string procedureName, System.Collections.ObjectModel.Collection<DBParameters> parameters, string databaseConnection)
        {
            List<T> returnValue = new List<T>();

            // Create a suitable command type and add the required parameter
            using (SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(databaseConnection))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = _appSetting;
                if (parameters != null)
                {
                    /*Add different Parameter from Model object Property*/
                    AddParameters(ref sqlCommand, parameters);
                }

                /*Execute Procedure from supplied Execution type*/
                returnValue = DataReaderToList<T>(sqlCommand.ExecuteReader());
            }

            return returnValue;
        }


        /// <summary>
        /// Execute the Stored Procedure with given Parameters
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="parameters"></param>
        /// <param name="databaseConnection"></param>
        /// <returns> returns data table</returns>
      
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "This will only return predefined Procedure Name, not user inputs")]
        public static DataTable ExecuteProcedureDatatable(string procedureName, System.Collections.ObjectModel.Collection<DBParameters> parameters, string databaseConnection)
        {
            
            DataTable returnValue = new DataTable();
            // Create a suitable command type and add the required parameter
            using (SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(databaseConnection))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = _appSetting;
                if (parameters != null)
                {
                    /*Add different Parameter from Model object Property*/
                    AddParameters(ref sqlCommand, parameters);
                }

                /*Execute Procedure from supplied Execution type*/
                SqlDataReader dr = (sqlCommand.ExecuteReader());
                returnValue.Load(dr);
            }
            return returnValue;
        }

        /// <summary>
        /// Add Parameter from Model Object Property
        /// </summary>
        /// <param name="sqlCommand">Database object</param>
        /// <param name="parameters">Command Object</param>
        private static void AddParameters(ref SqlCommand sqlCommand, System.Collections.ObjectModel.Collection<DBParameters> parameters)
        {
            foreach (DBParameters parameter in parameters)
            {
                SqlParameter sqlParameter = new SqlParameter();
                // Convert default value to null.
                if (parameter.SqlDbType != null)
                    sqlParameter.SqlDbType = parameter.SqlDbType.Value;
                else
                    sqlParameter.DbType = parameter.DBType;

                sqlParameter.Direction = ParameterDirection.Input;
                sqlParameter.ParameterName = parameter.Name;
                sqlParameter.Value = parameter.Value;
                sqlCommand.Parameters.Add(sqlParameter);
            }
        }

        /// <summary>
        /// Convert Data Reader to List
        /// </summary>
        /// <typeparam name="T">Entity Object</typeparam>
        /// <param name="dr">data reader object</param>
        /// <returns>return list of objects</returns>
        private static List<T> DataReaderToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();

            T obj = default(T);

            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();

                for (int i = 0; i < dr.FieldCount; i++)
                {
                    PropertyInfo info = obj.GetType().GetProperties().FirstOrDefault(o => o.Name.ToLower() == dr.GetName(i).ToLower());
                    if (info != null)
                    {
                        /*Set the Value to Model*/
                        info.SetValue(obj, dr.GetValue(i) != System.DBNull.Value ? dr.GetValue(i) : null, null);
                    }
                }

                list.Add(obj);
            }

            return list;
        }

        /// <summary>
        /// Set Pagination Properties
        /// </summary>
        /// <param name="totalRecords">Total Records</param>
        /// <param name="pages">Pagination Objects</param>
        private static void SetPaginationInformation(int totalRecords, ref Pagination pages)
        {
            pages.total_records = totalRecords;
            pages.TotalPages = (pages.total_records / pages.PageSize) + ((pages.total_records % pages.PageSize > 0) ? 1 : 0);
            pages.HasPreviousPage = pages.PageNo > pages.PagerSize;
            int currentPagerNo = pages.PageNo / (pages.PagerSize + (pages.PageNo % pages.PagerSize > 0 ? 1 : 0));
            int currentPagerRecords = currentPagerNo * pages.PagerSize;
            pages.HasNextPage = pages.TotalPages > pages.PagerSize ? ((pages.TotalPages % pages.PagerSize) == 0 ? (currentPagerRecords < pages.TotalPages - (pages.TotalPages % pages.PagerSize)) : (currentPagerRecords <= pages.TotalPages - (pages.TotalPages % pages.PagerSize))) : false;
        }

        // <summary>
        /// Executes the procedurewith out value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="databaseConnection">The database connection.</param>
        /// <param name="dic">The dic.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "This will only return predefined Procedure Name, not user inputs")]
        public static IList<T> ExecuteProcedurewithOutParameter<T>(string procedureName, System.Collections.ObjectModel.Collection<DBParameters> parameters, string databaseConnection, out Dictionary<string, string> dic)
        {
            dic = new Dictionary<string, string>();
            List<T> returnValue = new List<T>();

            // Create a suitable command type and add the required parameter
            using (SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(databaseConnection))
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = _appSetting;
                /*Add different Parameter from Model object Property*/
                AddParameters(ref sqlCommand, parameters);

                /*Execute Procedure from supplied Execution type*/
                returnValue = DataReaderToList<T>(sqlCommand.ExecuteReader());

                if (parameters.Any(a => a.Direction == ParameterDirection.Output))
                {
                    foreach (var db in parameters.Where(w => w.Direction == ParameterDirection.Output))
                    {
                        dic.Add(sqlCommand.Parameters[db.Name].ParameterName, Convert.ToString(sqlCommand.Parameters[db.Name].Value));
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Executes the procedurewith OutParameter.        
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <param name="executeType">Type of the execute.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="databaseConnection">The database connection.</param>
        /// <param name="dictionary">The dic.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "This will only return predefined Procedure Name, not user inputs")]
        public static object ExecuteProcedurewithOutParameter(string procedureName, ExecuteType executeType, System.Collections.ObjectModel.Collection<DBParameters> parameters, string databaseConnection, out Dictionary<string, string> dictionary)
        {
            dictionary = new Dictionary<string, string>();
            object returnValue;
            //// Create a suitable command type and add the required parameter
            using (SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(databaseConnection))
            {
                sqlConnection.Open();

                SqlCommand sqlCommand = new SqlCommand(procedureName, sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = _appSetting;
                /*Add different Parameter from Model object Property*/
                AddParameters(ref sqlCommand, parameters);

                /*Execute Procedure from supplied Execution type*/
                if (executeType == ExecuteType.ExecuteScalar)
                {
                    returnValue = sqlCommand.ExecuteScalar();
                }
                else if (executeType == ExecuteType.ExecuteDataSet)
                {
                    DataSet dataSet = new DataSet();
                    dataSet.Locale = System.Globalization.CultureInfo.InvariantCulture;
                    SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCommand);
                    sqlAdapter.Fill(dataSet);
                    returnValue = dataSet;
                }
                else if (executeType == ExecuteType.ExecuteNonQuery)
                {
                    returnValue = sqlCommand.ExecuteNonQuery();
                }
                else if (executeType == ExecuteType.ExecuteReader)
                {
                    returnValue = sqlCommand.ExecuteReader();
                }
                else
                {
                    returnValue = "No Proper execute type provide";
                }

                if (parameters.Any(a => a.Direction == ParameterDirection.Output))
                {
                    foreach (var db in parameters.Where(w => w.Direction == ParameterDirection.Output))
                    {
                        dictionary.Add(sqlCommand.Parameters[db.Name].ParameterName, Convert.ToString(sqlCommand.Parameters[db.Name].Value));
                    }
                }
            }
            return returnValue;
        }

        #endregion
    }
}
