﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace STFU.UploadLib.Communication.Youtube {
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
    internal class WebResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal WebResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("STFU.UploadLib.Communication.Youtube.WebResources", typeof(WebResources).Assembly);
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
        ///   Looks up a localized string similar to https://www.googleapis.com/oauth2/v4/token.
        /// </summary>
        internal static string AuthCodeAddress {
            get {
                return ResourceManager.GetString("AuthCodeAddress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to code={0}&amp;client_id={1}&amp;client_secret={2}&amp;redirect_uri={3}&amp;grant_type=authorization_code.
        /// </summary>
        internal static string AuthCodeContent {
            get {
                return ResourceManager.GetString("AuthCodeContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to POST.
        /// </summary>
        internal static string AuthCodeMethod {
            get {
                return ResourceManager.GetString("AuthCodeMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Authorization: Bearer {0}.
        /// </summary>
        internal static string AuthHeader {
            get {
                return ResourceManager.GetString("AuthHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to client_id={0}&amp;client_secret={1}&amp;refresh_token={2}&amp;grant_type=refresh_token.
        /// </summary>
        internal static string AuthRefreshContent {
            get {
                return ResourceManager.GetString("AuthRefreshContent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://accounts.google.com/o/oauth2/auth?client_id={0}&amp;redirect_uri={1}&amp;scope={2}&amp;response_type={3}&amp;approval_prompt=force&amp;access_type=offline.
        /// </summary>
        internal static string AuthRequestUrl {
            get {
                return ResourceManager.GetString("AuthRequestUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to code.
        /// </summary>
        internal static string AuthResponseType {
            get {
                return ResourceManager.GetString("AuthResponseType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://accounts.google.com/o/oauth2/revoke?token={0}.
        /// </summary>
        internal static string AuthRevokeAddress {
            get {
                return ResourceManager.GetString("AuthRevokeAddress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to content-range: bytes */{0}.
        /// </summary>
        internal static string CheckStatusContentRangeHeader {
            get {
                return ResourceManager.GetString("CheckStatusContentRangeHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 812042275170-db6cf7ujravcq2l7vhu7gb7oodgii3e4.apps.googleusercontent.com.
        /// </summary>
        internal static string ClientId {
            get {
                return ResourceManager.GetString("ClientId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to cKUCRQz0sE4UUmvUHW6qckbP.
        /// </summary>
        internal static string ClientSecret {
            get {
                return ResourceManager.GetString("ClientSecret", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to content-length: {0}.
        /// </summary>
        internal static string ContentLengthHeader {
            get {
                return ResourceManager.GetString("ContentLengthHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to content-type: {0}.
        /// </summary>
        internal static string ContentTypeHeader {
            get {
                return ResourceManager.GetString("ContentTypeHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to application/x-www-form-urlencoded.
        /// </summary>
        internal static string FormContentType {
            get {
                return ResourceManager.GetString("FormContentType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GET.
        /// </summary>
        internal static string GetChannelDetailsMethod {
            get {
                return ResourceManager.GetString("GetChannelDetailsMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.googleapis.com/youtube/v3/channels?part=snippet&amp;mine=true&amp;key={0}.
        /// </summary>
        internal static string GetChannelDetailsUrl {
            get {
                return ResourceManager.GetString("GetChannelDetailsUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GET.
        /// </summary>
        internal static string GetLanguagesMethod {
            get {
                return ResourceManager.GetString("GetLanguagesMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.googleapis.com/youtube/v3/i18nLanguages?part=snippet&amp;hl={1}&amp;key={0}.
        /// </summary>
        internal static string GetLanguagesUrl {
            get {
                return ResourceManager.GetString("GetLanguagesUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.googleapis.com/youtube/v3/playlistItems?part=contentDetails&amp;maxResults=50&amp;playlistId={0}{1}.
        /// </summary>
        internal static string GetPlaylistItemsUrl {
            get {
                return ResourceManager.GetString("GetPlaylistItemsUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GET.
        /// </summary>
        internal static string GetPlaylistMethod {
            get {
                return ResourceManager.GetString("GetPlaylistMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &amp;pageToken={0}.
        /// </summary>
        internal static string GetPlaylistTokenAddition {
            get {
                return ResourceManager.GetString("GetPlaylistTokenAddition", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GET.
        /// </summary>
        internal static string GetVideoCategoriesMethod {
            get {
                return ResourceManager.GetString("GetVideoCategoriesMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.googleapis.com/youtube/v3/videoCategories?part=snippet&amp;hl={2}&amp;regionCode={1}&amp;key={0}.
        /// </summary>
        internal static string GetVideoCategoriesUrl {
            get {
                return ResourceManager.GetString("GetVideoCategoriesUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to application/json; charset=utf-8.
        /// </summary>
        internal static string JSONContentType {
            get {
                return ResourceManager.GetString("JSONContentType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.google.com/accounts/Logout?continue={0}.
        /// </summary>
        internal static string LogoffAndContinueLink {
            get {
                return ResourceManager.GetString("LogoffAndContinueLink", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://localhost.
        /// </summary>
        internal static string RedirectUriAutoGet {
            get {
                return ResourceManager.GetString("RedirectUriAutoGet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to urn:ietf:wg:oauth:2.0:oob.
        /// </summary>
        internal static string RedirectUriManuGet {
            get {
                return ResourceManager.GetString("RedirectUriManuGet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Content-Range: bytes {0}-{1}/{2}.
        /// </summary>
        internal static string ResumeUploadContentRangeHeader {
            get {
                return ResourceManager.GetString("ResumeUploadContentRangeHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PUT.
        /// </summary>
        internal static string UploadMethod {
            get {
                return ResourceManager.GetString("UploadMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.googleapis.com/auth/youtube.upload+https://www.googleapis.com/auth/youtube.readonly.
        /// </summary>
        internal static string UploadScope {
            get {
                return ResourceManager.GetString("UploadScope", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to https://www.googleapis.com/upload/youtube/v3/videos?uploadType=resumable&amp;autoLevels={1}&amp;notifySubscribers={0}&amp;stabilize={2}&amp;part=snippet,status,contentDetails.
        /// </summary>
        internal static string UploadUrl {
            get {
                return ResourceManager.GetString("UploadUrl", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to POST.
        /// </summary>
        internal static string UpoadInitMethod {
            get {
                return ResourceManager.GetString("UpoadInitMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to video/*.
        /// </summary>
        internal static string VideoContentType {
            get {
                return ResourceManager.GetString("VideoContentType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to x-upload-content-length: {0}.
        /// </summary>
        internal static string XUploadContentLengthHeader {
            get {
                return ResourceManager.GetString("XUploadContentLengthHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to x-upload-content-type: {0}.
        /// </summary>
        internal static string XUploadContentTypeHeader {
            get {
                return ResourceManager.GetString("XUploadContentTypeHeader", resourceCulture);
            }
        }
    }
}
