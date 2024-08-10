---

# [DSharpPlus](https://dsharpplus.github.io/DSharpPlus/) Bot Template

This repository provides a template for creating a Discord bot using DSharpPlus in C#. It includes a basic setup for handling commands and events. 

## Project Structure

The project is structured as follows:

```
/ArtcordAdminBot
│
├── /Features
│   ├── CommandsModule.cs      # Contains command handlers for the bot
│   └── EventsModule.cs        # Contains event handlers for the bot
│
├── appsettings.json           # Configuration file for bot token and other settings
├── Program.cs                 # Entry point of the application, sets up and runs the bot
└── ArtcordAdminBot.csproj     # Project file, includes dependencies and build settings
```

## Setup

1. **Clone the Repository:**
   ```sh
   git clone <repository-url>
   cd DSharpPlusTemplate
   ```

2. **Install Dependencies:**
   Make sure you have the [.NET SDK](https://learn.microsoft.com/en-us/dotnet/core/install/windows) installed. If not, install it via your package manager.

   For Arch Linux:
   ```sh
   sudo pacman -S dotnet-sdk
   ```

3. **Configure Your Bot Token:**
   Update `appsettings.json` with your bot's token:
   ```json
   {
     "Token": "YOUR_BOT_TOKEN_HERE"
   }
   ```
   You can get it from the [Discord Dev Portal](https://discord.com/developers/applications)

4. **Build the Project:**
   ```sh
   dotnet build
   ```

5. **Run the Bot:**
   ```sh
   dotnet run
   ```

## Features

- **CommandsModule.cs:**
  - Contains the `CommandsModule` class with example commands (`/ping` and `/echo`).
  - Commands support optional parameters for sending responses as embedded messages or plain text.

- **EventsModule.cs:**
  - Contains the `EventsModule` class with an example event handler for when the bot is ready.

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.
