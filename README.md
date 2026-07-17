# Features

- Displays the interior scrap amount, beehive amount and egg amounts to everyone at the start of the day
- Works both client-side and server-side
    - If you join a lobby where only you have the mod, everyone will see your scan.
      You can use /scan to repeat the message (only for yourself)
    - If you host a lobby with this mod, everyone will see your scan.
      Everyone can use !scan to repeat the message (only for themselves)
- No duplicates! If you join a lobby with this mod and someone else also has it, only one message will appear*

  <sub>*multiple messages may appear due to connection issues like high ping or different mod versions</sub>
- Basic color customization (see ChatCommandAPI config)

## Notes

For better accuracy the message can be delayed up to 15 seconds from the ship beginning to land.

Additionally, the checks for Beehives and Eggs are currently hard-coded. This means that new outside items introduced in
future updates or mods will count towards the normal scrap amount.