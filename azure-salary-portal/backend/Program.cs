using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        // Add custom services here
        // services.AddScoped<IPayslipService, PayslipService>();
        // services.AddScoped<IEmployeeService, EmployeeService>();
    })
    .Build();

host.Run();