# A Really bad day

This project has been made under 72h for the LudumDare 47. 
The theme of the jam was : "Stuck in a loop".
The LudumDare is a game jam where you have to make a game under a certain amount of time with a certain theme. More details on : ldjam.com .

# Idea

This game, is a textual game. It narrates a story where you have to make multiple choices. 
If you do a wrong choice or go a wrong path of choices, you'll die and wake up again to do the exact same day. 
Unless you do the right choices, you'll be stuck in the same day. 

The purpose was to lose the player in a lot a choices and not making a wrong choice obvious. A wrong choice can be a wrong path that continues on and on with also multiple choices. Making the player unable to know the wrong choice right away and be "Stuck in a loop" for a long time. 
Unfortunately, making a good story with that much choices wasn't possible under 72h. 

But since we liked the idea, we wanted to open it to everyone that wants to collaborate to make this game better.
Below, we explain how the game is structured if you want to participate.

# Implementation

The core of the game has been made to accept any suggestions of potentiel stories if anyone wants to collaborate. 

Few things you need to know : 

- **Questions.json** : This file is under Assets/Data and represents the structure of the story. Each "Question" object in the json file represents a step that has answers, each answer has a question id that represents the next step that shows up on screen. It is structured as follows : 

	- Id : The question identifier. This is a number that allows us to determine a step.
	- Question : This is the step label. It contains a text of this portion of the story.
	- Answers : This represents the choices that we have to make.
			-Label : The text on the choice
			-NextQuestionId : the "Id" that redirects to a "Question" of said "Id".
			-Change : Optional flag activation to affect choices' availability:
				-Key : The name of the flag.
				-BoolValue : Turn on/off said flag.
			-Required : Optional requirement for displaying this choice:
                -Key : The name of the required flag.
                -BoolValue : The required condition of the flag.
	- Image : The path of the Image inside the Unity project. It has to be uploaded under the "Assets/Resources" folder.
	- Success : A boolean indicating that we won. If this is true, you have to have at least one answer that acts as a got to main menu button.
	- Failure : A boolean indicating that we died. If this is true, you have to have at least one answer that acts as a Replay button.
- **Assets/Resources/Sprites/Backgrounds** : In this folder, you can put an image that you wish to be shown at a certain step. Adding an image to this folder doesn't suffice. You have to follow these steps in order to make them recognized by the engine.
	- Add your image to this folder.
	- Open Unity
	- Select your Image
	- In the Unity Inspector of your Image, change "Texture type" from Default to "Sprite (2D and UI)" 
	![alt UnityInspector](https://docs.unity3d.com/uploads/SpriteEditorButton.png)
