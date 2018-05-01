using Metope.DAL;
 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Metope.DAL;
namespace MetopeMVCApp.Validators
{
    //Custom Validate .(see Scott Gu psight vid Mvc4)

    public class ValidatePriceCurrAttribute :  ValidationAttribute
    {
        MetopeDbEntities db = new MetopeDbEntities();

           protected override ValidationResult IsValid(
                                        object value, ValidationContext validationContext)  // 'value' will be the attribute
            {
                if (value != null)  // now inspect that value
                { 
                    var model = (Security_Dividend_Detail)validationContext.ObjectInstance;  

                    //now get the SecDetail record to compare the priceCurr. :

                    //var sD = from p in db.Security_Detail
                    //           where p.Security_ID == model.Security_ID 
                    //           select p; 
                    var sD = db.Security_Detail.Where(w => w.Security_ID ==  model.Security_ID ).FirstOrDefault<Security_Detail>();


                    if (sD.Price_Curr ==  Convert.ToString(value) )
                    {
                        if (model.Actual_FX_Rate != 1)
                            return new ValidationResult("As the Currency is the same as the security's, set Actual FX Rate to 1");
                    }

                }

                return ValidationResult.Success
                    ;
            }    
    }
}