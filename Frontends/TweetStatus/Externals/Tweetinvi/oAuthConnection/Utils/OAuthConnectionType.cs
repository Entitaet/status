﻿using System.Runtime.Serialization;

namespace oAuthConnection.Utils
{
    /// <summary>
    /// Provide different solution to connect to an url
    /// </summary>
    [DataContract]
    public enum OAuthConnectionType
    {
        [EnumMember]
        AuthorizationHeaders,
        [EnumMember]
        BaseHtmlString
    }
}
