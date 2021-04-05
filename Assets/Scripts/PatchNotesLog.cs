using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatchNotesLog
{
    public class notes
    {
        string[] PatchNote = new string[3];

        public notes()
        {
            PatchNote[0] =      "Example Note \n" +
                                "• example example example";

            PatchNote[1] =      "Added my new game to the store, ist new so it may have a fewe bugs :)\n"+
                                "• Added Jumping \n" +
                                "• Added Player Movement";

            PatchNote[2] = "Squashed some bugs and other improvements!\n"+
                           "• Added ground check, so no more infinte jumping >:( \n" +
                           "• Added Boxes & Enemies\n"+
                           "• Added some assets";
        }


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
