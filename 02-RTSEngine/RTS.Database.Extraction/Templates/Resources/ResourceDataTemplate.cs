﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime : 16.0.0.0
//  
//     Les changements apportés à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace RTS.Database.Extraction
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System.Globalization;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    
    #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public partial class ResourceDataTemplate : ResourceDataTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            this.Write("\n");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\../BaseTemplate.tt"

string ToPascalCase(string text)
{
    return CultureInfo.InvariantCulture.TextInfo
        .ToTitleCase(text.ToLowerInvariant())
        .Replace("-", 
		"")
        .Replace("_", "")
        .Replace(" ", "");
}

string ToString(bool pValue)
{
    if (pValue == true)
        return "true";
    else
        return "false";
}



            
            #line default
            #line hidden
            this.Write("\n");
            this.Write("\nusing System.Collections.Generic;\n\nnamespace RTS.Models\n{\n    public static clas" +
                    "s ResourceData\n    {\n\t\tpublic static List<ResourceModel> GetAllResources(){\n\t\t\n\t" +
                    "\t\treturn new List<ResourceModel>{ \n");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
 for (int i = 0; i < Resources.Count; i++) { 
            
            #line default
            #line hidden
            this.Write("\n\t\t\t\tResourceData.");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ToPascalCase(Resources[i].name)));
            
            #line default
            #line hidden
            this.Write(",\n");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\t\t\t\n\t\t\t};\n\t\t}\n\n\t\tpublic static ResourceModel GetResourceById(int pId)\n\t\t{\n\t\t\tswi" +
                    "tch (pId)\n\t\t\t{\n");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
 for (int i = 0; i < Resources.Count; i++) { 
            
            #line default
            #line hidden
            this.Write("\n\t\t\t\tcase ");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Resources[i].id));
            
            #line default
            #line hidden
            this.Write(" :\n\t\t\t\t\treturn ResourceData.");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ToPascalCase(Resources[i].name)));
            
            #line default
            #line hidden
            this.Write(";\n");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\n\t\t\t\tdefault:\n\t\t\t\t\treturn null;\n\t\t\t}\n        }\n\n");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
 for (int i = 0; i < Resources.Count; i++) { 
            
            #line default
            #line hidden
            this.Write("\n\t\tpublic static ResourceModel ");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(ToPascalCase(Resources[i].name)));
            
            #line default
            #line hidden
            this.Write("\n        {\n            get { return new ResourceModel{\n                id = ");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Resources[i].id));
            
            #line default
            #line hidden
            this.Write(",\n                name = \"");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Resources[i].name));
            
            #line default
            #line hidden
            this.Write("\",\n                AvailableInPlayerMap = ");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Resources[i].AvailableInPlayerMap.ToString().ToLower()));
            
            #line default
            #line hidden
            this.Write(",\n                type = new ResourceTypeModel {\n                    id = ");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Resources[i].type.id));
            
            #line default
            #line hidden
            this.Write(",\n                    name = \"");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(Resources[i].type.name));
            
            #line default
            #line hidden
            this.Write("\"\n                    }\n                };\n            }\n        }\n\n");
            
            #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"
 } 
            
            #line default
            #line hidden
            this.Write("\t\n\t\t\n\t}\n}");
            return this.GenerationEnvironment.ToString();
        }
        
        #line 1 "D:\01-Projets\Unity\RTSOnline-Prototype\02-Database\RTS.Database.Extraction\Templates\Resources\ResourceDataTemplate.tt"

private global::System.Collections.Generic.List<RTS.Models.ResourceModel> _ResourcesField;

/// <summary>
/// Access the Resources parameter of the template.
/// </summary>
private global::System.Collections.Generic.List<RTS.Models.ResourceModel> Resources
{
    get
    {
        return this._ResourcesField;
    }
}


/// <summary>
/// Initialize the template
/// </summary>
public virtual void Initialize()
{
    if ((this.Errors.HasErrors == false))
    {
bool ResourcesValueAcquired = false;
if (this.Session.ContainsKey("Resources"))
{
    this._ResourcesField = ((global::System.Collections.Generic.List<RTS.Models.ResourceModel>)(this.Session["Resources"]));
    ResourcesValueAcquired = true;
}
if ((ResourcesValueAcquired == false))
{
    object data = global::System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("Resources");
    if ((data != null))
    {
        this._ResourcesField = ((global::System.Collections.Generic.List<RTS.Models.ResourceModel>)(data));
    }
}


    }
}


        
        #line default
        #line hidden
    }
    
    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "16.0.0.0")]
    public class ResourceDataTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
