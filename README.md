# BLOCK BREAKER
Developed by Luiz Thiago de Souza

## ABOUT

Block Breaker is a project developed in Unity3D 2022.4.46f1 LTS, requested by Argus Labs as part of their hiring process.
Since it was not explicitly stated in the project requirements, I decided to develop the game in 2D. However, the project can be easily converted to 3D if necessary.

## DESIGN CHOICES

### Event System

Based on my past experiences, I tend to prefer a game architecture approach using Observables/Event Systems to avoid dependencies between components as much as possible. However, I do use Dependency Injection when necessary or, in some cases, direct serialization of a dependency in the Inspector.
One downside of using the Event System is the difficulty in performing a trace during debugging.

### Physics

Since this project doesn’t require complex physics, I decided to handle physics calculations manually instead of using Unity3D's built-in physics system. This is managed in the ProjectilesController.cs. Essentially, this controller maintains a list of active projectiles in the scene and iterates through them to perform the necessary calculations.

### Block Patterns Creation/Editing (AKA Stages)

For the feature of creating block patterns for stage instantiation, I designed a system that uses Scriptable Objects to define the stages. This specific Scriptable Object implements a custom inspector to facilitate editing and creating stages. The game designer can define the grid size, the spacing between elements, and the positioning and HitPoints value of each block.

### Pooling

Another important aspect is that, due to the high number of projectiles that may be present in the scene, a Pooling system was used to manage the projectiles, preventing resource waste and improving performance.

## PROJECT DETAILS

### Managers and Controllers

- GameManager: Singleton responsible for managing the game flow and containing references to the game controllers for easier access when needed.
- CannonController: Intercepts player inputs to control cannon rotation and fire projectiles.
- ProjectileController: Manages all projectiles in the scene and is responsible for activating and deactivating projectiles.
- StageController: Manages the list of patterns (AKA stages), handling the necessary instantiation and destruction of blocks.
- UIController: Controls the simple UI elements of the game, such as showing and hiding UI elements.
- ConsoleLoggerController: Handles logging game events.

### Challenges and Difficulties

During the development of the stage editor, I initially treated the grid as a two-dimensional array (x, y). However, I forgot that Unity3D does not serialize two-dimensional arrays, causing changes made in the editor to be lost upon starting the game.
Once I realized the mistake, I tried using a linked list, as I was sure Unity serialized this type of structure. To my surprise, the same problem occurred. I was confused and, to avoid further delays, decided to create some serializable classes to solve the issue.

### Potential Improvements

Currently, the stage system only supports a single block prefab. An improvement here would be to allow the use of multiple prefabs. This would require a small refactor of the stage and editor architecture, but it’s entirely feasible.
Although it is not within the project scope, various improvements could be made to the game design, such as power-ups, moving blocks, etc.
