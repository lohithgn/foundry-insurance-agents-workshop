# Sample Claims Dataset

Synthetic insurance claim submissions for the workshop. The text is intentionally **messy and conversational** to mimic how real customers describe incidents. Paste any of these into the **Claims Assessor** agent (Lab 01) or the **workflow** (Lab 03).

Each claim references a policy in `../policies/`. An **answer key** for presenters/onsite staff is at the bottom — don't show it to the agent; use it to check whether the agent's assessment is correct.

---

## Claims used directly in the labs

### CLM-1001 — Burst pipe (Lab 01 test)
> Hi, I need to file a claim. Last Tuesday night a pipe under my kitchen sink burst while we were asleep and water flooded the kitchen floor and soaked the lower cabinets. We turned off the water in the morning and called a plumber. The floor is warped and we think the cabinets are ruined. Policy number HOME-2024-00871. Not sure what's covered or what you need from me.

### CLM-1002 — Storm + falling branch (Lab 03 workflow test)
> I want to report a claim on policy HOME-2024-00871. During the storm two nights ago, a tree branch fell and broke our living room window. Rain got in overnight and damaged the carpet and a sofa before we could cover the window. We've taken photos. What's covered and what do you need from us to proceed?

---

## Additional claims for self-exploration

### CLM-1003 — Basement flood (should be DECLINED)
> Filing a claim on HOME-2024-00871. The river behind our house overflowed after three days of rain and about a foot of water came into the finished basement. The drywall, carpet, and a treadmill are all damaged. Please advise on next steps.

### CLM-1004 — Slow leak over months (should be DECLINED)
> Policy HOME-2024-00871. We noticed the cabinet under the bathroom sink had gone soft and smelled musty. The plumber said a fitting had been slowly dripping for probably a couple of months and the wood and subfloor are rotted. How much will you cover?

### CLM-1005 — Meteorite (NOT ADDRESSED / DECLINED)
> This is going to sound strange but something fell from the sky last night and put a hole in our roof — the news says it may have been a small meteorite. There's a dent in the attic and rain came in. Policy HOME-2024-00871. Is this covered?

### CLM-1006 — Theft after break-in (COVERED, needs police report)
> Someone broke in through the back door while we were away for the weekend and took a laptop, a camera, and some jewelry. We're on HOME-2024-00871. We've reported it to the police. What do you need to process this?

### CLM-1007 — Hail roof damage (COVERED)
> Big hailstorm yesterday. Our roof has a bunch of dents and two windows cracked, and there's water staining on the upstairs ceiling now. Policy HOME-2024-00871. Roofer is coming Friday to give an estimate.

### CLM-1008 — Auto: deer strike (COVERED, comprehensive)
> I hit a deer on the county road last night. The front bumper, hood, and one headlight are smashed. Nobody hurt. My policy is AUTO-2024-03345. Do I pay a deductible?

### CLM-1009 — Auto: rideshare crash (DECLINED)
> I was in an accident while picking up a passenger for a rideshare app — rear-ended someone at a light. Damage to my front end. Policy AUTO-2024-03345. I don't have any rideshare add-on but figured I'd ask.

### CLM-1010 — Renters: neighbor's pipe (COVERED under renters)
> The apartment above mine had a pipe burst and water came through my ceiling and ruined my couch, a rug, and some books. I rent, so I'm on RENT-2024-01290. The building damage is the landlord's problem but my stuff is wrecked. What can I claim?

### CLM-1011 — Partial info / vague (NEEDS MORE INFORMATION)
> Hi there's been some damage at my house and I'd like to make a claim please. HOME-2024-00871. Let me know.

---

## Answer key (presenter / onsite staff only — do NOT give to the agent)

| Claim | Expected outcome | Policy basis | Deductible | Notes |
|-------|------------------|--------------|------------|-------|
| CLM-1001 | **Covered** | HOME 4.2 Water Damage (burst pipe) | $1,000 | Ask for date, photos, two repair estimates |
| CLM-1002 | **Covered** | HOME 4.3 Falling Objects + 4.1 resulting interior damage | $1,000 | Photos already provided; ask for estimates |
| CLM-1003 | **Not covered** | HOME 5.1 Flood excluded | — | Suggest separate flood insurance |
| CLM-1004 | **Not covered** | HOME 5.7 Continuous Seepage excluded | — | Gradual, not sudden & accidental |
| CLM-1005 | **Not addressed** | HOME 4.3 covers terrestrial falling objects only | — | Policy silent on meteorites; do not invent coverage |
| CLM-1006 | **Covered** | HOME 4.5 Theft | $1,000 | Requires police report; itemized list + proof of ownership |
| CLM-1007 | **Covered** | HOME 4.1 Windstorm/Hail (+ resulting interior) | $1,000 | Ask for roofer estimate |
| CLM-1008 | **Covered** | AUTO 2.3 Comprehensive (animal strike) | $250 | Comprehensive, not collision |
| CLM-1009 | **Not covered** | AUTO 3.1 commercial/ride-share use excluded | — | No ride-share endorsement |
| CLM-1010 | **Covered** | RENT 2.6 Sudden water discharge | $500 | Building damage is landlord's; contents covered at ACV |
| CLM-1011 | **Needs more info** | Insufficient detail to assess | — | Ask for cause, date, what was damaged, photos |
