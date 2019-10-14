﻿using PipServices3.Commons.Config;
using PipServices3.Commons.Refer;
using System.Threading.Tasks;
using Xunit;
using Guides.Persistence;
using Guides.Logic;
using Guides.Services.Version1;
using System;

namespace Guides.Clients.Version1
{
    public class GuidesHttpClientV1Test
    {
        private static readonly ConfigParams HttpConfig = ConfigParams.FromTuples(
            "connection.protocol", "http",
            "connection.host", "localhost",
            "connection.port", 8080
        );

        private GuidesMemoryPersistence _persistence;
        private GuidesController _controller;
        private GuidesHttpClientV1 _client;
        private GuidesHttpServiceV1 _service;
        private GuidesClientV1Fixture _fixture;

        public GuidesHttpClientV1Test()
        {
            _persistence = new GuidesMemoryPersistence();
            _controller = new GuidesController();
            _client = new GuidesHttpClientV1();
            _service = new GuidesHttpServiceV1();

            IReferences references = References.FromTuples(
                new Descriptor("guides", "persistence", "memory", "default", "1.0"), _persistence,
                new Descriptor("guides", "controller", "default", "default", "1.0"), _controller,
                new Descriptor("guides", "client", "http", "default", "1.0"), _client,
                new Descriptor("guides", "service", "http", "default", "1.0"), _service
            );

            _controller.SetReferences(references);

            _service.Configure(HttpConfig);
            _service.SetReferences(references);

            _client.Configure(HttpConfig);
            _client.SetReferences(references);

            _fixture = new GuidesClientV1Fixture(_client);

            _service.OpenAsync(null).Wait();
            _client.OpenAsync(null).Wait();
        }

        [Fact]
        public async Task TestCrudOperationsAsync()
        {
            await _fixture.TestCrudOperationsAsync();
        }
    }
}