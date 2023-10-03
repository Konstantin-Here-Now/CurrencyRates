# Currency Rates

Console app for get currency exchange rates using Russian Central Bank API.

## Contents

- [Requirements](#requirements)
- [Quick start](#quick-start)
- [Configuration](#configuration)
- [Documentation](#documentation)
- [Technologies](#technologies)
- [File structure](#file-structure)
- [Testing](#testing)

## Requirements

To use the project, you need to install [.NET](https://dotnet.microsoft.com/en-us/download) (version 7.0 recommended).

## Quick start
First you need to build solution, executing:

```bash
dotnet build
```

Execute following command to run the program:

```bash
dotnet run --project src/CurrencyRates/
```

To stop the program press Enter.

## Configuration

Configuration can be found in `app.config`. In XML-elements `add` attribute `value` is responsible for configs.

| Key                | Description                                                           | Default value                                  |
|--------------------|-----------------------------------------------------------------------|------------------------------------------------|
| cbURL              | URL-address of CB RF API. Change only in case of API url changes.     | https://cbr.ru/DailyInfoWebServ/DailyInfo.asmx |
| resultsFilename    | Name of file (including its extention) with results of API responses. | results.txt                                    |
| serializeNeeded    | Boolean value: is response should be JSON-serialized.                 | true                                           |
| timerIntervalHours | Interval of API calls, in hours.                                      | 24                                             |

## Documentation

To see documentation of the project, execute command:

```bash
docfx docfx_project/docfx.json --serve
```

Then follow the given link to localhost. Press `Ctrl+C` to shut down.

## Technologies

- [C#](https://learn.microsoft.com/ru-ru/dotnet/csharp/)
- [.NET ](https://dotnet.microsoft.com/en-us/)

## File structure

.
├── CurrencyRatesSolution.sln
├── README.md
├── src
│   └── CurrencyRates
│       ├── app.config
│       ├── CbSoapEnvelope.cs
│       ├── CurrencyRates.csproj
│       ├── CurrencyRates.sln
│       ├── CursOnDate.cs
│       ├── FileConnection.cs
│       ├── Logging.cs
│       ├── Program.cs
│       ├── SoapConnection.cs
│       └── XMLOperationsList.cs
└── tests
    └── CurrencyRatesProjectTests
        ├── CurrencyRatesProjectTests.csproj
        ├── UnitTest1.cs
        └── Usings.cs


## Testing

Code is partly covered with unit-tests. Execute following command to run them:

```bash
dotnet test
```