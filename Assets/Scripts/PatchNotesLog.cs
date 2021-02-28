using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatchNotesLog
{
    public class notes
    {
        string[,] PatchNote = new string[1,2];

        public notes()
        {
            PatchNote[0,0] =    "-Added Jumping \n" +
                                "-Added Player Movement";
            PatchNote[0, 1] =   "- \n" +
                                "-";
        }

        public string getNote(int world, int level)
        {
            try
            {
                return PatchNote[world, level];
            }
            catch
            {
                return ""; 
            }
            
        }
    }
}
