using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MetopeMVCApp.Models.MyMetaData 
{
 

    public class SecurityDetailModelMetaData
    {
        [Required]
        [Display(Name = "Security Name")] 
        public object Security_Name { get; set; }

        [Required]
       // [RegularExpression(@"\d{6,10}", ErrorMessage = "Account # must be between 6 and 10 digits.")]
        [Display(Name = "Sec Type Code")]
        [StringLength(10, ErrorMessage =
                 "Security Type Code should be max 10 characters ")] 
        public object Security_Type_Code { get; set; }

        [Display(Name = "Issue Date")]
        public object Issue_Date { get; set; }

        [Required]
        [Display(Name = "Price Multiplier")]
        public object Price_Multiplier { get; set; }

                [StringLength(4, ErrorMessage =
                 "Secondary Exch should be max 4 characters ")]
        [Display(Name = "Secondary Exchange")]
        public object Secondary_Exch { get; set; }

        [Required] 
        [Display(Name = "Short Name")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Short name should be between 3 and 100 characters")]
        public object Short_Name { get; set; }

        [Required]
        [Display(Name = "Primary Exchange")]
        [StringLength(4, ErrorMessage =
                 "Prim Exch Code should be max 4 characters ")] 
        public string Primary_Exch { get; set; }
                 
        [Required]
        [Display(Name = "Country Domocile")]
        public string Country_Of_Domicile { get; set; }

        [Required]
        [Display(Name = "Country Risk")]
        public string Country_Of_Risk { get; set; }

        [Display(Name = "Income Frequency")]
        //[RegularExpression(@"^[\d]*$\d{3}", ErrorMessage = "Income frequency should be max 3 numbers")]   
        [Range(0,300)]
        public Nullable<decimal> Income_Frequency { get; set; }

        [Required]
        [Display(Name = "Issuer Code")]
        public string Issuer_Code { get; set; }
        [Required]
        [Display(Name = "Ultimate Issr Code")]
        public string Ultimate_Issuer_Code { get; set; }
        [Required]
        [Display(Name = "Asset Currency")]
        public string Asset_Currency { get; set; }
    
        [Display(Name = "Min Lot size")]
        public Nullable<decimal> Min_Lot_Size { get; set; }
        [Required]
        [Display(Name = "Decimal Precision")]
        public int Decimal_Precision { get; set; }
        [Required]
        [Display(Name = "Ave Price Rounding")]
        public int AvePrice_Rounding { get; set; }
      
        [Display(Name = "Maturity Date")]
        public Nullable<System.DateTime> Maturity_Date { get; set; }
        
        [Display(Name = "Coupon Rate")]
        public Nullable<decimal> Coupon_Rate { get; set; }
        [Required]
        [Display(Name = "Price Exchange")]
        public string Price_Exchange { get; set; }
        [Required]
  
        [Display(Name = "Trade Currency")]
        public string Trade_Currency { get; set; }
        [Required]
        [Display(Name = "Price Currency")]
        public string Price_Curr { get; set; }
        
        [Display(Name = "Currency Paircode")]
        public string Currency_Pair_Code { get; set; }
       
        [Display(Name = "Share Class")]
        public string Share_Class { get; set; }
        [Required]
        [Display(Name = "Current Market Price")]
        public decimal Current_Market_Price { get; set; }
        
        [Display(Name = "Index Type ")]
        public string Index_Type { get; set; }
      
        [Display(Name = "Clean PriceFormula ")]
        public string Clean_Price_Formula { get; set; }
         
        [Display(Name = "Accr Income Price Formula")]
        public string Accrued_Income_Price_Formula { get; set; }
         
        [Display(Name = "Odd 1st Coupon date ")]
        public Nullable<System.DateTime> Odd_First_Coupon_Date { get; set; }
         
        [Display(Name = "Odd Last Coupon date ")]
        public Nullable<System.DateTime> Odd_Last_Coupon_Date { get; set; }
         
        [Display(Name = "Coupon Anniversary")]
        public string Coupon_Anniversary_Indicator { get; set; }
         
        [Display(Name = "Track EOM Flag ")]
        public Nullable<bool> Track_EOM_Flag { get; set; }
         
        [Display(Name = "Next Coupon Date")]
        public Nullable<System.DateTime> Next_Coupon_Date { get; set; }
         
        [Display(Name = "Previous Coupon Date")]
        public Nullable<System.DateTime> Previous_Coupon_Date { get; set; }
         
        [Display(Name = "Payment Frequency ")]
        public string Payment_Frequency { get; set; }
         
        [Display(Name = "Coupon BusDay Adjust")]
        public string Coupon_BusDay_Adjustment { get; set; }
         
        [Display(Name = "Next Ex Div Date")]
        public Nullable<System.DateTime> Next_Ex_Div_Date { get; set; }
         
        [Display(Name = "Ex Div BusDay Adjust")]
        public string Ex_Div_BusDay_Adjustment { get; set; }
         
        [Display(Name = "Ex Div Period ")]
        public string Ex_Div_Period { get; set; }

        [Required] 
        public string Ticker { get; set; }
      
        [Display(Name = "Inet ID ")]
        public string Inet_ID { get; set; }
  
        [Display(Name = "Bloomberg ID ")]
        public string Bloomberg_ID { get; set; }
        [Required]
        [Display(Name = "External SecID ")]
        public string External_Sec_ID { get; set; }
   
        [Display(Name = "Reuters ID ")]
        public string Reuters_ID { get; set; }
  
        public string ISIN { get; set; }
     
        [Display(Name = " Call Account Flag")]
        public Nullable<bool> Call_Account_Flag { get; set; }
        [Required]
        [Display(Name = "System Locked ")]
        public bool System_Locked { get; set; }
      
        [Display(Name = "Last Update User ")]
        public string Last_Update_User { get; set; } 

        [Display(Name = "Last Update Date")]
        public System.DateTime Last_Update_Date { get; set; } 

        [Display(Name = "Security Status ")]
        public string Security_Status { get; set; }
 
        [Display(Name = "Benchmark Portfolio ")]
        public string Benchmark_Portfolio { get; set; }
  
    }
}