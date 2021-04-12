using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PatchNotesLog
{
    public class notes
    {
        string[] PatchNote = new string[4];

        /*  Tyler McPhee
         *  The patch notes are stored in a 1D array
         */
        public notes()
        {
            PatchNote[0] =      "Example Note \n" +
                                "• example example example";

            PatchNote[1] =      "Aded my n3w game to the stor, ist new so it may have a fewe bugs :)\n"+
                                "• Added Jumping \n" +
                                "• Added Player Movement";

            PatchNote[2] = "Squashed some bugs and other improvements!\n"+
                           "• Added ground check, so no more infinte jumping >:( \n" +
                           "• Added Boxes & Enemies\n"+
                           "• Added Boxes & Enemies\n" +
                           "• Added some assets";
            PatchNote[3] = "Fixed all the bugs!\n" +
                           "• Fixed the strange chekpoint glitch \n" +
                           "• fixed trampolines\n" +
                           "• Added credits";
        }

        /*  Tyler McPhee
         *  If the patch notes exist then return the note
         *  If not return an empty string
         */
        public string getNote(int world, int level)
        {
            try
            {
                return PatchNote[world];
            }
            catch
            {
                return ""; 
            }
        }
    }
}
