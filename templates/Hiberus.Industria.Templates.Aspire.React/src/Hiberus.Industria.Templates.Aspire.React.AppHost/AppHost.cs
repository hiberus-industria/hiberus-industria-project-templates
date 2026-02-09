var builder = DistributedApplication.CreateBuilder(args);

var server = builder.AddProject<Projects.Hiberus_Industria_Templates_Aspire_React_Server>("server")
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

var webfrontend = builder.AddViteApp("webfrontend", "../frontend")
    .WithReference(server)
    .WaitFor(server);

server.PublishWithContainerFiles(webfrontend, "wwwroot");

builder.Build().Run();
