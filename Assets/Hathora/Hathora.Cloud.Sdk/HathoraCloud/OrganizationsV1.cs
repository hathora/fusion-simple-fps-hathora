
//------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by Speakeasy (https://speakeasy.com). DO NOT EDIT.
//
// Changes to this file may cause incorrect behavior and will be lost when
// the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
#nullable enable
namespace HathoraCloud
{
    using HathoraCloud.Models.Errors;
    using HathoraCloud.Models.Operations;
    using HathoraCloud.Models.Shared;
    using HathoraCloud.Utils;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using System;
    using UnityEngine.Networking;

    public interface IOrganizationsV1
    {
        Task<AcceptInviteResponse> AcceptInviteAsync(AcceptInviteRequest request);
        Task<GetOrgMembersResponse> GetOrgMembersAsync(GetOrgMembersRequest request);
        Task<GetOrgPendingInvitesResponse> GetOrgPendingInvitesAsync(GetOrgPendingInvitesRequest request);

        /// <summary>
        /// Returns an unsorted list of all organizations that you are a member of (an accepted membership invite). An organization is uniquely identified by an `orgId`.
        /// </summary>
        Task<GetOrgsResponse> GetOrgsAsync();
        Task<GetUserPendingInvitesResponse> GetUserPendingInvitesAsync();
        Task<InviteUserResponse> InviteUserAsync(InviteUserRequest request);
        Task<RejectInviteResponse> RejectInviteAsync(RejectInviteRequest request);
        Task<RescindInviteResponse> RescindInviteAsync(RescindInviteRequest request);
    }

    public class OrganizationsV1: IOrganizationsV1
    {
        public SDKConfig SDKConfiguration { get; private set; }
        private const string _target = "unity";
        private const string _sdkVersion = "0.30.0";
        private const string _sdkGenVersion = "2.409.3";
        private const string _openapiDocVersion = "0.0.1";
        private const string _userAgent = "speakeasy-sdk/unity 0.30.0 2.409.3 0.0.1 HathoraCloud";
        private string _serverUrl = "";
        private ISpeakeasyHttpClient _defaultClient;
        private Func<Security>? _securitySource;

        public OrganizationsV1(ISpeakeasyHttpClient defaultClient, Func<Security>? securitySource, string serverUrl, SDKConfig config)
        {
            _defaultClient = defaultClient;
            _securitySource = securitySource;
            _serverUrl = serverUrl;
            SDKConfiguration = config;
        }
        

        
        public async Task<AcceptInviteResponse> AcceptInviteAsync(AcceptInviteRequest request)
        {
            string baseUrl = this.SDKConfiguration.GetTemplatedServerDetails();
            var urlString = URLBuilder.Build(baseUrl, "/orgs/v1/{orgId}/invites/accept", request);

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbPOST);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);

            var client = _defaultClient;
            if (_securitySource != null)
            {
                client = SecuritySerializer.Apply(_defaultClient, _securitySource);
            }

            var httpResponse = await client.SendAsync(httpRequest);
            int? errorCode = null;
            string? contentType = null;
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    errorCode = (int)httpRequest.responseCode;
                    contentType = httpRequest.GetResponseHeader("Content-Type");
                    httpRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Console.WriteLine("Success");
                    break;
            }

            if (contentType == null)
            {
                contentType = httpResponse.GetResponseHeader("Content-Type") ?? "application/octet-stream";
            }
            int httpCode = errorCode ?? (int)httpResponse.responseCode;
            var response = new AcceptInviteResponse
            {
                StatusCode = httpCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            if (httpCode == 204)
            {
            }
            else if (new List<int>{401, 404, 429}.Contains(httpCode))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    throw obj!;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (httpCode >= 400 && httpCode < 500 || httpCode >= 500 && httpCode < 600)
            {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            else
            {
                throw new SDKException("unknown status code received", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            return response;
        }

        

        
        public async Task<GetOrgMembersResponse> GetOrgMembersAsync(GetOrgMembersRequest request)
        {
            string baseUrl = this.SDKConfiguration.GetTemplatedServerDetails();
            var urlString = URLBuilder.Build(baseUrl, "/orgs/v1/{orgId}/members", request);

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbGET);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);

            var client = _defaultClient;
            if (_securitySource != null)
            {
                client = SecuritySerializer.Apply(_defaultClient, _securitySource);
            }

            var httpResponse = await client.SendAsync(httpRequest);
            int? errorCode = null;
            string? contentType = null;
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    errorCode = (int)httpRequest.responseCode;
                    contentType = httpRequest.GetResponseHeader("Content-Type");
                    httpRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Console.WriteLine("Success");
                    break;
            }

            if (contentType == null)
            {
                contentType = httpResponse.GetResponseHeader("Content-Type") ?? "application/octet-stream";
            }
            int httpCode = errorCode ?? (int)httpResponse.responseCode;
            var response = new GetOrgMembersResponse
            {
                StatusCode = httpCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            if (httpCode == 200)
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<OrgMembersPage>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    response.OrgMembersPage = obj;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (new List<int>{401, 429}.Contains(httpCode))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    throw obj!;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (httpCode >= 400 && httpCode < 500 || httpCode >= 500 && httpCode < 600)
            {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            else
            {
                throw new SDKException("unknown status code received", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            return response;
        }

        

        
        public async Task<GetOrgPendingInvitesResponse> GetOrgPendingInvitesAsync(GetOrgPendingInvitesRequest request)
        {
            string baseUrl = this.SDKConfiguration.GetTemplatedServerDetails();
            var urlString = URLBuilder.Build(baseUrl, "/orgs/v1/{orgId}/invites/pending", request);

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbGET);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);

            var client = _defaultClient;
            if (_securitySource != null)
            {
                client = SecuritySerializer.Apply(_defaultClient, _securitySource);
            }

            var httpResponse = await client.SendAsync(httpRequest);
            int? errorCode = null;
            string? contentType = null;
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    errorCode = (int)httpRequest.responseCode;
                    contentType = httpRequest.GetResponseHeader("Content-Type");
                    httpRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Console.WriteLine("Success");
                    break;
            }

            if (contentType == null)
            {
                contentType = httpResponse.GetResponseHeader("Content-Type") ?? "application/octet-stream";
            }
            int httpCode = errorCode ?? (int)httpResponse.responseCode;
            var response = new GetOrgPendingInvitesResponse
            {
                StatusCode = httpCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            if (httpCode == 200)
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<PendingOrgInvitesPage>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    response.PendingOrgInvitesPage = obj;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (new List<int>{401, 429}.Contains(httpCode))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    throw obj!;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (httpCode >= 400 && httpCode < 500 || httpCode >= 500 && httpCode < 600)
            {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            else
            {
                throw new SDKException("unknown status code received", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            return response;
        }

        

        
        public async Task<GetOrgsResponse> GetOrgsAsync()
        {
            string baseUrl = this.SDKConfiguration.GetTemplatedServerDetails();
            var urlString = baseUrl + "/orgs/v1";

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbGET);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);

            var client = _defaultClient;
            if (_securitySource != null)
            {
                client = SecuritySerializer.Apply(_defaultClient, _securitySource);
            }

            var httpResponse = await client.SendAsync(httpRequest);
            int? errorCode = null;
            string? contentType = null;
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    errorCode = (int)httpRequest.responseCode;
                    contentType = httpRequest.GetResponseHeader("Content-Type");
                    httpRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Console.WriteLine("Success");
                    break;
            }

            if (contentType == null)
            {
                contentType = httpResponse.GetResponseHeader("Content-Type") ?? "application/octet-stream";
            }
            int httpCode = errorCode ?? (int)httpResponse.responseCode;
            var response = new GetOrgsResponse
            {
                StatusCode = httpCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            if (httpCode == 200)
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<OrgsPage>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    response.OrgsPage = obj;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (new List<int>{401, 404, 429}.Contains(httpCode))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    throw obj!;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (httpCode >= 400 && httpCode < 500 || httpCode >= 500 && httpCode < 600)
            {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            else
            {
                throw new SDKException("unknown status code received", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            return response;
        }

        

        
        public async Task<GetUserPendingInvitesResponse> GetUserPendingInvitesAsync()
        {
            string baseUrl = this.SDKConfiguration.GetTemplatedServerDetails();
            var urlString = baseUrl + "/orgs/v1/user/invites/pending";

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbGET);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);

            var client = _defaultClient;
            if (_securitySource != null)
            {
                client = SecuritySerializer.Apply(_defaultClient, _securitySource);
            }

            var httpResponse = await client.SendAsync(httpRequest);
            int? errorCode = null;
            string? contentType = null;
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    errorCode = (int)httpRequest.responseCode;
                    contentType = httpRequest.GetResponseHeader("Content-Type");
                    httpRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Console.WriteLine("Success");
                    break;
            }

            if (contentType == null)
            {
                contentType = httpResponse.GetResponseHeader("Content-Type") ?? "application/octet-stream";
            }
            int httpCode = errorCode ?? (int)httpResponse.responseCode;
            var response = new GetUserPendingInvitesResponse
            {
                StatusCode = httpCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            if (httpCode == 200)
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<PendingOrgInvitesPage>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    response.PendingOrgInvitesPage = obj;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (new List<int>{401, 429}.Contains(httpCode))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    throw obj!;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (httpCode >= 400 && httpCode < 500 || httpCode >= 500 && httpCode < 600)
            {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            else
            {
                throw new SDKException("unknown status code received", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            return response;
        }

        

        
        public async Task<InviteUserResponse> InviteUserAsync(InviteUserRequest request)
        {
            string baseUrl = this.SDKConfiguration.GetTemplatedServerDetails();
            var urlString = URLBuilder.Build(baseUrl, "/orgs/v1/{orgId}/invites", request);

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbPUT);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);

            var serializedBody = RequestBodySerializer.Serialize(request, "CreateUserInvite", "json", false, false);
            if (serializedBody != null)
            {
                httpRequest.uploadHandler = new UploadHandlerRaw(serializedBody.Body);
                httpRequest.SetRequestHeader("Content-Type", serializedBody.ContentType);
            }

            var client = _defaultClient;
            if (_securitySource != null)
            {
                client = SecuritySerializer.Apply(_defaultClient, _securitySource);
            }

            var httpResponse = await client.SendAsync(httpRequest);
            int? errorCode = null;
            string? contentType = null;
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    errorCode = (int)httpRequest.responseCode;
                    contentType = httpRequest.GetResponseHeader("Content-Type");
                    httpRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Console.WriteLine("Success");
                    break;
            }

            if (contentType == null)
            {
                contentType = httpResponse.GetResponseHeader("Content-Type") ?? "application/octet-stream";
            }
            int httpCode = errorCode ?? (int)httpResponse.responseCode;
            var response = new InviteUserResponse
            {
                StatusCode = httpCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            if (httpCode == 200)
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<PendingOrgInvite>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    response.PendingOrgInvite = obj;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (new List<int>{401, 422, 429}.Contains(httpCode))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    throw obj!;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (httpCode >= 400 && httpCode < 500 || httpCode >= 500 && httpCode < 600)
            {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            else
            {
                throw new SDKException("unknown status code received", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            return response;
        }

        

        
        public async Task<RejectInviteResponse> RejectInviteAsync(RejectInviteRequest request)
        {
            string baseUrl = this.SDKConfiguration.GetTemplatedServerDetails();
            var urlString = URLBuilder.Build(baseUrl, "/orgs/v1/{orgId}/invites/reject", request);

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbPOST);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);

            var client = _defaultClient;
            if (_securitySource != null)
            {
                client = SecuritySerializer.Apply(_defaultClient, _securitySource);
            }

            var httpResponse = await client.SendAsync(httpRequest);
            int? errorCode = null;
            string? contentType = null;
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    errorCode = (int)httpRequest.responseCode;
                    contentType = httpRequest.GetResponseHeader("Content-Type");
                    httpRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Console.WriteLine("Success");
                    break;
            }

            if (contentType == null)
            {
                contentType = httpResponse.GetResponseHeader("Content-Type") ?? "application/octet-stream";
            }
            int httpCode = errorCode ?? (int)httpResponse.responseCode;
            var response = new RejectInviteResponse
            {
                StatusCode = httpCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            if (httpCode == 204)
            {
            }
            else if (new List<int>{401, 404, 429}.Contains(httpCode))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    throw obj!;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (httpCode >= 400 && httpCode < 500 || httpCode >= 500 && httpCode < 600)
            {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            else
            {
                throw new SDKException("unknown status code received", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            return response;
        }

        

        
        public async Task<RescindInviteResponse> RescindInviteAsync(RescindInviteRequest request)
        {
            string baseUrl = this.SDKConfiguration.GetTemplatedServerDetails();
            var urlString = URLBuilder.Build(baseUrl, "/orgs/v1/{orgId}/invites/rescind", request);

            var httpRequest = new UnityWebRequest(urlString, UnityWebRequest.kHttpVerbPOST);
            DownloadHandlerStream downloadHandler = new DownloadHandlerStream();
            httpRequest.downloadHandler = downloadHandler;
            httpRequest.SetRequestHeader("user-agent", _userAgent);

            var serializedBody = RequestBodySerializer.Serialize(request, "RescindUserInvite", "json", false, false);
            if (serializedBody != null)
            {
                httpRequest.uploadHandler = new UploadHandlerRaw(serializedBody.Body);
                httpRequest.SetRequestHeader("Content-Type", serializedBody.ContentType);
            }

            var client = _defaultClient;
            if (_securitySource != null)
            {
                client = SecuritySerializer.Apply(_defaultClient, _securitySource);
            }

            var httpResponse = await client.SendAsync(httpRequest);
            int? errorCode = null;
            string? contentType = null;
            switch (httpResponse.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    errorCode = (int)httpRequest.responseCode;
                    contentType = httpRequest.GetResponseHeader("Content-Type");
                    httpRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Console.WriteLine("Success");
                    break;
            }

            if (contentType == null)
            {
                contentType = httpResponse.GetResponseHeader("Content-Type") ?? "application/octet-stream";
            }
            int httpCode = errorCode ?? (int)httpResponse.responseCode;
            var response = new RescindInviteResponse
            {
                StatusCode = httpCode,
                ContentType = contentType,
                RawResponse = httpResponse
            };
            if (httpCode == 204)
            {
            }
            else if (new List<int>{401, 404, 422, 429, 500}.Contains(httpCode))
            {
                if(Utilities.IsContentTypeMatch("application/json",response.ContentType))
                {                    
                    var obj = JsonConvert.DeserializeObject<ApiError>(httpResponse.downloadHandler.text, new JsonSerializerSettings(){ NullValueHandling = NullValueHandling.Ignore, Converters = Utilities.GetDefaultJsonDeserializers() });
                    throw obj!;
                }
                else
                {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
                }
            }
            else if (httpCode >= 400 && httpCode < 500 || httpCode >= 500 && httpCode < 600)
            {
                throw new SDKException("API error occurred", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            else
            {
                throw new SDKException("unknown status code received", httpCode, httpResponse.downloadHandler.text, httpResponse);
            }
            return response;
        }

        
    }
}