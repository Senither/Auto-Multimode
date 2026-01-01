# Auto Multimode

Auto Multimode is a plugin for Dalamud that will automatically enable [AutoRetainer's](https://github.com/PunishXIV/AutoRetainer) multi mode when it's detected that a player is AFK.

It allows you to set a custom AFK time of between 1 and 60 minutes, after which the plugin will automatically enable multi mode. This is useful for players who are AFK for long periods of time and want to ensure that their retainers are still able to bring in items.

## Installation

To use Auto Multimode you'll need to add the plugin repository to Dalamud by following the steps below:

* Open Dalamud's settings (`/xlsettings`)
* Click on the Experimental tab
* Scroll Down to Custom Plugin Repositories
* Add the following URL: https://dalamud-plugins.senither.com/authors/senither.json, then click on the Plus next to it
* Click on the Save icon in the bottom right corner

Once you've added the repository to Dalamud, you can now install the plugin by searching for "Auto Multimode" or "Senither" in the Dalamud plugin list.

## How to use

Once you've installed the plugin, you can configure it by opening the Dalamud plugins list, finding the plugin and clicking on the settings button, alternatively you can open the settings by typing `/automultimode config` or `/amm config` in the chat.
From there you can select the AFK time you want to use, and the plugin will automatically enable multi mode after the selected time has passed.

> **Note:** The plugin will only enable multi mode if [AutoRetainer](https://github.com/PunishXIV/AutoRetainer) is installed and enabled.

## License

Auto Multimode is open-sourced software licensed under the [MIT license](LICENSE.md).

## Third Party Licenses

Auto Multimode relies on the following projects:

| Name | License  |
|:---|:---|
| [Dalamud](https://github.com/goatcorp/Dalamud) | [GNU Affero General Public License v3.0](https://github.com/goatcorp/Dalamud/blob/master/LICENSE) |
| [ECommons](https://github.com/NightmareXIV/ECommons) | [MIT License](https://github.com/NightmareXIV/ECommons/blob/master/LICENSE.md) |
