# ArtcordAdminBot

This project is made using C# and [DSharpPlus](https://dsharpplus.github.io/DSharpPlus/) and [SQLite](https://www.sqlite.org/index.html). 
It's a custom moderation Discord bot made for the [ArtCord Discord Server](https://discord.gg/ArtCord). 

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
   git clone https://github.com/SniffBakaSniff/ArtcordAdminBot
   cd ArtcordAdminBot
   ```

2. **Install Dependencies:**
   Make sure you have the [.NET SDK](https://learn.microsoft.com/en-us/dotnet/core/install/windows) installed. If not, install it via your package manager.

   For Arch Linux:
   ```sh
   sudo pacman -S dotnet-sdk
   ```

3. **Configure the configuration file:**
   Create a file named `appsettings.json` with the following contents:
   ```json
   {
     "Token": "YOUR_BOT_TOKEN_HERE"
   }
   ```
   Replace `YOUR_BOT_TOKEN_HERE` with your Discord Bot's token, which you can get from [Discord's Developer Portal](https://discord.com/developers/applications).

4. **Build the Project:**
   ```sh
   dotnet build
   ```

5. **Run the Bot:**
   ```sh
   dotnet run
   ```

Alternatively, you can open the project using [Visual Studio](https://visualstudio.microsoft.com/), which will handle everything except for step 1 and 3 for you. 

## Features

- **CommandsModule.cs:**
  - Contains the `CommandsModule` class with example commands (`/ping` and `/echo`).
  - Commands support optional parameters for sending responses as embedded messages or plain text.

- **EventsModule.cs:**
  - Contains the `EventsModule` class with an example event handler for when the bot is ready.

## License

This project is licensed under the GNU General Public License v3.0 - see the [LICENSE](LICENSE) file for details.
