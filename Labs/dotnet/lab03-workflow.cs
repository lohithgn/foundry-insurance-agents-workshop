#:package Azure.AI.Projects@2.1.0-beta.3
#:package Azure.AI.Projects.Agents@2.1.0-beta.3
#:package Azure.AI.Extensions.OpenAI@2.1.0-beta.3
#:package Microsoft.Agents.AI.Foundry@1.10.0-preview.260610.1
#:package Microsoft.Agents.AI.Workflows@1.10.0
#:package Azure.Identity@1.21.0
#:package DotNetEnv@3.2.0

// Lab 03 — Sequential Workflow (Microsoft Agent Framework, in code)
//
// Mirrors Labs/foundry/03-workflow-orchestration.md: wire the two agents from Labs 01
// and 02 into one sequential workflow (Claims Assessor -> Customer Response) and run a
// single claim through it.
//
// This lab REUSES the agents created by Labs 01 and 02 — run those first (with
// CLEANUP=false). It references each Foundry agent by name via AgentReference, bridges
// it to an AIAgent with AsAIAgent, then orchestrates them with
// AgentWorkflowBuilder.BuildSequential.
//
// Run from the Labs/dotnet folder after `az login` and filling in `.env`:
//   dotnet run lab03-workflow.cs

using Azure.AI.Projects;
using Azure.AI.Projects.Agents;
using Azure.AI.Extensions.OpenAI;
using Azure.Identity;
using DotNetEnv;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Foundry;
using Microsoft.Agents.AI.Workflows;

LoadEnvNextToSource();

string endpoint = Require("FOUNDRY_PROJECT_ENDPOINT");
string initials = (Environment.GetEnvironmentVariable("INITIALS") ?? "").Trim();

string assessorName = TagName(initials, "claims-assessor");
string responderName = TagName(initials, "customer-response");

const string StormClaim = """
    I want to report a claim on policy HOME-2024-00871. During the
    storm two nights ago, a tree branch fell and broke our living
    room window. Rain got in overnight and damaged the carpet and a
    sofa before we could cover the window. We've taken photos. What's
    covered and what do you need from us to proceed?
    """;

AIProjectClient projectClient = new(
    endpoint: new Uri(endpoint),
    tokenProvider: new AzureCliCredential());

Console.WriteLine($"Referencing assessor : {assessorName}");
Console.WriteLine($"Referencing responder: {responderName}");

// Bridge each existing server-side agent (latest version) into an AIAgent the
// framework can orchestrate. File search configured on the assessor in Lab 01 is part
// of its server-side definition, so it still grounds answers here.
AIAgent assessor = projectClient.AsAIAgent(new AgentReference(assessorName));
AIAgent responder = projectClient.AsAIAgent(new AgentReference(responderName));

// Sequential pipeline: assessment from node 1 flows straight into node 2.
AIAgent workflow = AgentWorkflowBuilder.BuildSequential(assessor, responder).AsAIAgent();

Console.WriteLine("\nRunning the workflow (Claims Assessor → Customer Response)...\n");

string? lastAuthor = null;
await foreach (var update in workflow.RunStreamingAsync(StormClaim))
{
    // Skip workflow-only lifecycle events that carry no message content.
    if ((update.Contents is null || update.Contents.Count == 0) && update.RawRepresentation is WorkflowEvent)
    {
        continue;
    }

    if (lastAuthor != update.AuthorName)
    {
        lastAuthor = update.AuthorName;
        Console.WriteLine($"\n** {update.AuthorName} **");
    }

    Console.Write(update.Text);
}

Console.WriteLine("\n\nWorkflow complete. Open the run in the Foundry portal's Tracing view to");
Console.WriteLine("see both agents' spans and the hand-off between them.");

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
