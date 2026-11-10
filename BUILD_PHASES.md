# Build Phases — Three-Member Plan

## Phase 0 — Setup
- Use repository `NeatKnight18586/OOP10FEXLab`.
- Invite the other two students under Settings > Collaborators.
- Replace placeholders in `TEAM_MEMBERS.txt`.
- Each person creates a feature branch from main.
- Open `HeavensKarinderya.csproj` in Visual Studio 2026 on Windows.

## Phase 1 — UI first (parallel)

### Member 1 — Mark Jemiel P. Guevarra
Branch: `feature/queue-monitor`
File: `HeavensKarinderya/UI/QueueMonitorForm.cs`
- Polish the branded header, NOW SERVING and PREPARING card layout.
- Ensure big order numbers, contrast, clear empty state, live clock.
- Test F9 staff controls and monitor updates.
- Keep shared models and transaction storage unchanged.

### Member 2 — Kiosk
Branch: `feature/kiosk`
File: `HeavensKarinderya/UI/KioskForm.cs`
- Improve the starter kiosk: category sidebar, meal grid, price and food details.
- Add quantity decrease/remove from cart and cart total.
- Design a clear Place Order and order confirmation screen.
- Keep the same calls to `MenuCatalog.Load()` and `TransactionStore.PlaceOrder(...)`.
- Do not change Member 1's monitor file.

### Member 3 — Shared/integration
Branch: `feature/integration`
Files: `Services/`, `Models/`, `Data/menu.json`, launcher and splash.
- Maintain the menu text file and saved order records.
- Check unique order numbers and statuses.
- Refine splash/launcher; prepare database replacement for teacher's connection/query code.
- Test startup and end-to-end order flow.
- Add both names to `TEAM_MEMBERS.txt`.

## Phase 2 — Integrate
- Merge changes after reviewing each member's PR.
- Run Windows app with both screens.
- Place orders in kiosk → Preparing → Call Next → Now Serving → Complete Oldest.
- Restart and verify record persistence. Check malformed/missing menu file handling.
- If instructor supplies database code, replace the file provider and test again.

## Phase 3 — Submission
- Confirm three collaborator invitations accepted.
- Confirm repository name, team file, source code, screenshots and runnable Windows build.
- Each member explains their own file, events, data flow and test evidence.

## Codespaces
Codespaces provides a Linux .NET editing environment, **not** a Windows Forms visual runtime. Validate final UI on Windows, not in the Linux Codespace.
