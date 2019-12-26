using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ICSB.Business.Models
{
   
    public class DocumentTypeTemplateModel 
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Template_uid value.
        /// </summary>
        [Required(ErrorMessage = "*")]
        public int document_type_id { get; set; }

        /// <summary>
        /// Gets or sets the language_code value.
        /// </summary>
        [Required(ErrorMessage = "*")]       
        public string document_type_name { get; set; }

        /// <summary>
        /// Gets or sets the is_country_specific value.
        /// </summary>       
        public bool is_country_specific { get; set; }

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

        #endregion

    }
}
