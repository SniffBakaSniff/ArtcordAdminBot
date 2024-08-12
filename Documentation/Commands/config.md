# Config Command

The `config` command allows staff to configure all types of settings. A web interface to alter the config is coming soon.

## Prefix

The prefix to all commands without `/`. Slash commands work regardless of this setting. 

`/config prefix [prefix (string)]`
- `[prefix]`: The prefix string, such as `.` or `ac.`.

Default value: `?`

## Reset

Reset a config option to its default value.

`/config reset [option (string)]`

- `[option]`: The `/config` option to be reset (such as "prefix").