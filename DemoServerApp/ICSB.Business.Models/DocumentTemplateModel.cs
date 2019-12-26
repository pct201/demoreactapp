using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICSB.Business.Models
{
   
    public class DocumentTemplateModel 
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Template_uid value.
        /// </summary>
        [Required(ErrorMessage = "*")]
        public string Template_uid { get; set; }

        /// <summary>
        /// Gets or sets the Category_uid value.
        /// </summary>
        [Required(ErrorMessage = "*")]       
        public string Category_uid { get; set; }

        /// <summary>
        /// Gets or sets the Category_uid value.
        /// </summary>
        /// 
       [NotMapped]
        public string Category_name { get; set; }

        /// <summary>
        /// Gets or sets the Document_type_id value.
        /// </summary>
        [Required(ErrorMessage = "*")]
        public int Document_type_id { get; set; }

        /// <summary>
        /// Gets or sets the Template_name value.
        /// </summary>
        [Required(ErrorMessage = "*")]
        public string Template_name { get; set; }


        /// <summary>
        /// Gets or sets the File_Name value.
        /// </summary>
        [NotMapped]
        public string File_Name { get; set; }

        /// <summary>
        /// Gets or sets the template_content value.
        /// </summary>
        [NotMapped]
        public string template_content { get; set; }


        /// <summary>
        /// Gets or sets the Company_Id value.
        /// </summary>
        /// 
        [NotMapped]
        public int Company_Id { get; set; }

        /// <summary>
        /// Gets or sets the language_code value.
        /// </summary>
        [NotMapped]
        public string language_code { get; set; }

        /// <summary>
        /// Gets or sets the Created_by value.
        /// </summary>
        [Required(ErrorMessage = "*")]
        public int Created_by { get; set; }

        /// <summary>
        /// Gets or sets the Updated_by value.
        /// </summary>
        public int? Updated_by { get; set; }

        /// <summary>
        /// Gets or sets the Created_date value.
        /// </summary>
        [Required(ErrorMessage = "*")]
        public DateTime Created_date { get; set; }

        /// <summary>
        /// Gets or sets the Updated_date value.
        /// </summary>
        public DateTime? Updated_date { get; set; }

        public int total_records { get; set; }

        #endregion

    }
}
