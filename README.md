# Renewable Electricity Explorer

![C#](https://img.shields.io/badge/C%23-.NET-512BD4?logo=dotnet) ![XML](https://img.shields.io/badge/Data-XML%20%2F%20XPath-orange)

A C# console application that queries a real-world dataset of global renewable electricity production in 2021. Users can explore energy data by country, by source type, or by filtering countries within a custom percentage range of renewable output.

---

## Tech Stack

- **Language:** C# (.NET)
- **Data Format:** XML, queried via XPath
- **Core APIs:** `System.Xml`, `System.Xml.XPath`, `XmlDocument`, `XmlNodeList`

---

## Features

- **Query by Country** — Display all renewable energy sources for a selected country, including GWh output, percent of total electricity, and percent of renewables
- **Query by Source Type** — Compare all countries side-by-side for a specific source (e.g. Solar, Wind, Hydro)
- **Query by Percentage Range** — Filter countries by their total renewable energy share using a custom min/max range
- **Session Logging** — Saves the user's last query parameters to `Electricity.xml` on exit
- **Input Validation** — Handles non-integer input and invalid menu selections gracefully with inline error feedback

---

## Installation

> Requires [.NET SDK](https://dotnet.microsoft.com/download) (6.0 or later)

```bash
git clone https://github.com/your-username/renewable-electricity-explorer.git
cd renewable-electricity-explorer
dotnet run
```

Ensure `renewable-electricity.xml` is located at the expected relative path (`../../../renewable-electricity.xml`) from the project output directory, or update `XmlFile` in `Program.cs` accordingly.

---

## Usage

On launch, the application presents a menu:

```
Renewable Electricity Production in 2021
========================================

Enter 'C' to Select a country, 'S' to select a specific source, 'P' to select
a % range of renewable production or 'X' to quit:
```

| Option | Description |
|--------|-------------|
| `C` | Browse and select a country to view its full renewable energy breakdown |
| `S` | Select a source type and compare output across all countries |
| `P` | Enter a min/max renewable percentage to filter matching countries |
| `X` | Exit and save session query to `Electricity.xml` |

---

## Key Implementation Details

- XPath expressions are constructed dynamically at runtime based on user input, enabling flexible single-query data retrieval without loading all records into memory
- A shared `getParameters()` helper method keeps XML traversal logic DRY — it clears and repopulates a static list for each attribute query
- Output is formatted using `PadRight` / `PadLeft` for aligned console columns across variable-length country and source names