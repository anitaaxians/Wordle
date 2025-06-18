using Google.Protobuf.WellKnownTypes;

var builder = DistributedApplication.CreateBuilder(args);

var api =builder.AddProject<Projects.Wordle>("wordle");


var frontend = builder.AddNpmApp("front", "C:\\Users\\anita.hoti\\wordle>", "start")
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();
builder.Build().Run();
