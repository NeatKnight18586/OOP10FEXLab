# Heaven's Karinderya — Product Requirements

Course: OOP10 Final Laboratory Examination
Team size: 3 students
Implementation: C# Windows Forms (.NET 10)

## Required
1. Submit a GitHub repository named `OOP10FEXLab` and invite all members.
2. Include a Notepad-readable file containing all three members' names.
3. Build **two** interfaces: the **Queue Monitor** and the **Kiosk**.
4. Retrieve available package meals from a database or Notepad-readable file.
5. Save every submitted order to a transaction record.
6. Integrate the instructor's provided connection and query code when received.

## Kiosk UI
- Heaven's Karinderya branding; warm red, cream and gold.
- Food categories, individual meal cards, description and price.
- Add to cart, item quantities, total price, confirmation.
- New confirmed orders are saved with a unique order number and status Preparing.
- Show errors for missing menu files, invalid meals and failed saves.

## Queue Monitor UI
- Large NOW SERVING and PREPARING columns; readable at a distance.
- Show order numbers prominently; empty states should be understandable.
- Refresh automatically from saved transactions.
- Staff actions: F9 shows Call Next (Preparing to Serving) and Complete Oldest (Serving to Completed).
- Use visible labels, not color alone.

## Storage contract
Menu: `Data/menu.json`, an editable text file.
Local transactions: `%LOCALAPPDATA%/HeavensKarinderya/transactions.json`.
Order data: number, creation time, list of meals, quantities, unit prices, total, status.
The local file supports two forms/processes on the same Windows user account. A teacher-supplied shared database adapter is required for independent devices.

## Optional
Splash screen and launcher improve presentation but are not explicit exam requirements. Prioritize the two interfaces, file loading, and transaction saving.

## Acceptance tests
- Open both windows.
- Place two meal orders; each receives a different number.
- Both numbers appear under Preparing without restarting the monitor.
- Call Next moves the oldest number to Now Serving.
- Complete Oldest removes it from active service.
- Close and reopen the program; saved order records remain available.
- Edit the menu text and verify the change is shown after restarting the kiosk.
- Build on Windows and verify both screens at 1366x768.
