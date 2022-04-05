<br />

# FlowerShop.Cli

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="about-the-project">About The Project</a>
      <ul>
        <li><a href="#builtwith">Built With</a></li>
      </ul>
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#prerequisites">Prerequisites</a></li>
        <li>
          <a href="#build-and-run">Build and run</a>
          <ul>
            <li><a href="#with-net-sdk">With .NET SDK</a></li>
            <li><a href="#with-docker">With Docker</a></li>
          </ul>
        </li>
        <li><a href="configuring-available-bundles">Configuring available bundles</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
    <li><a href="#to-do">To do</a></li>
    <li><a href="#known-issues">Known issues</a></li>
    <li><a href="#contributing">Contributing</a></li>
    <li><a href="#license">License</a></li>
  </ol>
</details>


<!-- ABOUT THE PROJECT -->
## About The Project

FlowerShop helps you run your wonderful flower shop (I'm kidding, it's just a coding exercise).  
As of now it provides a CLI application with only one implemented feature: the optimization of the number of bundles needed to satisfy a given order.

The architecture reflect in some way my take on Clean Architecture even though it's far from complete.

Some concepts/patterns of Domain Driven Design are also used in the design of the domain layer.

### Built With

* [.NET 6](https://dotnet.microsoft.com/en-us/)
* [OR Tools](https://developers.google.com/optimization/)
* [Spectre.Console](https://spectreconsole.net/)
* [xUnit](https://xunit.net/)
* [MediatR](https://github.com/jbogard/MediatR)
* [Serilog](https://serilog.net/)

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- GETTING STARTED -->
## Getting Started

You can build (and run) the project either with the .NET 6 SDK or Docker.

### Prerequisites

* [.NET 6](https://dotnet.microsoft.com/en-us/download) or [Docker](https://docs.docker.com/get-started/)
* The source code
   ```sh
   git clone https://github.com/your_username_/Project-Name.git
   ```

<p align="right">(<a href="#top">back to top</a>)</p>

### Build and run

#### With .NET SDK

1. Enter the repo folder
   ```sh
   cd flowershop
   ```
2. Restore project dependencies
   ```sh
   dotnet restore "FlowerShop.Cli/FlowerShop.Cli.csproj"
   ```
3. Publish the project
   ```sh
   dotnet publish "FlowerShop.Cli/FlowerShop.Cli.csproj" -c Release -o ./your_preferred_output_path
   ```
4. Run
   ```sh
   ./your_preferred_output_path/FlowerShop.Cli create-quote "10 R12" "15 L09" "13 T58"
   ```

<p align="right">(<a href="#top">back to top</a>)</p>

#### With Docker

1. Enter the repo folder
   ```sh
   cd flowershop
   ```
2. Build docker image
   ```sh
   ./build-flowershop-cli-docker-image.sh
   ```
3. Run
   ```sh
   docker run --rm nico/flowershop-cli create-quote "10 R12" "15 L09" "13 T58"
   ```

<p align="right">(<a href="#top">back to top</a>)</p>

### Configuring available bundles

You can change available bundles configuration in the `AddConsoleAppInfrastructure` method of [ServiceCollectionExtensions](FlowerShop.Cli/Infrastructure/ServiceCollectionExtensions.cs) class, where `AddFlowerShopApplicationInfrastructure` method is invoked.

<!-- USAGE -->
## Usage

FlowerShop.Cli expect the first argument to be a command and the following ones the command's arguments.
```sh
$ FlowerShop.Cli <command> <command-argument-1> <command-argument-2> ...
```

### Commands

#### create-quote

`create-quote` command expect the rows of an order to be fed as arguments.  
The expected format of each order row is `quantity product-code`.  
Please note that because of the blank space between quantity and product code, rows must be wrapped in double quotes.

Examples:
```sh
$ FlowerShop.Cli create-quote "10 R12" "15 L09" "13 T58"
```
```sh
$ docker run --rm nico/flowershop-cli create-quote "10 R12" "15 L09" "13 T58"
```

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- TO DO -->
## To do

- Fix known issues
- Write a description of the architecture
- Introduce exception handler in FlowerShop.Cli to print more meaningful information based on exception type
- Bring code coverage to 80%
- Support multiple currency
- Implement file storage backend for available bundles 

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- KNOWN ISSUES -->
## Known issues

- When running the application though the Docker image, the currency symbol is not rendered correctly

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- CONTRIBUTING -->
## Contributing

Thank you, but there is no need to contribute to this project, still, if you want we can talk about it.

<p align="right">(<a href="#top">back to top</a>)</p>



<!-- LICENSE -->
## License

Distributed under the MIT License. See [LICENSE](LICENSE) for more information.

<p align="right">(<a href="#top">back to top</a>)</p>