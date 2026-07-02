# Lab 01 — Agent 1: Claims Assessor

**Goal:** Build an agent that reads a submitted insurance claim and assesses coverage **grounded only in your uploaded policy documents** — then test it and inspect what it actually did.

**You'll learn:** how instructions shape behavior, how file-upload knowledge grounds an agent in *your* data, and where to observe a run.

**Time:** ~15 minutes · **Prerequisite:** [Lab 00](./00-prerequisites.md) complete.

---

## Part A — Create the agent

1. From the top menu, select **Build**.
2. Open the **Agents** tab.
3. Select **+ New agent** > **Build an agent**.
4. In **Name**, enter your tagged name: `<initials>-claims-assessor`.
5. In **Model**, select the shared chat model from Lab 00 (e.g. `gpt-4.1`).
6. Leave other settings at their defaults for now.

✅ **Checkpoint A:** A new agent named `<initials>-claims-assessor` exists and has a model assigned.

---

## Part B — Write the instructions

The **instructions** are how you describe the agent's job in plain English. This is where governance starts.

1. Find the **Instructions** (sometimes **System message**) box in the agent's setup pane.
2. Paste the following:

```
You are a Claims Assessor for an insurance company.
Read the submitted claim and assess it ONLY against the policy
documents provided in your knowledge. For each claim:
1. State whether the claim appears covered, partially covered, or not covered.
2. Cite the specific policy section or clause that supports your decision.
3. List any missing information needed to finalize the assessment.
If the policy does not address something, say so — never invent coverage.
Be concise and factual.
```

3. **Save** the agent.

> Read the two constraints you just gave it: *assess only against the provided policies* and *never invent coverage*. Those two lines are a risk-and-governance decision, written in English, with no code.

✅ **Checkpoint B:** Instructions are saved on the agent.

---

## Part C — Upload files (your data)

This is the "your data" moment. You ground the agent in your policy documents using the **File search** tool. Foundry uploads your files and builds a **vector store** (a managed search index) behind the scenes — you don't stand up any separate search service.

1. In the agent's **Setup** pane, find the **Tools** section and select **Add** (**+ Add tool**).
2. From the tool list, choose **File search**.
3. In the file search dialog, select **Upload files** (or **Select local files**) and pick the policy file **`HOME-2024-00871-homeowners`** from the workshop's `data/` folder — Markdown (`data/markdown/`) or PDF (`data/pdf/`) both work. You can also upload the auto and renters policies for richer exploration.
4. The dialog creates a **vector store** for these files (accept the default name, or rename it). Confirm/**Add** to attach the tool.
5. Wait for processing to finish — the file status moves from *in progress* to **completed** and the file is listed under the File search tool.
6. **Save** the agent.

> Behind the scenes Foundry parses, chunks, and embeds each file into the vector store, then uses hybrid (keyword + semantic) search at query time. The agent answers grounded in these files — it won't reach the open internet, and it won't invent coverage that isn't in your documents.

✅ **Checkpoint C:** The **File search** tool is attached, and at least one policy file shows status **completed** in its vector store.

🛠 If upload fails: confirm the file type is supported (PDF / DOCX / TXT / MD are all supported; max 512 MB per file); try a single smaller file first; ask an onsite helper to check storage permissions.

---

## Part D — Test in the playground

1. With the agent selected, open the **Playground** (or **Try in playground**).
2. **Test 1 — assess a claim.** Paste the following claim and send it:

```
Hi, I need to file a claim. Last Tuesday night a pipe under my
kitchen sink burst while we were asleep and water flooded the
kitchen floor and soaked the lower cabinets. We turned off the
water in the morning and called a plumber. The floor is warped
and we think the cabinets are ruined. Policy number HOME-2024-00871.
Not sure what's covered or what you need from me.
```

   - Expect: a coverage decision (covered / partial / not covered) that **cites a specific policy clause** from your uploaded file, plus any missing info.
3. **Test 2 — coverage question.** Paste the following question and send it:

```
Under policy HOME-2024-00871, is sudden water damage from a burst
pipe covered, and what deductible applies?
```

   - Expect: a grounded answer that references the policy, not a generic guess.
4. **Probe the grounding.** Ask something the policy does *not* cover:

```
Is damage from a meteor strike covered under my policy?
```

   - Expect: the agent says the policy doesn't address it — rather than inventing coverage. This proves the guardrail in your instructions is working.

> Note: `HOME-2024-00871` and "burst pipe / water damage" should match a scenario in the policy documents you uploaded. If your files use different policy numbers or coverage terms, adjust the text above to match — the point is to test a claim the policy *does* cover.

✅ **Checkpoint D:** Both test prompts return answers that cite your uploaded policy, and the out-of-scope probe is correctly declined.

---

## Part E — Observability: see what the agent did

Don't just trust the answer — inspect it.

1. **In-playground:** expand the latest response. Many runs show a **references / citations** panel indicating which file (and section) grounded the answer. Confirm it points at your uploaded policy.
2. **Tracing:** in the project, open the **Tracing** / **Observability** view.
   - Select your most recent run (thread).
   - Step through the **spans** to see the sequence: the prompt in, the file-search (vector store) retrieval, the model call, and the response out.
   - Note the **token usage** and **latency** on the run — this is your early read on cost and performance.

> Takeaway: every answer has a traceable lineage — *which document grounded it, how long it took, what it cost.* That's the difference between a demo and something you can govern in production.

✅ **Checkpoint E:** You located the citation/reference for an answer **and** opened the run in Tracing to see its steps.

---

## Lab 01 complete 🎉

You built an agent that is grounded in your own policy data, tested that the grounding holds, and inspected a run end to end.

| You now have | Value |
|--------------|-------|
| `<initials>-claims-assessor` agent | Grounded coverage assessment, no code |
| Uploaded policy files (file search vector store) | "Your data" boundary enforced |
| A traced test run | Auditable lineage + cost visibility |

➡️ **Next:** [Lab 02 — Agent 2: Customer Response](./02-agent-customer-response.md)
