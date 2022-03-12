using Microsoft.Extensions.Logging;

namespace FunctionApp1;

internal class TestService : ITestService
{
    private readonly ILogger<TestService> _log;

    public TestService(ILogger<TestService> log) => _log = log;

    public void TestLogger(string message) => _log.LogInformation(message);
}