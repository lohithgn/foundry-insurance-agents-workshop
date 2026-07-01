# Lab 03 — Workflow: Sequential Orchestration

**Goal:** Connect your two agents into one **sequential workflow** so a single claim flows automatically: **Claims Assessor → Customer Response**. Then run it end to end and observe the hand-off. All visual, no code.

**You'll learn:** how no-code orchestration works, why a visual workflow is auditable, and how to read an end-to-end run trace.

**Time:** ~15 minutes · **Prerequisite:** [Lab 01](./01-agent-claims-assessor.md) and [Lab 02](./02-agent-customer-response.md) complete (both agents exist).

---

## Part A — Create a sequential workflow

1. From the top menu, select **Build**.
2. Select **Create new workflow** → **Sequential**.
   - The *Sequential* pattern passes the result of one agent to the next in a defined order — exactly our claim → assessment → response pipeline.
3. Name the workflow `<initials>-claims-workflow`.

> ⚠️ **Foundry does not autosave workflows.** Select **Save** after every change in this lab, or you'll lose work.

✅ **Checkpoint A:** A blank sequential workflow named `<initials>-claims-workflow` is open in the visual builder.

---

## Part B — Assign your agents to the nodes

The sequential template gives you agent nodes wired in order. You assign *which* agent runs at each step.

1. Select the **first agent node**.
2. Assign your **`<initials>-claims-assessor`** (Lab 01) to it.
3. Select the **second agent node**.
4. Assign your **`<initials>-customer-response`** (Lab 02) to it.
5. Confirm the connection runs **Assessor → Response** (the output of node 1 feeds the input of node 2).
6. Select **Save**.

> This is the whole orchestration: claim enters node 1, the assessment it produces flows straight into node 2, which writes the customer reply. You wired a multi-agent system by selecting two boxes.

✅ **Checkpoint B:** Node 1 = `<initials>-claims-assessor`, Node 2 = `<initials>-customer-response`, connected in order, and **saved**.

---

## Part C — Run the workflow

1. Select **Run Workflow**.
2. In the chat window, paste the following claim and send it:

```
I want to report a claim on policy HOME-2024-00871. During the
storm two nights ago, a tree branch fell and broke our living
room window. Rain got in overnight and damaged the carpet and a
sofa before we could cover the window. We've taken photos. What's
covered and what do you need from us to proceed?
```

3. Watch the **visualizer**: each node should light up / complete in sequence as the claim moves through.
4. Read the final message in the chat window.

**Verify your run** — confirm all three:
1. Each node **completes** in the visualizer (no node stuck or errored).
2. The chat window shows the **expected responses** — a grounded assessment, then a customer-ready reply.
3. The final output is a clean customer message (empathetic, jargon-free) consistent with the claim above.

✅ **Checkpoint C:** The workflow ran end to end and produced a customer-ready response from a single claim input.

🛠 **Troubleshooting**

| Symptom | Fix |
|---------|-----|
| A node errors immediately | Re-open the node and confirm the correct agent is assigned; **Save** and re-run. |
| Changes don't take effect | You probably didn't **Save** — Foundry doesn't autosave workflows. |
| Second node output ignores the assessment | Confirm the nodes are connected Assessor → Response (output → input), not reversed. |
| Run hangs >60s | The shared model may be busy; wait and re-run, or flag onsite staff. |

---

## Part D — Observability: read the end-to-end trace

This is the payoff for any architect or governance owner — a complete, inspectable audit trail of a multi-agent decision.

1. **Visualizer trace:** after the run, review each node's state in the workflow visualizer — you can see where the claim was at each step and the data handed between nodes.
2. **Project Tracing:** open the project's **Tracing** / **Observability** view and select this workflow run.
   - You'll see **both agents' spans** under one run: the Assessor's retrieval + reasoning, then the Response writer's generation.
   - Step through to see the **hand-off**: the assessment produced by node 1 becomes the input to node 2.
   - Note total **token usage** and **latency** across the whole pipeline — your end-to-end cost and performance picture.
3. **Monitoring dashboard:** open the **Agent Monitoring Dashboard** (backed by Application Insights) to see this run alongside others — interactions, token/cost trends, and behavior over time.

> Takeaway: when someone asks *"why did the system tell the customer that?"*, this trace is your answer — each agent's step, the data passed between them, the cost, all recorded and reviewable.

✅ **Checkpoint D:** You opened the workflow run in Tracing and identified **both** agents' steps and the hand-off between them.

---

## Lab 03 complete — you built a governed multi-agent system 🎉

| You now have | Value |
|--------------|-------|
| `<initials>-claims-workflow` | One claim in → assessment → customer reply out, automatically |
| A visual orchestration | Architects can *read* the flow; no code to review |
| An end-to-end trace | Full audit trail + cost visibility across both agents |

### What you accomplished across all three labs

1. An agent grounded in **your** policy data (Lab 01).
2. A specialized communication agent (Lab 02).
3. A no-code **sequential workflow** that orchestrates them, fully observable (Lab 03).

All in the browser. Zero installs. Zero code.

### Where your engineers go next (roadmap)

- **Tools** — let agents *act* in your systems, not just read.
- **MCP integration** — connect agents securely to claims systems, CRMs, policy admin.
- **Evaluation pipelines** — automated groundedness and safety scoring before each release.

➡️ Back to the [Labs index](../README.md).
