# Lab 02 — Agent 2: Customer Response

**Goal:** Build a second, specialized agent that takes a coverage **assessment** and turns it into a clear, empathetic, plain-language message to the customer. No knowledge files needed — this one is pure instructions.

**You'll learn:** why roles are separated (specialization, reuse, governance), and how a prompt-only agent differs from a knowledge-grounded one.

**Time:** ~10 minutes · **Prerequisite:** [Lab 01](./01-agent-claims-assessor.md) complete.

---

## Part A — Create the agent

1. From the top menu, select **Build** → **Agents** tab.
2. Select **+ New agent**.
3. In **Name**, enter: `<initials>-customer-response`.
4. In **Model / Deployment**, select the same shared chat model (e.g. `gpt-4.1`).

> Note: this agent has **no File search tool / file upload**. It doesn't need policy files — it works from the assessment text it's given. That's what makes it faster to build.

✅ **Checkpoint A:** A new agent named `<initials>-customer-response` exists with a model assigned.

---

## Part B — Write the instructions

1. Open the **Instructions** box.
2. Paste the following:

```
You are a Customer Response writer for an insurance company.
You receive a claim assessment from the Claims Assessor.
Turn it into a clear, warm, plain-language message to the customer.
- Lead with empathy and acknowledge their situation.
- Explain the coverage decision in simple terms (no jargon, no clause numbers).
- If information is missing, politely list exactly what the customer needs to provide.
- Keep it short, respectful, and professional. Do not promise anything
  the assessment did not state.
```

3. **Save** the agent.

> The last line — *don't promise anything the assessment didn't state* — keeps this agent from over-committing. Each agent is guardrailed for its own job.

✅ **Checkpoint B:** Instructions are saved.

---

## Part C — Test in the playground

You'll simulate the hand-off that the workflow will automate later.

1. Open the **Playground** for `<initials>-customer-response`.
2. Go back to **Lab 01's** Claims Assessor playground and **copy the assessment text** it produced for the burst-pipe claim (covered/partial + clause + missing info).
3. Paste that assessment into this agent's playground and send it.
   - Expect: a short, warm, jargon-free customer message that explains the decision and politely lists any missing info — **without** clause numbers or insurance jargon.
4. **Contrast test.** Notice how the *same* assessment becomes a completely different output: technical → human. That's specialization in action.

🛠 No assessment handy? Paste this stand-in:

```
Assessment: Claim appears COVERED under Section 4.2 (Water Damage).
Missing information: date of incident and two repair quotes.
```

✅ **Checkpoint C:** The agent returns an empathetic, plain-language message based on the assessment you pasted.

---

## Part D — Observability

1. **In-playground:** expand the response. Since there's no knowledge file, you should see **no document citations** here — a useful contrast with Agent 1. This agent is reasoning purely from the input text.
2. **Tracing:** open the project's **Tracing** / **Observability** view and select this run.
   - Step through the spans: prompt in → model call → response out (note there's **no retrieval step**, unlike the Assessor).
   - Check **token usage** and **latency** — typically lower than Agent 1 since there's no file search.

> Takeaway: two agents, two very different runtime shapes. The platform lets you observe and govern each role independently — costs, behavior, and risk are visible per agent.

✅ **Checkpoint D:** You confirmed this run has no retrieval/citation step and viewed its trace.

---

## Why two agents instead of one?

| Reason | What it buys you |
|--------|------------------|
| **Specialization** | Each agent does one job well; instructions stay focused and testable. |
| **Reuse** | The response writer can serve other workflows, not just this one. |
| **Governance** | You can audit, guardrail, and monitor each role separately. |

---

## Lab 02 complete 🎉

You now have both halves of the pipeline: an agent that *assesses* and an agent that *communicates*.

➡️ **Next:** [Lab 03 — Workflow: Sequential Orchestration](./03-workflow-orchestration.md)
