﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StockingSelector.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("StockingSelector.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;!DOCTYPE HTML&gt;
        ///&lt;html&gt;
        ///&lt;head&gt;
        ///  &lt;title&gt;Stocking Assignment: {0} has {1}&lt;/title&gt;
        ///
        ///  &lt;meta http-equiv=&quot;content-type&quot; content=&quot;text/html;&quot; /&gt;
        ///  &lt;meta charset=&quot;UTF-8;&quot; /&gt;
        ///  &lt;meta name=&quot;author&quot; content=&quot;Robert Miller&quot;&gt;
        ///  &lt;meta name=&quot;application-name&quot; content=&quot;Stocking Selector&quot;&gt;
        ///  &lt;style type=&quot;text/css&quot;&gt;
        ///    @import url(&apos;https://fonts.googleapis.com/css?family=Lato|Open+Sans&apos;);
        ///
        ///    a { color: #3BBECE; }
        ///
        ///    body {
        ///      background-color: #EFEFEF;
        ///      color: #606060;
        ///      font-family: &apos;Open  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string EmailTemplate {
            get {
                return ResourceManager.GetString("EmailTemplate", resourceCulture);
            }
        }
    }
}
