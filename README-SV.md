# Features

- Displays the interior scrap amount, beehive amount and egg amounts to everyone at the start of the day
- Host-Only: When you host a lobby with this mod, everyone will see your scan.
  Everyone can use !scan to repeat the message (only for themselves)
- Basic color customization (see ChatCommandAPI config)

## Notes

For better accuracy the message can be delayed up to 11 seconds from the ship beginning to land.
This is a pretty arbitrary amount of time, but it's what the game considers safe.

Additionally, the checks for Beehives and Eggs are currently hard-coded. This means that new outside items introduced in
future updates or mods will count towards the normal scrap amount.