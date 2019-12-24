using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ICSB.Business.Models;
using ICSB.Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        #region Template List   

        [HttpGet]
        public IList<DocumentTemplateModel> BindTemplateList(string language, string regionUid, int documentTypeId, string product_uid, int companyId = -1)
        {
            try
            {
                using (var documetnService = new DocumentTemplateService())
                {
                    IList<DocumentTemplateModel> documentList = documetnService.GetTemplateList(language, regionUid, documentTypeId, companyId, product_uid);
                    return documentList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public IList<SelectListItem> BindRegionDDL(string productUid)
        {
            using (var documetnService = new DocumentTemplateService())          
                return documetnService.GetRegionListForDropdown(productUid);            
        }

        [HttpGet]
        public IList<SelectListItem> BindLanguageDDL(string productUid)
        {
            using (var documetnService = new DocumentTemplateService())           
                return documetnService.GetLanguageListForDropdown(productUid);          
        }

        public List<SelectListItem> BindBrokerCompaniesDDL(int documentTypeId, string languageCode, string regionUid, string productUid)
        {
            using (var documetnService = new DocumentTemplateService())
                return documetnService.GetBrokerCompaniesHasTemplate(documentTypeId, languageCode, regionUid, productUid).ToList();           
        }
        #endregion

        #region Edit Template

        [HttpPost]
        [AllowAnonymous]      
        [ValidateAntiForgeryToken]
        public void EditTemplate(string languageCode, string regionUid, int companyId, string templateUid, string templateContent, string fileName, int updatedBy=1, string productUid=null)
        {
            try
            {
                using (var documetnService = new DocumentTemplateService())
                {
                    documetnService.SaveTemplateContent(languageCode, regionUid, companyId, templateUid, templateContent, fileName, updatedBy, productUid);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        #endregion

        #region Download Template

        public string Download(string templateId, string language, string regionUid, int companyId, string productUid)
        {
            using (var documetnService = new DocumentTemplateService())
            {
                DocumentTemplateModel objTemplatetextModel = documetnService.GetDocumetTemplateContentByTemplateName(null, language, null, productUid, companyId, templateId, regionUid).FirstOrDefault();
                if (objTemplatetextModel != null && objTemplatetextModel.template_content != null)
                {                   
                    string templatefilename = !string.IsNullOrEmpty(objTemplatetextModel.Template_name) ? objTemplatetextModel.Template_name + ".html"  : "output_document_template.html";
                    string templateContent = WebUtility.HtmlDecode(objTemplatetextModel.template_content);
                    var result = new { filename = templatefilename, contentType = "application/force-download", templateContent = WebUtility.HtmlDecode(objTemplatetextModel.template_content)};
                    return JsonConvert.SerializeObject(result);
                }
                else
                    return JsonConvert.SerializeObject("");
            }
        }

        #endregion

        #region View Template
        public string GetTemplateTextByTemplateId(string templateId, string language, string regionUid, int companyId, string productUid)
        {
            using (var documetnService = new DocumentTemplateService())
            {
                DocumentTemplateModel objTemplatetextModel = documetnService.GetDocumetTemplateContentByTemplateName(null, language, null, productUid, companyId, templateId, regionUid).FirstOrDefault();
                if (objTemplatetextModel != null && !string.IsNullOrEmpty(objTemplatetextModel.template_content))
                {
                    var result = new { TemplateContent = WebUtility.HtmlDecode(objTemplatetextModel.template_content), errormsg = "" };
                    return JsonConvert.SerializeObject(result);
                }
                else
                {
                    var result = new { TemplateContent = "", errormsg = "No template Content found please upload content first." };
                    return JsonConvert.SerializeObject(result);
                }
            }               
        }
        #endregion
    }
}