# Purge

Purges a specified number of messages from the current channel. Defaults to 10 if no number is specified.

### Usage

`/purge <amount> <afterMessage>`

### Arguments
- `<amount (int:1~250)=10>`: The amount of messages to purge from the channel.
- `<afterMessage (message)>`: The message link or ID to delete after. It itself will not get deleted.

**Note:** This command will execute one API request per 100 messages.