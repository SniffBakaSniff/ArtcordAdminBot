# Purge

Purges a specified number of messages from the current channel. Defaults to 10 if no number is specified.

### Usage

`/purge <amount>`

### Arguments
- `<amount (int:1~250)=10>`: The amount of messages to purge from the channel.

**Note:** This command will execute one API request per 100 messages.