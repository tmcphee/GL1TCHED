using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PatchNotesLog
{
    public class notes
    {
        string[,] PatchNote = new string[1,1];

        public notes()
        {
            PatchNote[0,0] =    "-this is a test \n" +
                                "-next line";
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
