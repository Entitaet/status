﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18052
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TwitterToken.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("TwitterToken.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to http://api.twitter.com/oauth/access_token.
        /// </summary>
        internal static string OAuthRequestAccessToken {
            get {
                return ResourceManager.GetString("OAuthRequestAccessToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string OAuthRequestAccessTokenekq {
            get {
                return ResourceManager.GetString("OAuthRequestAccessTokenekq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/access_token?x_auth_password ={0}x_auth_password={1}&amp;x_auth_mode=client_auth.
        /// </summary>
        internal static string OAuthRequestAccessTokenWithUserCredentials {
            get {
                return ResourceManager.GetString("OAuthRequestAccessTokenWithUserCredentials", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://api.twitter.com/oauth/authorize.
        /// </summary>
        internal static string OAuthRequestAuthorize {
            get {
                return ResourceManager.GetString("OAuthRequestAuthorize", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/oauth/request_token.
        /// </summary>
        internal static string OAuthRequestToken {
            get {
                return ResourceManager.GetString("OAuthRequestToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to validate oauth signature and token.
        /// </summary>
        internal static string OAuthRequestTokenFailedValidation {
            get {
                return ResourceManager.GetString("OAuthRequestTokenFailedValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to oauth_token=(?&lt;oauth_token&gt;(?:\w|\-)*)&amp;oauth_token_secret=(?&lt;oauth_token_secret&gt;(?:\w)*)&amp;user_id=(?&lt;user_id&gt;(?:\d)*)&amp;screen_name=(?&lt;screen_name&gt;(?:\w)*).
        /// </summary>
        internal static string OAuthTokenAccessRegex {
            get {
                return ResourceManager.GetString("OAuthTokenAccessRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to oauth_token=(?&lt;oauth_token&gt;(?:\w|\-)*)&amp;oauth_token_secret=(?&lt;oauth_token_secret&gt;(?:\w)*)&amp;oauth_callback_confirmed=(?&lt;oauth_callback_confirmed&gt;(?:\w)*).
        /// </summary>
        internal static string OAuthTokenRequestRegex {
            get {
                return ResourceManager.GetString("OAuthTokenRequestRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://api.twitter.com/1.1/application/rate_limit_status.json.
        /// </summary>
        internal static string QueryRateLimit {
            get {
                return ResourceManager.GetString("QueryRateLimit", resourceCulture);
            }
        }
    }
}
