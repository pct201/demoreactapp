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
using Newtonsoft.Json.Linq;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        #region Template List   

        [HttpGet]
        public IList<DocumentTemplateModel> BindTemplateList(string language, string regionUid, int documentTypeId, string product_uid, int companyId = -1)
        {
            try
            {
                using (var documentService = new DocumentTemplateService())
                {
                    IList<DocumentTemplateModel> documentList = documentService.GetTemplateList(language, regionUid, documentTypeId, companyId, product_uid);
                    return documentList;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpGet]
        public IList<DocumentTypeTemplateModel> BindDocumentTypeDDL(string productUid = null)
        {
            using (var documentService = new DocumentTemplateService())
                return documentService.GetDocumentTypeListForDropdown(productUid);
          
        }
        
        [HttpGet]
        public IList<SelectListItem> BindRegionDDL(string productUid)
        {
            using (var documentService = new DocumentTemplateService())          
                return documentService.GetRegionListForDropdown(productUid);            
        }

        [HttpGet]
        public IList<SelectListItem> BindLanguageDDL(string productUid)
        {
            using (var documentService = new DocumentTemplateService())           
                return documentService.GetLanguageListForDropdown(productUid);          
        }

        public List<SelectListItem> BindBrokerCompaniesDDL(int documentTypeId, string languageCode, string regionUid, string productUid)
        {
            using (var documentService = new DocumentTemplateService())
                return new List<SelectListItem>() ;           
        }
        #endregion

        #region Edit Template

        [HttpPost]
        public bool EditTemplate([FromBody]JObject templateObj)
        {
            try
            {
                using (var documentService = new DocumentTemplateService())
                    return documentService.SaveTemplateContent(Convert.ToString(templateObj["languageCode"]), Convert.ToString(templateObj["regionUid"]), Convert.ToInt32(templateObj["companyId"]), Convert.ToString(templateObj["templateUid"]), WebUtility.HtmlEncode(Convert.ToString(templateObj["templateContent"])), Convert.ToString(templateObj["fileName"]), 1, null);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Download Template

        public string Download(string templateId, string language, string regionUid, int companyId, string productUid)
        {
            using (var documentService = new DocumentTemplateService())
            {
                DocumentTemplateModel objTemplatetextModel = documentService.GetDocumetTemplateContentByTemplateName(null, language, null, productUid, companyId, templateId, regionUid).FirstOrDefault();
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
            using (var documentService = new DocumentTemplateService())
            {
                DocumentTemplateModel objTemplatetextModel = documentService.GetDocumetTemplateContentByTemplateName(null, language, null, productUid, companyId, templateId, regionUid).FirstOrDefault();
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