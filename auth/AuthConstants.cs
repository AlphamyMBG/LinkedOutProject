using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackendApp.Auth
{
    public class AuthConstants
    {
        public static class PolicyNames
        {
            public const string HasIdEqualToUserIdParamPolicyName = "HasIdEqualToUserIdParamPolicy";
            public const string HasIdEqualToSenderIdPolicyName = "HasIdEqualToSenderIdPolicy";
            public const string HasIdEqualToIdParamPolicyName = "HasIdEqualToIdPolicy";
            public const string HasNotificationPolicyName = "HasNotificationPolicy";
            public const string SentMessagePolicyName = "SentMessagePolicy";
            public const string IsAdminPolicyName = "IsAdminPolicy";
        }

        public static class ClaimTypes
        {
            public const string Email = "email";
            public const string Subject = "sub";
            public const string Role = "role";
        }
    }
}