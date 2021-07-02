# OptIn
A SCP:SL plugin based around giving players a higher chance of playing the role they want.

## How to Use
After installing OptIn, you can simply open the console (`~` by default) and type in `.optin`. This will show you all the roles you are currently opted in for. To actually opt in, type `.optin <role_name>`. The following role names are supported;

- SCP
- DClass
- Guard
- Scientist

## How it works
By running through every player's role before the game starts, the algorithm determines whether or not a player can be swapped with another. The priority is in the order of SCP, Gaurd, DClass, Scientist. So, if Player 1 had both the SCP and DClass opted in, they would get SCP before DClass if they were able to.

The order players join does not matter, as the player list is shuffled to allow for equal opportunities.

