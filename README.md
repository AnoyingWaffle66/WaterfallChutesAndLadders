git checkout -b branch_name <br />
// code some stuff <br />
git status <br />
git add . (or add specific files) <br />
git commit -m "commit message" <br />
git switch main <br />
git pull <br />
git switch branch_name <br />
git rebase main <br />
git status <br />
git push -u origin branch_name <br />
//Create pull request on GitHub

to run the Chutes And Ladders console app you need to. REQUIREMENTS: You need an IDE capable of running .net such as Microsoft Visual Studio
1. download the zip under the code button on GitHub
2. unzip the file to wherever you want it located
3. open the whole file in your IDE
4. Click start without debugging in your IDE
   4a. if this does not work navigate to "Program.cs"
   4b. Then run the Program.cs file
5. a console should open prompting the number of people playing the game from 2-4 players, input the number of players then click enter
6. the board should be generated now, the players will each be displayed as different color Ps
   6a. The board has 100 tiles of either empty, chutes, or ladders
   6b. The chutes will be marked with a purple C and then a number of the corresponding chute location (ie. where that chute leads)
   6c. The Ladders will be marked with a yellow L and then a number of the corresponding ladder locations just like the chutes
7. each player takes turns rolling a die (from 1-6) by clicking enter
8. after a player rolls if they land on a chute or ladder they will be informed and told where it took them, they will then be prompted to click enter to continue
9. once someone has reached tile 100 or more they will be declared the victor
   
