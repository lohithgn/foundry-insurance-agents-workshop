#:package Azure.AI.Projects@2.1.0-beta.3
#:package Azure.AI.Projects.Agents@2.1.0-beta.3
#:package Azure.AI.Extensions.OpenAI@2.1.0-beta.3
#:package Azure.Identity@1.21.0
#:package DotNetEnv@3.2.0

// Lab 02 — Agent 2: Customer Response (Foundry Agent Service, in code)
//
// Mirrors Labs/foundry/02-agent-customer-response.md using the Foundry projects (new)
// API, as documented at:
//   https://learn.microsoft.com/azure/foundry/agents/quickstarts/prompt-agent?tabs=csharp
//
// A prompt-only declarative agent (NO file search) that turns a coverage assessment
// into a warm, plain-language message to the customer.
//
// Run from the Labs/dotnet folder after `az login` and filling in `.env`:
//   dotnet run lab02-customer-response.cs

#pragma warning disable OPENAI001 // OpenAI Responses APIs are experimental.

using Azure.AI.Projects;
using Azure.AI.Projects.Agents;
using Azure.Identity;
using DotNetEnv;

LoadEnvNextToSource();

string endpoint = Require("FOUNDRY_PROJECT_ENDPOINT");
string model = Require("MODEL_DEPLOYMENT_NAME");
string initials = (Environment.GetEnvironmentVariable("INITIALS") ?? "").Trim();
bool cleanup = (Environment.GetEnvironmentVariable("CLEANUP") ?? "false")
    .Equals("true", StringComparison.OrdinalIgnoreCase);

string agentName = TagName(initials, "customer-response");

const string Instructions = """
    You are a Customer Response writer for an insurance company.
    You receive a claim assessment from the Claims Assessor.
    Turn it into a clear, warm, plain-language message to the customer.
    - Lead with empathy and acknowledge their situation.
    - Explain the coverage decision in simple terms (no jargon, no clause numbers).
    - If information is missing, politely list exactly what the customer needs to provide.
    - Keep it short, respectful, and professional. Do not promise anything
      the assessment did not state.
    """;

// Stand-in assessment from Lab 02's troubleshooting note. In the real workflow
// (Lab 03) this text is produced by the Claims Assessor and handed off automatically.
const string Assessment = """
    Assessment: Claim appears COVERED under Section 4.2 (Water Damage).
    Missing information: date of incident and two repair quotes.
    """;

// Create the project client for the Foundry (new) API.
AIProjectClient projectClient = new(
    endpoint: new Uri(endpoint),
    tokenProvider: new AzureCliCredential());

// Create a prompt-only declarative agent (no tools).
Console.WriteLine($"Creating agent: {agentName} (no tools — prompt only)");
DeclarativeAgentDefinition definition = new(model: model)
{
    Instructions = Instructions,
};
var agent = projectClient.AgentAdministrationClient.CreateAgentVersion(
    agentName: agentName,
    options: new(definition)).Value;
Console.WriteLine($"  agent id: {agent.Id} (version {agent.Version})");

// Turn the assessment into a customer message.
var responses = projectClient.ProjectOpenAIClient.GetProjectResponsesClientForAgent(agent.Name);

Console.WriteLine("\n=== Turn the assessment into a customer message ===");
var response = responses.CreateResponse(Assessment).Value;
Console.WriteLine(response.GetOutputText());

if (cleanup)
{
    Console.WriteLine("\nCLEANUP=true — deleting agent...");
    projectClient.AgentAdministrationClient.DeleteAgentVersion(agent.Name, agent.Version);
    Console.WriteLine("Cleanup complete.");
}
else
{
    Console.WriteLine($"\nAgent '{agentName}' kept so Lab 03 can reuse it.");
    Console.WriteLine("Note: in Tracing this run has no retrieval step, unlike Lab 01.");
}

return 0;

// ----- helpers -------------------------------------------------------------

// Load .env from the folder that contains this source file, so it works no
// matter what working directory the debugger or `dotnet run` uses.
static void LoadEnvNextToSource(
    [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "")
{
    string envPath = Path.Combine(Path.GetDirectoryName(sourcePath)!, ".env");
    if (File.Exists(envPath))
    {
        Env.Load(envPath);
    }
}

static string TagName(string initials, string baseName) =>
    initials.Length == 0 ? baseName : $"{initials}-{baseName}";

static string Require(string key)
{
    string? value = Environment.GetEnvironmentVariable(key);
    if (string.IsNullOrWhiteSpace(value))
    {
        throw new InvalidOperationException(
            $"Missing required environment variable '{key}'. Copy .env.example to .env and fill it in.");
    }
    return value;
}
