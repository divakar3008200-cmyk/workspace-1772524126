Project: Transportation Management (Console)

Overview

This is a simple .NET console application that implements a menu-driven CRUD system for managing transportation assets (vehicles). The application uses a static in-memory List<T> as storage and keeps all business logic inside Program.cs. The model is a single class Transport placed in the Models folder. The application runs until the user selects the Exit option and interacts entirely via console input and output.

Model

Transport (namespace: dotnetapp.Models)
- Purpose: Represents a single transportation asset (vehicle) with identifying, descriptive, and operational fields.
- Properties (name: Type):
  - Id: int
  - VehicleType: string  // e.g., Bus, Truck, Car
  - LicensePlate: string
  - Manufacturer: string
  - Model: string
  - Year: int
  - Capacity: int  // number of passengers or payload capacity
  - Status: string  // e.g., Active, In Maintenance, Retired
  - LastMaintenanceDate: DateTime

All properties are public auto-properties. Id is assigned by the application (auto-increment) and should not be read from user input.

Program and Method Signatures

All application logic is contained in Program.cs (namespace dotnetapp). Key methods and their signatures (visibility and return types) used by the application flow are:

- static void Main(string[] args)
  - Entry point. Presents an interactive menu and loops until Exit is chosen.

- static void CreateTransport()
  - Creates a new Transport instance, validates inputs, assigns an auto-increment Id, and adds the instance to the in-memory list. On success it writes: "Transport created with Id: {id}" where {id} is the newly assigned integer Id.

- static void ListTransports()
  - Prints a summary line for each Transport in the list. If the list is empty it prints: "No transports found.". Summary lines follow the exact textual structure used by the application: "Id: {Id} | {VehicleType} | {LicensePlate} | {Manufacturer} {Model} | Year: {Year} | Capacity: {Capacity} | Status: {Status}".

- static void ViewTransport()
  - Prompts the user for an integer Id and, if found, prints detailed information using labels exactly as: "Transport Details (Id: {Id})" followed by lines: "VehicleType: {VehicleType}", "LicensePlate: {LicensePlate}", "Manufacturer: {Manufacturer}", "Model: {Model}", "Year: {Year}", "Capacity: {Capacity}", "Status: {Status}", "LastMaintenanceDate: {yyyy-MM-dd}". If the Id is not found it prints: "Transport not found." (case and wording important).

- static void UpdateTransport()
  - Prompts the user for an Id to update. If not found it prints: "Transport not found.". Otherwise it prompts field-by-field showing current values and allows blank input to keep existing values (user instruction printed: "Leave blank to keep existing value."). For numeric/date fields, invalid input results in console messages exactly as: when Year is invalid during update: "Invalid year entered. Keeping existing value."; when Capacity invalid: "Invalid capacity entered. Keeping existing value."; when LastMaintenanceDate invalid: "Invalid date format. Keeping existing value." After successful update it prints: "Transport updated.".

- static void DeleteTransport()
  - Prompts for an Id to delete. If the Id does not exist it prints: "Transport not found.". On successful removal it prints: "Transport deleted.".

- static void SearchByLicensePlate()
  - Prompts: "Enter License Plate to search: " (the prompt uses ReadRequiredString semantics). Searches case-insensitively and prints summary lines for all matching transports using the same summary format as ListTransports. If no matches are found it prints: "No transports found with that license plate.".

Helper Methods (signatures & behaviors)

These helper methods encapsulate user input and validation logic. Their exact prompt strings and validation messages are part of the application's observable behavior and must be treated as fixed text by any automated checks.

- static string ReadRequiredString(string prompt)
  - Repeatedly writes the provided prompt (e.g., "Vehicle Type (e.g., Bus, Truck, Car): ") and reads input. If the input is null/empty/whitespace it prints exactly: "This field is required." and repeats the prompt. Upon valid input it returns the trimmed string.

- static string ReadStringAllowEmpty(string prompt)
  - Writes prompt and returns the trimmed string; empty input is returned as empty string (used during updates to keep existing values when blank).

- static int ReadInt(string prompt)
  - Writes the prompt and expects an integer. For invalid integer input it prints: "Please enter a valid integer." and reprompts until an integer is provided. Returns the parsed integer.

- static int ReadIntInRange(string prompt, int min, int max)
  - Writes the prompt and expects an integer between min and max. For invalid input it prints exactly: "Please enter an integer between {min} and {max}." and repeats until a valid value is entered. Returns the valid integer.

- static DateTime ReadDate(string prompt)
  - Writes the prompt and expects a date in format yyyy-MM-dd. For invalid input it prints exactly: "Please enter a date in yyyy-MM-dd format." and repeats until a valid date is entered. Returns the parsed DateTime.

Printing Helpers

- static void PrintTransportSummary(Transport t)
  - Writes a single-line summary using the format: "Id: {Id} | {VehicleType} | {LicensePlate} | {Manufacturer} {Model} | Year: {Year} | Capacity: {Capacity} | Status: {Status}".

- static void PrintTransportDetails(Transport t)
  - Writes multiple labeled lines as described in ViewTransport(), with LastMaintenanceDate formatted as yyyy-MM-dd.

Console Interaction: Exact Menu and Prompts

On each loop, the Main method prints the top-level menu exactly as:

=== Transportation Management ===
1. Create Transport
2. List All Transports
3. View Transport by Id
4. Update Transport
5. Delete Transport
6. Search by License Plate
7. Exit
Select an option: 

The application recognizes input strings "1" through "7" for the actions above. Unrecognized input prints: "Invalid option. Please try again." The application prints "Exiting application. Goodbye!" when the Exit choice is made.

Data Storage and Lifespan

- Storage: a private static List<Transport> named transports held in Program.cs.
- Auto-increment Id: a private static int nextId starts at 1 and is incremented when creating a new transport. Id is assigned by CreateTransport() as t.Id = nextId++.
- No persistence: data is kept in memory only for the lifetime of the application run.

Validation Rules (summary with exact messages)

- Required string fields: VehicleType, LicensePlate, Manufacturer, Model, Status. Prompt messages vary by field but the helper enforces non-empty and prints: "This field is required." for empty input.
- Year: accepted integer range is 1900 to current year. ReadIntInRange enforces this and reports: "Please enter an integer between {min} and {max}." when input invalid.
- Capacity: integer >= 0. During creation ReadIntInRange (0..100000) enforces. During update invalid entries print: "Invalid capacity entered. Keeping existing value.".
- Dates: LastMaintenanceDate must be entered in yyyy-MM-dd format; invalid entries prompt: "Please enter a date in yyyy-MM-dd format.". During update invalid date entries produce: "Invalid date format. Keeping existing value.".

Behavioral Notes & Examples (how the flow appears to a user)

- Creating a Transport example sequence of prompts (exact prompt fragments are important):
  - Vehicle Type (e.g., Bus, Truck, Car): 
  - License Plate: 
  - Manufacturer: 
  - Model: 
  - Year: 
  - Capacity (number of passengers / payload): 
  - Status (Active, In Maintenance, Retired): 
  - Last Maintenance Date (yyyy-MM-dd): 
  - On success, console prints: Transport created with Id: {id}

- Listing: prints header "--- All Transports ---" then one summary line per transport. If none: "No transports found.".

- Viewing by Id: prompts "Enter Id: " and prints the detailed block headed by: "Transport Details (Id: {Id})". Property labels are printed exactly (e.g., "LicensePlate: {value}"). If not found: "Transport not found.".

- Updating: prompts "Enter Id to update: ". If found, the program prints: "Leave blank to keep existing value." and then prompts for each field including showing the current value in parentheses (e.g., "Vehicle Type (Bus): "). Messages for invalid numeric/date updates are exact and preserve existing values when invalid input is provided. After completion it prints: "Transport updated.".

- Deleting: prompts "Enter Id to delete: ". On success prints: "Transport deleted."; if missing prints: "Transport not found.".

- Searching: prompts "Enter License Plate to search: ", does a case-insensitive exact match and prints matching summary lines or "No transports found with that license plate." if none match.

Build, Run, and Verification

- Project file: dotnetapp.csproj (core console application). To build and run from the workspace root the commands are:
  - dotnet build dotnetconsole/dotnetapp
  - dotnet run --project dotnetconsole/dotnetapp

- The application logs and prompt strings described above are intentionally exact because automated verification or test harnesses depend on the precise text and formats (e.g., menu lines, validation messages, summary/detail labels, date format yyyy-MM-dd).

Implementation Constraints and Design Choices

- Single-model requirement: only one model class (Transport) resides under Models/Transport.cs.
- Single-file logic requirement: all runtime logic (menu loop, CRUD functions, helpers, and printing) resides in Program.cs. Helper methods are private static and return primitive types or void as described above.
- Reflection-friendly design: The application exposes internal state in private static fields (transports and nextId) that tests may access via reflection for automated checks; tests that interact with program behavior often simulate console input and capture Console output.

Summary

This console application offers a compact, deterministic, and easily verifiable Transportation CRUD system using an in-memory static List. The description above documents the Transport model, the Program methods with their signatures and return types, the exact console prompts and messages, input validation rules, and the expected formatting of summary and detail prints. These specifics capture the exact textual and behavioral contract that external verification (for example, automated tests) will rely upon when exercising the application flows.