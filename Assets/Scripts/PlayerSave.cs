using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSave
{
    public class playersave
    {
        //Default Values
        private static string savefile = System.IO.Directory.GetCurrentDirectory() + "\\PlayerSave.json";
        private static string BestTimeSave = System.IO.Directory.GetCurrentDirectory() + "\\BestTimes.Gl1";
        public int world = 0;
        public int level = 0;
        public int checkpoint = 0;
        public string Resolution = "1920x1080";
        public bool VSync = false;
        public bool Fullscreen = true;
        private string[,] BestTimes = new string[1,1];

        /*  Tyler McPhee
         *  Loads Player Save Data on start from disk
         */
        public playersave()
        {
            load();
            readBestTime();
        }

        /*  Tyler McPhee
         *  Gets the Specified value and saves result to disk.
         *  INPUT: Setting value
         */
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
        public void setResolution(string Resolution)
        {
            this.Resolution = Resolution;
            ApplyResolution();
            save();
        }
        public void setVsync(bool state)
        {
            this.VSync = state;
            save();
        }
        public void setFullscreen(bool state)
        {
            this.Fullscreen = state;
            ApplyResolution();
            save();
        }

        /*  Tyler McPhee
         *  Gets the Specified value and returns result.
         */
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
        public string getResolution()
        {
            return this.Resolution;
        }
        public bool getVsync()
        {
            return this.VSync;
        }
        public bool getFullscreen()
        {
            return this.Fullscreen;
        }
        public string getBestTime()
        {
            if(this.world < BestTimes.GetLength(0) && this.level < BestTimes.GetLength(1))
            {
                return BestTimes[this.world, this.level];
            }
            return "0.0";
            
        }

        /*  Tyler McPhee
         *  Checks to see if the Players Save File exists
         *  OUTPUT: true if file exist || false if files does not exist
         */
        public bool SaveExists()
        {
            if (System.IO.File.Exists(savefile))
            {
                return true;
            }
            return false;
        }

        /*  Tyler McPhee
         *  Sets the Screen Resolution and Fullscreen (True or false) baised on the settings
         */
        private void ApplyResolution()
        {
            int width = 1920;
            int height = 1080;
            string[] res = this.Resolution.Split('x');
            int.TryParse(res[0], out width);
            int.TryParse(res[1], out height);
            Screen.SetResolution(width, height, this.Fullscreen);
        }

        /*  Tyler McPhee
         *  Sets the Screen to use the monitors refresh rate or not baised on the settings
         */
        private void ApplyVsync()
        {
            int type = 0;
            if (this.VSync)
            {
                type = 1;
            }
            QualitySettings.vSyncCount = type;
        }

        /*  Tyler McPhee
         *  Checks to see if the Player Save File exists.
         *  If exists load the json file into the playersave class
         */
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

        /*  Tyler McPhee
         *  Converts the playersave class into a json string. 
         *  Saves the file to disk overriding any existing file
         */
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

        public void saveBestTime(string time)
        {
            addBestTime(time);
            writeBestTime();
        }

        private void addBestTime(string time, int world=-1, int level=-1)
        {
            if(world == -1)
            {
                world = this.world;
            }

            if (level == -1)
            {
                level = this.level;
            }

            int w = BestTimes.GetLength(0);
            int l = BestTimes.GetLength(1);

            string[,] temparr = new string[w, l];
            Array.Copy(BestTimes, 0, temparr, 0, BestTimes.Length);
            bool trigger = false;
            if (world >= w)
            {
                w = world + 1;
                trigger = true;
            }
            if (level >= l)
            {
                l = level + 1;
                trigger = true;
            }
            if (trigger)
            {
                BestTimes = new string[w, l];
                //arrayCopy2D(temparr, BestTimes);
                Array.Copy(temparr, 0, BestTimes, 0, temparr.Length);
            }
            
            BestTimes[world, level] = time;
        }

        private void writeBestTime()
        {
            using (System.IO.StreamWriter sw = System.IO.File.CreateText(BestTimeSave))
            {
                for (int i = 0; i < BestTimes.GetLength(0); i++)
                {
                    for (int j = 0; j < BestTimes.GetLength(1); j++)
                    {
                        sw.Write(i + "-" + j + " " + BestTimes[i, j] + "\n");
                    }
                }
                sw.Close();
            }
        }

        private void readBestTime()
        {
            if (System.IO.File.Exists(BestTimeSave))
            {
                System.IO.StreamReader file = new System.IO.StreamReader(BestTimeSave);
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] data = line.Split(' ');
                    string[] leveldata = data[0].Split('-');

                    int w = 0;
                    int l = 0;
                    int.TryParse(leveldata[0], out w);
                    int.TryParse(leveldata[1], out l);

                    addBestTime(data[1], w, l);
                }
                file.Close();
            }
        }
    }
}
