using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSave
{
    public class playersave
    {
        private static string savefile = System.IO.Directory.GetCurrentDirectory() + "\\PlayerSave.json";
        public int world = 0;
        public int level = 0;
        public int checkpoint = 0;

        public playersave()
        {
            load();
        }

        //Sets Level data
        public void setWorld(int world)
        {
            this.world = world;
            save();
        }
        public void setLevel(int level)
        {
            this.level = level;
            save();
        }
        public void setCheckpoint(int checkpoint)
        {
            this.checkpoint = checkpoint;
            save();
        }
        public void incrementCheckpoint()
        {
            this.checkpoint++;
            save();
        }
        public void incrementLevel()
        {
            this.level++;
            save();
        }
        public void incrementWorld()
        {
            this.world++;
            save();
        }
        public void resetdata()
        {
            this.world = 0;
            this.level = 0;
            this.checkpoint = 0;
            save();
        }

        //Returns Level data
        public int getWorld()
        {
            return this.world;
        }
        public int getLevel()
        {
            return this.level;
        }
        public int getCheckpoint()
        {
            return this.checkpoint;
        }

        public bool SaveExists()
        {
            if (System.IO.File.Exists(savefile))
            {
                return true;
            }
            return false;
        }

        //Loads level data from file
        public void load()
        {
            if (SaveExists())
            {
                string json = System.IO.File.ReadAllText(savefile);
                JsonUtility.FromJsonOverwrite(json, this);
                Debug.Log("Save Loaded");
            }
            else
            {
                Debug.Log("Save Not found");
            }
        }

        //Saves level data to file
        public void save()
        {
            string json = JsonUtility.ToJson(this);

            using (System.IO.StreamWriter sw = System.IO.File.CreateText(savefile))
            {
                Debug.Log("SAVE: " + json);
                sw.Write(json);
                sw.Close();
            }
        }
    }
}
