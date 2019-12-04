using ICSB.DataContext;

namespace ICSB.Business.Services
{
  
   

    /// <summary>
    /// This class is used to Define Database object for Save, Search, Delete
    /// </summary>
    /// <CreatedBy>Nishant Bharathan</CreatedBy>
    /// <CreatedDate>11-Nov-2017</CreatedDate>
    public  class ServiceContext : DBContext
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ServiceContext class.
        /// </summary>
        /// <param name="isMasterDatabase">Is master database or not</param>
        public ServiceContext()
        {
            this.PagingInformation = new Pagination() { PageSize = DefaultPageSize, PagerSize = DefaultPagerSize };

            this.CheckForDuplicate = false;
        }

        /// <summary>
        /// Initializes a new instance of the ServiceContext class for checking duplicate value for one column
        /// </summary>
        /// <param name="Col1Name">column Name</param>
        public ServiceContext(string Col1Name)
        {
            this.PagingInformation = new Pagination() { PageSize = DefaultPageSize, PagerSize = DefaultPagerSize };

            this.CheckForDuplicate = true;
            this.Col1Name = Col1Name;
            this.CombinationCheckRequired = false;
        }

        /// <summary>
        /// Initializes a new instance of the ServiceContext class for checking duplicate value for two column with combination
        /// </summary>
        /// <param name="Col1Name">first column name</param>
        /// <param name="Col2Name">second column name</param>
        /// <param name="isMasterDatabase">Is master database or not</param>
        public ServiceContext(string Col1Name, string Col2Name)
        {
            this.PagingInformation = new Pagination() { PageSize = DefaultPageSize, PagerSize = DefaultPagerSize };

            this.CheckForDuplicate = true;
            this.Col1Name = Col1Name;
            this.Col2Name = Col2Name;
            this.CombinationCheckRequired = true;
        }

        /// <summary>
        /// Initializes a new instance of the ServiceContext class for checking duplicate value for two column
        /// </summary>
        /// <param name="Col1Name">first column name</param>
        /// <param name="Col2Name">second column name</param>
        /// <param name="CombinationCheckRequired">Combination Check Required</param>
        /// <param name="isMasterDatabase">Is master database or not</param>
        public ServiceContext(string Col1Name, string Col2Name, bool CombinationCheckRequired)
        {
            this.PagingInformation = new Pagination() { PageSize = DefaultPageSize, PagerSize = DefaultPagerSize };

            this.CheckForDuplicate = true;
            this.Col1Name = Col1Name;
            this.Col2Name = Col2Name;
            this.CombinationCheckRequired = CombinationCheckRequired;
        }

        /// <summary>
        ///  Initializes a new instance of the ServiceContext class for checking duplicate value for two column with parent column
        /// </summary>
        /// <param name="Col1Name"></param>
        /// <param name="Col2Name"></param>
        /// <param name="ParentColumnName"></param>
        /// <param name="CombinationCheckRequired"></param>
        public ServiceContext(string Col1Name, string Col2Name, string ParentColumnName, bool CombinationCheckRequired)
        {
            this.PagingInformation = new Pagination() { PageSize = DefaultPageSize, PagerSize = DefaultPagerSize };

            this.CheckForDuplicate = true;
            this.Col1Name = Col1Name;
            this.Col2Name = Col2Name;
            this.ParentColumnName = ParentColumnName;

            this.CombinationCheckRequired = CombinationCheckRequired;
        }

        #endregion

        #region Custom Methods
        

        #endregion
    }
}
