using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Depofis.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CascadeFieldAttribute : Attribute, IMetadataAware
    {
        public string ApiUrl { get; private set; }
        public string SelectSelector { get; private set; }
        public string InputSelector { get; private set; }
        public string FieldName { get; private set; }

        public CascadeFieldAttribute(string apiUrl, string selectSelector, string inputSelector, string fieldName)
        {
            ApiUrl = apiUrl;
            SelectSelector = selectSelector;
            InputSelector = inputSelector;
            FieldName = fieldName;
        }

        #region Script
        private const string ScriptText =
            "<script data-eval='true' type='text/javascript'>" +
                "jQuery(document).ready(function () {" +

                    "jQuery('{{1}}').change(function() {" +
                        "var self = this;" +
                        "var selectedValue = jQuery(self).val();" +

                        "jQuery('{{2}}').attr('disabled', 'disabled');" +
                        "jQuery('{{2}}').val('');" +

                        "if(selectedValue != null && selectedValue != '') {" +
                            "if(!isNaN(parseInt(selectedValue))) {" +
                                "jQuery.getJSON('{{0}}' + '/' + parseInt(selectedValue) + '?fieldName=' + '{{3}}', function(s) {" +
                                    "jQuery('{{2}}').val(s);" +
                                    "jQuery('{{2}}').removeAttr('disabled');" +
                                "});" +
                            "} else {" +
                                "jQuery('{{2}}').removeAttr('disabled');" +
                            "}" +
                        "}" +
                    "});" +
                "});" +
            "</script>";
        #endregion

        internal HttpContextBase Context
        {
            get { return new HttpContextWrapper(HttpContext.Current); }
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            var list = Context.Items["Scripts"] as IList<string> ?? new List<string>();

            var s = ScriptText
                .Replace("{{0}}", ApiUrl)
                .Replace("{{1}}", SelectSelector)
                .Replace("{{2}}", InputSelector)
                .Replace("{{3}}", FieldName);

            if (!list.Contains(s))
                list.Add(s);

            Context.Items["Scripts"] = list;
        }
    }
}
