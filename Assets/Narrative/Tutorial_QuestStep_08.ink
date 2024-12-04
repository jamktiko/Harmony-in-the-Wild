INCLUDE Globals.ink

You found your way here. Amazing! #speaker:Bear
Did you find berries along the way?
    + [Yes] -> yes
    + [No] -> no

=== yes ===
Great work! I hope you managed to gather a few. Keep an eye out as you explore — the island is full of unique items to discover. 
Best of luck finding them!
-> dialogueAfterChoice

=== no ===
That’s alright, don’t worry! Berries are easy to spot — they shimmer with a golden glow when the sunlight hits them just right. Keep an eye out as you explore. 
The island is filled with all kinds of treasures waiting to be found. Good luck!
-> dialogueAfterChoice

=== dialogueAfterChoice ===
Welcome to the Heart of Island. The place where all aspects of nature meet. The forest, the snow, the ocean.
What is that? #speaker:Fox
This? The Tree of Life. #speaker:Bear
It represents the state of the island. The harmony in the wild. The happiness and peace we have here.
Or what’s left of it.
You can see how the tree is almost completely dead. When the evil overtook our island, the tree lost its beauty.
But you should be able to restore the tree to its former glory.
If you manage to overcome the evil, I’m sure the tree will slowly regain its powers.
That sounds like a big responsibility. Will you help me? #speaker:Fox
At this point I am far too old for adventures and challenges. #speaker:Bear
As a symbol of the island's trust in you, the Tree of Life has gifted you a new ability — gliding. 
With this power, you can leap into the air and gracefully navigate across the land. Use it well as you explore the island and uncover its secrets.
We all have faith in you, young fox. You can help all of us.
~ latestTutorialQuestStepDialogueCompleted = 5
-> END