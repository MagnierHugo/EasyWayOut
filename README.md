# Easy Way Out

![Gameplay Banner Placeholder](put_your_image_link_here.png)

A gritty, high-stakes Russian Roulette simulator developed for a Game Jam under the theme "Just one last game." Built entirely from scratch in exactly 42 hours by a team of 4 developers and 4 artists.

You are a prisoner on death row. Your only chance at freedom is to survive a grueling tournament against five other inmates. Win six consecutive rounds of lethal chance, and you walk free. Lose, and it truly is your last game.

## About The Project

This game subverts the standard Russian Roulette formula by introducing unique, modified weapons and distinct AI personalities. It is designed to be a claustrophobic, tension-filled experience where probability math meets psychological warfare. As the rounds progress, the environment degrades, the lighting grows harsher, and the opponents become mathematically ruthless.

Built in Unity, the project relies on a robust state-machine architecture to handle turn-based logic, dynamic lighting progression, and Expected Value calculations for the enemy AI.

## Gameplay & Rules

The game is played in a turn-based loop. On your turn, you must make one of three choices:

1. **Shoot Yourself:** If the chamber is loaded, you die and the game ends. If the chamber is empty, you survive and earn a free extra turn.
2. **Shoot the Opponent:** If the chamber is loaded, you win the round. If the chamber is empty, you safely pass the gun to your opponent, and it becomes their turn.
3. **Use Special:** Activate the unique modifier of the current weapon to manipulate the odds.

## The Arsenal

Instead of relying on a single gun, the tournament forces players to adapt to five different modified weapons, each with its own probability logic and special ability.

* **The Standard Revolver** (1 Bullet, 6 Chambers)
  * *Special:* Adds an extra bullet to the cylinder, shuffles it, and forces the user to shoot themselves before ending their turn. A desperate escape from certain death.
* **The Double-Barrel** (2 Magazines: 1 Bullet, 6 Chambers each)
  * *Special:* Swaps between the two magazines. This ability has a cooldown and cannot be used again until the opponent also uses it.
* **The Burst** (1 Bullet, 10 Chambers)
  * *Limitation:* Players can only target themselves. 
  * *Special:* Fires two chambers simultaneously. 
* **The Shotgun** (Variable: 2 to 8 Bullets, 8 Chambers total)
  * *Special:* No mid-round special. Instead, at the start of the round, both players blindly choose to load between 1 and 4 live shells into the queue.
* **The Nailgun** (3 Nails, 11 Slots)
  * *Limitation:* Players can only target themselves.
  * *Special:* The player shoots their own hand instead of their head, guaranteeing survival for that turn. Cannot be used consecutively or if the hand is already nailed.

## AI Personalities

The inmates you face are not entirely random. Each round introduces an opponent with a specific decision-making framework, escalating in difficulty.

1. **The Maniac:** Pure chaos. 50/50 chance of shooting you or themselves.
2. **The Thug:** Aggressive but reckless. Always targets the player and uses special abilities without tactical thought.
3. **The Coward:** Plays defensively. Shoots themselves when the risk is low to farm free turns, but panics and targets the player when the odds of death climb.
4. **The Gambler:** A risk-taker who pushes the odds, intentionally shooting themselves multiple times to build tension before striking.
5. **The Calculator:** The final boss. Analyzes the exact float probability of every available action and executes the choice with the highest mathematical Expected Value of survival.

## Installation & How to Play

*Note: This game is currently available for Windows.*

1. Download the latest build from the Releases page.
2. Extract the ZIP file to your preferred directory.
3. Run the `.exe` file to start the game.

**Controls:**
* **Mouse Scroll Wheel:** Rotate your arm / aim the weapon.
* **Left Mouse Click:** Interact with UI buttons to make your choice.

## Credits

Developed in 42 hours for the "Just one last game" Game Jam by a team of 4 Developers and 4 Artists.

Developers :
- Hugo Magnier
- Antoine Riviale
- Jonathan de Vaulchier
- Lucas

Artists :
- 
-
-
-
