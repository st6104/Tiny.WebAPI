using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tiny.Api.Exceptions;
using Tiny.Application.Interfaces;

namespace Tiny.Api.ApplicationImplements;

public class DbConnectionStore : IDbConnectionStore
{
    public string Default { get; }

    public DbConnectionStore(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Tiny");
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new DbConnectionStringErrorException("Does Not Exists DbConnectionString.");

        Default = connectionString;
    }
}
