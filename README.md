# OptIn
A SCP:SL plugin based around giving players a higher chance of playing the role they want.

## How to Use
After installing OptIn, you can simply open the console (`~` by default) and type in the right command. There are two commands, which depend on the version you are using.
If you are using the algotithmic version, use `.optin`. This will show you all the roles you are currently opted in for. To actually opt in, type `.optin <role_name>`. 
If you are using the guarntee-optin version, use `.priority` or `.pr`. The correct arguments are `.pr <role_name> <priority_position>`, where `priority_position` can be 1-4.

The following role names are supported;

- SCP
- DClass
- Guard
- Scientist

## How it works
The algorithmic system works by running through every player's role before the game starts, the algorithm determines whether or not a player can be swapped with another. The priority is in the order of SCP, Gaurd, DClass, Scientist. So, if Player 1 had both the SCP and DClass opted in, they would get SCP before DClass if they were able to.

The order players join does not matter, as the player list is shuffled to allow for equal opportunities.

The Guarntee-Optin version uses a weighted dictionary, based on the priority choice. The way this is determined is simple. `priority_chance / amount_in_priority`.
For example, if 4 players have opted in for SCP as priority 1, they will have a `20%` chance spread amongst them, as `80 / 4` is `20`.

The chances are as follows:
- Priority 1 : 80%
- Priority 2 : 10%
- Priority 3 : 07%
- Priority 4 : 03%
