# Cubie
Cubie is a simple mini-game in Unity that allows players to select cubes that sum up to a target value.

# Instructions
To Run Cubie simply open the CubieGame scene and run in the Editor.<br>
<img width="990" alt="Cubie1" src="https://user-images.githubusercontent.com/512300/176906845-c9a7d5d5-5d4a-4bad-973f-685c383015d0.png">

# Details and Configuration
Cubie generates a tree-like structure of cubes with values. To tune the random structure of the tree a scriptable object TreeConfig is used.<br>
<img width="301" alt="Cubie_TreeConfig" src="https://user-images.githubusercontent.com/512300/176907383-922d135a-2d55-4b61-a627-d0ac3f6e575e.png">

You can change the parameters at runtime and press <B>Reset</B> button to generate a new tree structure. Even when params are not changed, the Tree will regenerate randomly based on existing parameters.

# Architecture 
The game code is fairly straight forward (Game Controller is about 60 lines of code), but the Tree generation and rendering has a few notable details. <br><br>
The <B>Node</B> class is a data structure contains all the details about the value and its children along with with some positional data for layout. <br><br>
<B>NodeTree</B> is another data structure that is more high level that holds all common nodes data and a flat list of all nodes. When the NodeTree is created it used the Config data provided by the scriptable object and a value generator function (in this case it's just a random int) and recursevely generates the basic structure of the Tree (all nodes) without the layout details. <br><br>
<B>NodeRenderer</B> is a game specific MonoBehaviour that is responsible for rendering and layout of the generated NodeTree. It's main function RenderTree uses Reingold-Tilford Algorithm https://hci.stanford.edu/courses/cs448b/f09/lectures/CS448B-20091021-GraphsAndTrees.pdf to layout Tree in 2 passes. The 3rd pass is used to actually instantiate Unity game objects with cubes and connected lines based on the layout done by previous passes. <br><br>
Most of the node specific rendering and input event propogation happens in <B>NodeView</B> class. When the NodeView is clicked it passes the event up the chain to NodeRenderer which then passes the value to NodeTreeGeneration and back to the GameController. This in turn makes the GameController very lightweight. <br><br>
Most of the UI specific actions happen in <B>UIController</B>. Similarly to the NodeView, it is responsible for passing events of button clicks up the chain to GameController which in turn acts as a mediator to Submit answers and Reset the Tree structure. <br><br>
One other small componenent of the game is that it randomly downloads Succcess images from an API called PicSum. This is done via <B>ImageDownloader</B> using UnityWebRequestTexture. To test this API submit a correct answer once, submit a wrong answer and submit the correct answer again. The downloaded image file is optimized for the size of the view.  
