using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientDependency.Core;
using Umbraco.Core.PropertyEditors;
using Umbraco.Web.PropertyEditors;

namespace Aubergine.StyledTextbox
{
    [PropertyEditorAsset(ClientDependencyType.Css, "~/App_Plugins/StyledTextbox/styledText.css")]
    [PropertyEditorAsset(ClientDependencyType.Javascript, "~/App_Plugins/StyledTextbox/styledTextController.js")]
    [PropertyEditor("Styled.TextBox", "Styled TextBox", "~/App_Plugins/StyledTextbox/styledText.html", 
        Icon = "icon-brush", ValueType = "text", Group = "common", IsParameterEditor = true)]
    public class StyledTextboxPropertyEditor : PropertyEditor
    {
        private IDictionary<string, object> _defaultPreValues; 
        public override IDictionary<string, object> DefaultPreValues
        {
            get { return _defaultPreValues; }
            set { _defaultPreValues = value; }
        }

        public StyledTextboxPropertyEditor()
        {
            // default values
            _defaultPreValues = new Dictionary<string, object>
            {
                { "multiLine", false },
                { "style", "font-size: 36px;\nline-height; 45px;\nfont-weight: bold;\n" },
                { "cssclass", ""},
                { "charCount", 0 },
                { "enforceLimit", false },
                { "hideLable", 0 }
            };
        }


        protected override PreValueEditor CreatePreValueEditor()
        {

            return new StyledTextboxPreValueEditor();
        }
    }

    public class StyledTextboxPreValueEditor : PreValueEditor
    {
        [PreValueField("style", "Style", "textarea", 
            Description = "Addtional styles to apply")]
        public string Style { get; set; }

        [PreValueField("cssclass", "Css Class", "textstring", 
            Description = "CSS classes to apply")]
        public string CssClass { get; set; }

        [PreValueField("multiLine", "Multi-line", "boolean",
            Description = "Multiline textbox (textarea)")]
        public bool MultiLine { get; set; }

        [PreValueField("placeholder", "Placeholder", "textstring", 
            Description = "Placeholder text to put in box when empty")]
        public string Placeholder { get; set; }

        [PreValueField("charCount", "Character Count", "number",
            Description = "Number fo characters before warning/limit is reached")]
        public int MaxChars { get; set; }

        [PreValueField("enforceLimit", "Enforce Limit", "boolean",
            Description = "Don't let the editor go beyond the character count")]
        public bool EnforceLimit { get; set; }

        [PreValueField("hideLabel", "Full width", "boolean",
            Description = "Make the box go right across the editor (like a grid control does)")]
        public bool FullWidth { get; set; }
    }

}
