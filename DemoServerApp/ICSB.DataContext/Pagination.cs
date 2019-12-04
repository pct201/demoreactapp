//-----------------------------------------------------------------------
// <copyright file="Pagination.cs" company="TatvaSoft">
//     Copyright (c) TatvaSoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace ICSB.DataContext
{
    /// <summary>
    /// This class is used to Define Pagination properties which will be used for entities
    /// </summary>
    /// <CreatedBy></CreatedBy>
	/// <CreatedDate>09-Nov-2017</CreatedDate>
	public sealed class Pagination
    {
        #region Pagination Property

        /// <summary>
        /// Current Page No
        /// </summary>
        public int PageNo { get; set; }

        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Pager Size
        /// </summary>
        public int PagerSize { get; set; }

        /// <summary>
        /// Total Pages
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Total Records
        /// </summary>
        public int total_records { get; set; }

        /// <summary>
        /// Is Next Page available
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Is Next Page available
        /// </summary>
        public bool HasPreviousPage { get; set; }

        #endregion

    }
}
