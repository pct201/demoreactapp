//-----------------------------------------------------------------------
// <copyright file="DBParameters.cs" company="TatvaSoft">
//     Copyright (c) TatvaSoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ICSB.DataContext
{
    using System.Data;

    /// <summary>
    /// This class to defain default properties for Parameters for DbCommand 
    /// </summary>
	/// <CreatedBy>Tatvasoft</CreatedBy>
	/// <CreatedDate>09-Nov-2017</CreatedDate>
    public sealed class DBParameters
    {
        #region Properties

        public string Name
        { get; set; }

        public object Value
        { get; set; }

        public DbType DBType
        { get; set; }

        public ParameterDirection Direction { get; set; }

        public SqlDbType? SqlDbType
        { get; set; }
        #endregion
    }
}
