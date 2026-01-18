var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.datntdev_Microservice_App_Identity>("datntdev-microservice-app-identity");

builder.Build().Run();
