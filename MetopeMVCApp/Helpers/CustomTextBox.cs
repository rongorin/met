using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomHelpers
{
 public static class CustomTextBox
    {
          public static string TextBox(string name, string value)
     {       // to use: @CustomTextBox.TextBox("Manager", "Manager") 
                 return String.Format("<input id='{0}' name='{0}' value='{1}' type='text' />", name, value);
                  //
          }
     }
}