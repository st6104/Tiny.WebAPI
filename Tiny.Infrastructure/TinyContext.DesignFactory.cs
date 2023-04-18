using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Logging;
using Tiny.MultiTenant.Interfaces;

namespace Tiny.Infrastructure;

public class TinyContextDesignFactory : IDesignTimeDbContextFactory<TinyDbContext>
{
    public TinyDbContext CreateDbContext(string[] args)
    {
        return new TinyDbContext(new FakeMediator(), new FakeLoggerFactory(), new FakeMultiTenant(),
            new FakeMultiTenantSettings());
    }

    private class FakeMultiTenantSettings : IMultiTenantSettings
    {
        public string TenantIdFieldName { get; } = string.Empty;
        public bool UseTenantIdField { get; } = true;
        public Type MiddlewareType { get; } = null!;
        public bool UseDistributedCache { get; } = false;
    }

    private class FakeMultiTenant : IMultiTenantService
    {
        public ITenantInfo Current { get => new FakeTenantInfo(); set { } }

        private class FakeTenantInfo : ITenantInfo
        {
            public string Id => string.Empty;

            public string Name => string.Empty;

            public string ConnectionString => string.Empty;

            public bool IsActive => true;

            public void ChangeName(string name)
            {
            }

            public void ChangeConnectionString(string connectionString)
            {
            }

            public void Active()
            {
            }

            public void Inactive()
            {
            }
        }
    }


    private class FakeLoggerFactory : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new FakeLogger();
        }

        public void Dispose()
        {
        }
    }

    private class FakeLogger : ILogger
    {
        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
        }
    }

    private class FakeMediator : IMediator
    {
        public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            return default!;
        }

        public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
        {
            return default!;
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            return Task.CompletedTask;
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            return Task.FromResult<TResponse>(default!);
        }

        public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default)
            where TRequest : IRequest
        {
            return Task.CompletedTask;
        }

        public Task<object?> Send(object request, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<object?>(default);
        }
    }
}
