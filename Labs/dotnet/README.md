# Foundry Labs — in code (.NET 10)

Code versions of the three workshop labs, built as **.NET 10 file-based apps** — one
self-contained `.cs` file per lab, no project file required.

| Lab | File | SDK |
|-----|------|-----|
| 01 — Claims Assessor | [`lab01-claims-assessor.cs`](./lab01-claims-assessor.cs) | Foundry Agent Service — Foundry projects (new) API (`Azure.AI.Projects` 2.x) |
| 02 — Customer Response | [`lab02-customer-response.cs`](./lab02-customer-response.cs) | Foundry Agent Service — Foundry projects (new) API (`Azure.AI.Projects` 2.x) |
| 03 — Sequential Workflow | [`lab03-workflow.cs`](./lab03-workflow.cs) | Microsoft Agent Framework (`Microsoft.Agents.AI.Foundry`) |
| 03 — Sequential Workflow (DevUI) | [`lab03-devui.cs`](./lab03-devui.cs) | Same as Lab 03, hosted in the Agent Framework DevUI (`Microsoft.Agents.AI.DevUI`) |

These mirror the no-code labs in [`../`](../README.md): same instructions, same sample
claims. Labs 01–02 create **declarative prompt agents** with the Foundry projects (new)
API ([prompt-agent quickstart](https://learn.microsoft.com/azure/foundry/agents/quickstarts/prompt-agent?tabs=csharp));
Lab 03 **reuses** those agents by name, so run them in order.

## Prerequisites

These labs run against real Azure resources and authenticate with `AzureCliCredential`,
so **you must be signed in with `az login` before running any lab** — there are no keys
or secrets in `.env`.

You need all of the following:

- **Azure subscription access** — an account with access to a subscription.
- **A Foundry account and project** — with a **deployed chat model** (set up in
  [Lab 00](../foundry/00-prerequisites.md)) and permission to create agents in it.
- **.NET 10 SDK** — `dotnet --version` → `10.x`.
- **Signed in via `az login`** — auth uses `AzureCliCredential`, which reads the
  identity from your active Azure CLI session. Run it once before the labs:
  ```powershell
  az login
  ```
  If you have more than one subscription, select the right one:
  ```powershell
  az account set --subscription "<subscription-id-or-name>"
  ```

> ⚠️ **`az login` is mandatory.** If you skip it, every lab fails at startup with an
> authentication error — `AzureCliCredential` has no signed-in identity to use.

## Setup

```powershell
cd Labs/dotnet
Copy-Item .env.example .env
# edit .env and set FOUNDRY_PROJECT_ENDPOINT (and MODEL_DEPLOYMENT_NAME if not gpt-4.1)
```

`.env` keys:

| Key | Purpose |
|-----|---------|
| `FOUNDRY_PROJECT_ENDPOINT` | `https://<resource>.services.ai.azure.com/api/projects/<project>` |
| `MODEL_DEPLOYMENT_NAME` | Deployed chat model, e.g. `gpt-4.1` |
| `POLICY_FILE_PATH` | Policy doc uploaded in Lab 01 (defaults to the workshop homeowners policy) |
| `INITIALS` | Optional name tag so assets don't collide in a shared project |
| `CLEANUP` | `true` deletes created assets at the end; keep `false` so Lab 03 can reuse them |

## Run

Run each lab from this folder, in order:

```powershell
dotnet run lab01-claims-assessor.cs
dotnet run lab02-customer-response.cs
dotnet run lab03-workflow.cs
```

The first run restores NuGet packages (declared inline via `#:package` directives) and
may take a minute. Agents you create show up in the Foundry portal under
**Build → Agents**, and each run is visible in **Tracing** — the same observability you
explored in the no-code labs.

## Notes

- **Lab 03 (DevUI)** — [`lab03-devui.cs`](./lab03-devui.cs) is an optional variant of Lab 03
  that hosts the **Agent Framework DevUI** instead of streaming to the console. It is a
  .NET 10 file-based *web* app (`#:sdk Microsoft.NET.Sdk.Web`) because DevUI is an ASP.NET
  Core integration. It reuses the same Labs 01/02 agents (run them first with
  `CLEANUP=false`), registers them plus the sequential workflow with the hosting builder,
  and serves an interactive browser UI:
  ```powershell
  dotnet run lab03-devui.cs
  ```
  Then open `http://localhost:5000/devui`, pick **claims-workflow**, and submit a claim to
  watch the Claims Assessor → Customer Response hand-off stream in the browser. DevUI is a
  dev-time tool — it is gated behind the Development environment and is not for production.
- **Lab 03 depends on Labs 01 and 02.** It references the two agents by name via
  `AgentReference`; if either is missing the call fails — run Labs 01–02 first with
  `CLEANUP=false`.
- **Re-running Labs 01/02** creates a new *version* of the same named declarative agent
  each time (the Foundry projects API is version-based); Lab 03 uses the latest version.
  Set `CLEANUP=true` for a throwaway run that deletes the agent version, vector store,
  and file.
- Labs 01–02 use `AIProjectClient` + `DeclarativeAgentDefinition` and chat through the
  Responses API (`GetProjectResponsesClientForAgent`). Lab 03 bridges each named agent to
  an `AIAgent` with `AIProjectClient.AsAIAgent(new AgentReference(name))` and orchestrates
  them with `AgentWorkflowBuilder.BuildSequential`.
- The `OPENAI001` experimental-API warning is intentionally suppressed in Labs 01–02
  because the Responses / vector store types are still marked experimental.
