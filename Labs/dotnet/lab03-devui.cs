#:sdk Microsoft.NET.Sdk.Web
#:package Azure.AI.Projects@2.1.0-beta.3
#:package Azure.AI.Projects.Agents@2.1.0-beta.3
#:package Azure.AI.Extensions.OpenAI@2.1.0-beta.3
#:package Microsoft.Agents.AI.Foundry@1.10.0-preview.260610.1
#:package Microsoft.Agents.AI.Workflows@1.10.0
#:package Microsoft.Agents.AI.Hosting@1.10.0-preview.260610.1
#:package Microsoft.Agents.AI.Hosting.OpenAI@1.10.0-alpha.260610.1
#:package Microsoft.Agents.AI.DevUI@1.10.0-preview.260610.1
#:package Azure.Identity@1.21.0
#:package DotNetEnv@3.2.0

// Lab 03 (DevUI) — Sequential Workflow with an interactive web UI
//
// Same sequential workflow as lab03-workflow.cs (Claims Assessor -> Customer Response),
// but instead of streaming to the console it hosts the Microsoft Agent Framework DevUI:
// a browser UI for invoking and debugging the agents and the workflow, with streaming
// output and visible agent hand-offs.
//
// Unlike the console lab, this is a .NET 10 file-based *web* app (`#:sdk
// Microsoft.NET.Sdk.Web`), because DevUI is an ASP.NET Core integration. It still REUSES
// the agents created by Labs 01 and 02 — run those first (with CLEANUP=false). Each
// Foundry agent is referenced by name via AgentReference and bridged to an AIAgent with
// AsAIAgent, then registered with the hosting builder so DevUI can drive it.
//
// Run from the Labs/dotnet folder after `az login` and filling in `.env`:
//   dotnet run lab03-devui.cs
// then open the printed URL (http://localhost:5000/devui) and pick "claims-workflow".

using Azure.AI.Projects;
using Azure.AI.Projects.Agents;
using Azure.AI.Extensions.OpenAI;
using Azure.Identity;
using DotNetEnv;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.DevUI;
using Microsoft.Agents.AI.Foundry;
using Microsoft.Agents.AI.Hosting;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.DependencyInjection;

LoadEnvNextToSource();

string endpoint = Require("FOUNDRY_PROJECT_ENDPOINT");
string initials = (Environment.GetEnvironmentVariable("INITIALS") ?? "").Trim();

string assessorName = TagName(initials, "claims-assessor");
string responderName = TagName(initials, "customer-response");

var builder = WebApplication.CreateBuilder(args);

// One project client, shared by the agent factory delegates below.
AIProjectClient projectClient = new(
    endpoint: new Uri(endpoint),
    tokenProvider: new AzureCliCredential());

Console.WriteLine($"Referencing assessor : {assessorName}");
Console.WriteLine($"Referencing responder: {responderName}");

// Bridge each existing server-side agent (latest version) into an AIAgent and register it
// with the hosting builder. The DI key must match the wrapped agent's Name, which the
// AgentReference overload takes from the server-side Foundry agent (the tagged name), so
// register under that same name. File search configured on the assessor in Lab 01 is part
// of its server-side definition, so it still grounds answers here.
builder.AddAIAgent(assessorName,
    (sp, key) => projectClient.AsAIAgent(new AgentReference(assessorName)));
builder.AddAIAgent(responderName,
    (sp, key) => projectClient.AsAIAgent(new AgentReference(responderName)));

// Same sequential pipeline as the console lab (assessment flows into the response),
// surfaced to DevUI as a single invokable agent named "claims-workflow".
builder.AddWorkflow("claims-workflow", (sp, key) =>
{
    AIAgent assessor = sp.GetRequiredKeyedService<AIAgent>(assessorName);
    AIAgent responder = sp.GetRequiredKeyedService<AIAgent>(responderName);
    return AgentWorkflowBuilder.BuildSequential(workflowName: key, assessor, responder);
}).AddAsAIAgent();

// DevUI's chat surface is served over the OpenAI Responses/Conversations endpoints.
builder.Services.AddOpenAIResponses();
builder.Services.AddOpenAIConversations();

// Register the DevUI services (auth filter, middleware, options) that MapDevUI resolves.
builder.Services.AddDevUI();

// HTTP keeps local dev simple (no dev HTTPS cert needed). DevUI is dev-only — see below.
builder.WebHost.UseUrls("http://localhost:5000");

var app = builder.Build();

app.MapOpenAIResponses();
app.MapOpenAIConversations();

// Gate DevUI behind the Development environment — it is a debugging tool, not for prod.
//if (app.Environment.IsDevelopment())
//{
    app.MapDevUI();
//}

Console.WriteLine("\nDevUI:            http://localhost:5000/devui");
Console.WriteLine("OpenAI Responses: http://localhost:5000/v1/responses");
Console.WriteLine("\nOpen the DevUI, choose \"claims-workflow\", and submit a claim to watch the");
Console.WriteLine("Claims Assessor -> Customer Response hand-off stream in the browser.");
Console.WriteLine("Press Ctrl+C to stop.\n");

app.Run();

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
