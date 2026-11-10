# Heaven's Karinderya

A three-member OOP10 final laboratory project using C# Windows Forms (.NET 10).

## Requirements
- Queue Monitor showing Preparing and Now Serving
- Self-service Kiosk with food items and cart
- Meal packages loaded from text data or database
- Saved order transactions and order numbers shown on the queue
- GitHub collaborators and `TEAM_MEMBERS.txt`

## Run on Windows
Open `HeavensKarinderya/HeavensKarinderya.csproj` in Visual Studio 2026 with the .NET desktop development workload and .NET 10. Run the launcher to open both screens. Place an order in the kiosk and check the Preparing column on the monitor. Press F9 on the monitor for staff queue controls.

## Codespaces
Use **Code > Codespaces > Create codespace on main**. The devcontainer supplies the .NET 10 SDK and C# extensions for editing source files. Native WinForms UI must be run and tested on Windows; Linux Codespaces cannot show Windows Forms.

## Data
`HeavensKarinderya/Data/menu.json` contains the meal catalog. Transactions are written to `%LOCALAPPDATA%/HeavensKarinderya/transactions.json` on Windows. This starter storage works on one computer. The teacher-provided database query and connection code can be integrated afterward.

## Responsibilities
| Member | Focus |
| --- | --- |
| 1 — Mark Jemiel P. Guevarra | Queue Monitor and staff queue controls |
| 2 — to be completed | Kiosk design, food cards, cart, and order flow |
| 3 — Sean Caling / LEAD | Splash screen, menu and transaction storage, integration and tests |

See `PRD.md` and `BUILD_PHASES.md` for the requirements and work schedule.

## Progress
Initial project structure and screens are included as a starting point. Verify the Windows build and complete the UI acceptance checks before submitting.
