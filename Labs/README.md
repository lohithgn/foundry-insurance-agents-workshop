# Foundry Agents Workshop — Hands-on Labs

A step-by-step lab series for building an insurance **claims-triage** agent system in Microsoft Foundry — entirely in the browser, with no code.

By the end you will have built **two AI agents** and one **sequential workflow** that takes a messy claim in and produces a clear, customer-ready response out — all grounded in your own policy documents, and fully observable.

## Lab map

| # | Lab | What you build | Time |
|---|-----|----------------|------|
| 00 | [Prerequisites](./foundry/00-prerequisites.md) | Access, project, model, naming, sample data | — |
| 01 | [Agent 1 — Claims Assessor](./foundry/01-agent-claims-assessor.md) | Create → add files (file search) → test → observe | ~15 min |
| 02 | [Agent 2 — Customer Response](./foundry/02-agent-customer-response.md) | Create → test → observe | ~10 min |
| 03 | [Workflow — Sequential Orchestration](./foundry/03-workflow-orchestration.md) | Build → run → observe the end-to-end pipeline | ~15 min |

Do the labs **in order** — Lab 03 depends on the agents from Labs 01 and 02.

## Two ways to do these labs

| Track | Folder | What it is |
|-------|--------|------------|
| **No-code (portal)** | [`foundry/`](./foundry/) | Build the agents and workflow in the browser at [ai.azure.com](https://ai.azure.com). Start here. Browser only, no installs. |
| **In code (.NET 10)** | [`dotnet/`](./dotnet/) | The same labs as .NET 10 single-file apps — Foundry SDK for Labs 01–02, Microsoft Agent Framework for Lab 03. Additionally requires the **.NET 10 SDK** and **`az login`** (auth uses `AzureCliCredential`) — see the [dotnet README](./dotnet/README.md#prerequisites). |

## Conventions used in every lab

- **Objective** — what you'll have at the end.
- **Steps** — numbered, click-by-click.
- **Test** — how to confirm it works.
- **Observability** — where to see what the agent actually did.
- ✅ **Checkpoint** — the state you must reach before moving on.
- 🛠 **Troubleshooting** — common snags and fixes.

### Naming convention (for shared projects)

If several people build in the **same Foundry project**, agent and workflow names can collide. To keep everyone's work separate, give every asset you create a **unique tag** — the simplest is your **initials**, prepended (or appended) to the name.

Throughout these labs, replace `<initials>` with your own tag. For example, if your initials are **JD**:

| Asset | Name pattern | Example (initials "JD") |
|-------|-------------|--------------------------|
| Agent 1 | `<initials>-claims-assessor` | `jd-claims-assessor` |
| Agent 2 | `<initials>-customer-response` | `jd-customer-response` |
| Workflow | `<initials>-claims-workflow` | `jd-claims-workflow` |

> Prefer appending? `claims-assessor-jd` works just as well — pick one style and stay consistent. If two people might share initials, add a number (e.g. `jd2-`).
>
> **Working in your own private project?** The tag is optional — you can drop it and just name things `claims-assessor`, `customer-response`, and `claims-workflow`.

### Portal note

These labs use **Foundry (new)** at [ai.azure.com](https://ai.azure.com). Make sure the **New Foundry** toggle (top of the portal) is **on**. The portal evolves; if a label differs slightly, the nearest equivalent is correct — ask an onsite helper if unsure.

## Sample prompts

Every prompt you need to paste is **embedded inline** in each lab — the sample claims, the coverage question, and the workflow input are all in code blocks ready to copy. You don't need anything outside these docs.
