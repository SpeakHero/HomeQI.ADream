// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace HomeQI.Adream.Identity.EntityFrameworkCore
{
    public class UserStore : UserOnlyStore
    {
        public UserStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, Microsoft.Extensions.Logging.ILoggerFactory loggerFactory) : base(context, errorDescriber, loggerFactory)
        {
        }
    }
}
