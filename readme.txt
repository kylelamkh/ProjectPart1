Name:	Lam Kai Ho
SID:	1155094496

Topic 4

The play is a programmer fighting against the work from boss, probably using Scrum.
It can be seen that all words are terms in programming.
Everyone deserves a second chance so there is 5 chances.
The timer is set to count down 1 minute.

When the start button is clicked, it will hide itself.
Then the other elements will show up and call the Game controller to start generating words. 
The word is randomly picked from the array of words.
The falling of word is acheived by DoubleAnimation and Storyboard.

A handler of the Text box will return the input by hearing the space input.
Using space bar as return is because 
	1) typing those words in programing seldom press enter;
	2) it resemble some typing test that require user type a essay.
If the word is match with the words existing in the canvas, the text box will be gone
and the player scores.
At the same time, whenever space bar is hit, a new word is generated. 

When the animation of the text block containing the word finished,
but the word still exist, the player failed to type the word and live will be deducted.

If the player failed for 5 times, he/she will be fired.
Otherwise, he/she survived that 1 minute.
