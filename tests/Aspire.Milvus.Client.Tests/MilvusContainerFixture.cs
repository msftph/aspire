// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Aspire.Components.Common.Tests;
using Aspire.Hosting.Milvus;
using Testcontainers.Milvus;
using Xunit;

namespace Aspire.Milvus.Client.Tests;

public sealed class MilvusContainerFixture : IAsyncLifetime
{
    public MilvusContainer? Container { get; private set; }

    public string GetConnectionString() => Container?.GetEndpoint().AbsoluteUri ??
        throw new InvalidOperationException("The test container was not initialized.");

    public async Task InitializeAsync()
    {
        if (RequiresDockerTheoryAttribute.IsSupported)
        {
            Container = new MilvusBuilder()
                .WithImage($"{MilvusContainerImageTags.Image}:{MilvusContainerImageTags.Tag}")
                .WithPortBinding(19530)
                .Build();
            await Container.StartAsync();
        }
    }

    public async Task DisposeAsync()
    {
        if (Container is not null)
        {
            await Container.DisposeAsync();
        }
    }
}
