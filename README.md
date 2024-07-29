This project contains multiple Games that resemble my learning process in Game Development and Unity in general.
* Breakout
  - Moves with Left and Right Arrow, has set Lives of 3.
  - Rigidbody with ApplyForce and Reflect for ball bounce.
  - Gameover after losing all Lives.
  ![image](https://github.com/user-attachments/assets/6ef9e201-2364-48a3-8b7f-43d28d01caaf)

* Flappy Bird
  - Press Space to AddForce for Bird's Flap.
  - Tunnels are Instantiated after a set number of Times, Invoke an EventHandler after Bird passes the Tunnel position to add Score.
  - Gameover after contact with Tunnel.
  ![image](https://github.com/user-attachments/assets/bf3983f4-a49c-4d11-9c5f-45ac9c42db96)

* Jetpack Joyride
  - Hold Space to AddForce for Jetpack.
  - Has 4 different Obstacles (Horizontal, Vertical, Plus, Tunnel) and 2 Moving mechanics (Default and Lerping up and down).
  - Has a difficult ramp after every 150 scores using Time.scale.
  - Gameover after getting pushed out of Sceen.
    ![image](https://github.com/user-attachments/assets/0d56cc23-64c4-41d6-81da-7c638269bd67)

* Pong
  - Moves with W and S.
  - Ball bounces using Refect with Random x Axis.
  - A simple bot that can't play the game xD, using Lerp with Random Move Time hoping it can hit the Ball somehow.
  - Scoring for both the Player and the Bot. If the bot can ever have any point xD.
  ![image](https://github.com/user-attachments/assets/e99f3d60-087a-43b3-b44d-89bc1fbfb34d)

* Space Invaders
  - Moves with A and D or Left and Right Arrow. Press Space or Left Mouse Click to Shoot Bullet.
  - Enemy moves in a Rectangle shape using Lerp and can shoot out Bullets towards the Player.
  - Enemy's Bullets are calculated to shoot from the one Opposing the Player and 2 others one next to the center of it. Idk how to describe it in English, please forgive me :(
  - Gameover after the Player is shot.
  ![image](https://github.com/user-attachments/assets/33dd544e-f9bc-4329-90f9-e8aab51acabf)

* 2048
  - Using Arrow Keys or ASWD to move.
  - Spawn 2 or 4 after a move with randomize 75/25% at a random empty Cell.
  - For moving, if I Press the Right Arrow Keys or D, it will start checking the second right-most column whether it can move or merge if another number is presented at the first right-most column. The same principle applies to other directions.
  - Game over if no other Move can be done.
  - Has a New Game Button and Play Again for replayability.
  ![image](https://github.com/user-attachments/assets/025475e2-864c-44da-981c-cc637b33a5f7)

* Tetris
  - Press Q or E equivalent to Left and Right Rotation, Left and Right Arrow Keys for Moving, Down Arrow Keys for Moving Down, and Space for a Drop.
  - Manipulating Tilemap with SetTile, HasTile, and ClearAllTiles to simulate gameplay.
  - For ClearLine, after checking a row is Full, clear tiles from that Line and move all tiles starting from the one above down one Row.
  - Game over if no other Move can be done. 
  - Currently has no Scoring and UI.
  ![image](https://github.com/user-attachments/assets/4295692e-470f-48bf-ab74-27b98575c3f8)


