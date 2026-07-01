# Foundry Agents Workshop

A hands-on, **zero-code, browser-only** workshop for building a multi-agent system in Microsoft Foundry. Participants build two AI agents and a sequential workflow that triages an insurance claim end to end — grounded in their own policy documents, guardrailed, and fully observable.

Originally designed for a leadership audience (show-and-tell + light hands-on), but written to be **reusable for any audience**.

## What's in this repo

| Folder | For | Contents |
|--------|-----|----------|
| **[`Labs/`](./Labs/README.md)** | Participants | Step-by-step hands-on labs — prerequisites, two agents, and the workflow. Start here to build. |
| **[`data/`](./data/README.md)** | Everyone | Synthetic insurance datasets — policy documents (Markdown + PDF) and sample claims — uploaded as the agents' knowledge. |

## The scenario

**Insurance claims triage.** A messy, real-sounding claim comes in; two agents collaborate to produce a clear, customer-ready response.

- **Agent 1 — Claims Assessor** reads a claim and assesses coverage against uploaded policy documents.
- **Agent 2 — Customer Response** turns that assessment into a clear, empathetic customer reply.
- **Workflow** — a no-code, visual sequential pipeline: Assessor → Response.

## Where to start

There are **two ways** to do these labs — pick the track that fits you. Both build the same two agents and workflow.

- **No-code (portal)** — Build everything in the browser at [ai.azure.com](https://ai.azure.com). No installs. Start at [`Labs/foundry/00-prerequisites.md`](./Labs/foundry/00-prerequisites.md) and work through the labs in order (00 → 03).
- **In code (.NET 10)** — The same labs as .NET 10 single-file apps (Foundry SDK + Microsoft Agent Framework). Requires the .NET 10 SDK and `az login`. Copy [`Labs/dotnet/.env.example`](./Labs/dotnet/.env.example) to `Labs/dotnet/.env` and fill in your own values first (the `.env` file is git-ignored and never committed). Start at [`Labs/dotnet/README.md`](./Labs/dotnet/README.md).

See [`Labs/README.md`](./Labs/README.md) for the full lab map and conventions.

- **Setting up the data?** See [`data/README.md`](./data/README.md) — upload `HOME-2024-00871-homeowners` (Markdown or PDF) as the Claims Assessor's knowledge.

## At a glance

- **Platform:** Microsoft Foundry (new) — [ai.azure.com](https://ai.azure.com), browser only, no installs.
- **Build time:** ~40 minutes of hands-on across the three labs.
- **Outcome:** two agents + one workflow delivering a governed, observable business outcome.

> ⚠️ All workshop data is **synthetic** (fictional carrier "Northwind Mutual", made-up people and policy numbers). It is for training only and must never be used for any real coverage decision.
