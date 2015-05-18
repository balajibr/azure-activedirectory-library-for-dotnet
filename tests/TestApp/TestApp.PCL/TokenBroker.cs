﻿//----------------------------------------------------------------------
// Copyright (c) Microsoft Open Technologies, Inc.
// All Rights Reserved
// Apache License 2.0
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//----------------------------------------------------------------------

using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Test.ADAL.Common;

namespace TestApp.PCL
{
    public class TokenBroker
    {
        private AuthenticationContext context;

        private Sts sts;

        public TokenBroker()
        {
            this.sts = new AadSts();
            context = new AuthenticationContext(this.sts.Authority, true);
        }

        public async Task<string> GetTokenInteractiveAsync(IPlatformParameters parameters)
        {
            try
            {
                var authorizationCode = await context.AcquireUserAuthorizationAsync(new [] { sts.ValidResource }, sts.ValidClientId, sts.ValidNonExistingRedirectUri, parameters, new UserIdentifier(sts.ValidUserName, UserIdentifierType.OptionalDisplayableId));
                var result = await context.AcquireTokenByAuthorizationCodeAsync(authorizationCode, sts.ValidNonExistingRedirectUri, sts.ValidClientId);

                return result.AccessToken;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> GetTokenInteractiveWithMsAppAsync(IPlatformParameters parameters)
        {
            try
            {
                var authorizationCode = await context.AcquireUserAuthorizationAsync(new[] { sts.ValidResource }, sts.ValidClientId, null, parameters, new UserIdentifier(sts.ValidUserName, UserIdentifierType.OptionalDisplayableId));
                var result = await context.AcquireTokenByAuthorizationCodeAsync(authorizationCode, sts.ValidNonExistingRedirectUri, sts.ValidClientId);

                return result.AccessToken;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public void ClearTokenCache()
        {
            this.context.TokenCache.Clear();
        }
    }
}
