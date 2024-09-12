# DQB2NPCViewer
NPC editor with a visual component for feedback. Info taken from [the save editor](https://github.com/turtle-insect/DQB2) by turtle-insect.

## Progress report:

### Pre-Alpha build is avaliable.</br>
<img src="./Screenshots/PreAlpha1.png"> </br>

Has basic Save and Load functionality. Every value should be edited and loaded proterly. Model picking has some glitches related to picking non-valid models & lock appearance.

**BIGGEST PROBLEM** -> Textures & Models are not optimized. I couldn't even upload the body models from how big they are. I will have to do some optimizing on those.</br>
**COLORS ARE NOT ACCURATE** -> Masks are missing. Blending is not fine-tuned.

Note: I only have formal knowdledge on some Python and base C. Everything else is self-taught. If some code hurts the eyes I would be grateful for any suggestions.

### Implementation priority list:
**Easy**
- Add all images to basic values
  
**Things to do**
- Create ALL images for the models
- Optimize textures (turn PNG) and models (change format)
- Masks for colours
- Categorize ALL type and their model locks.
  
**Low priority**
- Weapon & Armourn(Problem is the colour filters with those-should I add weapon models to the viewer?)
- Colour balance (Recreate glitch I found in-game)
- Inventory
- Info box

## Current screenshots:
- Menu </br>
<img src="./Screenshots/General.png"> </br>
- Model updating </br>
<img src="./Screenshots/General2.png"></br>
- Colour selection </br>
<img src="./Screenshots/Colour2.png"></br>
- Resizeable </br>
<img src="./Screenshots/Colour.png"></br>
- Menus will have dropdown for everything </br>
<img src="./Screenshots/drop.png"></br>
