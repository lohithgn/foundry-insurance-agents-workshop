# Lab 00 — Prerequisites

**Goal:** Confirm you can sign in, reach the shared project, and have everything you need before building. Five minutes of setup saves you the whole workshop.

> Most of these are **pre-provisioned by the presenter**. Your job in this lab is to *verify access*, not to create infrastructure.

---

## 1. What's already been set up for you

The presenter has prepared, ahead of time:

- A single **Microsoft Foundry account** with **one shared project** for all participants.
- A deployed **chat model** (e.g. GPT-4.1 / GPT-4o) with enough capacity for everyone running at once.
- **RBAC** so each participant can read and use the shared project.
- A **synthetic insurance dataset** — policy documents and sample claims (nothing real, nothing sensitive).
- **Content filters / Prompt Shields** configured on the project.
- **Application Insights** connected for monitoring.

You build the agents and workflow; the platform is ready and waiting.

---

## 2. Verify your access

1. Open a browser and go to **[ai.azure.com](https://ai.azure.com)**.
2. Sign in with the credentials provided by onsite staff.
3. At the top of the portal, confirm the **New Foundry** toggle is **on**.
4. From the project picker, select the **shared workshop project** (the presenter will name it on screen).
5. Confirm you can see the **Build** menu in the top navigation — this is where Agents, Knowledge, and Workflows live.

✅ **Checkpoint:** You're signed in, on **Foundry (new)**, inside the shared project, and can see the **Build** menu.

---

## 3. Confirm the model deployment

1. From the top menu, open **Build**, then look for the **Deployments** on the left hand navigation menu.
2. Confirm there is a deployed chat model (the presenter will tell you its name, e.g. `gpt-4.1`).
3. You don't need to create or change anything — just confirm it exists. You'll select it when creating each agent.

🛠 **If you don't see a model:** You may be in the wrong project. Re-check the project name with onsite staff.

---

## 4. Note your naming tag

If you're sharing this project with other participants, you'll all be creating agents in the same place — so names can collide. To keep everyone's work separate, give every asset you create a **unique tag**. The simplest choice is your **initials**, prepended (or appended) to each name.

- Your tag: `<initials>` → write it down now (e.g. initials "JD" → `jd`).
- Agent 1 will be `<initials>-claims-assessor`.
- Agent 2 will be `<initials>-customer-response`.
- Workflow will be `<initials>-claims-workflow`.

> This matters: if two people both create an agent called "claims-assessor", it gets confusing fast. Your tag is your namespace.
>
> **Working in your own private project?** The tag is optional — drop it and just name things `claims-assessor`, etc.

---

## 5. Locate your sample data

You'll need two things to paste/upload into the agents:

- **Sample prompts** — every claim and question you'll paste is **embedded inline in the labs** as copy-ready code blocks. Nothing to print or look up separately.
- **Policy documents** — the synthetic policy files you'll upload as Agent 1's knowledge live in the workshop's **`data/`** folder: editable Markdown in **`data/markdown/`** and ready-to-share PDFs in **`data/pdf/`** (the presenter will share the link/location). For the core labs you only need **`HOME-2024-00871-homeowners`** (either format).

Make sure you can:
- Open the lab docs (this folder) in your browser or viewer.
- Access the **policy document file(s)** to upload in Lab 01.

✅ **Checkpoint:** You have your `<initials>` tag written down, you can open the lab docs, and you can access the policy files to upload.

---

## 6. Quick readiness check

Before moving to Lab 01, confirm all of these:

- [ ] Signed in to **ai.azure.com** with **New Foundry** on.
- [ ] Inside the **shared workshop project**.
- [ ] **Build** menu visible.
- [ ] A **chat model** deployment exists (you know its name).
- [ ] Your **`<initials>` tag** is written down.
- [ ] You can open the **lab docs** and access the **policy files**.

🛠 **Troubleshooting**

| Symptom | Fix |
|---------|-----|
| Can't sign in | Re-check credentials with onsite staff; watch for trailing spaces. |
| No project listed | You may lack the role — flag an onsite helper to confirm your RBAC. |
| "New Foundry" toggle missing | Refresh the page; try a different browser tab. |
| No **Build** menu | You're likely outside the project or in classic Foundry — toggle New Foundry on and reselect the project. |

➡️ **Next:** [Lab 01 — Agent 1: Claims Assessor](./01-agent-claims-assessor.md)
