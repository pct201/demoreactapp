using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ICSB.Business.Models
{
    
    public class UserModel 
    {
        public UserModel()
        {
            EducationList = new List<SelectListItem>();
        }

        #region Properties
            
        public int UserId { get; set; }  

        public string Email { get; set; }
        
        public string First_name { get; set; }
       
        public string Last_name { get; set; }

        public string Mobile_Number { get; set; }

        public int Education_Id { get; set; }

        public string Education_Name { get; set; }

        public decimal Salary { get; set; }

        public DateTime Birth_Date { get; set; }

        public bool Is_Married { get; set; }

        public string Address { get; set; }

        public string Blog { get; set; }

        public Byte[] Profile_Picture { get; set; }

        public Byte[] Document { get; set; }

        public string Document_Name { get; set; }
         
        public int Created_by { get; set; }
       
        public int? Updated_by { get; set; }
      
        public DateTime Created_date { get; set; }

        public DateTime? Updated_date { get; set; }

        public IList<SelectListItem> EducationList { get; set; }

        public int total_records { get; set; }

        #endregion
    }
}
