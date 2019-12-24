using ICSB.DataContext;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using ICSB.Business.Models;
using System.Collections.ObjectModel;
using System;
using System.Net;
using System.Text;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace ICSB.Business.Services
{
    public class DocumentTemplateService : ServiceContext
    {

        private readonly UserServices _userService;
        //private readonly ProposalService _proposalService;

        public DocumentTemplateService()
        {
            this.PagingInformation = new Pagination() { PageSize = DefaultPageSize, PagerSize = DefaultPagerSize };
        }

        //public DocumentTemplateService(ProposalService proposalService, UserServices userServices)
        //{
        //    this._proposalService = proposalService;
        //    this._userService = userServices;
        //    this.PagingInformation = new Pagination() { PageSize = DefaultPageSize, PagerSize = DefaultPagerSize };
        //}
        /// <summary>
        /// save the Tempate content into the database.        
        /// </summary>
        /// <param name="languageCode"></param>
        /// <param name="templateUid"></param>
        /// <param name="templateContent"></param>
        /// <param name="productUid"></param>
        /// <returns></returns>
        public bool SaveTemplateContent(string languageCode, string regionUid, int companyId, string templateUid, string templateContent,string fileName, int updatedBy, string productUid)
        {
            if (string.IsNullOrEmpty(productUid))
                return false;
            System.Collections.ObjectModel.Collection<DBParameters> parameters = new System.Collections.ObjectModel.Collection<DBParameters>();
            parameters.Add(new DBParameters() { Name = "@template_uid", Value = templateUid, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@language_code", Value = languageCode, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@company_id", Value = companyId, DBType = DbType.Int32 });
            parameters.Add(new DBParameters() { Name = "@template_content", Value = templateContent, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@file_name", Value = fileName, DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@updated_by", Value = updatedBy, DBType = DbType.Int32 });

            if (!string.IsNullOrEmpty(regionUid))
                parameters.Add(new DBParameters() { Name = "@region_uid", Value = regionUid, DBType = DbType.AnsiString });
            else
                parameters.Add(new DBParameters() { Name = "@region_uid", Value = "-1", DBType = DbType.AnsiString });

            this.ExecuteProcedure("[doc].[templates_text_update]", ExecuteType.ExecuteScalar, parameters, productUid);
            return true;
        }

        /// <summary>
        /// Get Documet Template Content By Template Name from the database.        
        /// </summary>
        /// <param name="templateName"></param>
        /// <param name="languageCode"></param>
        /// <param name="productUid"></param>
        /// <returns></returns>
        public virtual IList<DocumentTemplateModel> GetDocumetTemplateContentByTemplateName(string templateName, string languageCode, string country_code, string productUid, int companyId, string tamplateUid = null,string region_uid= null)
        {
            if (string.IsNullOrEmpty(productUid))
                return new List<DocumentTemplateModel>();

            System.Collections.ObjectModel.Collection<DBParameters> parameters = new System.Collections.ObjectModel.Collection<DBParameters>();

            if (!string.IsNullOrEmpty(languageCode))
                parameters.Add(new DBParameters() { Name = "@language_code", Value = languageCode, DBType = DbType.AnsiString });

            if (!string.IsNullOrEmpty(languageCode))
                parameters.Add(new DBParameters() { Name = "@template_uid", Value = tamplateUid, DBType = DbType.AnsiString });

            if (!string.IsNullOrEmpty(templateName))
                parameters.Add(new DBParameters() { Name = "@template_name", Value = templateName, DBType = DbType.AnsiString });

            if (!string.IsNullOrEmpty(country_code))
                parameters.Add(new DBParameters() { Name = "@country_code", Value = country_code, DBType = DbType.AnsiString });

            if (!string.IsNullOrEmpty(region_uid))
                parameters.Add(new DBParameters() { Name = "@region_uid", Value = region_uid, DBType = DbType.AnsiString });

            parameters.Add(new DBParameters() { Name = "@company_id", Value = companyId, DBType = DbType.AnsiString });


            return this.ExecuteProcedure<DocumentTemplateModel>("[doc].[templates_text_get]", parameters, productUid).ToList();
        }        

        /// <summary>
        /// Fetch Template list from database
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="language"></param>
        /// <param name="documentTypeId"></param>
        /// <param name="productUid"></param>
        /// <returns></returns>
        public IList<DocumentTemplateModel> GetTemplateList(string language, string regionUid, int documentTypeId, int companyId, string productUid)
        {
            //if (string.IsNullOrEmpty(productUid))
            //    return new List<DocumentTemplateModel>();

            Collection<DBParameters> parameters = new Collection<DBParameters>();

            if (!string.IsNullOrEmpty(language))
                parameters.Add(new DBParameters() { Name = "language_code", Value = language, DBType = DbType.AnsiString });

            if (!string.IsNullOrEmpty(regionUid))
                parameters.Add(new DBParameters() { Name = "region_uid", Value = regionUid, DBType = DbType.AnsiString });

            if (documentTypeId > 0)
                parameters.Add(new DBParameters() { Name = "document_type_id", Value = documentTypeId, DBType = DbType.Int32 });

            parameters.Add(new DBParameters() { Name = "company_id", Value = companyId, DBType = DbType.Int32 });
            return this.ExecuteProcedure<DocumentTemplateModel>("[doc].[document_templatelist_get]", parameters).ToList();
            //return this.ExecuteProcedure<DocumentTemplateModel>("[doc].[document_templatelist_get]", parameters, productUid).ToList();
        }

        public virtual IList<SelectListItem> GetCountryListForDropdown(string productId)
        {
            return ExecuteProcedurewithoutPagination<SelectListItem>("[doc].[country_get]");
            //return ExecuteProcedurewithoutPagination<SelectListItem>("[doc].[country_get]", productId);
        }

        public virtual IList<SelectListItem> GetRegionListForDropdown(string productId)
        {
            return ExecuteProcedureWithPerameterwithoutPagination<SelectListItem>("[regnl].[templete_region_get]", null);
            //return ExecuteProcedureWithPerameterwithoutPagination<SelectListItem>("[regnl].[templete_region_get]", null, productId);
        }


        public virtual IList<SelectListItem> GetLanguageListForDropdown(string productId)
        {
            return ExecuteProcedureWithPerameterwithoutPagination<SelectListItem>("[doc].[language_get]", null);
            //return ExecuteProcedureWithPerameterwithoutPagination<SelectListItem>("[doc].[language_get]", null, productId);
        }


       
        public string GetFormatedAmount(decimal? amount, string culture = "en-US")
        {
            if (amount != null)
            {
                return string.Format(System.Globalization.CultureInfo.GetCultureInfo(culture), "{0:C}", amount.Value).Replace("$", "");
            }
            return "0.00";
        }

        public virtual IList<SelectListItem> GetBrokerCompaniesHasTemplate(int documentTypeId, string languageCode, string regionUid, string productUid)
        {
            if (string.IsNullOrEmpty(productUid))
                return null;

            Collection<DBParameters> parameters = new Collection<DBParameters>();
            parameters.Add(new DBParameters() { Name = "@document_type_id", Value = documentTypeId, DBType = DbType.Int32 });
            parameters.Add(new DBParameters() { Name = "@language_code", Value = !string.IsNullOrEmpty(languageCode) ? languageCode : "-1", DBType = DbType.AnsiString });
            parameters.Add(new DBParameters() { Name = "@region_uid", Value = !string.IsNullOrEmpty(regionUid) ? regionUid : "-1", DBType = DbType.AnsiString });
            return ExecuteProcedureWithPerameterwithoutPagination<SelectListItem>("[doc].[template_broker_company_dropdown_get]", parameters, productUid);
        }
    }
}
