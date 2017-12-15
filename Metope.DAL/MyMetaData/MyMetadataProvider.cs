﻿using System;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Text;
namespace MetopeMVCApp.Models.MyMetaData
{
    public class MyMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);

            if (metadata.DisplayName == null)
                metadata.DisplayName = GetDisplayNameFromDBName(propertyName);

            return metadata;
        }

     private string GetDisplayNameFromDBName(string propertyName)
    {  
        StringBuilder b = new StringBuilder(propertyName);
        b.Replace("_", " ");
        return b.ToString();

    }
    }
}