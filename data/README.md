# Workshop Data

Synthetic — but realistic — insurance datasets for the Foundry agents workshop. Everything here is fictional (made-up names, addresses, policy numbers) and safe to upload to a sandbox.

## What's here

```
data/
  markdown/                         ← editable source: upload as the Claims Assessor's knowledge
    HOME-2024-00871-homeowners.md   ← primary policy (used by the labs)
    AUTO-2024-03345-auto.md         ← auto policy (self-exploration)
    RENT-2024-01290-renters.md      ← renters policy (self-exploration)
    sample-claims.md                ← 11 messy, real-sounding claims + answer key
    claims.csv                      ← same claims in structured form
    policyholders.csv               ← quick policy/coverage reference
  pdf/                              ← PDF versions (real policy-document look)
    HOME-2024-00871-homeowners.pdf
    AUTO-2024-03345-auto.pdf
    RENT-2024-01290-renters.pdf
    sample-claims.pdf
    claims.pdf
    policyholders.pdf
  README.md
```

> **Markdown or PDF for upload?** Either works as the agent's knowledge. The `markdown/` files are easiest to edit; the `pdf/` versions look like real policy documents and are nice for a polished demo or handout. Upload whichever set you prefer — don't upload both copies of the same policy (it duplicates the knowledge).

## How it's used in the labs

| File(s) | Used as | Where |
|---------|---------|-------|
| `markdown/*.md` (or `pdf/*.pdf`) policies | Agent 1 **knowledge base** (file upload) | Lab 01, Part C |
| `markdown/sample-claims.md` → CLM-1001 | Test prompt for the Assessor | Lab 01, Part D |
| `markdown/sample-claims.md` → CLM-1002 | Full-workflow input | Lab 03, Part C |
| remaining claims | Self-exploration after the workshop | — |

### Minimum upload for the labs
For the core labs you only need **`HOME-2024-00871-homeowners`** uploaded (Markdown or PDF) — both lab test claims reference that policy. Upload all three policies if you want participants to explore auto and renters scenarios too.

## Design notes (why these are realistic)

- **Grounded outcomes.** Every sample claim maps to a specific policy clause, so the agent can cite a real section (e.g., "Section 4.2 Water Damage"). The expected results are in the answer key at the bottom of `sample-claims.md`.
- **Full spread of outcomes.** Covered, partially covered, **not covered** (flood, seepage, ride-share), **not addressed** (meteorite — proves the agent won't invent coverage), and **needs-more-info** — so the demo shows real judgement, not just happy-path approvals.
- **Messy customer voice.** Claims are written the way customers actually write them: run-on, missing details, emotional — which is what makes the grounding and the "missing information" step impressive.
- **Consistent universe.** One fictional carrier (Northwind Mutual), one town (Maplefield), interlinked policyholders and claims, so it reads like one book of business.

## Format note

Policies are provided as **Markdown** (`.md`, in `markdown/`) for easy reading and editing, and as **PDF** (in `pdf/`) for a real policy-document look. Foundry's agent file-upload accepts both (plus DOCX and TXT). Markdown uploads keep the structure the agent cites; PDFs are nicer as handouts. The PDFs are generated from the Markdown — if you edit a policy, re-export the PDF to keep them in sync.

> ⚠️ All data is synthetic and for training only. It is not real insurance, not real people, and must not be used for any actual coverage decision.
