# Cat Adventure! ~o( =∩ω∩= )m

Welcome to our cozy, dungeon adventure game!

## Halfway project review:

### Name of game: Cat Adventure

### Names of group members: 
* Talia Goody
* Estephanie Fernandez
* Nuonnettra Kanzaki
* Alisa Sriphet

### Current build progress:
https://youtu.be/jZTyv9U5xlM

### Description of current target for the game - final target :

It will an avatar based game where the player plays as a cat while the camera watches slightly behind it in a 3rd person view. The player will navigate through a maze to find a chest hidden in the maze (Victory situation). As the cat moves, it will use up stamina which can only be refilled through completion of a simon-says minigame also located in the maze. 

The maze will have 4 main rooms:
1. Goal room - the room with the yarn chest
2. Sliding puzzle room - with have a puzzle 
3. Stamina replenish room - will have a typing minigame presented as a pond. Upon victory, the player will receive a fish that restores their stamina by a fixed amount if won. If they lose, they'll get bones that reduces their stamina more
4. Initial room - this is where the player spawns

### Contributions from each person 
Talia Goody
* Fish minigame - Stamina replenish game: 
    * game logic
    * fish spawning
* Level Design:
    * design maze and construct path layout
    * start and end points 
    * thwumps (enemies - the same as the ones in mario) 
    * fish ponds (stamina minigame)

Estephanie Fernandez
* Player control:
    * handle camera movement and rotation
    * cat animation
    * sound
    * implements cat movement 
        * push 
        * pull
        * run (wasd directions)
* Puzzle Design - what you'll be doing
    * Designs and implements sliding box mechanics to challenge player
    * game logic

Nuonnettra Kanzaki
* Menu Design: 
    * Builds Start and Pause Menu
    * Connect menu options to scene
* Prop placement - what you'll be doing:
    * visuals of the game

Alisa Sriphet
* Stamina System:
    * stamina bar UI
    * logic for regeneration and degeneration
    * stamina adjustments to movement
    * Connects stamina rewards/penalties
* Obstacles: 
    * Implements thwump motion, collision, and stamina reduction logic
    * animation/movement
    * sound effects
* Fishing minigame:
    * Created Persistent data storage for saving state allowing for reload
* Player:
    * Created Player knockback system during collisions
* Menu Navigation:
    * Connected 0 stamina to end of game menu