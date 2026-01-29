var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.datntdev_Microservice_App_Identity>("app-identity")
    .WithHttpHealthCheck("/alive");

var srvIdentity = builder.AddProject<Projects.datntdev_Microservice_Srv_Identity_Web_Host>("srv-identity");
var srvAdmin = builder.AddProject<Projects.datntdev_Microservice_Srv_Admin_Web_Host>("srv-admin");
var srvNotify = builder.AddProject<Projects.datntdev_Microservice_Srv_Notify_Web_Host>("srv-notify");
var srvPayment = builder.AddProject<Projects.datntdev_Microservice_Srv_Payment_Web_Host>("srv-payment");

builder.AddProject<Projects.datntdev_Microservice_Infra_Gateway>("gateway")
    .WithReference(srvIdentity).WaitFor(srvIdentity).WithExternalHttpEndpoints()
    .WithReference(srvAdmin).WaitFor(srvAdmin).WithExternalHttpEndpoints()
    .WithReference(srvNotify).WaitFor(srvNotify).WithExternalHttpEndpoints()
    .WithReference(srvPayment).WaitFor(srvPayment).WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/alive");

builder.Build().Run();
