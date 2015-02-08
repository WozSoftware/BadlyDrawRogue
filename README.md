# BadlyDrawRogue
Roguelike Engine Using the functional side of C#

The name Badly Drawn Rogue comes from the fact that I am no artist so the graphics 
for the game will be hand drawn… badly.

I have always loved roguelikes and have dabbled with engines in the past, each time
exploring different techniques of implementation. Over the past couple of years I
have become a real fan of functional programming and immutability so I decided it 
was time to revisit a roguelike engine using these techniques. The aim is to create 
a simple game backed by a very flexible engine that will allow creation of far more 
complex games down the line.

I have no yet chosen the render techniques I will use yet. The main application is 
currently an old WinForms project but as the engine lives in a separate project this 
will be switched out when I decide on the UI.

The following is a quick overview of the core concepts

- The object Entity is used to represent most items in the game: tiles, items, actors 
  etc.
- Entity is an immutable object composed of a type, numeric attributes, boolean flags
  and child entities. This structure gives great flexibility to define objects without 
  the need for complex object graphs.
- My immutable objects are closer in concept to F# records than OO objects, most of the
  functionality is via queries and helper methods.
- Heavy use is made of linq and monadic comprehension, because of this you will find very
  few loops in the code.
- The Maybe monad is provided by the Functional.Maybe library.
- Error comprehension is provided by my own Error monad.
- Immutable collections are provided by the Microsoft.Bcl.Collections library.

Until now I have been feeling out the core concepts of the engine and I wanted the
flexibility of clay like code. This has meant the code is sparse on UnitTest as they add 
drag. Now I have passed this stage I will start to rectify the test situation before I 
push on too much further.
