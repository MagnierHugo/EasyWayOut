# Easy Way Out

A gritty, high-stakes Russian Roulette simulator developed for a Game Jam under the theme "Just one last game"[cite: 7]. Built entirely from scratch in exactly 42 hours by a team of 4 developers and 4 artists[cite: 7]. 

You are a prisoner on death row[cite: 7]. Your only chance at freedom is to survive a grueling tournament against five other inmates[cite: 7]. Win six consecutive rounds of lethal chance, and you walk free[cite: 7]. 

## About The Project

This game subverts the standard Russian Roulette formula by introducing unique, modified weapons and distinct AI personalities[cite: 7]. It is designed to be a claustrophobic, tension-filled experience where probability math meets psychological warfare[cite: 7]. As the rounds progress, the environment degrades, the lighting grows harsher, and the opponents become mathematically ruthless[cite: 7].

Built in Unity, the project relies on a robust state-machine architecture to handle turn-based logic, dynamic lighting progression, and Expected Value (EV) calculations for the enemy AI[cite: 7].

## Core Systems & Architecture

### System A: Probability-Driven AI & Expected Value (EV)
Creating a scalable AI framework for 5 distinct opponent personalities that make decisions based on dynamic probability math rather than simple random chance[cite: 8].
- **State-Machine Architecture:** Implemented a robust turn-based state machine that handles transitions between player input, AI calculation, and resolution phases without race conditions[cite: 8].
- **Mathematical Decision Making:** Engineered "The Calculator" AI to act as the final boss[cite: 8]. Instead of using `Random.Range`, the AI reads the current array size of the weapon's chambers, calculates the exact float probability of a live round, and executes the action (Shoot Self, Shoot Opponent, or Special) that yields the highest mathematical Expected Value (EV) of survival[cite: 8].
- **Personality Weights:** The other 4 AI profiles (Maniac, Thug, Coward, Gambler) utilize weighted decision matrices that skew these base probability calculations to simulate recklessness, defense, or panic[cite: 8].

### System B: Modular Weapon Logic
Managing multiple conflicting rulesets (varying chamber sizes, dual magazines, self-targeting limitations) within the exact same core turn-based loop during a tight 42-hour deadline[cite: 8].
- **Polymorphism:** Built a base weapon class that handles standard chamber indexing and firing logic[cite: 8].
- **Override Mechanics:** Unique weapons like the Double-Barrel (swapping between two distinct magazine arrays) and the Shotgun (blindly loading variable shell counts) override the base methods, allowing the core game loop to remain clean and agnostic to the specific weapon currently equipped[cite: 8].

## Cross-Discipline Pipeline

Collaborated closely with the 4-person art team to link the dynamic lighting progression and environment degradation to the internal state machine, heightening the tension as rounds progressed without bottlenecking the gameplay code[cite: 9].

## Key Engineering Takeaways

- **AI Balancing:** Balancing the AI decision weights so that "The Coward" and "The Gambler" actually felt like distinct personalities to the player required constant iteration on the probability thresholds[cite: 9].
- **Architectural Scaling:** Architecting a clean state machine for turn-based combat in the first few hours of a jam prevented spaghetti code when adding complex weapons like the Nailgun (which temporarily alters the "damage" target) late in the development cycle[cite: 9].

## Installation & How to Play

*Note: This game is currently available for Windows[cite: 7].*

1. Download the latest build from the Releases page[cite: 7].
2. Extract the ZIP file to your preferred directory[cite: 7].
3. Run the `.exe` file to start the game[cite: 7].

**Controls:**
- **Mouse Scroll Wheel:** Rotate your arm / aim the weapon[cite: 7].
- **Left Mouse Click:** Interact with UI buttons to make your choice[cite: 7].

## Credits

Developed by:
- Hugo Magnier[cite: 7]
- Antoine Riviale[cite: 7]
- Jonathan de Vaulchier[cite: 7]
- Lucas[cite: 7]
