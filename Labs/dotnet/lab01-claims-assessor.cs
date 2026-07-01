#:package Azure.AI.Projects@2.1.0-beta.3
#:package Azure.AI.Projects.Agents@2.1.0-beta.3
#:package Azure.AI.Extensions.OpenAI@2.1.0-beta.3
#:package Azure.Identity@1.21.0
#:package DotNetEnv@3.2.0

// Lab 01 — Agent 1: Claims Assessor (Foundry Agent Service, in code)
//
// Mirrors Labs/foundry/01-agent-claims-assessor.md using the Foundry projects (new)
// API, as documented at:
//   https://learn.microsoft.com/azure/foundry/agents/quickstarts/prompt-agent?tabs=csharp
//   https://learn.microsoft.com/azure/foundry/agents/how-to/tools/file-search?tabs=csharp
//
// Flow: upload the policy file -> create a vector store -> create a declarative agent
// with the file-search tool -> ask it to assess a burst-pipe claim, grounded ONLY in
// the uploaded policy.
//
// Run from the Labs/dotnet folder after `az login` and filling in `.env`:
//   dotnet run lab01-claims-assessor.cs

#pragma warning disable OPENAI001 // OpenAI Responses / vector store APIs are experimental.

using Azure.AI.Projects;
using Azure.AI.Projects.Agents;
using Azure.Identity;
using OpenAI.Files;
using OpenAI.Responses;
using OpenAI.VectorStores;
using DotNetEnv;

LoadEnvNextToSource();

string endpoint = Require("FOUNDRY_PROJECT_ENDPOINT");
string model = Require("MODEL_DEPLOYMENT_NAME");
string initials = (Environment.GetEnvironmentVariable("INITIALS") ?? "").Trim();
string policyPath = (Environment.GetEnvironmentVariable("POLICY_FILE_PATH") ?? "").Trim();
if (policyPath.Length == 0)
{
    policyPath = "../../data/pdf/HOME-2024-00871-homeowners.pdf";
}
policyPath = ResolveFromSource(policyPath);
bool cleanup = (Environment.GetEnvironmentVariable("CLEANUP") ?? "false")
    .Equals("true", StringComparison.OrdinalIgnoreCase);

string agentName = TagName(initials, "claims-assessor");

if (!File.Exists(policyPath))
{
    Console.Error.WriteLine($"Policy file not found: {Path.GetFullPath(policyPath)}");
    Console.Error.WriteLine("Set POLICY_FILE_PATH in .env or run from the Labs/dotnet folder.");
    return 1;
}

const string Instructions = """
    You are a Claims Assessor for an insurance company.
    Read the submitted claim and assess it ONLY against the policy
    documents provided in your knowledge. For each claim:
    1. State whether the claim appears covered, partially covered, or not covered.
    2. Cite the specific policy section or clause that supports your decision.
    3. List any missing information needed to finalize the assessment.
    If the policy does not address something, say so — never invent coverage.
    Be concise and factual.
    """;

const string BurstPipeClaim = """
    Hi, I need to file a claim. Last Tuesday night a pipe under my
    kitchen sink burst while we were asleep and water flooded the
    kitchen floor and soaked the lower cabinets. We turned off the
    water in the morning and called a plumber. The floor is warped
    and we think the cabinets are ruined. Policy number HOME-2024-00871.
    Not sure what's covered or what you need from me.
    """;

const string MeteorProbe =
    "Is damage from a meteor strike covered under my policy?";

// Create the project client for the Foundry (new) API.
AIProjectClient projectClient = new(
    endpoint: new Uri(endpoint),
    tokenProvider: new AzureCliCredential());

// Upload the policy file using the OpenAI file mechanism.
Console.WriteLine($"Uploading policy file: {Path.GetFullPath(policyPath)}");
OpenAIFileClient fileClient = projectClient.ProjectOpenAIClient.GetOpenAIFileClient();
OpenAIFile uploadedFile = fileClient.UploadFile(policyPath, FileUploadPurpose.Assistants);
Console.WriteLine($"  file id: {uploadedFile.Id}");

// Build a vector store from the uploaded file.
Console.WriteLine("Creating vector store (file search index)...");
VectorStoreClient vectorStoreClient = projectClient.ProjectOpenAIClient.GetVectorStoreClient();
VectorStore vectorStore = vectorStoreClient.CreateVectorStore(options: new VectorStoreCreationOptions
{
    Name = TagName(initials, "claims-policies"),
    FileIds = { uploadedFile.Id },
});
Console.WriteLine($"  vector store id: {vectorStore.Id}");

// Create a declarative agent that can use file search over the vector store.
Console.WriteLine($"Creating agent: {agentName}");
DeclarativeAgentDefinition definition = new(model: model)
{
    Instructions = Instructions,
    Tools = { ResponseTool.CreateFileSearchTool(vectorStoreIds: new[] { vectorStore.Id }) },
};
var agent = projectClient.AgentAdministrationClient.CreateAgentVersion(
    agentName: agentName,
    options: new(definition)).Value;
Console.WriteLine($"  agent id: {agent.Id} (version {agent.Version})");

// Ask the agent against the deployed agent name.
var responses = projectClient.ProjectOpenAIClient.GetProjectResponsesClientForAgent(agent.Name);

void Ask(string label, string userText)
{
    Console.WriteLine($"\n=== {label} ===");
    var response = responses.CreateResponse(userText).Value;
    Console.WriteLine(response.GetOutputText());
}

Ask("Test 1 — assess the burst-pipe claim", BurstPipeClaim);
Ask("Probe — out-of-scope (meteor strike)", MeteorProbe);

if (cleanup)
{
    Console.WriteLine("\nCLEANUP=true — deleting agent, vector store, and file...");
    projectClient.AgentAdministrationClient.DeleteAgentVersion(agent.Name, agent.Version);
    vectorStoreClient.DeleteVectorStore(vectorStore.Id);
    fileClient.DeleteFile(uploadedFile.Id);
    Console.WriteLine("Cleanup complete.");
}
else
{
    Console.WriteLine($"\nAgent '{agentName}' kept so Lab 03 can reuse it.");
    Console.WriteLine("View it in the Foundry portal under Build → Agents (and Tracing).");
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

// Resolve a relative path against this source file's folder, so it works no
// matter what working directory the debugger or `dotnet run` uses.
static string ResolveFromSource(
    string path, [System.Runtime.CompilerServices.CallerFilePath] string sourcePath = "")
{
    return Path.IsPathRooted(path)
        ? path
        : Path.GetFullPath(Path.Combine(Path.GetDirectoryName(sourcePath)!, path));
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
