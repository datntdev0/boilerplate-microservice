var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.datntdev_Microservice_App_Identity>("datntdev-microservice-app-identity");

builder.AddProject<Projects.datntdev_Microservice_Srv_Identity_Web_Host>("datntdev-microservice-srv-identity-web-host");

builder.AddProject<Projects.datntdev_Microservice_Srv_Admin_Web_Host>("datntdev-microservice-srv-admin-web-host");

builder.AddProject<Projects.datntdev_Microservice_Srv_Notify_Web_Host>("datntdev-microservice-srv-notify-web-host");

builder.AddProject<Projects.datntdev_Microservice_Srv_Payment_Web_Host>("datntdev-microservice-srv-payment-web-host");

builder.AddProject<Projects.datntdev_Microservice_Infra_Gateway>("datntdev-microservice-infra-gateway");

builder.Build().Run();
