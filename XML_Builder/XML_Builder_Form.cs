using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;

namespace XML_Builder
{
    public partial class XML_Builder_Form : Form
    {
        public XML_Builder_Form()
        {
            InitializeComponent();

            // Disables all textBoxes, checkBoxes, comboBoxes and buttons
            textBox3.Enabled = false;
            textBox5.Enabled = false;
            textBox8.Enabled = false;
            textBox10.Enabled = false;
            textBox11.Enabled = false;
            textBox12.Enabled = false;
            checkBox1.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            comboBox6.Enabled = false;
            comboBox7.Enabled = false;
            comboBox8.Enabled = false;
            comboBox10.Enabled = false;
            comboBox11.Enabled = false;
            comboBox12.Enabled = false;
            comboBox13.Enabled = false;
            comboBox14.Enabled = false;
            comboBox15.Enabled = false;
            comboBox16.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;

            // Creates temp folder
            Directory.CreateDirectory("samplexmls/temp");

            // "carcols.txt" file (stores fixed colors in a temporary file) and "carcolsOriginal.txt" file (stores original colors in a temporary file)
            File.WriteAllText("samplexmls/temp/carcols.txt", File.ReadAllText("samplexmls/originalFiles/carcols.dat"));
            File.WriteAllText("samplexmls/temp/carcolsOriginal.txt", File.ReadAllText("samplexmls/originalFiles/carcols.dat"));

            // Cursor is on "vehicle name" textBox during startup
            textBox9.Select();
        }

        // Stores carcols line value here if it's valid/fixable
        private static string fixedCarcolsLine = string.Empty;

        /// <summary>
        /// Fixes carcols line in "carcols.DAT line" textBox if needed.
        /// </summary>
        private void FixCarcolsLine()
        {
            // Writes text from "fixedCarcolsLine" value to file "carcolsLine.txt"
            File.WriteAllText("samplexmls/temp/carcolsLine.txt", fixedCarcolsLine);

            // Gets all colors (carcols) from "carcolsLine.txt" file
            string[] carcolsLineValues = File.ReadAllText("samplexmls/temp/carcolsLine.txt").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            // Gets all colors (carcols) from "carcolsOriginal.txt" file
            string[] carcolsOriginalValues = File.ReadAllLines("samplexmls/temp/carcolsOriginal.txt");

            // Gets all colors (carcols) from "carcols.txt" file
            string[] carcolsValues = File.ReadAllLines("samplexmls/temp/carcols.txt");

            for (int i = 0; i < carcolsLineValues.Length; i++)
            {
                // Gets individual colors (carcols) from "carcolsLineValues"
                string[] individualCarcols = carcolsLineValues[i].Split(',');

                int foundCarcolsCount = 0;

                for (int j = 0; j < individualCarcols.Length; j++)
                {
                    // Searches for individual color in "carcolsOriginal.txt" file
                    for (int k = 0; k < carcolsOriginalValues.Length; k++)
                    {
                        string originalColorNumber = carcolsOriginalValues[k].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[1];

                        // Checks if individual color was found
                        if (individualCarcols[j].CompareTo(originalColorNumber) == 0)
                        {
                            foundCarcolsCount++;

                            string colorNumber = carcolsValues[k].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[1];

                            // Checks if individual color doesn't match a color in "carcols.txt" file
                            if (individualCarcols[j].CompareTo(colorNumber) != 0)
                            {
                                // Replaces carcols line (in carcolsLine.txt file)
                                if (File.ReadAllText("samplexmls/temp/carcolsLine.txt").Contains(individualCarcols[j]))
                                {
                                    File.WriteAllText("samplexmls/temp/carcolsLine.txt", File.ReadAllText("samplexmls/temp/carcolsLine.txt").Replace(individualCarcols[j], colorNumber));
                                }
                            }

                            // Checks if all individual colors were found in "carcolsOriginal.txt" file
                            if (foundCarcolsCount == individualCarcols.Length)
                            {
                                break;
                            }
                        }
                    }
                }

                // Checks if not all individual colors were found in "carcolsOriginal.txt" file
                if (foundCarcolsCount != individualCarcols.Length)
                {
                    // Checks if "carcolsLine.txt" file is empty
                    if (File.ReadAllText("samplexmls/temp/carcolsLine.txt").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Length == 0)
                    {
                        break;
                    }
                    else
                    {
                        // Checks if a color is not at the end of "carcolsLine.txt" file
                        if (carcolsLineValues[i] != File.ReadAllText("samplexmls/temp/carcolsLine.txt").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Last())
                        {
                            // Deletes a color from "carcolsLine.txt" file
                            if (File.ReadAllText("samplexmls/temp/carcolsLine.txt").Contains(carcolsLineValues[i]))
                            {
                                File.WriteAllText("samplexmls/temp/carcolsLine.txt", File.ReadAllText("samplexmls/temp/carcolsLine.txt").Replace(carcolsLineValues[i] + ", ", ""));
                            }
                        }
                        else if (File.ReadAllText("samplexmls/temp/carcolsLine.txt").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
                        {
                            // Deletes a color from "carcolsLine.txt" file
                            if (File.ReadAllText("samplexmls/temp/carcolsLine.txt").Contains(carcolsLineValues[i]))
                            {
                                File.WriteAllText("samplexmls/temp/carcolsLine.txt", File.ReadAllText("samplexmls/temp/carcolsLine.txt").Replace(carcolsLineValues[i], ""));
                            }
                        }
                        else
                        {
                            // Deletes a color from "carcolsLine.txt" file
                            if (File.ReadAllText("samplexmls/temp/carcolsLine.txt").Contains(carcolsLineValues[i - 1]))
                            {
                                File.WriteAllText("samplexmls/temp/carcolsLine.txt", File.ReadAllText("samplexmls/temp/carcolsLine.txt").Replace(carcolsLineValues[i - 1] + ", ", carcolsLineValues[i - 1]));
                            }
                            if (File.ReadAllText("samplexmls/temp/carcolsLine.txt").Contains(carcolsLineValues[i]))
                            {
                                File.WriteAllText("samplexmls/temp/carcolsLine.txt", File.ReadAllText("samplexmls/temp/carcolsLine.txt").Replace(carcolsLineValues[i], ""));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if "vehicle type" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool VehicleTypeIsValid()
        {
            // Checks if "vehicle type" comboBox is not empty
            if (comboBox9.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Vehicle type]: Vehicle type field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "vehicle category" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool VehicleCategoryIsValid()
        {
            // Checks if "vehicle category" comboBox is not empty
            if (comboBox10.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Vehicle category]: Vehicle category field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "extra flags" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool ExtraFlagsAreValid()
        {
            // Checks if "extra flags" comboBox is not empty
            comboBox1.Text = comboBox1.Text.Trim();
            if (comboBox1.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Extra flags]: Extra flags field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "immunity" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool ImmunityIsValid()
        {
            // Checks if "immunity" comboBox value is set to "Rhino"
            comboBox2.Text = comboBox2.Text.Trim();
            if (comboBox2.Text == "Rhino")
            {
                return true;
            }

            // Checks if "immunity" comboBox is not empty
            if (comboBox2.Text != string.Empty)
            {
                int n;
                bool isNumeric = int.TryParse(comboBox2.Text, out n);

                // Checks if "immunity" comboBox value is an integer
                if (isNumeric)
                {
                    // Checks if "immunity" comboBox value is between 0 and 255 [0;255]
                    if (n >= 0 && n <= 255)
                    {
                        return true;
                    }
                    else
                    {
                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[Immunity]: Invalid immunity value \"" + comboBox2.Text + "\" (should be a number from 0 to 255).";
                    }
                }
                else
                {
                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[Immunity]: Invalid immunity value \"" + comboBox2.Text + "\" (should be a number from 0 to 255).";
                }
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Immunity]: Immunity field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "engine audio" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool EngineAudioIsValid()
        {
            // Checks if "engine audio" comboBox is not empty
            if (comboBox3.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Engine audio]: Engine audio field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "horn audio" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool HornAudioIsValid()
        {
            // Checks if "horn audio" comboBox is not empty
            if (comboBox4.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Horn audio]: Horn audio field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "siren audio" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool SirenAudioIsValid()
        {
            // Checks if "siren audio" comboBox is not empty
            if (comboBox5.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Siren audio]: Siren audio field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "door audio" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool DoorAudioIsValid()
        {
            // Checks if "door audio" comboBox is not empty
            if (comboBox6.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Door audio]: Door audio field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "boat engine audio" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool BoatEngineAudioIsValid()
        {
            // Checks if "boat engine audio" comboBox is not empty
            if (comboBox7.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Boat engine audio]: Boat engine audio field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "helicopter data" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool HelicopterDataIsValid()
        {
            // Checks if "helicopter data" comboBox is not empty
            if (comboBox8.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[Helicopter data]: Helicopter data field is empty.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "carcols.DAT line" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool CarcolsLineComboBoxIsValid()
        {
            // Checks if "carcols.DAT line" comboBox is not empty
            if (comboBox11.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[carcols.DAT comboBox]: Empty carcols.DAT comboBox.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "default.IDE line" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool DefaultLineComboBoxIsValid()
        {
            // Checks if "default.IDE line" comboBox is not empty
            if (comboBox12.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[default.IDE comboBox]: Empty default.IDE comboBox.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "handling.CFG line" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool HandlingLineComboBoxIsValid()
        {
            // Checks if "handling.CFG line" comboBox is not empty
            if (comboBox13.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[handling.CFG comboBox]: Empty handling.CFG comboBox.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "bike data line" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool BikeDataLineComboBoxIsValid()
        {
            // Checks if "bike data line" comboBox is not empty
            if (comboBox14.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[bike data comboBox]: Empty bike data comboBox.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "boat data line" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool BoatDataLineComboBoxIsValid()
        {
            // Checks if "boat data line" comboBox is not empty
            if (comboBox15.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[boat data comboBox]: Empty boat data comboBox.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "flying data line" comboBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool FlyingDataLineComboBoxIsValid()
        {
            // Checks if "flying data line" comboBox is not empty
            if (comboBox16.Text != string.Empty)
            {
                return true;
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[flying data comboBox]: Empty flying data comboBox.";
            }
            return false;
        }

        /// <summary>
        /// Checks if "carcols.DAT line" textBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool CarcolsLineIsValid()
        {
            // Checks if "carcols.DAT line" comboBox value is not empty and set to "Custom"
            if (comboBox11.Text != string.Empty && comboBox11.Text != "Custom" && checkBox1.Checked == false)
            {
                return true;
            }

            // Checks if "carcols.DAT line" textBox is empty
            if (textBox3.Text == string.Empty)
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[carcols.DAT line]: Empty carcols.DAT line (see \"Example:\").";

                return false;
            }

            // Removes non numbers at the start of "carcols.DAT line" textBox if needed
            if (textBox3.Text != string.Empty)
            {
                if (Char.IsNumber(textBox3.Text.First()) == false)
                {
                    string text = textBox3.Text;
                    while (Char.IsNumber(text.First()) == false)
                    {
                        text = text.Remove(0, 1);
                    }
                    textBox3.Text = text.Trim();
                }
            }

            // Modifies "carcols.DAT line" textBox value if needed
            // Gets trimmed carcols line from "carcols.DAT line" textBox
            string carcolsLine = textBox3.Text.Trim();

            // Stores new carcols line here if it's valid/fixable
            string carcolsLineNew = "";

            for (int i = 0; i < carcolsLine.Length; i++)
            {
                // Checks if current carcols line symbol is a number (this is the start of a first color number)
                if (Char.IsNumber(carcolsLine[i]))
                {
                    // Indicates whether the second color was found
                    bool secondCarcolFound = false;

                    // Helps to form a new carcols line (stores number symbols and a comma "," separator)
                    string carcol = carcolsLine[i].ToString();

                    // New index for seaching more number symbols
                    int newIndex = i + 1;

                    // Checks if new index isn't out of bounds of an array
                    if (newIndex < carcolsLine.Length)
                    {
                        // Seaches for more number symbols to complete the first color number
                        while (Char.IsNumber(carcolsLine[newIndex]))
                        {
                            carcol += carcolsLine[newIndex].ToString();
                            newIndex++;

                            // Checks if new index is out of bounds of an array
                            if (newIndex >= carcolsLine.Length)
                            {
                                break;
                            }
                        }
                    }

                    // First color number is saved, the second number will be separated by a comma ","
                    carcol += ",";

                    // Checks if new index isn't out of bounds of an array
                    if (newIndex < carcolsLine.Length)
                    {
                        // Seaches for a start index of the second color number
                        while (Char.IsNumber(carcolsLine[newIndex]) == false)
                        {
                            newIndex++;

                            // Checks if new index is out of bounds of an array
                            if (newIndex >= carcolsLine.Length)
                            {
                                break;
                            }
                        }
                    }

                    // Checks if new index isn't out of bounds of an array
                    if (newIndex < carcolsLine.Length)
                    {
                        // Seaches for more number symbols to complete the second color number
                        while (Char.IsNumber(carcolsLine[newIndex]))
                        {
                            secondCarcolFound = true;
                            carcol += carcolsLine[newIndex].ToString();
                            newIndex++;

                            // Checks if new index is out of bounds of an array
                            if (newIndex >= carcolsLine.Length)
                            {
                                break;
                            }
                        }
                    }

                    // Checks if the second color number was found
                    if (secondCarcolFound)
                    {
                        // Forms new carcols line
                        carcolsLineNew += carcol + ", ";

                        // Changes index to search for more color numbers
                        i = newIndex - 1;
                    }
                    else
                    {
                        // Changes index to search for more color numbers
                        i = newIndex - 1;
                    }
                }
            }

            // Checks if new carcols line was formed
            if (carcolsLineNew != "")
            {
                // Removes non number symbols from the end of new carcols line
                while (Char.IsNumber(carcolsLineNew.Last()) == false)
                {
                    carcolsLineNew = carcolsLineNew.Remove(carcolsLineNew.Length - 1);
                }

                // New carcols line is stored in a static member "fixedCarcolsLine"
                fixedCarcolsLine = carcolsLineNew;

                return true;
            }
            else
            {
                // New carcols line is invalid/coudn't be fixed
                fixedCarcolsLine = string.Empty;

                // Appends "error" textBox message
                textBox13.Text += "\r\n[carcols.DAT line]: Invalid carcols.DAT line (see \"Example:\").";
            }
            return false;
        }

        /// <summary>
        /// Checks if "default.IDE line" textBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool DefaultLineIsValid()
        {
            textBox5.Text = textBox5.Text.ToLower();

            // Checks if "default.IDE line" comboBox value is not empty and set to "Custom"
            if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
            {
                return true;
            }

            // Checks if "default.IDE line" textBox is not empty
            if (textBox5.Text != string.Empty)
            {
                bool errorsWereFound = false;
                int requiredElementCount = 0;

                // Determines required elements count of default.IDE line by checking "vehicle type" and "vehicle category" comboBox
                if (new string[] { "Bike", "Car", "Helicopter" }.Contains(comboBox9.Text) || comboBox10.Text.CompareTo("RC Baron") == 0)
                {
                    requiredElementCount = 13;
                }
                else if (comboBox9.Text.CompareTo("Boat") == 0 || comboBox10.Text.CompareTo("Skimmer") == 0)
                {
                    requiredElementCount = 11;
                }

                // Gets default.IDE line values
                string[] defaultValues = textBox5.Text.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                // Checks if default.IDE line has a required element count
                if (defaultValues.Length == requiredElementCount)
                {
                    // Checks if default.IDE line doesn't contain a valid <type> value
                    if (new string[] { "car", "plane", "bike", "boat", "heli" }.Contains(defaultValues[3].ToLower()) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[default.IDE line]: Element 4 <type> \"" + defaultValues[3] + "\" is invalid.\r\n[default.IDE line]: Available types: car, plane, bike, boat, heli.";
                    }

                    // Checks if default.IDE line doesn't contain a valid <anims> value
                    if (new string[] { "null", "van", "bikeh", "coach", "bikev", "bikes", "biked" }.Contains(defaultValues[6].ToLower()) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[default.IDE line]: Element 7 <anims> \"" + defaultValues[6] + "\" is invalid.\r\n[default.IDE line]: Available anims: null, van, bikeh, coach, bikev, bikes, biked.";
                    }

                    // Checks if default.IDE line doesn't contain a valid <class> value
                    if (new string[] { "richfamily", "ignore", "executive", "worker", "normal", "big", "taxi", "workerboat", "moped", "motorbike", "poorfamily", "leisureboat" }.Contains(defaultValues[7].ToLower()) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[default.IDE line]: Element 8 <class> \"" + defaultValues[7] + "\" is invalid.\r\n[default.IDE line]: Available classes: richfamily, ignore, executive, worker, normal, big, taxi, workerboat, moped, motorbike, poorfamily, leisureboat.";
                    }

                    // Checks if default.IDE line doesn't contain a valid <freq> value
                    int n;
                    bool isNumeric = int.TryParse(defaultValues[8], out n);
                    if (isNumeric == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[default.IDE line]: Element 9 <freq> \"" + defaultValues[8] + "\" is invalid (should be an integer).";
                    }

                    // Checks if default.IDE line doesn't contain a valid <level> value
                    isNumeric = int.TryParse(defaultValues[9], out n);
                    if (isNumeric == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[default.IDE line]: Element 10 <level> \"" + defaultValues[9] + "\" is invalid (should be an integer).";
                    }

                    // Checks if default.IDE line doesn't contain a valid <comprules> value
                    if (new string[] { "1f10", "2ff0", "4fff", "30123345", "0" }.Contains(defaultValues[10].ToLower()) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[default.IDE line]: Element 11 <comprules> \"" + defaultValues[10] + "\" is invalid.\r\n[default.IDE line]: Available comprules: 1f10, 2ff0, 4fff, 30123345, 0.";
                    }

                    // Checks if default.IDE line contains 13 elements: element 12 and element 13 are used in bikes, cars, helicopters and RC Baron
                    if (requiredElementCount == 13)
                    {
                        // Checks if "vehicle type" comboBox has a "Bike" value
                        if (comboBox9.Text.CompareTo("Bike") == 0)
                        {
                            // Checks if default.IDE line doesn't contain a valid <wheelrotangle> value (only for bikes)
                            isNumeric = int.TryParse(defaultValues[11], out n);
                            if (isNumeric == false)
                            {
                                errorsWereFound = true;

                                // Appends "error" textBox message
                                textBox13.Text += "\r\n[default.IDE line]: Element 12 <wheelrotangle> \"" + defaultValues[11] + "\" is invalid (should be an integer).";
                            }
                        }
                        else
                        {
                            // Checks if default.IDE line doesn't contain a valid <wheelmodel> value (only for cars, helicopters and RC Baron)
                            isNumeric = int.TryParse(defaultValues[11], out n);
                            if (isNumeric == false || new string[] { "237", "238", "239", "249", "250", "251", "252", "253", "254", "255", "256" }.Contains(defaultValues[11]) == false)
                            {
                                errorsWereFound = true;

                                // Appends "error" textBox message
                                textBox13.Text += "\r\n[default.IDE line]: Element 12 <wheelmodel> \"" + defaultValues[11] + "\" is invalid (should be an integer).\r\n[default.IDE line]: Available wheel models: 237, 238, 239, 249 (will be replaced with 237), 250, 251, 252, 253, 254, 255, 256.";
                            }
                        }

                        // Checks if default.IDE line doesn't contain a valid <wheelscale> value (only for bikes, cars, helicopters and RC Baron)
                        decimal value;
                        if (Decimal.TryParse(defaultValues[12], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[default.IDE line]: Element 13 <wheelscale> \"" + defaultValues[12] + "\" is invalid (should be a decimal).";
                        }
                    }

                    // Checks if errors were found in default.IDE line
                    if (errorsWereFound)
                    {
                        return false;
                    }
                }
                else if (defaultValues.Length < requiredElementCount)
                {
                    // Error found: default.IDE contains less elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[default.IDE line]: Less than " + requiredElementCount + " elements in default.IDE line.";

                    return false;
                }
                else
                {
                    // Error found: default.IDE contains more elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[default.IDE line]: More than " + requiredElementCount + " elements in default.IDE line.";

                    return false;
                }
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[default.IDE line]: Empty default.IDE line.";

                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if "handling.CFG line" textBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool HandlingLineIsValid()
        {
            textBox8.Text = textBox8.Text.ToUpper();

            // Checks if "handling.CFG line" comboBox value is not empty and set to "Custom"
            if (comboBox13.Text != string.Empty && comboBox13.Text != "Custom" && checkBox1.Checked == false)
            {
                return true;
            }

            // Checks if "handling.CFG line" textBox is not empty
            if (textBox8.Text != string.Empty)
            {
                bool errorsWereFound = false;

                // Removes non numbers at the start of "handling.CFG line" textBox if needed
                if (Char.IsNumber(textBox8.Text.First()) == false)
                {
                    string text = textBox8.Text;
                    while (Char.IsNumber(text.First()) == false)
                    {
                        text = text.Remove(0, 1);
                    }
                    textBox8.Text = text.Trim();
                }

                // Required element count in handling.CFG line
                int requiredElementCount = 32;

                // Gets handling.CFG line values
                string[] handlingValues = textBox8.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                // Checks if handling.CFG line has a required element count
                if (handlingValues.Length == requiredElementCount)
                {
                    int n;
                    decimal value;

                    // Checks if handling.CFG line doesn't contain a valid <mass> value
                    if (Decimal.TryParse(handlingValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 1 <mass> \"" + handlingValues[0] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <dimensions.x> value
                    if (Decimal.TryParse(handlingValues[1], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 2 <dimensions.x> \"" + handlingValues[1] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <dimensions.y> value
                    if (Decimal.TryParse(handlingValues[2], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 3 <dimensions.y> \"" + handlingValues[2] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <dimensions.z> value
                    if (Decimal.TryParse(handlingValues[3], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 4 <dimensions.z> \"" + handlingValues[3] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <centreofmass.x> value
                    if (Decimal.TryParse(handlingValues[4], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 5 <centreofmass.x> \"" + handlingValues[4] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <centreofmass.y> value
                    if (Decimal.TryParse(handlingValues[5], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 6 <centreofmass.y> \"" + handlingValues[5] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <centreofmass.z> value
                    if (Decimal.TryParse(handlingValues[6], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 7 <centreofmass.z> \"" + handlingValues[6] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <percentsubmerged> value
                    bool isNumeric = int.TryParse(handlingValues[7], out n);
                    if (isNumeric == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 8 <percentsubmerged> \"" + handlingValues[7] + "\" is invalid (should be an integer).";
                    }

                    // Checks <boatsteering> values for boats or skimmer
                    if (comboBox9.Text.CompareTo("Boat") == 0 || comboBox10.Text.CompareTo("Skimmer") == 0)
                    {
                        // Checks if handling.CFG line doesn't contain a valid <boatsteering.bankforcemult> value
                        if (Decimal.TryParse(handlingValues[8], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 9 <boatsteering.bankforcemult> \"" + handlingValues[8] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <boatsteering.rudderturnforce> value
                        if (Decimal.TryParse(handlingValues[9], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 10 <boatsteering.rudderturnforce> \"" + handlingValues[9] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <boatsteering.speedsteerfalloff> value
                        if (Decimal.TryParse(handlingValues[10], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 11 <boatsteering.speedsteerfalloff> \"" + handlingValues[10] + "\" is invalid (should be a decimal).";
                        }
                    }
                    else
                    {
                        // Checks if handling.CFG line doesn't contain a valid <traction.multiplier> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[8], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 9 <traction.multiplier> \"" + handlingValues[8] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <traction.loss> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[9], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 10 <traction.loss> \"" + handlingValues[9] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <traction.bias> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[10], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 11 <traction.bias> \"" + handlingValues[10] + "\" is invalid (should be a decimal).";
                        }
                    }

                    // Checks if handling.CFG line doesn't contain a valid <transmission.numofgears> value
                    isNumeric = int.TryParse(handlingValues[11], out n);
                    if (isNumeric == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 12 <transmission.numofgears> \"" + handlingValues[11] + "\" is invalid (should be an integer).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <transmission.maxspeed> value
                    if (Decimal.TryParse(handlingValues[12], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 13 <transmission.maxspeed> \"" + handlingValues[12] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <transmission.acceleration> value
                    if (Decimal.TryParse(handlingValues[13], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 14 <transmission.acceleration> \"" + handlingValues[13] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <transmission.drivetype> value
                    if (new string[] { "F", "4", "R" }.Contains(handlingValues[14].ToUpper()) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 15 <transmission.drivetype> \"" + handlingValues[14] + "\" is invalid.\r\n[handling.CFG line]: Available transmission drive types: F, 4, R.";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <transmission.enginetype> value
                    if (new string[] { "P", "D", "E" }.Contains(handlingValues[15].ToUpper()) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 16 <transmission.enginetype> \"" + handlingValues[15] + "\" is invalid.\r\n[handling.CFG line]: Available transmission engine types: P, D, E.";
                    }

                    // Checks <boatbrakes> values for boats or skimmer
                    if (comboBox9.Text.CompareTo("Boat") == 0 || comboBox10.Text.CompareTo("Skimmer") == 0)
                    {
                        // Checks if handling.CFG line doesn't contain a valid <boatbrakes.verticalwavehitlimit> value
                        if (Decimal.TryParse(handlingValues[16], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 17 <boatbrakes.verticalwavehitlimit> \"" + handlingValues[16] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <boatbrakes.forwardwavehitbrake> value
                        if (Decimal.TryParse(handlingValues[17], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 18 <boatbrakes.forwardwavehitbrake> \"" + handlingValues[17] + "\" is invalid (should be a decimal).";
                        }
                    }
                    else
                    {
                        // Checks if handling.CFG line doesn't contain a valid <brakes.deceleration> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[16], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 17 <brakes.deceleration> \"" + handlingValues[16] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <brakes.bias> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[17], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 18 <brakes.bias> \"" + handlingValues[17] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <brakes.abs> value (for bikes, cars, helicopters and RC Baron)
                        if (new string[] { "0", "1" }.Contains(handlingValues[18]) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 19 <brakes.abs> \"" + handlingValues[18] + "\" is invalid.\r\n[handling.CFG line]: Available brake ABS values: 0, 1.";
                        }
                    }

                    // Checks if handling.CFG line doesn't contain a valid <steeringlock> value
                    if (Decimal.TryParse(handlingValues[19], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 20 <steeringlock> \"" + handlingValues[19] + "\" is invalid (should be a decimal).";
                    }

                    // Checks <boatsuspension> values for boats or skimmer
                    if (comboBox9.Text.CompareTo("Boat") == 0 || comboBox10.Text.CompareTo("Skimmer") == 0)
                    {
                        // Checks if handling.CFG line doesn't contain a valid <boatsuspension.waterresvolumemult> value
                        if (Decimal.TryParse(handlingValues[20], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 21 <boatsuspension.waterresvolumemult> \"" + handlingValues[20] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <boatsuspension.waterdampingmult> value
                        if (Decimal.TryParse(handlingValues[21], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 22 <boatsuspension.waterdampingmult> \"" + handlingValues[21] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <boatsuspension.upperlimit> value
                        if (Decimal.TryParse(handlingValues[25], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 26 <boatsuspension.upperlimit> \"" + handlingValues[25] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <boatsuspension.handbrakedragmult> value
                        if (Decimal.TryParse(handlingValues[26], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 27 <boatsuspension.handbrakedragmult> \"" + handlingValues[26] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <boatsuspension.sideslipforce> value
                        if (Decimal.TryParse(handlingValues[27], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 28 <boatsuspension.sideslipforce> \"" + handlingValues[27] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <boatsuspension.antidive> value
                        if (Decimal.TryParse(handlingValues[28], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 29 <boatsuspension.antidive> \"" + handlingValues[28] + "\" is invalid (should be a decimal).";
                        }
                    }
                    else
                    {
                        // Checks if handling.CFG line doesn't contain a valid <suspension.forcelevel> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[20], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 21 <suspension.forcelevel> \"" + handlingValues[20] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <suspension.dampening> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[21], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 22 <suspension.dampening> \"" + handlingValues[21] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <suspension.upperlimit> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[25], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 26 <suspension.upperlimit> \"" + handlingValues[25] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <suspension.lowerlimit> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[26], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 27 <suspension.lowerlimit> \"" + handlingValues[26] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <suspension.bias> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[27], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 28 <suspension.bias> \"" + handlingValues[27] + "\" is invalid (should be a decimal).";
                        }

                        // Checks if handling.CFG line doesn't contain a valid <suspension.antidive> value (for bikes, cars, helicopters and RC Baron)
                        if (Decimal.TryParse(handlingValues[28], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                        {
                            errorsWereFound = true;

                            // Appends "error" textBox message
                            textBox13.Text += "\r\n[handling.CFG line]: Element 29 <suspension.antidive> \"" + handlingValues[28] + "\" is invalid (should be a decimal).";
                        }
                    }

                    // Checks if handling.CFG line doesn't contain a valid <seatoffset> value
                    if (Decimal.TryParse(handlingValues[22], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 23 <seatoffset> \"" + handlingValues[22] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <damagemultiplier> value
                    if (Decimal.TryParse(handlingValues[23], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 24 <damagemultiplier> \"" + handlingValues[23] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid (monetary) <value> value
                    isNumeric = int.TryParse(handlingValues[24], out n);
                    if (isNumeric == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 25 (monetary) <value> \"" + handlingValues[24] + "\" is invalid (should be an integer).";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <lights.front> value
                    if (new string[] { "0", "1", "2", "3" }.Contains(handlingValues[30]) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 31 <lights.front> \"" + handlingValues[30] + "\" is invalid.\r\n[handling.CFG line]: Available front lights: 0, 1, 2, 3.";
                    }

                    // Checks if handling.CFG line doesn't contain a valid <lights.rear> value
                    if (new string[] { "0", "1", "2", "3" }.Contains(handlingValues[31]) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[handling.CFG line]: Element 32 <lights.rear> \"" + handlingValues[31] + "\" is invalid.\r\n[handling.CFG line]: Available rear lights: 0, 1, 2, 3.";
                    }

                    // Checks if errors were found in handling.CFG line
                    if (errorsWereFound)
                    {
                        return false;
                    }
                }
                else if (handlingValues.Length < requiredElementCount)
                {
                    // Error found: handling.CFG contains less elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[handling.CFG line]: Less than " + requiredElementCount + " elements in handling.CFG line (see \"Example:\").";

                    return false;
                }
                else
                {
                    // Error found: handling.CFG contains more elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[handling.CFG line]: More than " + requiredElementCount + " elements in handling.CFG line (see \"Example:\").";

                    return false;
                }
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[handling.CFG line]: Empty handling.CFG line (see \"Example:\").";

                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if "bike data handling.CFG line" textBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool BikeDataIsValid()
        {
            // Checks if "bike data line" comboBox value is not empty and set to "Custom"
            if (comboBox14.Text != string.Empty && comboBox14.Text != "Custom" && checkBox1.Checked == false)
            {
                return true;
            }

            // Checks if "bike data handling.CFG line" textBox is not empty
            if (textBox10.Text != string.Empty)
            {
                bool errorsWereFound = false;

                // Removes non numbers at the start of "bike data handling.CFG line" textBox if needed
                if (Char.IsNumber(textBox10.Text.First()) == false)
                {
                    string text = textBox10.Text;
                    while (Char.IsNumber(text.First()) == false)
                    {
                        text = text.Remove(0, 1);
                    }
                    textBox10.Text = text.Trim();
                }

                // Required element count in bike data handling.CFG line
                int requiredElementCount = 15;

                // Gets bike data handling.CFG line values
                string[] bikeDataHandlingValues = textBox10.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                // Checks if bike data handling.CFG line has a required element count
                if (bikeDataHandlingValues.Length == requiredElementCount)
                {
                    decimal value;

                    // Checks if bike data handling.CFG line doesn't contain a valid <leanfwdcom> value
                    if (Decimal.TryParse(bikeDataHandlingValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 1 <leanfwdcom> \"" + bikeDataHandlingValues[0] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <leanfwdforce> value
                    if (Decimal.TryParse(bikeDataHandlingValues[1], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 2 <leanfwdforce> \"" + bikeDataHandlingValues[1] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <leanbackcom> value
                    if (Decimal.TryParse(bikeDataHandlingValues[2], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 3 <leanbackcom> \"" + bikeDataHandlingValues[2] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <leanbackforce> value
                    if (Decimal.TryParse(bikeDataHandlingValues[3], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 4 <leanbackforce> \"" + bikeDataHandlingValues[3] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <maxlean> value
                    if (Decimal.TryParse(bikeDataHandlingValues[4], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 5 <maxlean> \"" + bikeDataHandlingValues[4] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <fullanimlean> value
                    if (Decimal.TryParse(bikeDataHandlingValues[5], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 6 <fullanimlean> \"" + bikeDataHandlingValues[5] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <deslean> value
                    if (Decimal.TryParse(bikeDataHandlingValues[6], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 7 <deslean> \"" + bikeDataHandlingValues[6] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <speedsteer> value
                    if (Decimal.TryParse(bikeDataHandlingValues[7], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 8 <speedsteer> \"" + bikeDataHandlingValues[7] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <slipsteer> value
                    if (Decimal.TryParse(bikeDataHandlingValues[8], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 9 <slipsteer> \"" + bikeDataHandlingValues[8] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <noplayercomz> value
                    if (Decimal.TryParse(bikeDataHandlingValues[9], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 10 <noplayercomz> \"" + bikeDataHandlingValues[9] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <wheelieang> value
                    if (Decimal.TryParse(bikeDataHandlingValues[10], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 11 <wheelieang> \"" + bikeDataHandlingValues[10] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <stoppieang> value
                    if (Decimal.TryParse(bikeDataHandlingValues[11], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 12 <stoppieang> \"" + bikeDataHandlingValues[11] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <wheeliesteer> value
                    if (Decimal.TryParse(bikeDataHandlingValues[12], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 13 <wheeliesteer> \"" + bikeDataHandlingValues[12] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <wheeliestabmult> value
                    if (Decimal.TryParse(bikeDataHandlingValues[13], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 14 <wheeliestabmult> \"" + bikeDataHandlingValues[13] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if bike data handling.CFG line doesn't contain a valid <stoppiestabmult> value
                    if (Decimal.TryParse(bikeDataHandlingValues[14], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[bike data handling.CFG line]: Element 15 <stoppiestabmult> \"" + bikeDataHandlingValues[14] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if errors were found in bike data handling.CFG line
                    if (errorsWereFound)
                    {
                        return false;
                    }
                }
                else if (bikeDataHandlingValues.Length < requiredElementCount)
                {
                    // Error found: bike data handling.CFG contains less elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[bike data handling.CFG line]: Less than " + requiredElementCount + " elements in bike data handling.CFG line (see \"Example:\").";

                    return false;
                }
                else
                {
                    // Error found: bike data handling.CFG contains more elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[bike data handling.CFG line]: More than " + requiredElementCount + " elements in bike data handling.CFG line (see \"Example:\").";

                    return false;
                }
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[bike data handling.CFG line]: Empty bike data handling.CFG line (see \"Example:\").";

                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if "boat data handling.CFG line" textBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool BoatDataIsValid()
        {
            // Checks if "boat data line" comboBox value is not empty and set to "Custom"
            if (comboBox15.Text != string.Empty && comboBox15.Text != "Custom" && checkBox1.Checked == false)
            {
                return true;
            }

            // Checks if "boat data handling.CFG line" textBox is not empty
            if (textBox11.Text != string.Empty)
            {
                bool errorsWereFound = false;

                // Checks if "boat data handling.CFG line" textBox is not empty
                if (Char.IsNumber(textBox11.Text.First()) == false)
                {
                    string text = textBox11.Text;
                    while (Char.IsNumber(text.First()) == false)
                    {
                        text = text.Remove(0, 1);
                    }
                    textBox11.Text = text.Trim();
                }

                // Required element count in boat data handling.CFG line
                int requiredElementCount = 14;

                // Gets boat data handling.CFG line values
                string[] boatDataHandlingValues = textBox11.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                // Checks if boat data handling.CFG line has a required element count
                if (boatDataHandlingValues.Length == requiredElementCount)
                {
                    decimal value;

                    // Checks if boat data handling.CFG line doesn't contain a valid <thrusty> value
                    if (Decimal.TryParse(boatDataHandlingValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 1 <thrusty> \"" + boatDataHandlingValues[0] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <thrustz> value
                    if (Decimal.TryParse(boatDataHandlingValues[1], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 2 <thrustz> \"" + boatDataHandlingValues[1] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <thrustappz> value
                    if (Decimal.TryParse(boatDataHandlingValues[2], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 3 <thrustappz> \"" + boatDataHandlingValues[2] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <aqplaneforce> value
                    if (Decimal.TryParse(boatDataHandlingValues[3], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 4 <aqplaneforce> \"" + boatDataHandlingValues[3] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <aqplanelimit> value
                    if (Decimal.TryParse(boatDataHandlingValues[4], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 5 <aqplanelimit> \"" + boatDataHandlingValues[4] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <aqplaneoffset> value
                    if (Decimal.TryParse(boatDataHandlingValues[5], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 6 <aqplaneoffset> \"" + boatDataHandlingValues[5] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <waveaudiomult> value
                    if (Decimal.TryParse(boatDataHandlingValues[6], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 7 <waveaudiomult> \"" + boatDataHandlingValues[6] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <moveres.x> value
                    if (Decimal.TryParse(boatDataHandlingValues[7], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 8 <moveres.x> \"" + boatDataHandlingValues[7] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <moveres.y> value
                    if (Decimal.TryParse(boatDataHandlingValues[8], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 9 <moveres.y> \"" + boatDataHandlingValues[8] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <moveres.z> value
                    if (Decimal.TryParse(boatDataHandlingValues[9], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 10 <moveres.z> \"" + boatDataHandlingValues[9] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <turnres.x> value
                    if (Decimal.TryParse(boatDataHandlingValues[10], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 11 <turnres.x> \"" + boatDataHandlingValues[10] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <turnres.y> value
                    if (Decimal.TryParse(boatDataHandlingValues[11], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 12 <turnres.y> \"" + boatDataHandlingValues[11] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <turnres.z> value
                    if (Decimal.TryParse(boatDataHandlingValues[12], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 13 <turnres.z> \"" + boatDataHandlingValues[12] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if boat data handling.CFG line doesn't contain a valid <looklrbcamheight> value
                    if (Decimal.TryParse(boatDataHandlingValues[13], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[boat data handling.CFG line]: Element 14 <looklrbcamheight> \"" + boatDataHandlingValues[13] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if errors were found in boat data handling.CFG line
                    if (errorsWereFound)
                    {
                        return false;
                    }
                }
                else if (boatDataHandlingValues.Length < requiredElementCount)
                {
                    // Error found: boat data handling.CFG contains less elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[boat data handling.CFG line]: Less than " + requiredElementCount + " elements in boat data handling.CFG line (see \"Example:\").";

                    return false;
                }
                else
                {
                    // Error found: boat data handling.CFG contains more elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[boat data handling.CFG line]: More than " + requiredElementCount + " elements in boat data handling.CFG line (see \"Example:\").";

                    return false;
                }
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[boat data handling.CFG line]: Empty boat data handling.CFG line (see \"Example:\").";

                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if "flying data handling.CFG line" textBox is valid.
        /// </summary>
        /// <returns></returns>
        private bool FlyingDataIsValid()
        {
            // Checks if "flying data line" comboBox value is not empty and set to "Custom"
            if (comboBox16.Text != string.Empty && comboBox16.Text != "Custom" && checkBox1.Checked == false)
            {
                return true;
            }

            // Checks if "flying data handling.CFG line" textBox is not empty
            if (textBox12.Text != string.Empty)
            {
                bool errorsWereFound = false;

                // Removes non numbers at the start of "flying data handling.CFG line" textBox if needed
                if (Char.IsNumber(textBox12.Text.First()) == false)
                {
                    string text = textBox12.Text;
                    while (Char.IsNumber(text.First()) == false)
                    {
                        text = text.Remove(0, 1);
                    }
                    textBox12.Text = text.Trim();
                }

                // Required element count in flying data handling.CFG line
                int requiredElementCount = 18;

                // Gets flying data handling.CFG line values
                string[] flyingDataHandlingValues = textBox12.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                // Checks if flying data handling.CFG line has a required element count
                if (flyingDataHandlingValues.Length == requiredElementCount)
                {
                    decimal value;

                    // Checks if flying data handling.CFG line doesn't contain a valid <thrust> value
                    if (Decimal.TryParse(flyingDataHandlingValues[0], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 1 <thrust> \"" + flyingDataHandlingValues[0] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <thrustfalloff> value
                    if (Decimal.TryParse(flyingDataHandlingValues[1], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 2 <thrustfalloff> \"" + flyingDataHandlingValues[1] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <yaw> value
                    if (Decimal.TryParse(flyingDataHandlingValues[2], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 3 <yaw> \"" + flyingDataHandlingValues[2] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <yawstab> value
                    if (Decimal.TryParse(flyingDataHandlingValues[3], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 4 <yawstab> \"" + flyingDataHandlingValues[3] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <sideslip> value
                    if (Decimal.TryParse(flyingDataHandlingValues[4], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 5 <sideslip> \"" + flyingDataHandlingValues[4] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <roll> value
                    if (Decimal.TryParse(flyingDataHandlingValues[5], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 6 <roll> \"" + flyingDataHandlingValues[5] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <rollstab> value
                    if (Decimal.TryParse(flyingDataHandlingValues[6], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 7 <rollstab> \"" + flyingDataHandlingValues[6] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <pitch> value
                    if (Decimal.TryParse(flyingDataHandlingValues[7], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 8 <pitch> \"" + flyingDataHandlingValues[7] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <pitchstab> value
                    if (Decimal.TryParse(flyingDataHandlingValues[8], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 9 <pitchstab> \"" + flyingDataHandlingValues[8] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <formlift> value
                    if (Decimal.TryParse(flyingDataHandlingValues[9], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 10 <formlift> \"" + flyingDataHandlingValues[9] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <attacklift> value
                    if (Decimal.TryParse(flyingDataHandlingValues[10], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 11 <attacklift> \"" + flyingDataHandlingValues[10] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <moveres> value
                    if (Decimal.TryParse(flyingDataHandlingValues[11], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 12 <moveres> \"" + flyingDataHandlingValues[11] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <turnres.x> value
                    if (Decimal.TryParse(flyingDataHandlingValues[12], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 13 <turnres.x> \"" + flyingDataHandlingValues[12] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <turnres.y> value
                    if (Decimal.TryParse(flyingDataHandlingValues[13], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 14 <turnres.y> \"" + flyingDataHandlingValues[13] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <turnres.z> value
                    if (Decimal.TryParse(flyingDataHandlingValues[14], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 15 <turnres.z> \"" + flyingDataHandlingValues[14] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <speedres.x> value
                    if (Decimal.TryParse(flyingDataHandlingValues[15], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 16 <speedres.x> \"" + flyingDataHandlingValues[15] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <speedres.y> value
                    if (Decimal.TryParse(flyingDataHandlingValues[16], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 17 <speedres.y> \"" + flyingDataHandlingValues[16] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if flying data handling.CFG line doesn't contain a valid <speedres.z> value
                    if (Decimal.TryParse(flyingDataHandlingValues[17], NumberStyles.Any, CultureInfo.InvariantCulture, out value) == false)
                    {
                        errorsWereFound = true;

                        // Appends "error" textBox message
                        textBox13.Text += "\r\n[flying data handling.CFG line]: Element 18 <speedres.z> \"" + flyingDataHandlingValues[17] + "\" is invalid (should be a decimal).";
                    }

                    // Checks if errors were found in flying data handling.CFG line
                    if (errorsWereFound)
                    {
                        return false;
                    }
                }
                else if (flyingDataHandlingValues.Length < requiredElementCount)
                {
                    // Error found: flying data handling.CFG contains less elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[flying data handling.CFG line]: Less than " + requiredElementCount + " elements in flying data handling.CFG line (see \"Example:\").";

                    return false;
                }
                else
                {
                    // Error found: flying data handling.CFG contains more elements than required
                    errorsWereFound = true;

                    // Appends "error" textBox message
                    textBox13.Text += "\r\n[flying data handling.CFG line]: More than " + requiredElementCount + " elements in flying data handling.CFG line (see \"Example:\").";

                    return false;
                }
            }
            else
            {
                // Appends "error" textBox message
                textBox13.Text += "\r\n[flying data handling.CFG line]: Empty flying data handling.CFG line (see \"Example:\").";

                return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if everything is valid for bike.
        /// </summary>
        /// <returns></returns>
        private bool EverythingIsValidForBike()
        {
            bool errorsWereFound = false;

            // Checks if every comboBox and textBox is valid
            if (VehicleTypeIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (VehicleCategoryIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (ExtraFlagsAreValid() == false)
            {
                errorsWereFound = true;
            }
            if (ImmunityIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (EngineAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HornAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (SirenAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DoorAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (CarcolsLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DefaultLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HandlingLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (BikeDataLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (CarcolsLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DefaultLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HandlingLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (BikeDataIsValid() == false)
            {
                errorsWereFound = true;
            }

            // Checks if any errors were found
            if (errorsWereFound)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if everything is valid for boat.
        /// </summary>
        /// <returns></returns>
        private bool EverythingIsValidForBoat()
        {
            bool errorsWereFound = false;

            // Checks if every comboBox and textBox is valid
            if (VehicleTypeIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (VehicleCategoryIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (ExtraFlagsAreValid() == false)
            {
                errorsWereFound = true;
            }
            if (ImmunityIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (EngineAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HornAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (SirenAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DoorAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (BoatEngineAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (CarcolsLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DefaultLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HandlingLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (BoatDataLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (CarcolsLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DefaultLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HandlingLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (BoatDataIsValid() == false)
            {
                errorsWereFound = true;
            }

            // Checks if any errors were found
            if (errorsWereFound)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if everything is valid for car.
        /// </summary>
        /// <returns></returns>
        private bool EverythingIsValidForCar()
        {
            bool errorsWereFound = false;

            // Checks if every comboBox and textBox is valid
            if (VehicleTypeIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (VehicleCategoryIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (ExtraFlagsAreValid() == false)
            {
                errorsWereFound = true;
            }
            if (ImmunityIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (EngineAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HornAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (SirenAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DoorAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (CarcolsLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DefaultLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HandlingLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (CarcolsLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DefaultLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HandlingLineIsValid() == false)
            {
                errorsWereFound = true;
            }

            // Checks if any errors were found
            if (errorsWereFound)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if everything is valid for helicopter.
        /// </summary>
        /// <returns></returns>
        private bool EverythingIsValidForHelicopter()
        {
            bool errorsWereFound = false;

            // Checks if every comboBox and textBox is valid
            if (VehicleTypeIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (VehicleCategoryIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (ExtraFlagsAreValid() == false)
            {
                errorsWereFound = true;
            }
            if (ImmunityIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (EngineAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HornAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (SirenAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DoorAudioIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (CarcolsLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DefaultLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HandlingLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HelicopterDataIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (FlyingDataLineComboBoxIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (CarcolsLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (DefaultLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (HandlingLineIsValid() == false)
            {
                errorsWereFound = true;
            }
            if (FlyingDataIsValid() == false)
            {
                errorsWereFound = true;
            }

            // Checks if any errors were found
            if (errorsWereFound)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if everything is valid for plane.
        /// </summary>
        /// <returns></returns>
        private bool EverythingIsValidForPlane()
        {
            bool errorsWereFound = false;

            // Checks if "vehicle category" comboBox is set to "RC Baron" or "Skimmer"
            if (comboBox10.Text == "RC Baron")
            {
                // Checks if every comboBox and textBox is valid
                if (VehicleTypeIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (VehicleCategoryIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (ExtraFlagsAreValid() == false)
                {
                    errorsWereFound = true;
                }
                if (ImmunityIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (EngineAudioIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (HornAudioIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (SirenAudioIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (DoorAudioIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (CarcolsLineComboBoxIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (DefaultLineComboBoxIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (HandlingLineComboBoxIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (FlyingDataLineComboBoxIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (CarcolsLineIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (DefaultLineIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (HandlingLineIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (FlyingDataIsValid() == false)
                {
                    errorsWereFound = true;
                }
            }
            else if (comboBox10.Text == "Skimmer")
            {
                // Checks if every comboBox and textBox is valid
                if (VehicleTypeIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (VehicleCategoryIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (ExtraFlagsAreValid() == false)
                {
                    errorsWereFound = true;
                }
                if (ImmunityIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (EngineAudioIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (HornAudioIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (SirenAudioIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (DoorAudioIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (BoatEngineAudioIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (CarcolsLineComboBoxIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (DefaultLineComboBoxIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (HandlingLineComboBoxIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (BoatDataLineComboBoxIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (FlyingDataLineComboBoxIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (CarcolsLineIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (DefaultLineIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (HandlingLineIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (BoatDataIsValid() == false)
                {
                    errorsWereFound = true;
                }
                if (FlyingDataIsValid() == false)
                {
                    errorsWereFound = true;
                }
            }

            // Checks if any errors were found
            if (errorsWereFound)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Opens file "flags.html" in a default browser.
        /// </summary>
        private void VisitLink()
        {
            // Change the color of the link text by setting LinkVisited to true.  
            linkLabel1.LinkVisited = true;

            // Call the Process.Start method to open the default browser with a URL:
            string path = Path.GetFullPath("flags.html");
            System.Diagnostics.Process.Start(path);
        }

        /// <summary>
        /// Finds sample XML file by vehicle name.
        /// </summary>
        /// <param name="vehicleName"></param>
        /// <returns></returns>
        private string FindSampleXML(string vehicleName)
        {
            if (vehicleName.CompareTo("Vice Cheetah") == 0)
            {
                return "samplexmls/vicechee.xml";
            }
            else if (vehicleName.CompareTo("RC Goblin") == 0)
            {
                return "samplexmls/rcgobli.xml";
            }
            else if (vehicleName.CompareTo("Hotring Racer (hotrina)") == 0)
            {
                return "samplexmls/hotrina.xml";
            }
            else if (vehicleName.CompareTo("Hotring Racer (hotrinb)") == 0)
            {
                return "samplexmls/hotrinb.xml";
            }
            else if (vehicleName.CompareTo("Hotring Racer (hotring)") == 0)
            {
                return "samplexmls/hotring.xml";
            }
            else if (vehicleName.CompareTo("Bloodring Banger (bloodra)") == 0)
            {
                return "samplexmls/bloodra.xml";
            }
            else if (vehicleName.CompareTo("Bloodring Banger (bloodrb)") == 0)
            {
                return "samplexmls/bloodrb.xml";
            }
            else
            {
                string[] sampleXMLs = Directory.GetFiles("samplexmls").Select(file => Path.GetFileName(file)).ToArray();
                for (int i = 0; i < sampleXMLs.Length; i++)
                {
                    if (File.ReadAllText("samplexmls/" + sampleXMLs[i]).Contains("<name>" + vehicleName + "</name>"))
                    {
                        return "samplexmls/" + sampleXMLs[i];
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Finds a specified line in sample XML file by given text.
        /// </summary>
        /// <param name="sampleXML"></param>
        /// <param name="givenText"></param>
        /// <returns></returns>
        private string FindLineInSampleXML(string sampleXML, string givenText)
        {
            string[] fileLines = File.ReadAllLines(sampleXML);
            for (int i = 0; i < fileLines.Length; i++)
            {
                if (fileLines[i].Contains(givenText))
                {
                    return fileLines[i];
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Builds car XML file from default.ide, handling.cfg and carcols.dat lines.
        /// </summary>
        /// <param name="isAmbulan"></param>
        /// <param name="isBaggage"></param>
        /// <param name="isCabbie"></param>
        /// <param name="isCoach"></param>
        /// <param name="isEnforcr"></param>
        /// <param name="isFbiranc"></param>
        /// <param name="isFiretrk"></param>
        /// <param name="isKaufman"></param>
        /// <param name="isPolicar"></param>
        /// <param name="isRcbandt"></param>
        /// <param name="isTaxi"></param>
        /// <param name="isVicechee"></param>
        /// <param name="isZebra"></param>
        /// <param name="carName"></param>
        /// <param name="extraflags"></param>
        /// <param name="immunity"></param>
        /// <param name="enginefarsample"></param>
        /// <param name="enginenearsample"></param>
        /// <param name="hornsample"></param>
        /// <param name="hornfreq"></param>
        /// <param name="sirensample"></param>
        /// <param name="sirenfreq"></param>
        /// <param name="doorsounds"></param>
        private void BuildCarXML(bool isAmbulan, bool isBaggage, bool isCabbie, bool isCoach, bool isEnforcr, bool isFbiranc, bool isFiretrk, bool isKaufman, bool isPolicar, bool isRcbandt, bool isTaxi, bool isVicechee, bool isZebra, string carName, string extraflags, string immunity, string enginefarsample, string enginenearsample, string hornsample, string hornfreq, string sirensample, string sirenfreq, string doorsounds)
        {
            // Get default.ide values
            string[] defaultValues = textBox5.Text.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handling.txt values
            string[] handlingValues = textBox8.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get carcols.txt values
            string[] carcolsValues = File.ReadAllLines("samplexmls/temp/carcols.txt");

            // Get carcolsLine.txt values
            string[] carcolsLineValues = File.ReadAllText("samplexmls/temp/carcolsLine.txt").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            // Check if file "carXML.xml" already exists
            if (File.Exists("samplexmls/temp/carXML.xml"))
            {
                File.Delete("samplexmls/temp/carXML.xml");
            }

            // StreamWriter
            using (var carXML = new StreamWriter("samplexmls/temp/carXML.xml", true))
            {
                // Write other stuff
                carXML.WriteLine("<?xml version=\"1.0\" encoding=\"ASCII\"?>");
                carXML.WriteLine("<vehicle>");
                carXML.WriteLine("	<basic>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write type
                    carXML.WriteLine("		<type>" + defaultValues[3] + "</type>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write type
                    carXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<type>"));
                }

                // Write name
                carXML.WriteLine("		<name>" + carName + "</name>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write anims
                    carXML.WriteLine("		<anims>" + defaultValues[6] + "</anims>");

                    // Write comprules
                    carXML.WriteLine("		<comprules>" + defaultValues[10] + "</comprules>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write anims
                    carXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<anims>"));

                    // Write comprules
                    carXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<comprules>"));
                }

                // Write maxpassengers if car replaces baggage, coach, firetrk or rcbandt
                if (isBaggage)
                {
                    carXML.WriteLine("		<maxpassengers>0</maxpassengers>");
                }
                else if (isCoach)
                {
                    carXML.WriteLine("		<maxpassengers>8</maxpassengers>");
                }
                else if (isFiretrk)
                {
                    carXML.WriteLine("		<maxpassengers>2</maxpassengers>");
                }
                else if (isRcbandt)
                {
                    carXML.WriteLine("		<maxpassengers>0</maxpassengers>");
                }

                // Write extraflags
                carXML.WriteLine("		<extraflags>" + extraflags + "</extraflags>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write wheelmodel
                    if (defaultValues[11].CompareTo("249") == 0)
                    {
                        carXML.WriteLine("		<wheelmodel>237</wheelmodel>");
                    }
                    else
                    {
                        carXML.WriteLine("		<wheelmodel>" + defaultValues[11] + "</wheelmodel>");
                    }

                    // Write wheelscale
                    carXML.WriteLine("		<wheelscale>" + defaultValues[12] + "</wheelscale>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write wheelmodel
                    carXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<wheelmodel>"));

                    // Write wheelscale
                    carXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<wheelscale>"));
                }

                // Writer immunity
                carXML.WriteLine("		<immunity>" + immunity + "</immunity>");

                // Write other stuff
                carXML.WriteLine("	</basic>");
                carXML.WriteLine();
                carXML.WriteLine("	<aidata>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write class
                    carXML.WriteLine("		<class>" + defaultValues[7] + "</class>");

                    // Write freq
                    carXML.WriteLine("		<freq>" + defaultValues[8] + "</freq>");

                    // Write level
                    carXML.WriteLine("		<level>" + defaultValues[9] + "</level>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write class
                    carXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<class>"));

                    // Write freq
                    carXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<freq>"));

                    // Write level
                    carXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<level>"));
                }

                // Write other stuff
                carXML.WriteLine("	</aidata>");
                carXML.WriteLine();
                carXML.WriteLine("	<colors>");

                // Checks if "carcols.DAT line comboBox" value is set to "Custom"
                if (comboBox11.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write rgbcol
                    for (int i = 0; i < carcolsLineValues.Length; i++)
                    {
                        string rgbcol = string.Empty;
                        string[] rgbValues = carcolsLineValues[i].Split(',');
                        for (int j = 0; j < rgbValues.Length; j++)
                        {
                            if (j == rgbValues.Length - 1)
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0];
                            }
                            else
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0] + ",";
                            }
                        }
                        carXML.WriteLine("		<rgbcol>" + rgbcol + "</rgbcol>");
                    }
                }
                else if (comboBox11.Text != string.Empty && comboBox11.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write carcol
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox11.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<carcol>"))
                        {
                            int carcolIndex = i;
                            while (fileLines[carcolIndex].Contains("<carcol>"))
                            {
                                carXML.WriteLine(fileLines[carcolIndex]);
                                carcolIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                carXML.WriteLine("	</colors>");
                carXML.WriteLine();
                carXML.WriteLine("	<audio>");

                // Write enginefarsample
                carXML.WriteLine("		<enginefarsample>" + enginefarsample + "</enginefarsample>");

                // Write enginenearsample
                carXML.WriteLine("		<enginenearsample>" + enginenearsample + "</enginenearsample>");

                // Write hornsample
                carXML.WriteLine("		<hornsample>" + hornsample + "</hornsample>");

                // Write hornfreq
                carXML.WriteLine("		<hornfreq>" + hornfreq + "</hornfreq>");

                // Write sirensample
                carXML.WriteLine("		<sirensample>" + sirensample + "</sirensample>");

                // Write sirenfreq
                carXML.WriteLine("		<sirenfreq>" + sirenfreq + "</sirenfreq>");

                // Write doorsounds
                carXML.WriteLine("		<doorsounds>" + doorsounds + "</doorsounds>");

                // Write other stuff
                carXML.WriteLine("	</audio>");
                carXML.WriteLine();
                carXML.WriteLine("	<handling>");

                // Checks if "handling.CFG line comboBox" value is set to "Custom"
                if (comboBox13.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write mass
                    carXML.WriteLine("		<mass>" + handlingValues[0] + "</mass>");

                    // Write percentsubmerged
                    carXML.WriteLine("		<percentsubmerged>" + handlingValues[7] + "</percentsubmerged>");

                    // Write steeringlock
                    carXML.WriteLine("		<steeringlock>" + handlingValues[19] + "</steeringlock>");

                    // Write seatoffset
                    carXML.WriteLine("		<seatoffset>" + handlingValues[22] + "</seatoffset>");

                    // Write damagemultiplier
                    carXML.WriteLine("		<damagemultiplier>" + handlingValues[23] + "</damagemultiplier>");

                    // Write value
                    carXML.WriteLine("		<value>" + handlingValues[24] + "</value>");

                    // Write flags
                    if (handlingValues[29].Length < 8)
                    {
                        int difference = 8 - handlingValues[29].Length;
                        carXML.WriteLine("		<flags>" + new string('0', difference) + handlingValues[29] + "</flags>");
                    }
                    else
                    {
                        carXML.WriteLine("		<flags>" + handlingValues[29] + "</flags>");
                    }

                    // Write other stuff
                    carXML.WriteLine();
                    carXML.WriteLine("		<dimensions>");

                    // Write dimensions x, y, z
                    carXML.WriteLine("			<x>" + handlingValues[1] + "</x>");
                    carXML.WriteLine("			<y>" + handlingValues[2] + "</y>");
                    carXML.WriteLine("			<z>" + handlingValues[3] + "</z>");

                    // Write other stuff
                    carXML.WriteLine("		</dimensions>");
                    carXML.WriteLine();
                    carXML.WriteLine("		<centreofmass>");

                    // Write centreofmass x, y, z
                    carXML.WriteLine("			<x>" + handlingValues[4] + "</x>");
                    carXML.WriteLine("			<y>" + handlingValues[5] + "</y>");
                    carXML.WriteLine("			<z>" + handlingValues[6] + "</z>");

                    // Write other stuff
                    carXML.WriteLine("		</centreofmass>");
                    carXML.WriteLine();
                    carXML.WriteLine("		<traction>");

                    // Write multiplier
                    carXML.WriteLine("			<multiplier>" + handlingValues[8] + "</multiplier>");

                    // Write loss
                    carXML.WriteLine("			<loss>" + handlingValues[9] + "</loss>");

                    // Write bias
                    carXML.WriteLine("			<bias>" + handlingValues[10] + "</bias>");

                    // Write other stuff
                    carXML.WriteLine("		</traction>");
                    carXML.WriteLine();
                    carXML.WriteLine("		<transmission>");

                    // Write numofgears
                    carXML.WriteLine("			<numofgears>" + handlingValues[11] + "</numofgears>");

                    // Write maxspeed
                    carXML.WriteLine("			<maxspeed>" + handlingValues[12] + "</maxspeed>");

                    // Write acceleration
                    carXML.WriteLine("			<acceleration>" + handlingValues[13] + "</acceleration>");

                    // Write drivetype
                    carXML.WriteLine("			<drivetype>" + handlingValues[14] + "</drivetype>");

                    // Write enginetype
                    carXML.WriteLine("			<enginetype>" + handlingValues[15] + "</enginetype>");

                    // Write other stuff
                    carXML.WriteLine("		</transmission>");
                    carXML.WriteLine();
                    carXML.WriteLine("		<brakes>");

                    // Write deceleration
                    carXML.WriteLine("			<deceleration>" + handlingValues[16] + "</deceleration>");

                    // Write bias
                    carXML.WriteLine("			<bias>" + handlingValues[17] + "</bias>");

                    // Write abs
                    carXML.WriteLine("			<abs>" + handlingValues[18] + "</abs>");

                    // Write other stuff
                    carXML.WriteLine("		</brakes>");
                    carXML.WriteLine();
                    carXML.WriteLine("		<suspension>");

                    // Write forcelevel
                    carXML.WriteLine("			<forcelevel>" + handlingValues[20] + "</forcelevel>");

                    // Write dampening
                    carXML.WriteLine("			<dampening>" + handlingValues[21] + "</dampening>");

                    // Write upperlimit
                    carXML.WriteLine("			<upperlimit>" + handlingValues[25] + "</upperlimit>");

                    // Write lowerlimit
                    carXML.WriteLine("			<lowerlimit>" + handlingValues[26] + "</lowerlimit>");

                    // Write bias
                    carXML.WriteLine("			<bias>" + handlingValues[27] + "</bias>");

                    // Write antidive
                    carXML.WriteLine("			<antidive>" + handlingValues[28] + "</antidive>");

                    // Write other stuff
                    carXML.WriteLine("		</suspension>");
                    carXML.WriteLine();
                    carXML.WriteLine("		<lights>");

                    // Write front
                    carXML.WriteLine("			<front>" + handlingValues[30] + "</front>");

                    // Write rear
                    carXML.WriteLine("			<rear>" + handlingValues[31] + "</rear>");

                    // Write other stuff
                    carXML.WriteLine("		</lights>");
                }
                else if (comboBox13.Text != string.Empty && comboBox13.Text != "Custom" && checkBox1.Checked == false)
                {
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox13.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<handling>"))
                        {
                            int handlingIndex = i + 1;
                            while (fileLines[handlingIndex].Contains("</handling>") == false)
                            {
                                carXML.WriteLine(fileLines[handlingIndex]);
                                handlingIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                carXML.WriteLine("	</handling>");

                // Write specials if car replaces ambulan, cabbie, enforcr, fbiranc, firetrk, kaufman, policar, taxi, vicechee or zebra
                if (isAmbulan)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<emlights>");
                    carXML.WriteLine("			<alwayson>false</alwayson>");
                    carXML.WriteLine("			<colourone>255,0,0</colourone>");
                    carXML.WriteLine("			<colourtwo>255,255,255</colourtwo>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<posleft>");
                    carXML.WriteLine("				<x>-1.1</x>");
                    carXML.WriteLine("				<y>0.9</y>");
                    carXML.WriteLine("				<z>1.6</z>");
                    carXML.WriteLine("			</posleft>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<posright>");
                    carXML.WriteLine("				<x>1.1</x>");
                    carXML.WriteLine("				<y>0.9</y>");
                    carXML.WriteLine("				<z>1.6</z>");
                    carXML.WriteLine("			</posright>");
                    carXML.WriteLine("		</emlights>");
                    carXML.WriteLine("	</specials>");
                }
                else if (isCabbie)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<taxilight>");
                    carXML.WriteLine("			<alwayson>false</alwayson>");
                    carXML.WriteLine("			<colour>128,128,0</colour>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<position>");
                    carXML.WriteLine("				<x>0.0</x>");
                    carXML.WriteLine("				<y>0.0</y>");
                    carXML.WriteLine("				<z>0.95</z>");
                    carXML.WriteLine("			</position>");
                    carXML.WriteLine("		</taxilight>");
                    carXML.WriteLine("	</specials>");
                }
                else if (isEnforcr)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<emlights>");
                    carXML.WriteLine("			<colourone>255,0,0</colourone>");
                    carXML.WriteLine("			<colourtwo>0,0,255</colourtwo>");
                    carXML.WriteLine("			<posleft>");
                    carXML.WriteLine("				<x>-1.1</x>");
                    carXML.WriteLine("				<y>0.8</y>");
                    carXML.WriteLine("				<z>1.2</z>");
                    carXML.WriteLine("			</posleft>");
                    carXML.WriteLine("			<posright>");
                    carXML.WriteLine("				<x>1.1</x>");
                    carXML.WriteLine("				<y>0.8</y>");
                    carXML.WriteLine("				<z>1.2</z>");
                    carXML.WriteLine("			</posright>");
                    carXML.WriteLine("		</emlights>");
                    carXML.WriteLine("	</specials>");
                }
                else if (isFbiranc)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<emlightsingle>");
                    carXML.WriteLine("			<alwayson>false</alwayson>");
                    carXML.WriteLine("			<colour>70,70,255</colour>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<position>");
                    carXML.WriteLine("				<x>0.4</x>");
                    carXML.WriteLine("				<y>0.6</y>");
                    carXML.WriteLine("				<z>0.3</z>");
                    carXML.WriteLine("			</position>");
                    carXML.WriteLine("		</emlightsingle>");
                    carXML.WriteLine("	</specials>");
                }
                else if (isFiretrk)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<emlights>");
                    carXML.WriteLine("			<alwayson>false</alwayson>");
                    carXML.WriteLine("			<colourone>255,0,0</colourone>");
                    carXML.WriteLine("			<colourtwo>255,255,0</colourtwo>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<posleft>");
                    carXML.WriteLine("				<x>-1.1</x>");
                    carXML.WriteLine("				<y>1.7</y>");
                    carXML.WriteLine("				<z>2.0</z>");
                    carXML.WriteLine("			</posleft>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<posright>");
                    carXML.WriteLine("				<x>1.1</x>");
                    carXML.WriteLine("				<y>1.7</y>");
                    carXML.WriteLine("				<z>2.0</z>");
                    carXML.WriteLine("			</posright>");
                    carXML.WriteLine("		</emlights>");
                    carXML.WriteLine("	</specials>");
                }
                else if (isKaufman)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<taxilight>");
                    carXML.WriteLine("			<alwayson>false</alwayson>");
                    carXML.WriteLine("			<colour>128,128,0</colour>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<position>");
                    carXML.WriteLine("				<x>0.0</x>");
                    carXML.WriteLine("				<y>0.0</y>");
                    carXML.WriteLine("				<z>0.95</z>");
                    carXML.WriteLine("			</position>");
                    carXML.WriteLine("		</taxilight>");
                    carXML.WriteLine("	</specials>");
                }
                else if (isPolicar)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<emlights>");
                    carXML.WriteLine("			<alwayson>false</alwayson>");
                    carXML.WriteLine("			<colourone>255,0,0</colourone>");
                    carXML.WriteLine("			<colourtwo>0,0,255</colourtwo>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<posleft>");
                    carXML.WriteLine("				<x>-0.7</x>");
                    carXML.WriteLine("				<y>-0.4</y>");
                    carXML.WriteLine("				<z>1.0</z>");
                    carXML.WriteLine("			</posleft>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<posright>");
                    carXML.WriteLine("				<x>0.7</x>");
                    carXML.WriteLine("				<y>-0.4</y>");
                    carXML.WriteLine("				<z>1.0</z>");
                    carXML.WriteLine("			</posright>");
                    carXML.WriteLine("		</emlights>");
                    carXML.WriteLine("	</specials>");
                }
                else if (isTaxi)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<taxilight>");
                    carXML.WriteLine("			<alwayson>false</alwayson>");
                    carXML.WriteLine("			<colour>128,128,0</colour>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<position>");
                    carXML.WriteLine("				<x>0.0</x>");
                    carXML.WriteLine("				<y>0.0</y>");
                    carXML.WriteLine("				<z>0.95</z>");
                    carXML.WriteLine("			</position>");
                    carXML.WriteLine("		</taxilight>");
                    carXML.WriteLine("	</specials>");
                }
                else if (isVicechee)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<emlightsingle>");
                    carXML.WriteLine("			<alwayson>false</alwayson>");
                    carXML.WriteLine("			<colour>70,70,255</colour>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<position>");
                    carXML.WriteLine("				<x>0.4</x>");
                    carXML.WriteLine("				<y>0.6</y>");
                    carXML.WriteLine("				<z>0.3</z>");
                    carXML.WriteLine("			</position>");
                    carXML.WriteLine("		</emlightsingle>");
                    carXML.WriteLine("	</specials>");
                }
                else if (isZebra)
                {
                    carXML.WriteLine();
                    carXML.WriteLine("	<specials>");
                    carXML.WriteLine("		<taxilight>");
                    carXML.WriteLine("			<alwayson>false</alwayson>");
                    carXML.WriteLine("			<colour>128,128,0</colour>");
                    carXML.WriteLine();
                    carXML.WriteLine("			<position>");
                    carXML.WriteLine("				<x>0.0</x>");
                    carXML.WriteLine("				<y>0.0</y>");
                    carXML.WriteLine("				<z>0.95</z>");
                    carXML.WriteLine("			</position>");
                    carXML.WriteLine("		</taxilight>");
                    carXML.WriteLine("	</specials>");
                }

                // Write other stuff
                carXML.WriteLine("</vehicle>");
            }
        }

        /// <summary>
        /// Builds bike XML file from default.ide, handling.cfg and carcols.dat lines.
        /// </summary>
        /// <param name="isPizzabo"></param>
        /// <param name="bikeName"></param>
        /// <param name="extraflags"></param>
        /// <param name="immunity"></param>
        /// <param name="enginefarsample"></param>
        /// <param name="enginenearsample"></param>
        /// <param name="hornsample"></param>
        /// <param name="hornfreq"></param>
        /// <param name="sirensample"></param>
        /// <param name="sirenfreq"></param>
        /// <param name="doorsounds"></param>
        private void BuildBikeXML(bool isPizzabo, string bikeName, string extraflags, string immunity, string enginefarsample, string enginenearsample, string hornsample, string hornfreq, string sirensample, string sirenfreq, string doorsounds)
        {
            // Get default.ide values
            string[] defaultValues = textBox5.Text.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handling.txt values
            string[] handlingValues = textBox8.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handlingBike.txt values
            string[] handlingBikeValues = textBox10.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get carcols.txt values
            string[] carcolsValues = File.ReadAllLines("samplexmls/temp/carcols.txt");

            // Get carcolsLine.txt values
            string[] carcolsLineValues = File.ReadAllText("samplexmls/temp/carcolsLine.txt").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            // Check if file "bikeXML.xml" already exists
            if (File.Exists("samplexmls/temp/bikeXML.xml"))
            {
                File.Delete("samplexmls/temp/bikeXML.xml");
            }

            // StreamWriter
            using (var bikeXML = new StreamWriter("samplexmls/temp/bikeXML.xml", true))
            {
                // Write other stuff
                bikeXML.WriteLine("<?xml version=\"1.0\" encoding=\"ASCII\"?>");
                bikeXML.WriteLine("<vehicle>");
                bikeXML.WriteLine("	<basic>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write type
                    bikeXML.WriteLine("		<type>" + defaultValues[3] + "</type>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write type
                    bikeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<type>"));
                }

                // Write name
                bikeXML.WriteLine("		<name>" + bikeName + "</name>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write anims
                    bikeXML.WriteLine("		<anims>" + defaultValues[6] + "</anims>");

                    // Write comprules
                    bikeXML.WriteLine("		<comprules>" + defaultValues[10] + "</comprules>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write anims
                    bikeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<anims>"));

                    // Write comprules
                    bikeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<comprules>"));
                }

                // Write maxpassengers if bike replaces pizzabo
                if (isPizzabo)
                {
                    bikeXML.WriteLine("		<maxpassengers>0</maxpassengers>");
                }

                // Write extraflags
                bikeXML.WriteLine("		<extraflags>" + extraflags + "</extraflags>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write wheelrotangle
                    bikeXML.WriteLine("		<wheelrotangle>" + defaultValues[11] + "</wheelrotangle>");

                    // Write wheelscale
                    bikeXML.WriteLine("		<wheelscale>" + defaultValues[12] + "</wheelscale>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write wheelrotangle
                    bikeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<wheelrotangle>"));

                    // Write wheelscale
                    bikeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<wheelscale>"));
                }

                // Writer immunity
                bikeXML.WriteLine("		<immunity>" + immunity + "</immunity>");

                // Write other stuff
                bikeXML.WriteLine("	</basic>");
                bikeXML.WriteLine();
                bikeXML.WriteLine("	<aidata>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write class
                    bikeXML.WriteLine("		<class>" + defaultValues[7] + "</class>");

                    // Write freq
                    bikeXML.WriteLine("		<freq>" + defaultValues[8] + "</freq>");

                    // Write level
                    bikeXML.WriteLine("		<level>" + defaultValues[9] + "</level>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write class
                    bikeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<class>"));

                    // Write freq
                    bikeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<freq>"));

                    // Write level
                    bikeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<level>"));
                }

                // Write other stuff
                bikeXML.WriteLine("	</aidata>");
                bikeXML.WriteLine();
                bikeXML.WriteLine("	<colors>");

                // Checks if "carcols.DAT line comboBox" value is set to "Custom"
                if (comboBox11.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write rgbcol
                    for (int i = 0; i < carcolsLineValues.Length; i++)
                    {
                        string rgbcol = string.Empty;
                        string[] rgbValues = carcolsLineValues[i].Split(',');
                        for (int j = 0; j < rgbValues.Length; j++)
                        {
                            if (j == rgbValues.Length - 1)
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0];
                            }
                            else
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0] + ",";
                            }
                        }
                        bikeXML.WriteLine("		<rgbcol>" + rgbcol + "</rgbcol>");
                    }
                }
                else if (comboBox11.Text != string.Empty && comboBox11.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write carcol
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox11.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<carcol>"))
                        {
                            int carcolIndex = i;
                            while (fileLines[carcolIndex].Contains("<carcol>"))
                            {
                                bikeXML.WriteLine(fileLines[carcolIndex]);
                                carcolIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                bikeXML.WriteLine("	</colors>");
                bikeXML.WriteLine();
                bikeXML.WriteLine("	<audio>");

                // Write enginefarsample
                bikeXML.WriteLine("		<enginefarsample>" + enginefarsample + "</enginefarsample>");

                // Write enginenearsample
                bikeXML.WriteLine("		<enginenearsample>" + enginenearsample + "</enginenearsample>");

                // Write hornsample
                bikeXML.WriteLine("		<hornsample>" + hornsample + "</hornsample>");

                // Write hornfreq
                bikeXML.WriteLine("		<hornfreq>" + hornfreq + "</hornfreq>");

                // Write sirensample
                bikeXML.WriteLine("		<sirensample>" + sirensample + "</sirensample>");

                // Write sirenfreq
                bikeXML.WriteLine("		<sirenfreq>" + sirenfreq + "</sirenfreq>");

                // Write doorsounds
                bikeXML.WriteLine("		<doorsounds>" + doorsounds + "</doorsounds>");

                // Write other stuff
                bikeXML.WriteLine("	</audio>");
                bikeXML.WriteLine();
                bikeXML.WriteLine("	<handling>");

                // Checks if "handling.CFG line comboBox" value is set to "Custom"
                if (comboBox13.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write mass
                    bikeXML.WriteLine("		<mass>" + handlingValues[0] + "</mass>");

                    // Write percentsubmerged
                    bikeXML.WriteLine("		<percentsubmerged>" + handlingValues[7] + "</percentsubmerged>");

                    // Write steeringlock
                    bikeXML.WriteLine("		<steeringlock>" + handlingValues[19] + "</steeringlock>");

                    // Write seatoffset
                    bikeXML.WriteLine("		<seatoffset>" + handlingValues[22] + "</seatoffset>");

                    // Write damagemultiplier
                    bikeXML.WriteLine("		<damagemultiplier>" + handlingValues[23] + "</damagemultiplier>");

                    // Write value
                    bikeXML.WriteLine("		<value>" + handlingValues[24] + "</value>");

                    // Write flags
                    if (handlingValues[29].Length < 8)
                    {
                        int difference = 8 - handlingValues[29].Length;
                        bikeXML.WriteLine("		<flags>" + new string('0', difference) + handlingValues[29] + "</flags>");
                    }
                    else
                    {
                        bikeXML.WriteLine("		<flags>" + handlingValues[29] + "</flags>");
                    }

                    // Write other stuff
                    bikeXML.WriteLine();
                    bikeXML.WriteLine("		<dimensions>");

                    // Write dimensions x, y, z
                    bikeXML.WriteLine("			<x>" + handlingValues[1] + "</x>");
                    bikeXML.WriteLine("			<y>" + handlingValues[2] + "</y>");
                    bikeXML.WriteLine("			<z>" + handlingValues[3] + "</z>");

                    // Write other stuff
                    bikeXML.WriteLine("		</dimensions>");
                    bikeXML.WriteLine();
                    bikeXML.WriteLine("		<centreofmass>");

                    // Write centreofmass x, y, z
                    bikeXML.WriteLine("			<x>" + handlingValues[4] + "</x>");
                    bikeXML.WriteLine("			<y>" + handlingValues[5] + "</y>");
                    bikeXML.WriteLine("			<z>" + handlingValues[6] + "</z>");

                    // Write other stuff
                    bikeXML.WriteLine("		</centreofmass>");
                    bikeXML.WriteLine();
                    bikeXML.WriteLine("		<traction>");

                    // Write multiplier
                    bikeXML.WriteLine("			<multiplier>" + handlingValues[8] + "</multiplier>");

                    // Write loss
                    bikeXML.WriteLine("			<loss>" + handlingValues[9] + "</loss>");

                    // Write bias
                    bikeXML.WriteLine("			<bias>" + handlingValues[10] + "</bias>");

                    // Write other stuff
                    bikeXML.WriteLine("		</traction>");
                    bikeXML.WriteLine();
                    bikeXML.WriteLine("		<transmission>");

                    // Write numofgears
                    bikeXML.WriteLine("			<numofgears>" + handlingValues[11] + "</numofgears>");

                    // Write maxspeed
                    bikeXML.WriteLine("			<maxspeed>" + handlingValues[12] + "</maxspeed>");

                    // Write acceleration
                    bikeXML.WriteLine("			<acceleration>" + handlingValues[13] + "</acceleration>");

                    // Write drivetype
                    bikeXML.WriteLine("			<drivetype>" + handlingValues[14] + "</drivetype>");

                    // Write enginetype
                    bikeXML.WriteLine("			<enginetype>" + handlingValues[15] + "</enginetype>");

                    // Write other stuff
                    bikeXML.WriteLine("		</transmission>");
                    bikeXML.WriteLine();
                    bikeXML.WriteLine("		<brakes>");

                    // Write deceleration
                    bikeXML.WriteLine("			<deceleration>" + handlingValues[16] + "</deceleration>");

                    // Write bias
                    bikeXML.WriteLine("			<bias>" + handlingValues[17] + "</bias>");

                    // Write abs
                    bikeXML.WriteLine("			<abs>" + handlingValues[18] + "</abs>");

                    // Write other stuff
                    bikeXML.WriteLine("		</brakes>");
                    bikeXML.WriteLine();
                    bikeXML.WriteLine("		<suspension>");

                    // Write forcelevel
                    bikeXML.WriteLine("			<forcelevel>" + handlingValues[20] + "</forcelevel>");

                    // Write dampening
                    bikeXML.WriteLine("			<dampening>" + handlingValues[21] + "</dampening>");

                    // Write upperlimit
                    bikeXML.WriteLine("			<upperlimit>" + handlingValues[25] + "</upperlimit>");

                    // Write lowerlimit
                    bikeXML.WriteLine("			<lowerlimit>" + handlingValues[26] + "</lowerlimit>");

                    // Write bias
                    bikeXML.WriteLine("			<bias>" + handlingValues[27] + "</bias>");

                    // Write antidive
                    bikeXML.WriteLine("			<antidive>" + handlingValues[28] + "</antidive>");

                    // Write other stuff
                    bikeXML.WriteLine("		</suspension>");
                    bikeXML.WriteLine();
                    bikeXML.WriteLine("		<lights>");

                    // Write front
                    bikeXML.WriteLine("			<front>" + handlingValues[30] + "</front>");

                    // Write rear
                    bikeXML.WriteLine("			<rear>" + handlingValues[31] + "</rear>");

                    // Write other stuff
                    bikeXML.WriteLine("		</lights>");
                }
                else if (comboBox13.Text != string.Empty && comboBox13.Text != "Custom" && checkBox1.Checked == false)
                {
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox13.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<handling>"))
                        {
                            int handlingIndex = i + 1;
                            while (fileLines[handlingIndex].Contains("</handling>") == false)
                            {
                                bikeXML.WriteLine(fileLines[handlingIndex]);
                                handlingIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                bikeXML.WriteLine("	</handling>");
                bikeXML.WriteLine();
                bikeXML.WriteLine("	<bikehandling>");

                // Checks if "bike data line comboBox" value is set to "Custom"
                if (comboBox14.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write leanfwdcom
                    bikeXML.WriteLine("		<leanfwdcom>" + handlingBikeValues[0] + "</leanfwdcom>");

                    // Write leanfwdforce
                    bikeXML.WriteLine("		<leanfwdforce>" + handlingBikeValues[1] + "</leanfwdforce>");

                    // Write leanbackcom
                    bikeXML.WriteLine("		<leanbackcom>" + handlingBikeValues[2] + "</leanbackcom>");

                    // Write leanbackforce
                    bikeXML.WriteLine("		<leanbackforce>" + handlingBikeValues[3] + "</leanbackforce>");

                    // Write maxlean
                    bikeXML.WriteLine("		<maxlean>" + handlingBikeValues[4] + "</maxlean>");

                    // Write fullanimlean
                    bikeXML.WriteLine("		<fullanimlean>" + handlingBikeValues[5] + "</fullanimlean>");

                    // Write deslean
                    bikeXML.WriteLine("		<deslean>" + handlingBikeValues[6] + "</deslean>");

                    // Write speedsteer
                    bikeXML.WriteLine("		<speedsteer>" + handlingBikeValues[7] + "</speedsteer>");

                    // Write slipsteer
                    bikeXML.WriteLine("		<slipsteer>" + handlingBikeValues[8] + "</slipsteer>");

                    // Write noplayercomz
                    bikeXML.WriteLine("		<noplayercomz>" + handlingBikeValues[9] + "</noplayercomz>");

                    // Write wheelieang
                    bikeXML.WriteLine("		<wheelieang>" + handlingBikeValues[10] + "</wheelieang>");

                    // Write stoppieang
                    bikeXML.WriteLine("		<stoppieang>" + handlingBikeValues[11] + "</stoppieang>");

                    // Write wheeliesteer
                    bikeXML.WriteLine("		<wheeliesteer>" + handlingBikeValues[12] + "</wheeliesteer>");

                    // Write wheeliestabmult
                    bikeXML.WriteLine("		<wheeliestabmult>" + handlingBikeValues[13] + "</wheeliestabmult>");

                    // Write stoppiestabmult
                    bikeXML.WriteLine("		<stoppiestabmult>" + handlingBikeValues[14] + "</stoppiestabmult>");
                }
                else if (comboBox14.Text != string.Empty && comboBox14.Text != "Custom" && checkBox1.Checked == false)
                {
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox14.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<bikehandling>"))
                        {
                            int bikeHandlingIndex = i + 1;
                            while (fileLines[bikeHandlingIndex].Contains("</bikehandling>") == false)
                            {
                                bikeXML.WriteLine(fileLines[bikeHandlingIndex]);
                                bikeHandlingIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                bikeXML.WriteLine("	</bikehandling>");
                bikeXML.WriteLine("</vehicle>");
            }
        }

        /// <summary>
        /// Builds plane XML file from default.ide, handling.cfg and carcols.dat lines.
        /// </summary>
        /// <param name="isRcbaron"></param>
        /// <param name="isSkimmer"></param>
        /// <param name="planeName"></param>
        /// <param name="extraflags"></param>
        /// <param name="immunity"></param>
        /// <param name="enginefarsample"></param>
        /// <param name="enginenearsample"></param>
        /// <param name="hornsample"></param>
        /// <param name="hornfreq"></param>
        /// <param name="sirensample"></param>
        /// <param name="sirenfreq"></param>
        /// <param name="doorsounds"></param>
        private void BuildPlaneXML(bool isRcbaron, bool isSkimmer, string planeName, string extraflags, string immunity, string enginefarsample, string enginenearsample, string hornsample, string hornfreq, string sirensample, string sirenfreq, string doorsounds)
        {
            // Get default.ide values
            string[] defaultValues = textBox5.Text.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handling.txt values
            string[] handlingValues = textBox8.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handlingPlane1.txt values (aero)
            string[] handlingPlaneValues1 = textBox12.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handlingPlane2.txt values (boat)
            string[] handlingPlaneValues2 = textBox11.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get carcols.txt values
            string[] carcolsValues = File.ReadAllLines("samplexmls/temp/carcols.txt");

            // Get carcolsLine.txt values
            string[] carcolsLineValues = File.ReadAllText("samplexmls/temp/carcolsLine.txt").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            // Check if file "planeXML.xml" already exists
            if (File.Exists("samplexmls/temp/planeXML.xml"))
            {
                File.Delete("samplexmls/temp/planeXML.xml");
            }

            // StreamWriter
            using (var planeXML = new StreamWriter("samplexmls/temp/planeXML.xml", true))
            {
                // Write other stuff
                planeXML.WriteLine("<?xml version=\"1.0\" encoding=\"ASCII\"?>");
                planeXML.WriteLine("<vehicle>");
                planeXML.WriteLine("	<basic>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write type
                    planeXML.WriteLine("		<type>" + defaultValues[3] + "</type>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write type
                    planeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<type>"));
                }

                // Write name
                planeXML.WriteLine("		<name>" + planeName + "</name>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write anims
                    planeXML.WriteLine("		<anims>" + defaultValues[6] + "</anims>");

                    // Write comprules
                    planeXML.WriteLine("		<comprules>" + defaultValues[10] + "</comprules>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write anims
                    planeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<anims>"));

                    // Write comprules
                    planeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<comprules>"));
                }

                // Write extraflags
                planeXML.WriteLine("		<extraflags>" + extraflags + "</extraflags>");

                // Write wheelmodel and wheelscale if plane replaces rcbaron
                if (isRcbaron)
                {
                    // Checks if "default.IDE line comboBox" value is set to "Custom"
                    if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                    {
                        // Write wheelmodel
                        if (defaultValues[11].CompareTo("249") == 0)
                        {
                            planeXML.WriteLine("		<wheelmodel>237</wheelmodel>");
                        }
                        else
                        {
                            planeXML.WriteLine("		<wheelmodel>" + defaultValues[11] + "</wheelmodel>");
                        }

                        // Write wheelscale
                        planeXML.WriteLine("		<wheelscale>" + defaultValues[12] + "</wheelscale>");
                    }
                    else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                    {
                        // Write wheelmodel
                        planeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<wheelmodel>"));

                        // Write wheelscale
                        planeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<wheelscale>"));
                    }
                }

                // Writer immunity
                planeXML.WriteLine("		<immunity>" + immunity + "</immunity>");

                // Write other stuff
                planeXML.WriteLine("	</basic>");
                planeXML.WriteLine();
                planeXML.WriteLine("	<aidata>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write class
                    planeXML.WriteLine("		<class>" + defaultValues[7] + "</class>");

                    // Write freq
                    planeXML.WriteLine("		<freq>" + defaultValues[8] + "</freq>");

                    // Write level
                    planeXML.WriteLine("		<level>" + defaultValues[9] + "</level>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write class
                    planeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<class>"));

                    // Write freq
                    planeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<freq>"));

                    // Write level
                    planeXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<level>"));
                }

                // Write other stuff
                planeXML.WriteLine("	</aidata>");
                planeXML.WriteLine();
                planeXML.WriteLine("	<colors>");

                // Checks if "carcols.DAT line comboBox" value is set to "Custom"
                if (comboBox11.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write rgbcol
                    for (int i = 0; i < carcolsLineValues.Length; i++)
                    {
                        string rgbcol = string.Empty;
                        string[] rgbValues = carcolsLineValues[i].Split(',');
                        for (int j = 0; j < rgbValues.Length; j++)
                        {
                            if (j == rgbValues.Length - 1)
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0];
                            }
                            else
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0] + ",";
                            }
                        }
                        planeXML.WriteLine("		<rgbcol>" + rgbcol + "</rgbcol>");
                    }
                }
                else if (comboBox11.Text != string.Empty && comboBox11.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write carcol
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox11.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<carcol>"))
                        {
                            int carcolIndex = i;
                            while (fileLines[carcolIndex].Contains("<carcol>"))
                            {
                                planeXML.WriteLine(fileLines[carcolIndex]);
                                carcolIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                planeXML.WriteLine("	</colors>");
                planeXML.WriteLine();
                planeXML.WriteLine("	<audio>");

                // Write enginefarsample
                planeXML.WriteLine("		<enginefarsample>" + enginefarsample + "</enginefarsample>");

                // Write enginenearsample
                planeXML.WriteLine("		<enginenearsample>" + enginenearsample + "</enginenearsample>");

                // Write hornsample
                planeXML.WriteLine("		<hornsample>" + hornsample + "</hornsample>");

                // Write hornfreq
                planeXML.WriteLine("		<hornfreq>" + hornfreq + "</hornfreq>");

                // Write sirensample
                planeXML.WriteLine("		<sirensample>" + sirensample + "</sirensample>");

                // Write sirenfreq
                planeXML.WriteLine("		<sirenfreq>" + sirenfreq + "</sirenfreq>");

                // Write doorsounds
                planeXML.WriteLine("		<doorsounds>" + doorsounds + "</doorsounds>");

                // Write boatengine if plane replaces skimmer
                if (isSkimmer)
                {
                    planeXML.WriteLine();
                    planeXML.WriteLine("		<boatengine>");
                    planeXML.WriteLine("			<type>0</type>");
                    planeXML.WriteLine("			<basevolume>20</basevolume>");
                    planeXML.WriteLine("			<basefrequency>1782</basefrequency>");
                    planeXML.WriteLine("			<volumeincrease>60.000000</volumeincrease>");
                    planeXML.WriteLine("			<frequencyincrease>463.000000</frequencyincrease>");
                    planeXML.WriteLine("		</boatengine>");
                }

                // Write other stuff
                planeXML.WriteLine("	</audio>");
                planeXML.WriteLine();
                planeXML.WriteLine("	<handling>");

                // Checks if "handling.CFG line comboBox" value is set to "Custom"
                if (comboBox13.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write mass
                    planeXML.WriteLine("		<mass>" + handlingValues[0] + "</mass>");

                    // Write percentsubmerged
                    planeXML.WriteLine("		<percentsubmerged>" + handlingValues[7] + "</percentsubmerged>");

                    // Write steeringlock
                    planeXML.WriteLine("		<steeringlock>" + handlingValues[19] + "</steeringlock>");

                    // Write seatoffset
                    planeXML.WriteLine("		<seatoffset>" + handlingValues[22] + "</seatoffset>");

                    // Write damagemultiplier
                    planeXML.WriteLine("		<damagemultiplier>" + handlingValues[23] + "</damagemultiplier>");

                    // Write value
                    planeXML.WriteLine("		<value>" + handlingValues[24] + "</value>");

                    // Write flags
                    if (handlingValues[29].Length < 8)
                    {
                        int difference = 8 - handlingValues[29].Length;
                        planeXML.WriteLine("		<flags>" + new string('0', difference) + handlingValues[29] + "</flags>");
                    }
                    else
                    {
                        planeXML.WriteLine("		<flags>" + handlingValues[29] + "</flags>");
                    }

                    // Write other stuff
                    planeXML.WriteLine();
                    planeXML.WriteLine("		<dimensions>");

                    // Write dimensions x, y, z
                    planeXML.WriteLine("			<x>" + handlingValues[1] + "</x>");
                    planeXML.WriteLine("			<y>" + handlingValues[2] + "</y>");
                    planeXML.WriteLine("			<z>" + handlingValues[3] + "</z>");

                    // Write other stuff
                    planeXML.WriteLine("		</dimensions>");
                    planeXML.WriteLine();
                    planeXML.WriteLine("		<centreofmass>");

                    // Write centreofmass x, y, z
                    planeXML.WriteLine("			<x>" + handlingValues[4] + "</x>");
                    planeXML.WriteLine("			<y>" + handlingValues[5] + "</y>");
                    planeXML.WriteLine("			<z>" + handlingValues[6] + "</z>");

                    // Write other stuff
                    planeXML.WriteLine("		</centreofmass>");
                    planeXML.WriteLine();

                    // Write boatsteering if plane replaces skimmer or traction if plane replaces rcbaron
                    if (isSkimmer)
                    {
                        // Write other stuff
                        planeXML.WriteLine("		<boatsteering>");

                        // Write bankforcemult
                        planeXML.WriteLine("			<bankforcemult>" + handlingValues[8] + "</bankforcemult>");

                        // Write rudderturnforce
                        planeXML.WriteLine("			<rudderturnforce>" + handlingValues[9] + "</rudderturnforce>");

                        // Write speedsteerfalloff
                        planeXML.WriteLine("			<speedsteerfalloff>" + handlingValues[10] + "</speedsteerfalloff>");

                        // Write other stuff
                        planeXML.WriteLine("		</boatsteering>");
                    }
                    else if (isRcbaron)
                    {
                        // Write other stuff
                        planeXML.WriteLine("		<traction>");

                        // Write multiplier
                        planeXML.WriteLine("			<multiplier>" + handlingValues[8] + "</multiplier>");

                        // Write loss
                        planeXML.WriteLine("			<loss>" + handlingValues[9] + "</loss>");

                        // Write bias
                        planeXML.WriteLine("			<bias>" + handlingValues[10] + "</bias>");

                        // Write other stuff
                        planeXML.WriteLine("		</traction>");
                    }

                    // Write other stuff
                    planeXML.WriteLine();
                    planeXML.WriteLine("		<transmission>");

                    // Write numofgears
                    planeXML.WriteLine("			<numofgears>" + handlingValues[11] + "</numofgears>");

                    // Write maxspeed
                    planeXML.WriteLine("			<maxspeed>" + handlingValues[12] + "</maxspeed>");

                    // Write acceleration
                    planeXML.WriteLine("			<acceleration>" + handlingValues[13] + "</acceleration>");

                    // Write drivetype
                    planeXML.WriteLine("			<drivetype>" + handlingValues[14] + "</drivetype>");

                    // Write enginetype
                    planeXML.WriteLine("			<enginetype>" + handlingValues[15] + "</enginetype>");

                    // Write other stuff
                    planeXML.WriteLine("		</transmission>");
                    planeXML.WriteLine();

                    // Write boatbrakes if plane replaces skimmer or brakes if plane replaces rcbaron
                    if (isSkimmer)
                    {
                        planeXML.WriteLine("		<boatbrakes>");

                        // Write verticalwavehitlimit
                        planeXML.WriteLine("			<verticalwavehitlimit>" + handlingValues[16] + "</verticalwavehitlimit>");

                        // Write forwardwavehitbrake
                        planeXML.WriteLine("			<forwardwavehitbrake>" + handlingValues[17] + "</forwardwavehitbrake>");

                        // Write other stuff
                        planeXML.WriteLine("		</boatbrakes>");
                    }
                    else if (isRcbaron)
                    {
                        // Write other stuff
                        planeXML.WriteLine("		<brakes>");

                        // Write deceleration
                        planeXML.WriteLine("			<deceleration>" + handlingValues[16] + "</deceleration>");

                        // Write bias
                        planeXML.WriteLine("			<bias>" + handlingValues[17] + "</bias>");

                        // Write abs
                        planeXML.WriteLine("			<abs>" + handlingValues[18] + "</abs>");

                        // Write other stuff
                        planeXML.WriteLine("		</brakes>");
                    }

                    // Write other stuff
                    planeXML.WriteLine();

                    // Write boatsuspension if plane replaces skimmer or suspension if plane replaces rcbaron
                    if (isSkimmer)
                    {
                        planeXML.WriteLine("		<boatsuspension>");

                        // Write waterresvolumemult
                        planeXML.WriteLine("			<waterresvolumemult>" + handlingValues[20] + "</waterresvolumemult>");

                        // Write waterdampingmult
                        planeXML.WriteLine("			<waterdampingmult>" + handlingValues[21] + "</waterdampingmult>");

                        // Write upperlimit
                        planeXML.WriteLine("			<upperlimit>" + handlingValues[25] + "</upperlimit>");

                        // Write handbrakedragmult
                        planeXML.WriteLine("			<handbrakedragmult>" + handlingValues[26] + "</handbrakedragmult>");

                        // Write sideslipforce
                        planeXML.WriteLine("			<sideslipforce>" + handlingValues[27] + "</sideslipforce>");

                        // Write antidive
                        planeXML.WriteLine("			<antidive>" + handlingValues[28] + "</antidive>");

                        // Write other stuff
                        planeXML.WriteLine("		</boatsuspension>");
                    }
                    else if (isRcbaron)
                    {
                        // Write other stuff
                        planeXML.WriteLine("		<suspension>");

                        // Write forcelevel
                        planeXML.WriteLine("			<forcelevel>" + handlingValues[20] + "</forcelevel>");

                        // Write dampening
                        planeXML.WriteLine("			<dampening>" + handlingValues[21] + "</dampening>");

                        // Write upperlimit
                        planeXML.WriteLine("			<upperlimit>" + handlingValues[25] + "</upperlimit>");

                        // Write lowerlimit
                        planeXML.WriteLine("			<lowerlimit>" + handlingValues[26] + "</lowerlimit>");

                        // Write bias
                        planeXML.WriteLine("			<bias>" + handlingValues[27] + "</bias>");

                        // Write antidive
                        planeXML.WriteLine("			<antidive>" + handlingValues[28] + "</antidive>");

                        // Write other stuff
                        planeXML.WriteLine("		</suspension>");
                    }

                    // Write other stuff
                    planeXML.WriteLine();
                    planeXML.WriteLine("		<lights>");

                    // Write front
                    planeXML.WriteLine("			<front>" + handlingValues[30] + "</front>");

                    // Write rear
                    planeXML.WriteLine("			<rear>" + handlingValues[31] + "</rear>");

                    // Write other stuff
                    planeXML.WriteLine("		</lights>");
                }
                else if (comboBox13.Text != string.Empty && comboBox13.Text != "Custom" && checkBox1.Checked == false)
                {
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox13.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<handling>"))
                        {
                            int handlingIndex = i + 1;
                            while (fileLines[handlingIndex].Contains("</handling>") == false)
                            {
                                planeXML.WriteLine(fileLines[handlingIndex]);
                                handlingIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                planeXML.WriteLine("	</handling>");
                planeXML.WriteLine();
                planeXML.WriteLine("	<aerohandling>");

                // Checks if "flying data line comboBox" value is set to "Custom"
                if (comboBox16.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write thrust
                    planeXML.WriteLine("		<thrust>" + handlingPlaneValues1[0] + "</thrust>");

                    // Write thrustfalloff
                    planeXML.WriteLine("		<thrustfalloff>" + handlingPlaneValues1[1] + "</thrustfalloff>");

                    // Write yaw
                    planeXML.WriteLine("		<yaw>" + handlingPlaneValues1[2] + "</yaw>");

                    // Write yawstab
                    planeXML.WriteLine("		<yawstab>" + handlingPlaneValues1[3] + "</yawstab>");

                    // Write sideslip
                    planeXML.WriteLine("		<sideslip>" + handlingPlaneValues1[4] + "</sideslip>");

                    // Write roll
                    planeXML.WriteLine("		<roll>" + handlingPlaneValues1[5] + "</roll>");

                    // Write rollstab
                    planeXML.WriteLine("		<rollstab>" + handlingPlaneValues1[6] + "</rollstab>");

                    // Write pitch
                    planeXML.WriteLine("		<pitch>" + handlingPlaneValues1[7] + "</pitch>");

                    // Write pitchstab
                    planeXML.WriteLine("		<pitchstab>" + handlingPlaneValues1[8] + "</pitchstab>");

                    // Write formlift
                    planeXML.WriteLine("		<formlift>" + handlingPlaneValues1[9] + "</formlift>");

                    // Write attacklift
                    planeXML.WriteLine("		<attacklift>" + handlingPlaneValues1[10] + "</attacklift>");

                    // Write moveres
                    planeXML.WriteLine("		<moveres>" + handlingPlaneValues1[11] + "</moveres>");

                    // Write other stuff
                    planeXML.WriteLine();
                    planeXML.WriteLine("		<turnres>");

                    // Write turnres x, y, z
                    planeXML.WriteLine("			<x>" + handlingPlaneValues1[12] + "</x>");
                    planeXML.WriteLine("			<y>" + handlingPlaneValues1[13] + "</y>");
                    planeXML.WriteLine("			<z>" + handlingPlaneValues1[14] + "</z>");

                    // Write other stuff
                    planeXML.WriteLine("		</turnres>");
                    planeXML.WriteLine();
                    planeXML.WriteLine("		<speedres>");

                    // Write speedres x, y, z
                    planeXML.WriteLine("			<x>" + handlingPlaneValues1[15] + "</x>");
                    planeXML.WriteLine("			<y>" + handlingPlaneValues1[16] + "</y>");
                    planeXML.WriteLine("			<z>" + handlingPlaneValues1[17] + "</z>");

                    // Write other stuff
                    planeXML.WriteLine("		</speedres>");
                }
                else if (comboBox16.Text != string.Empty && comboBox16.Text != "Custom" && checkBox1.Checked == false)
                {
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox16.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<aerohandling>"))
                        {
                            int aeroHandlingIndex = i + 1;
                            while (fileLines[aeroHandlingIndex].Contains("</aerohandling>") == false)
                            {
                                planeXML.WriteLine(fileLines[aeroHandlingIndex]);
                                aeroHandlingIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                planeXML.WriteLine("	</aerohandling>");

                // Write boathandling if plane replaces skimmer
                if (isSkimmer)
                {
                    // Write other stuff
                    planeXML.WriteLine();
                    planeXML.WriteLine("	<boathandling>");

                    // Checks if "boat data line comboBox" value is set to "Custom"
                    if (comboBox15.Text == "Custom" || checkBox1.Checked == true)
                    {
                        // Write thrusty
                        planeXML.WriteLine("		<thrusty>" + handlingPlaneValues2[0] + "</thrusty>");

                        // Write thrustz
                        planeXML.WriteLine("		<thrustz>" + handlingPlaneValues2[1] + "</thrustz>");

                        // Write thrustappz
                        planeXML.WriteLine("		<thrustappz>" + handlingPlaneValues2[2] + "</thrustappz>");

                        // Write aqplaneforce
                        planeXML.WriteLine("		<aqplaneforce>" + handlingPlaneValues2[3] + "</aqplaneforce>");

                        // Write aqplanelimit
                        planeXML.WriteLine("		<aqplanelimit>" + handlingPlaneValues2[4] + "</aqplanelimit>");

                        // Write aqplaneoffset
                        planeXML.WriteLine("		<aqplaneoffset>" + handlingPlaneValues2[5] + "</aqplaneoffset>");

                        // Write waveaudiomult
                        planeXML.WriteLine("		<waveaudiomult>" + handlingPlaneValues2[6] + "</waveaudiomult>");

                        // Write other stuff
                        planeXML.WriteLine();
                        planeXML.WriteLine("		<moveres>");

                        // Write moveres x, y, z
                        planeXML.WriteLine("			<x>" + handlingPlaneValues2[7] + "</x>");
                        planeXML.WriteLine("			<y>" + handlingPlaneValues2[8] + "</y>");
                        planeXML.WriteLine("			<z>" + handlingPlaneValues2[9] + "</z>");

                        // Write other stuff
                        planeXML.WriteLine("		</moveres>");
                        planeXML.WriteLine();
                        planeXML.WriteLine("		<turnres>");

                        // Write turnres x, y, z
                        planeXML.WriteLine("			<x>" + handlingPlaneValues2[10] + "</x>");
                        planeXML.WriteLine("			<y>" + handlingPlaneValues2[11] + "</y>");
                        planeXML.WriteLine("			<z>" + handlingPlaneValues2[12] + "</z>");

                        // Write other stuff
                        planeXML.WriteLine("		</turnres>");
                        planeXML.WriteLine();
                        planeXML.WriteLine("		<looklrbcamheight>" + handlingPlaneValues2[13] + "</looklrbcamheight>");
                    }
                    else if (comboBox15.Text != string.Empty && comboBox15.Text != "Custom" && checkBox1.Checked == false)
                    {
                        string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox15.Text));
                        for (int i = 0; i < fileLines.Length; i++)
                        {
                            if (fileLines[i].Contains("<boathandling>"))
                            {
                                int boatHandlingIndex = i + 1;
                                while (fileLines[boatHandlingIndex].Contains("</boathandling>") == false)
                                {
                                    planeXML.WriteLine(fileLines[boatHandlingIndex]);
                                    boatHandlingIndex++;
                                }
                                break;
                            }
                        }
                    }

                    // Write other stuff
                    planeXML.WriteLine("	</boathandling>");
                }

                // Write other stuff
                planeXML.WriteLine("</vehicle>");
            }
        }

        /// <summary>
        /// Builds heli XML file from default.ide, handling.cfg and carcols.dat lines.
        /// </summary>
        /// <param name="isHunter"></param>
        /// <param name="isSeaSpar"></param>
        /// <param name="heliName"></param>
        /// <param name="extraflags"></param>
        /// <param name="immunity"></param>
        /// <param name="enginefarsample"></param>
        /// <param name="enginenearsample"></param>
        /// <param name="hornsample"></param>
        /// <param name="hornfreq"></param>
        /// <param name="sirensample"></param>
        /// <param name="sirenfreq"></param>
        /// <param name="doorsounds"></param>
        /// <param name="helitype"></param>
        /// <param name="weapons"></param>
        /// <param name="rotorradius"></param>
        /// <param name="rotordammult"></param>
        private void BuildHeliXML(bool isHunter, bool isSeaSpar, string heliName, string extraflags, string immunity, string enginefarsample, string enginenearsample, string hornsample, string hornfreq, string sirensample, string sirenfreq, string doorsounds, string helitype, string weapons, string rotorradius, string rotordammult)
        {
            // Get default.ide values
            string[] defaultValues = textBox5.Text.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handling.txt values
            string[] handlingValues = textBox8.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handlingHeli.txt values
            string[] handlingHeliValues = textBox12.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get carcols.txt values
            string[] carcolsValues = File.ReadAllLines("samplexmls/temp/carcols.txt");

            // Get carcolsLine.txt values
            string[] carcolsLineValues = File.ReadAllText("samplexmls/temp/carcolsLine.txt").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            // Check if file "heliXML.xml" already exists
            if (File.Exists("samplexmls/temp/heliXML.xml"))
            {
                File.Delete("samplexmls/temp/heliXML.xml");
            }

            // StreamWriter
            using (var heliXML = new StreamWriter("samplexmls/temp/heliXML.xml", true))
            {
                // Write other stuff
                heliXML.WriteLine("<?xml version=\"1.0\" encoding=\"ASCII\"?>");
                heliXML.WriteLine("<vehicle>");
                heliXML.WriteLine("	<basic>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write type
                    heliXML.WriteLine("		<type>" + defaultValues[3] + "</type>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write type
                    heliXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<type>"));
                }

                // Write name
                heliXML.WriteLine("		<name>" + heliName + "</name>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write anims
                    heliXML.WriteLine("		<anims>" + defaultValues[6] + "</anims>");

                    // Write comprules
                    heliXML.WriteLine("		<comprules>" + defaultValues[10] + "</comprules>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write anims
                    heliXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<anims>"));

                    // Write comprules
                    heliXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<comprules>"));
                }

                // Write maxpassengers if heli replaces hunter
                if (isHunter)
                {
                    heliXML.WriteLine("		<maxpassengers>1</maxpassengers>");
                }

                // Write extraflags
                heliXML.WriteLine("		<extraflags>" + extraflags + "</extraflags>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write wheelmodel
                    if (defaultValues[11].CompareTo("249") == 0)
                    {
                        heliXML.WriteLine("		<wheelmodel>237</wheelmodel>");
                    }
                    else
                    {
                        heliXML.WriteLine("		<wheelmodel>" + defaultValues[11] + "</wheelmodel>");
                    }

                    // Write wheelscale
                    heliXML.WriteLine("		<wheelscale>" + defaultValues[12] + "</wheelscale>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write wheelmodel
                    heliXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<wheelmodel>"));

                    // Write wheelscale
                    heliXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<wheelscale>"));
                }

                // Writer immunity
                heliXML.WriteLine("		<immunity>" + immunity + "</immunity>");

                // Write other stuff
                heliXML.WriteLine("	</basic>");
                heliXML.WriteLine();
                heliXML.WriteLine("	<aidata>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write class
                    heliXML.WriteLine("		<class>" + defaultValues[7] + "</class>");

                    // Write freq
                    heliXML.WriteLine("		<freq>" + defaultValues[8] + "</freq>");

                    // Write level
                    heliXML.WriteLine("		<level>" + defaultValues[9] + "</level>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write class
                    heliXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<class>"));

                    // Write freq
                    heliXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<freq>"));

                    // Write level
                    heliXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<level>"));
                }

                // Write other stuff
                heliXML.WriteLine("	</aidata>");
                heliXML.WriteLine();
                heliXML.WriteLine("	<colors>");

                // Checks if "carcols.DAT line comboBox" value is set to "Custom"
                if (comboBox11.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write rgbcol
                    for (int i = 0; i < carcolsLineValues.Length; i++)
                    {
                        string rgbcol = string.Empty;
                        string[] rgbValues = carcolsLineValues[i].Split(',');
                        for (int j = 0; j < rgbValues.Length; j++)
                        {
                            if (j == rgbValues.Length - 1)
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0];
                            }
                            else
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0] + ",";
                            }
                        }
                        heliXML.WriteLine("		<rgbcol>" + rgbcol + "</rgbcol>");
                    }
                }
                else if (comboBox11.Text != string.Empty && comboBox11.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write carcol
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox11.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<carcol>"))
                        {
                            int carcolIndex = i;
                            while (fileLines[carcolIndex].Contains("<carcol>"))
                            {
                                heliXML.WriteLine(fileLines[carcolIndex]);
                                carcolIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                heliXML.WriteLine("	</colors>");
                heliXML.WriteLine();
                heliXML.WriteLine("	<audio>");

                // Write enginefarsample
                heliXML.WriteLine("		<enginefarsample>" + enginefarsample + "</enginefarsample>");

                // Write enginenearsample
                heliXML.WriteLine("		<enginenearsample>" + enginenearsample + "</enginenearsample>");

                // Write hornsample
                heliXML.WriteLine("		<hornsample>" + hornsample + "</hornsample>");

                // Write hornfreq
                heliXML.WriteLine("		<hornfreq>" + hornfreq + "</hornfreq>");

                // Write sirensample
                heliXML.WriteLine("		<sirensample>" + sirensample + "</sirensample>");

                // Write sirenfreq
                heliXML.WriteLine("		<sirenfreq>" + sirenfreq + "</sirenfreq>");

                // Write doorsounds
                heliXML.WriteLine("		<doorsounds>" + doorsounds + "</doorsounds>");

                // Write other stuff
                heliXML.WriteLine("	</audio>");
                heliXML.WriteLine();
                heliXML.WriteLine("	<helidata>");

                // Write helitype
                heliXML.WriteLine("		<helitype>" + helitype + "</helitype>");

                // Write weapons
                heliXML.WriteLine("		<weapons>" + weapons + "</weapons>");

                // Write rotorradius
                heliXML.WriteLine("		<rotorradius>" + rotorradius + "</rotorradius>");

                // Write rotordammult
                heliXML.WriteLine("		<rotordammult>" + rotordammult + "</rotordammult>");

                // Write other stuff
                heliXML.WriteLine("	</helidata>");
                heliXML.WriteLine();
                heliXML.WriteLine("	<handling>");

                // Checks if "handling.CFG line comboBox" value is set to "Custom"
                if (comboBox13.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write mass
                    heliXML.WriteLine("		<mass>" + handlingValues[0] + "</mass>");

                    // Write percentsubmerged
                    heliXML.WriteLine("		<percentsubmerged>" + handlingValues[7] + "</percentsubmerged>");

                    // Write steeringlock
                    heliXML.WriteLine("		<steeringlock>" + handlingValues[19] + "</steeringlock>");

                    // Write seatoffset
                    heliXML.WriteLine("		<seatoffset>" + handlingValues[22] + "</seatoffset>");

                    // Write damagemultiplier
                    heliXML.WriteLine("		<damagemultiplier>" + handlingValues[23] + "</damagemultiplier>");

                    // Write value
                    heliXML.WriteLine("		<value>" + handlingValues[24] + "</value>");

                    // Write flags
                    if (handlingValues[29].Length < 8)
                    {
                        int difference = 8 - handlingValues[29].Length;
                        heliXML.WriteLine("		<flags>" + new string('0', difference) + handlingValues[29] + "</flags>");
                    }
                    else
                    {
                        heliXML.WriteLine("		<flags>" + handlingValues[29] + "</flags>");
                    }

                    // Write other stuff
                    heliXML.WriteLine();
                    heliXML.WriteLine("		<dimensions>");

                    // Write dimensions x, y, z
                    heliXML.WriteLine("			<x>" + handlingValues[1] + "</x>");
                    heliXML.WriteLine("			<y>" + handlingValues[2] + "</y>");
                    heliXML.WriteLine("			<z>" + handlingValues[3] + "</z>");

                    // Write other stuff
                    heliXML.WriteLine("		</dimensions>");
                    heliXML.WriteLine();
                    heliXML.WriteLine("		<centreofmass>");

                    // Write centreofmass x, y, z
                    heliXML.WriteLine("			<x>" + handlingValues[4] + "</x>");
                    heliXML.WriteLine("			<y>" + handlingValues[5] + "</y>");
                    heliXML.WriteLine("			<z>" + handlingValues[6] + "</z>");

                    // Write other stuff
                    heliXML.WriteLine("		</centreofmass>");
                    heliXML.WriteLine();
                    heliXML.WriteLine("		<traction>");

                    // Write multiplier
                    heliXML.WriteLine("			<multiplier>" + handlingValues[8] + "</multiplier>");

                    // Write loss
                    heliXML.WriteLine("			<loss>" + handlingValues[9] + "</loss>");

                    // Write bias
                    heliXML.WriteLine("			<bias>" + handlingValues[10] + "</bias>");

                    // Write other stuff
                    heliXML.WriteLine("		</traction>");
                    heliXML.WriteLine();
                    heliXML.WriteLine("		<transmission>");

                    // Write numofgears
                    heliXML.WriteLine("			<numofgears>" + handlingValues[11] + "</numofgears>");

                    // Write maxspeed
                    heliXML.WriteLine("			<maxspeed>" + handlingValues[12] + "</maxspeed>");

                    // Write acceleration
                    heliXML.WriteLine("			<acceleration>" + handlingValues[13] + "</acceleration>");

                    // Write drivetype
                    heliXML.WriteLine("			<drivetype>" + handlingValues[14] + "</drivetype>");

                    // Write enginetype
                    heliXML.WriteLine("			<enginetype>" + handlingValues[15] + "</enginetype>");

                    // Write other stuff
                    heliXML.WriteLine("		</transmission>");
                    heliXML.WriteLine();
                    heliXML.WriteLine("		<brakes>");

                    // Write deceleration
                    heliXML.WriteLine("			<deceleration>" + handlingValues[16] + "</deceleration>");

                    // Write bias
                    heliXML.WriteLine("			<bias>" + handlingValues[17] + "</bias>");

                    // Write abs
                    heliXML.WriteLine("			<abs>" + handlingValues[18] + "</abs>");

                    // Write other stuff
                    heliXML.WriteLine("		</brakes>");
                    heliXML.WriteLine();
                    heliXML.WriteLine("		<suspension>");

                    // Write forcelevel
                    heliXML.WriteLine("			<forcelevel>" + handlingValues[20] + "</forcelevel>");

                    // Write dampening
                    heliXML.WriteLine("			<dampening>" + handlingValues[21] + "</dampening>");

                    // Write upperlimit
                    heliXML.WriteLine("			<upperlimit>" + handlingValues[25] + "</upperlimit>");

                    // Write lowerlimit
                    heliXML.WriteLine("			<lowerlimit>" + handlingValues[26] + "</lowerlimit>");

                    // Write bias
                    heliXML.WriteLine("			<bias>" + handlingValues[27] + "</bias>");

                    // Write antidive
                    heliXML.WriteLine("			<antidive>" + handlingValues[28] + "</antidive>");

                    // Write other stuff
                    heliXML.WriteLine("		</suspension>");
                    heliXML.WriteLine();
                    heliXML.WriteLine("		<lights>");

                    // Write front
                    heliXML.WriteLine("			<front>" + handlingValues[30] + "</front>");

                    // Write rear
                    heliXML.WriteLine("			<rear>" + handlingValues[31] + "</rear>");

                    // Write other stuff
                    heliXML.WriteLine("		</lights>");
                }
                else if (comboBox13.Text != string.Empty && comboBox13.Text != "Custom" && checkBox1.Checked == false)
                {
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox13.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<handling>"))
                        {
                            int handlingIndex = i + 1;
                            while (fileLines[handlingIndex].Contains("</handling>") == false)
                            {
                                heliXML.WriteLine(fileLines[handlingIndex]);
                                handlingIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                heliXML.WriteLine("	</handling>");
                heliXML.WriteLine();
                heliXML.WriteLine("	<aerohandling>");

                // Checks if "flying data line comboBox" value is set to "Custom"
                if (comboBox16.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write thrust
                    heliXML.WriteLine("		<thrust>" + handlingHeliValues[0] + "</thrust>");

                    // Write thrustfalloff
                    heliXML.WriteLine("		<thrustfalloff>" + handlingHeliValues[1] + "</thrustfalloff>");

                    // Write yaw
                    heliXML.WriteLine("		<yaw>" + handlingHeliValues[2] + "</yaw>");

                    // Write yawstab
                    heliXML.WriteLine("		<yawstab>" + handlingHeliValues[3] + "</yawstab>");

                    // Write sideslip
                    heliXML.WriteLine("		<sideslip>" + handlingHeliValues[4] + "</sideslip>");

                    // Write roll
                    heliXML.WriteLine("		<roll>" + handlingHeliValues[5] + "</roll>");

                    // Write rollstab
                    heliXML.WriteLine("		<rollstab>" + handlingHeliValues[6] + "</rollstab>");

                    // Write pitch
                    heliXML.WriteLine("		<pitch>" + handlingHeliValues[7] + "</pitch>");

                    // Write pitchstab
                    heliXML.WriteLine("		<pitchstab>" + handlingHeliValues[8] + "</pitchstab>");

                    // Write formlift
                    heliXML.WriteLine("		<formlift>" + handlingHeliValues[9] + "</formlift>");

                    // Write attacklift
                    heliXML.WriteLine("		<attacklift>" + handlingHeliValues[10] + "</attacklift>");

                    // Write moveres
                    heliXML.WriteLine("		<moveres>" + handlingHeliValues[11] + "</moveres>");

                    // Write other stuff
                    heliXML.WriteLine();
                    heliXML.WriteLine("		<turnres>");

                    // Write turnres x, y, z
                    heliXML.WriteLine("			<x>" + handlingHeliValues[12] + "</x>");
                    heliXML.WriteLine("			<y>" + handlingHeliValues[13] + "</y>");
                    heliXML.WriteLine("			<z>" + handlingHeliValues[14] + "</z>");

                    // Write other stuff
                    heliXML.WriteLine("		</turnres>");
                    heliXML.WriteLine();
                    heliXML.WriteLine("		<speedres>");

                    // Write speedres x, y, z
                    heliXML.WriteLine("			<x>" + handlingHeliValues[15] + "</x>");
                    heliXML.WriteLine("			<y>" + handlingHeliValues[16] + "</y>");
                    heliXML.WriteLine("			<z>" + handlingHeliValues[17] + "</z>");

                    // Write other stuff
                    heliXML.WriteLine("		</speedres>");
                }
                else if (comboBox16.Text != string.Empty && comboBox16.Text != "Custom" && checkBox1.Checked == false)
                {
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox16.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<aerohandling>"))
                        {
                            int heliHandlingIndex = i + 1;
                            while (fileLines[heliHandlingIndex].Contains("</aerohandling>") == false)
                            {
                                heliXML.WriteLine(fileLines[heliHandlingIndex]);
                                heliHandlingIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                heliXML.WriteLine("	</aerohandling>");

                // Write specials if heli replaces hunter or sea sparrow
                if (isHunter)
                {
                    heliXML.WriteLine();
                    heliXML.WriteLine("	<specials>");
                    heliXML.WriteLine("		<heliweps>");
                    heliXML.WriteLine("			<missilepos>");
                    heliXML.WriteLine("				<x>2.5</x>");
                    heliXML.WriteLine("				<y>1.0</y>");
                    heliXML.WriteLine("				<z>-0.5</z>");
                    heliXML.WriteLine("			</missilepos>");
                    heliXML.WriteLine();
                    heliXML.WriteLine("			<machinegunpos>");
                    heliXML.WriteLine("				<x>0.0</x>");
                    heliXML.WriteLine("				<y>4.8</y>");
                    heliXML.WriteLine("				<z>-1.3</z>");
                    heliXML.WriteLine("			</machinegunpos>");
                    heliXML.WriteLine("		</heliweps>");
                    heliXML.WriteLine("	</specials>");
                }
                else if (isSeaSpar)
                {
                    heliXML.WriteLine();
                    heliXML.WriteLine("	<specials>");
                    heliXML.WriteLine("		<heliweps>");
                    heliXML.WriteLine("			<machinegunpos>");
                    heliXML.WriteLine("				<x>0.0</x>");
                    heliXML.WriteLine("				<y>4.8</y>");
                    heliXML.WriteLine("				<z>-1.3</z>");
                    heliXML.WriteLine("			</machinegunpos>");
                    heliXML.WriteLine("		</heliweps>");
                    heliXML.WriteLine("	</specials>");
                }

                // Write other stuff
                heliXML.WriteLine("</vehicle>");
            }
        }

        /// <summary>
        /// Builds boat XML file from default.ide, handling.cfg and carcols.dat lines.
        /// </summary>
        /// <param name="boatName"></param>
        /// <param name="extraflags"></param>
        /// <param name="immunity"></param>
        /// <param name="enginefarsample"></param>
        /// <param name="enginenearsample"></param>
        /// <param name="hornsample"></param>
        /// <param name="hornfreq"></param>
        /// <param name="sirensample"></param>
        /// <param name="sirenfreq"></param>
        /// <param name="doorsounds"></param>
        /// <param name="type"></param>
        /// <param name="basevolume"></param>
        /// <param name="basefrequency"></param>
        /// <param name="volumeincrease"></param>
        /// <param name="frequencyincrease"></param>
        private void BuildBoatXML(string boatName, string extraflags, string immunity, string enginefarsample, string enginenearsample, string hornsample, string hornfreq, string sirensample, string sirenfreq, string doorsounds, string type, string basevolume, string basefrequency, string volumeincrease, string frequencyincrease)
        {
            // Get default.ide values
            string[] defaultValues = textBox5.Text.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handling.txt values
            string[] handlingValues = textBox8.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get handlingBoat.txt values
            string[] handlingBoatValues = textBox11.Text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

            // Get carcols.txt values
            string[] carcolsValues = File.ReadAllLines("samplexmls/temp/carcols.txt");

            // Get carcolsLine.txt values
            string[] carcolsLineValues = File.ReadAllText("samplexmls/temp/carcolsLine.txt").Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            // Check if file "boatXML.xml" already exists
            if (File.Exists("samplexmls/temp/boatXML.xml"))
            {
                File.Delete("samplexmls/temp/boatXML.xml");
            }

            // StreamWriter
            using (var boatXML = new StreamWriter("samplexmls/temp/boatXML.xml", true))
            {
                // Write other stuff
                boatXML.WriteLine("<?xml version=\"1.0\" encoding=\"ASCII\"?>");
                boatXML.WriteLine("<vehicle>");
                boatXML.WriteLine("	<basic>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write type
                    boatXML.WriteLine("		<type>" + defaultValues[3] + "</type>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write type
                    boatXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<type>"));
                }

                // Write name
                boatXML.WriteLine("		<name>" + boatName + "</name>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write anims
                    boatXML.WriteLine("		<anims>" + defaultValues[6] + "</anims>");

                    // Write comprules
                    boatXML.WriteLine("		<comprules>" + defaultValues[10] + "</comprules>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write anims
                    boatXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<anims>"));

                    // Write comprules
                    boatXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<comprules>"));
                }

                // Write extraflags
                boatXML.WriteLine("		<extraflags>" + extraflags + "</extraflags>");

                // Writer immunity
                boatXML.WriteLine("		<immunity>" + immunity + "</immunity>");

                // Write other stuff
                boatXML.WriteLine("	</basic>");
                boatXML.WriteLine();
                boatXML.WriteLine("	<aidata>");

                // Checks if "default.IDE line comboBox" value is set to "Custom"
                if (comboBox12.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write class
                    boatXML.WriteLine("		<class>" + defaultValues[7] + "</class>");

                    // Write freq
                    boatXML.WriteLine("		<freq>" + defaultValues[8] + "</freq>");

                    // Write level
                    boatXML.WriteLine("		<level>" + defaultValues[9] + "</level>");
                }
                else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write class
                    boatXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<class>"));

                    // Write freq
                    boatXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<freq>"));

                    // Write level
                    boatXML.WriteLine(FindLineInSampleXML(FindSampleXML(comboBox12.Text), "<level>"));
                }

                // Write other stuff
                boatXML.WriteLine("	</aidata>");
                boatXML.WriteLine();
                boatXML.WriteLine("	<colors>");

                // Checks if "carcols.DAT line comboBox" value is set to "Custom"
                if (comboBox11.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write rgbcol
                    for (int i = 0; i < carcolsLineValues.Length; i++)
                    {
                        string rgbcol = string.Empty;
                        string[] rgbValues = carcolsLineValues[i].Split(',');
                        for (int j = 0; j < rgbValues.Length; j++)
                        {
                            if (j == rgbValues.Length - 1)
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0];
                            }
                            else
                            {
                                rgbcol += carcolsValues[Convert.ToInt32(rgbValues[j])].Split(new[] { '#', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[0] + ",";
                            }
                        }
                        boatXML.WriteLine("		<rgbcol>" + rgbcol + "</rgbcol>");
                    }
                }
                else if (comboBox11.Text != string.Empty && comboBox11.Text != "Custom" && checkBox1.Checked == false)
                {
                    // Write carcol
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox11.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<carcol>"))
                        {
                            int carcolIndex = i;
                            while (fileLines[carcolIndex].Contains("<carcol>"))
                            {
                                boatXML.WriteLine(fileLines[carcolIndex]);
                                carcolIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                boatXML.WriteLine("	</colors>");
                boatXML.WriteLine();
                boatXML.WriteLine("	<audio>");

                // Write enginefarsample
                boatXML.WriteLine("		<enginefarsample>" + enginefarsample + "</enginefarsample>");

                // Write enginenearsample
                boatXML.WriteLine("		<enginenearsample>" + enginenearsample + "</enginenearsample>");

                // Write hornsample
                boatXML.WriteLine("		<hornsample>" + hornsample + "</hornsample>");

                // Write hornfreq
                boatXML.WriteLine("		<hornfreq>" + hornfreq + "</hornfreq>");

                // Write sirensample
                boatXML.WriteLine("		<sirensample>" + sirensample + "</sirensample>");

                // Write sirenfreq
                boatXML.WriteLine("		<sirenfreq>" + sirenfreq + "</sirenfreq>");

                // Write doorsounds
                boatXML.WriteLine("		<doorsounds>" + doorsounds + "</doorsounds>");

                // Write other stuff
                boatXML.WriteLine();
                boatXML.WriteLine("		<boatengine>");

                // Write type
                boatXML.WriteLine("			<type>" + type + "</type>");

                // Write basevolume
                boatXML.WriteLine("			<basevolume>" + basevolume + "</basevolume>");

                // Write basefrequency
                boatXML.WriteLine("			<basefrequency>" + basefrequency + "</basefrequency>");

                // Write volumeincrease
                boatXML.WriteLine("			<volumeincrease>" + volumeincrease + "</volumeincrease>");

                // Write frequencyincrease
                boatXML.WriteLine("			<frequencyincrease>" + frequencyincrease + "</frequencyincrease>");

                // Write other stuff
                boatXML.WriteLine("		</boatengine>");
                boatXML.WriteLine("	</audio>");
                boatXML.WriteLine();
                boatXML.WriteLine("	<handling>");

                // Checks if "handling.CFG line comboBox" value is set to "Custom"
                if (comboBox13.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write mass
                    boatXML.WriteLine("		<mass>" + handlingValues[0] + "</mass>");

                    // Write percentsubmerged
                    boatXML.WriteLine("		<percentsubmerged>" + handlingValues[7] + "</percentsubmerged>");

                    // Write steeringlock
                    boatXML.WriteLine("		<steeringlock>" + handlingValues[19] + "</steeringlock>");

                    // Write seatoffset
                    boatXML.WriteLine("		<seatoffset>" + handlingValues[22] + "</seatoffset>");

                    // Write damagemultiplier
                    boatXML.WriteLine("		<damagemultiplier>" + handlingValues[23] + "</damagemultiplier>");

                    // Write value
                    boatXML.WriteLine("		<value>" + handlingValues[24] + "</value>");

                    // Write flags
                    if (handlingValues[29].Length < 8)
                    {
                        int difference = 8 - handlingValues[29].Length;
                        boatXML.WriteLine("		<flags>" + new string('0', difference) + handlingValues[29] + "</flags>");
                    }
                    else
                    {
                        boatXML.WriteLine("		<flags>" + handlingValues[29] + "</flags>");
                    }

                    // Write other stuff
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<dimensions>");

                    // Write dimensions x, y, z
                    boatXML.WriteLine("			<x>" + handlingValues[1] + "</x>");
                    boatXML.WriteLine("			<y>" + handlingValues[2] + "</y>");
                    boatXML.WriteLine("			<z>" + handlingValues[3] + "</z>");

                    // Write other stuff
                    boatXML.WriteLine("		</dimensions>");
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<centreofmass>");

                    // Write centreofmass x, y, z
                    boatXML.WriteLine("			<x>" + handlingValues[4] + "</x>");
                    boatXML.WriteLine("			<y>" + handlingValues[5] + "</y>");
                    boatXML.WriteLine("			<z>" + handlingValues[6] + "</z>");

                    // Write other stuff
                    boatXML.WriteLine("		</centreofmass>");
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<boatsteering>");

                    // Write bankforcemult
                    boatXML.WriteLine("			<bankforcemult>" + handlingValues[8] + "</bankforcemult>");

                    // Write rudderturnforce
                    boatXML.WriteLine("			<rudderturnforce>" + handlingValues[9] + "</rudderturnforce>");

                    // Write speedsteerfalloff
                    boatXML.WriteLine("			<speedsteerfalloff>" + handlingValues[10] + "</speedsteerfalloff>");

                    // Write other stuff
                    boatXML.WriteLine("		</boatsteering>");
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<transmission>");

                    // Write numofgears
                    boatXML.WriteLine("			<numofgears>" + handlingValues[11] + "</numofgears>");

                    // Write maxspeed
                    boatXML.WriteLine("			<maxspeed>" + handlingValues[12] + "</maxspeed>");

                    // Write acceleration
                    boatXML.WriteLine("			<acceleration>" + handlingValues[13] + "</acceleration>");

                    // Write drivetype
                    boatXML.WriteLine("			<drivetype>" + handlingValues[14] + "</drivetype>");

                    // Write enginetype
                    boatXML.WriteLine("			<enginetype>" + handlingValues[15] + "</enginetype>");

                    // Write other stuff
                    boatXML.WriteLine("		</transmission>");
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<boatbrakes>");

                    // Write verticalwavehitlimit
                    boatXML.WriteLine("			<verticalwavehitlimit>" + handlingValues[16] + "</verticalwavehitlimit>");

                    // Write forwardwavehitbrake
                    boatXML.WriteLine("			<forwardwavehitbrake>" + handlingValues[17] + "</forwardwavehitbrake>");

                    // Write other stuff
                    boatXML.WriteLine("		</boatbrakes>");
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<boatsuspension>");

                    // Write waterresvolumemult
                    boatXML.WriteLine("			<waterresvolumemult>" + handlingValues[20] + "</waterresvolumemult>");

                    // Write waterdampingmult
                    boatXML.WriteLine("			<waterdampingmult>" + handlingValues[21] + "</waterdampingmult>");

                    // Write upperlimit
                    boatXML.WriteLine("			<upperlimit>" + handlingValues[25] + "</upperlimit>");

                    // Write handbrakedragmult
                    boatXML.WriteLine("			<handbrakedragmult>" + handlingValues[26] + "</handbrakedragmult>");

                    // Write sideslipforce
                    boatXML.WriteLine("			<sideslipforce>" + handlingValues[27] + "</sideslipforce>");

                    // Write antidive
                    boatXML.WriteLine("			<antidive>" + handlingValues[28] + "</antidive>");

                    // Write other stuff
                    boatXML.WriteLine("		</boatsuspension>");
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<lights>");

                    // Write front
                    boatXML.WriteLine("			<front>" + handlingValues[30] + "</front>");

                    // Write rear
                    boatXML.WriteLine("			<rear>" + handlingValues[31] + "</rear>");

                    // Write other stuff
                    boatXML.WriteLine("		</lights>");
                }
                else if (comboBox13.Text != string.Empty && comboBox13.Text != "Custom" && checkBox1.Checked == false)
                {
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox13.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<handling>"))
                        {
                            int handlingIndex = i + 1;
                            while (fileLines[handlingIndex].Contains("</handling>") == false)
                            {
                                boatXML.WriteLine(fileLines[handlingIndex]);
                                handlingIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                boatXML.WriteLine("	</handling>");
                boatXML.WriteLine();
                boatXML.WriteLine("	<boathandling>");

                // Checks if "boat data line comboBox" value is set to "Custom"
                if (comboBox15.Text == "Custom" || checkBox1.Checked == true)
                {
                    // Write thrusty
                    boatXML.WriteLine("		<thrusty>" + handlingBoatValues[0] + "</thrusty>");

                    // Write thrustz
                    boatXML.WriteLine("		<thrustz>" + handlingBoatValues[1] + "</thrustz>");

                    // Write thrustappz
                    boatXML.WriteLine("		<thrustappz>" + handlingBoatValues[2] + "</thrustappz>");

                    // Write aqplaneforce
                    boatXML.WriteLine("		<aqplaneforce>" + handlingBoatValues[3] + "</aqplaneforce>");

                    // Write aqplanelimit
                    boatXML.WriteLine("		<aqplanelimit>" + handlingBoatValues[4] + "</aqplanelimit>");

                    // Write aqplaneoffset
                    boatXML.WriteLine("		<aqplaneoffset>" + handlingBoatValues[5] + "</aqplaneoffset>");

                    // Write waveaudiomult
                    boatXML.WriteLine("		<waveaudiomult>" + handlingBoatValues[6] + "</waveaudiomult>");

                    // Write other stuff
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<moveres>");

                    // Write moveres x, y, z
                    boatXML.WriteLine("			<x>" + handlingBoatValues[7] + "</x>");
                    boatXML.WriteLine("			<y>" + handlingBoatValues[8] + "</y>");
                    boatXML.WriteLine("			<z>" + handlingBoatValues[9] + "</z>");

                    // Write other stuff
                    boatXML.WriteLine("		</moveres>");
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<turnres>");

                    // Write turnres x, y, z
                    boatXML.WriteLine("			<x>" + handlingBoatValues[10] + "</x>");
                    boatXML.WriteLine("			<y>" + handlingBoatValues[11] + "</y>");
                    boatXML.WriteLine("			<z>" + handlingBoatValues[12] + "</z>");

                    // Write other stuff
                    boatXML.WriteLine("		</turnres>");
                    boatXML.WriteLine();
                    boatXML.WriteLine("		<looklrbcamheight>" + handlingBoatValues[13] + "</looklrbcamheight>");
                }
                else if (comboBox15.Text != string.Empty && comboBox15.Text != "Custom" && checkBox1.Checked == false)
                {
                    string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox15.Text));
                    for (int i = 0; i < fileLines.Length; i++)
                    {
                        if (fileLines[i].Contains("<boathandling>"))
                        {
                            int boatHandlingIndex = i + 1;
                            while (fileLines[boatHandlingIndex].Contains("</boathandling>") == false)
                            {
                                boatXML.WriteLine(fileLines[boatHandlingIndex]);
                                boatHandlingIndex++;
                            }
                            break;
                        }
                    }
                }

                // Write other stuff
                boatXML.WriteLine("	</boathandling>");
                boatXML.WriteLine("</vehicle>");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.Filter = "dat files (*.dat)|*.dat|All files (*.*)|*.*";
            choofdlog.FilterIndex = 2;

            if (choofdlog.ShowDialog() == DialogResult.OK)
            {
                string sFileName = choofdlog.FileName;

                // "carcols.txt" file (stores fixed colors in a temporary file) and "carcolsOriginal.txt" file (stores original colors in a temporary file)
                File.WriteAllText("samplexmls/temp/carcols.txt", File.ReadAllText("samplexmls/originalFiles/carcols.dat"));
                File.WriteAllText("samplexmls/temp/carcolsOriginal.txt", File.ReadAllText("samplexmls/originalFiles/carcols.dat"));

                // Get all colors from selected file (sFileName)
                string[] carcolsLines = File.ReadAllLines(sFileName);

                Regex regex = new Regex(@"^(\s+)?(\d+)(\s+)?(\,)(\s+)?(\d+)(\s+)?(\,)(\s+)?(\d+)(\s+)(\#)(\s+)(\d+)");
                int matchesCount = 0;

                // StreamWriter "carcolsOriginal.txt"
                using (var carcolsOriginal = new StreamWriter("samplexmls/temp/carcolsOriginal.txt", true))
                {
                    // Adds new colors (carcols) from a selected file to file "carcolsOriginal.txt"
                    for (int i = 0; i < carcolsLines.Length; i++)
                    {
                        if (regex.Match(carcolsLines[i]).Success)
                        {
                            string[] carcolsLineValues = carcolsLines[i].Split(new[] { ' ', '\t', ',', '#' }, StringSplitOptions.RemoveEmptyEntries);
                            if (Convert.ToInt32(carcolsLineValues[3]) > 94)
                            {
                                carcolsOriginal.Write("\r\n" + carcolsLineValues[0] + "," + carcolsLineValues[1] + "," + carcolsLineValues[2] + " # " + carcolsLineValues[3]);
                            }
                        }
                    }
                }

                // StreamWriter "carcols.txt"
                using (var carcols = new StreamWriter("samplexmls/temp/carcols.txt", true))
                {
                    for (int i = 0; i < carcolsLines.Length; i++)
                    {
                        Match match = regex.Match(carcolsLines[i]);
                        if (match.Success)
                        {
                            string[] carcolsLineValues = carcolsLines[i].Split(new[] { ' ', '\t', ',', '#' }, StringSplitOptions.RemoveEmptyEntries);
                            if (Convert.ToInt32(carcolsLineValues[3]) > 94)
                            {
                                matchesCount++;
                            }

                            // Makes sure that the first match (color) starts with "# 95"
                            if (matchesCount == 1 && new Regex(@"^(\s+)?(\d+)(\s+)?(\,)(\s+)?(\d+)(\s+)?(\,)(\s+)?(\d+)(\s+)(\#)(\s+)(\9)(\5)").Match(carcolsLines[i]).Success)
                            {
                                carcols.Write("\r\n" + carcolsLineValues[0] + "," + carcolsLineValues[1] + "," + carcolsLineValues[2] + " # " + carcolsLineValues[3]);
                            }
                            else
                            {
                                // The first match (color) starts with "# 95"
                                if (matchesCount == 1 && Convert.ToInt32(carcolsLineValues[3]) > 94)
                                {
                                    carcolsLines[i] = new Regex(@"(\#\s+\d+)").Replace(carcolsLines[i], "# 95");
                                    carcolsLineValues = carcolsLines[i].Split(new[] { ' ', '\t', ',', '#' }, StringSplitOptions.RemoveEmptyEntries);
                                    carcols.Write("\r\n" + carcolsLineValues[0] + "," + carcolsLineValues[1] + "," + carcolsLineValues[2] + " # " + carcolsLineValues[3]);
                                }
                                else if (i - 1 >= 0)
                                {
                                    // Makes sure that matches (colors/carcols) follow a sequence: "# 95", "# 96", "# 97"...
                                    string[] previousCarcolsLineValues = carcolsLines[i - 1].Split(new[] { ' ', '\t', ',', '#' }, StringSplitOptions.RemoveEmptyEntries);
                                    if (Convert.ToInt32(carcolsLineValues[3]) > 94 && Convert.ToInt32(carcolsLineValues[3]) - Convert.ToInt32(previousCarcolsLineValues[3]) != 1)
                                    {
                                        string correctNumber = "# " + (Convert.ToInt32(previousCarcolsLineValues[3]) + 1).ToString();
                                        carcolsLines[i] = new Regex(@"(\#\s+\d+)").Replace(carcolsLines[i], correctNumber);
                                        carcolsLineValues = carcolsLines[i].Split(new[] { ' ', '\t', ',', '#' }, StringSplitOptions.RemoveEmptyEntries);
                                        carcols.Write("\r\n" + carcolsLineValues[0] + "," + carcolsLineValues[1] + "," + carcolsLineValues[2] + " # " + carcolsLineValues[3]);
                                    }
                                    else if (Convert.ToInt32(carcolsLineValues[3]) > 94 && Convert.ToInt32(carcolsLineValues[3]) - Convert.ToInt32(previousCarcolsLineValues[3]) == 1)
                                    {
                                        carcols.Write("\r\n" + carcolsLineValues[0] + "," + carcolsLineValues[1] + "," + carcolsLineValues[2] + " # " + carcolsLineValues[3]);
                                    }
                                }
                            }
                        }
                    }
                }
                // Checks if file is invalid
                if (matchesCount == 0)
                {
                    // Appends "error" textBox message
                    textBox13.ForeColor = Color.Blue;
                    textBox13.Visible = true;
                    textBox13.Text = "WARNING:";
                    textBox13.Text += "\r\n[carcols file]: Invalid carcols file (no new colors found).";
                }
                else
                {
                    textBox13.ForeColor = Color.Red;
                    textBox13.Visible = false;
                    textBox13.Text = textBox13.Tag.ToString();
                }
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox9_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Disables "Paste carcols, default and handling lines" checkBox
            checkBox1.Enabled = false;
            checkBox1.Checked = false;

            // Adds vehicles to "carcols.DAT" and "handling.CFG" comboBox if "vehicle type" comboBox value is not empty
            if (comboBox9.Text != string.Empty)
            {
                // Gets all vehicle xml files and names
                string[] vehicleFiles = Directory.GetFiles("samplexmls/normalVehicles").Select(file => Path.GetFileName(file)).ToArray();
                string[] vehicleNames = new string[vehicleFiles.Length];
                for (int i = 0; i < vehicleFiles.Length; i++)
                {
                    // Get <name> index from vehicle xml file
                    string[] nameLines = File.ReadAllLines("samplexmls/normalVehicles/" + vehicleFiles[i]);
                    for (int j = 0; j < nameLines.Length; j++)
                    {
                        if (nameLines[j].Contains("<name>"))
                        {
                            int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                            int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                            vehicleNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                            if (vehicleFiles[i].CompareTo("vicechee.xml") == 0)
                            {
                                vehicleNames[i] = "Vice Cheetah";
                            }
                            else if (vehicleFiles[i].CompareTo("rcgobli.xml") == 0)
                            {
                                vehicleNames[i] = "RC Goblin";
                            }
                            else if (vehicleFiles[i].CompareTo("hotrina.xml") == 0)
                            {
                                vehicleNames[i] = "Hotring Racer (hotrina)";
                            }
                            else if (vehicleFiles[i].CompareTo("hotrinb.xml") == 0)
                            {
                                vehicleNames[i] = "Hotring Racer (hotrinb)";
                            }
                            else if (vehicleFiles[i].CompareTo("hotring.xml") == 0)
                            {
                                vehicleNames[i] = "Hotring Racer (hotring)";
                            }
                            else if (vehicleFiles[i].CompareTo("bloodra.xml") == 0)
                            {
                                vehicleNames[i] = "Bloodring Banger (bloodra)";
                            }
                            else if (vehicleFiles[i].CompareTo("bloodrb.xml") == 0)
                            {
                                vehicleNames[i] = "Bloodring Banger (bloodrb)";
                            }
                            break;
                        }
                    }
                }

                // Adds vehicles to "carcols.DAT" comboBox
                comboBox11.Enabled = true;
                comboBox11.Items.Clear();
                comboBox11.Items.Add("Custom");
                comboBox11.Items.AddRange(vehicleNames);

                // Adds vehicles to "handling.CFG" comboBox
                comboBox13.Enabled = true;
                comboBox13.Items.Clear();
                comboBox13.Items.Add("Custom");
                comboBox13.Items.AddRange(vehicleNames);
            }

            // Determines which settings are available depending on "vehicle type" comboBox
            if (comboBox9.Text == "Bike")
            {
                // Gets all bike xml files and names
                string[] bikeFiles = Directory.GetFiles("samplexmls/bikes").Select(file => Path.GetFileName(file)).ToArray();
                string[] bikeNames = new string[bikeFiles.Length];
                for (int i = 0; i < bikeFiles.Length; i++)
                {
                    // Get <name> index from bike xml file
                    string[] nameLines = File.ReadAllLines("samplexmls/bikes/" + bikeFiles[i]);
                    for (int j = 0; j < nameLines.Length; j++)
                    {
                        if (nameLines[j].Contains("<name>"))
                        {
                            int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                            int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                            bikeNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                            break;
                        }
                    }
                }

                // Adds bikes to "vehicle type" comboBox
                comboBox10.Enabled = true;
                comboBox10.Items.Clear();
                comboBox10.Items.Add("Regular Bike");
                comboBox10.Items.Add("Pizza Boy");

                // Adds bikes to "extra flags" comboBox
                comboBox1.Enabled = true;
                comboBox1.Items.Clear();
                comboBox1.Text = comboBox1.Tag.ToString();
                comboBox1.Items.Add("0000");

                // Adds bikes to "immunity" comboBox
                comboBox2.Enabled = true;
                comboBox2.Items.Clear();
                comboBox2.Text = comboBox2.Tag.ToString();
                comboBox2.Items.Add("0");
                comboBox2.Items.Add("Rhino");

                // Adds bikes to "engine audio" comboBox
                comboBox3.Enabled = true;
                comboBox3.Items.Clear();
                comboBox3.Items.AddRange(bikeNames);

                // Adds bikes to "horn audio" comboBox
                comboBox4.Enabled = true;
                comboBox4.Items.Clear();
                comboBox4.Items.AddRange(bikeNames);

                // Adds bikes to "siren audio" comboBox
                comboBox5.Enabled = true;
                comboBox5.Items.Clear();
                comboBox5.Items.AddRange(bikeNames);

                // Adds bikes to "door audio" comboBox
                comboBox6.Enabled = true;
                comboBox6.Items.Clear();
                comboBox6.Items.AddRange(bikeNames);

                // Adds bikes to "default" comboBox
                comboBox12.Enabled = true;
                comboBox12.Items.Clear();
                comboBox12.Items.Add("Custom");
                comboBox12.Items.AddRange(bikeNames);

                // Adds bikes to "bike data" comboBox
                comboBox14.Enabled = true;
                comboBox14.Items.Clear();
                comboBox14.Items.Add("Custom");
                comboBox14.Items.AddRange(bikeNames);

                // Disables "boat data", "flying data", "boat engine audio" and "helicopter data" comboBox
                comboBox15.Enabled = false;
                comboBox16.Enabled = false;
                comboBox7.Enabled = false;
                comboBox8.Enabled = false;
                comboBox15.Items.Clear();
                comboBox16.Items.Clear();
                comboBox7.Items.Clear();
                comboBox8.Items.Clear();

                // Disables "carcols", "default", "handling", "bike handling", "boat/plane handling" and "helicopter/plane handling" textBox
                textBox3.Enabled = false;
                textBox5.Enabled = false;
                textBox8.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                textBox3.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox8.Text = string.Empty;
                textBox10.Text = string.Empty;
                textBox11.Text = string.Empty;
                textBox12.Text = string.Empty;

                // Enables "carcols.DAT" and "Save XML" button
                button1.Enabled = true;
                button2.Enabled = true;

                // Sets "vehicle category" comboBox value to "Regular Bike"
                comboBox10.Text = "Regular Bike";

                // Sets "carcols.DAT line", "default.IDE line", "handling.CFG line" and "bike data line" comboBox value to "Custom"
                comboBox11.Text = "Custom";
                comboBox12.Text = "Custom";
                comboBox13.Text = "Custom";
                comboBox14.Text = "Custom";
            }
            else if (comboBox9.Text == "Boat")
            {
                // Gets all boat xml files and names
                string[] boatFiles = Directory.GetFiles("samplexmls/boats").Select(file => Path.GetFileName(file)).ToArray();
                string[] boatNames = new string[boatFiles.Length];
                for (int i = 0; i < boatFiles.Length; i++)
                {
                    // Get <name> index from boat xml file
                    string[] nameLines = File.ReadAllLines("samplexmls/boats/" + boatFiles[i]);
                    for (int j = 0; j < nameLines.Length; j++)
                    {
                        if (nameLines[j].Contains("<name>"))
                        {
                            int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                            int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                            boatNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                            break;
                        }
                    }
                }

                // Adds boats to "vehicle type" comboBox
                comboBox10.Enabled = true;
                comboBox10.Items.Clear();
                comboBox10.Items.Add("Regular Boat");

                // Adds boats to "extra flags" comboBox
                comboBox1.Enabled = true;
                comboBox1.Items.Clear();
                comboBox1.Text = comboBox1.Tag.ToString();
                comboBox1.Items.Add("0000");
                comboBox1.Items.Add("Predator");

                // Adds boats to "immunity" comboBox
                comboBox2.Enabled = true;
                comboBox2.Items.Clear();
                comboBox2.Text = comboBox2.Tag.ToString();
                comboBox2.Items.Add("0");
                comboBox2.Items.Add("Rhino");

                // Adds boats to "engine audio" comboBox
                comboBox3.Enabled = true;
                comboBox3.Items.Clear();
                comboBox3.Items.AddRange(boatNames);

                // Adds boats to "horn audio" comboBox
                comboBox4.Enabled = true;
                comboBox4.Items.Clear();
                comboBox4.Items.AddRange(boatNames);

                // Adds boats to "siren audio" comboBox
                comboBox5.Enabled = true;
                comboBox5.Items.Clear();
                comboBox5.Items.AddRange(boatNames);

                // Adds boats to "door audio" comboBox
                comboBox6.Enabled = true;
                comboBox6.Items.Clear();
                comboBox6.Items.AddRange(boatNames);

                // Adds boats to "boat engine audio" comboBox
                comboBox7.Enabled = true;
                comboBox7.Items.Clear();
                comboBox7.Items.AddRange(boatNames);

                // Adds boats to "default" comboBox
                comboBox12.Enabled = true;
                comboBox12.Items.Clear();
                comboBox12.Items.Add("Custom");
                comboBox12.Items.AddRange(boatNames);

                // Adds boats to "boat data" comboBox
                comboBox15.Enabled = true;
                comboBox15.Items.Clear();
                comboBox15.Items.Add("Custom");
                comboBox15.Items.AddRange(boatNames);

                // Disables "bike data", "flying data" and "helicopter data" comboBox
                comboBox14.Enabled = false;
                comboBox16.Enabled = false;
                comboBox8.Enabled = false;
                comboBox14.Items.Clear();
                comboBox16.Items.Clear();
                comboBox8.Items.Clear();

                // Disables "carcols", "default", "handling", "bike handling", "boat/plane handling" and "helicopter/plane handling" textBox
                textBox3.Enabled = false;
                textBox5.Enabled = false;
                textBox8.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                textBox3.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox8.Text = string.Empty;
                textBox10.Text = string.Empty;
                textBox11.Text = string.Empty;
                textBox12.Text = string.Empty;

                // Enables "carcols.DAT" and "Save XML" button
                button1.Enabled = true;
                button2.Enabled = true;

                // Sets "vehicle category" comboBox value to "Regular Boat"
                comboBox10.Text = "Regular Boat";

                // Sets "carcols.DAT line", "default.IDE line", "handling.CFG line" and "boat data line" comboBox value to "Custom"
                comboBox11.Text = "Custom";
                comboBox12.Text = "Custom";
                comboBox13.Text = "Custom";
                comboBox15.Text = "Custom";
            }
            else if (comboBox9.Text == "Car")
            {
                // Gets all car xml files and names
                string[] carFiles = Directory.GetFiles("samplexmls/cars").Select(file => Path.GetFileName(file)).ToArray();
                string[] carNames = new string[carFiles.Length];
                for (int i = 0; i < carFiles.Length; i++)
                {
                    // Get <name> index from car xml file
                    string[] nameLines = File.ReadAllLines("samplexmls/cars/" + carFiles[i]);
                    for (int j = 0; j < nameLines.Length; j++)
                    {
                        if (nameLines[j].Contains("<name>"))
                        {
                            int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                            int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                            carNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                            if (carFiles[i].CompareTo("vicechee.xml") == 0)
                            {
                                carNames[i] = "Vice Cheetah";
                            }
                            else if (carFiles[i].CompareTo("hotrina.xml") == 0)
                            {
                                carNames[i] = "Hotring Racer (hotrina)";
                            }
                            else if (carFiles[i].CompareTo("hotrinb.xml") == 0)
                            {
                                carNames[i] = "Hotring Racer (hotrinb)";
                            }
                            else if (carFiles[i].CompareTo("hotring.xml") == 0)
                            {
                                carNames[i] = "Hotring Racer (hotring)";
                            }
                            else if (carFiles[i].CompareTo("bloodra.xml") == 0)
                            {
                                carNames[i] = "Bloodring Banger (bloodra)";
                            }
                            else if (carFiles[i].CompareTo("bloodrb.xml") == 0)
                            {
                                carNames[i] = "Bloodring Banger (bloodrb)";
                            }
                            break;
                        }
                    }
                }

                // Gets all car with specials (vehicle type) xml files and names
                string[] carSpecialsFiles = Directory.GetFiles("samplexmls/carSpecials").Select(file => Path.GetFileName(file)).ToArray();
                string[] carSpecialsNames = new string[carSpecialsFiles.Length];
                for (int i = 0; i < carSpecialsFiles.Length; i++)
                {
                    // Get <name> index from car with specials (vehicle type) xml file
                    string[] nameLines = File.ReadAllLines("samplexmls/carSpecials/" + carSpecialsFiles[i]);
                    for (int j = 0; j < nameLines.Length; j++)
                    {
                        if (nameLines[j].Contains("<name>"))
                        {
                            int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                            int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                            carSpecialsNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                            if (carSpecialsNames[i].CompareTo("Cheetah") == 0)
                            {
                                carSpecialsNames[i] = "Vice Cheetah";
                            }
                            break;
                        }
                    }
                }

                // Adds cars to "vehicle type" comboBox
                comboBox10.Enabled = true;
                comboBox10.Items.Clear();
                comboBox10.Items.Add("Regular Car");
                comboBox10.Items.AddRange(carSpecialsNames);

                // Adds cars to "immunity" comboBox
                comboBox2.Enabled = true;
                comboBox2.Items.Clear();
                comboBox2.Text = comboBox2.Tag.ToString();
                comboBox2.Items.Add("0");
                comboBox2.Items.Add("Rhino");

                // Adds cars to "default" comboBox
                comboBox12.Enabled = true;
                comboBox12.Items.Clear();
                comboBox12.Items.Add("Custom");
                comboBox12.Items.AddRange(carNames);

                // Disables "bike data", "boat data", "flying data", "boat engine audio" and "helicopter data" comboBox
                comboBox14.Enabled = false;
                comboBox15.Enabled = false;
                comboBox16.Enabled = false;
                comboBox7.Enabled = false;
                comboBox8.Enabled = false;
                comboBox14.Items.Clear();
                comboBox15.Items.Clear();
                comboBox16.Items.Clear();
                comboBox7.Items.Clear();
                comboBox8.Items.Clear();

                // Disables "extra flags" comboBox
                comboBox1.Enabled = false;
                comboBox1.Items.Clear();
                comboBox1.Text = comboBox1.Tag.ToString();

                // Disables "engine audio" comboBox
                comboBox3.Enabled = false;
                comboBox3.Items.Clear();

                // Disables "horn audio" comboBox
                comboBox4.Enabled = false;
                comboBox4.Items.Clear();

                // Disables "siren audio" comboBox
                comboBox5.Enabled = false;
                comboBox5.Items.Clear();

                // Disables "door audio" comboBox
                comboBox6.Enabled = false;
                comboBox6.Items.Clear();

                // Disables "carcols", "default", "handling", "bike handling", "boat/plane handling" and "helicopter/plane handling" textBox
                textBox3.Enabled = false;
                textBox5.Enabled = false;
                textBox8.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                textBox3.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox8.Text = string.Empty;
                textBox10.Text = string.Empty;
                textBox11.Text = string.Empty;
                textBox12.Text = string.Empty;

                // Enables "carcols.DAT" and "Save XML" button
                button1.Enabled = true;
                button2.Enabled = true;

                // Sets "vehicle category" comboBox value to "Regular Car"
                comboBox10.Text = "Regular Car";

                // Sets "carcols.DAT line", "default.IDE line" and "handling.CFG line" comboBox value to "Custom"
                comboBox11.Text = "Custom";
                comboBox12.Text = "Custom";
                comboBox13.Text = "Custom";
            }
            else if (comboBox9.Text == "Helicopter")
            {
                // Gets all helicopter xml files and names
                string[] helicopterFiles = Directory.GetFiles("samplexmls/helis").Select(file => Path.GetFileName(file)).ToArray();
                string[] helicopterNames = new string[helicopterFiles.Length];
                for (int i = 0; i < helicopterFiles.Length; i++)
                {
                    // Get <name> index from helicopter xml file
                    string[] nameLines = File.ReadAllLines("samplexmls/helis/" + helicopterFiles[i]);
                    for (int j = 0; j < nameLines.Length; j++)
                    {
                        if (nameLines[j].Contains("<name>"))
                        {
                            int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                            int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                            helicopterNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                            if (helicopterFiles[i].CompareTo("rcgobli.xml") == 0)
                            {
                                helicopterNames[i] = "RC Goblin";
                            }
                            break;
                        }
                    }
                }

                // Adds helicopters to "vehicle type" comboBox
                comboBox10.Enabled = true;
                comboBox10.Items.Clear();
                comboBox10.Items.Add("Regular Helicopter");
                comboBox10.Items.Add("Hunter");
                comboBox10.Items.Add("Sea Sparrow");

                // Adds helicopters to "extra flags" comboBox
                comboBox1.Enabled = true;
                comboBox1.Items.Clear();
                comboBox1.Text = comboBox1.Tag.ToString();
                comboBox1.Items.Add("0000");

                // Adds helicopters to "immunity" comboBox
                comboBox2.Enabled = true;
                comboBox2.Items.Clear();
                comboBox2.Text = comboBox2.Tag.ToString();
                comboBox2.Items.Add("0");
                comboBox2.Items.Add("Rhino");

                // Adds helicopters to "engine audio" comboBox
                comboBox3.Enabled = true;
                comboBox3.Items.Clear();
                comboBox3.Items.AddRange(helicopterNames);

                // Adds helicopters to "horn audio" comboBox
                comboBox4.Enabled = true;
                comboBox4.Items.Clear();
                comboBox4.Items.AddRange(helicopterNames);

                // Adds helicopters to "siren audio" comboBox
                comboBox5.Enabled = true;
                comboBox5.Items.Clear();
                comboBox5.Items.AddRange(helicopterNames);

                // Adds helicopters to "door audio" comboBox
                comboBox6.Enabled = true;
                comboBox6.Items.Clear();
                comboBox6.Items.AddRange(helicopterNames);

                // Adds helicopters to "default" comboBox
                comboBox12.Enabled = true;
                comboBox12.Items.Clear();
                comboBox12.Items.Add("Custom");
                comboBox12.Items.AddRange(helicopterNames);

                // Adds helicopters to "flying data" comboBox
                comboBox16.Enabled = true;
                comboBox16.Items.Clear();
                comboBox16.Items.Add("Custom");
                comboBox16.Items.AddRange(helicopterNames);

                // Disables "bike data", "boat data" and "boat engine audio" comboBox
                comboBox14.Enabled = false;
                comboBox15.Enabled = false;
                comboBox7.Enabled = false;
                comboBox14.Items.Clear();
                comboBox15.Items.Clear();
                comboBox7.Items.Clear();

                // Enables "helicopter data" comboBox
                comboBox8.Enabled = true;
                comboBox8.Items.Clear();
                comboBox8.Items.AddRange(helicopterNames);

                // Disables "carcols", "default", "handling", "bike handling", "boat/plane handling" and "helicopter/plane handling" textBox
                textBox3.Enabled = false;
                textBox5.Enabled = false;
                textBox8.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                textBox3.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox8.Text = string.Empty;
                textBox10.Text = string.Empty;
                textBox11.Text = string.Empty;
                textBox12.Text = string.Empty;

                // Enables "carcols.DAT" and "Save XML" button
                button1.Enabled = true;
                button2.Enabled = true;

                // Sets "vehicle category" comboBox value to "Regular Helicopter"
                comboBox10.Text = "Regular Helicopter";

                // Sets "carcols.DAT line", "default.IDE line", "handling.CFG line" and "flying data line" comboBox value to "Custom"
                comboBox11.Text = "Custom";
                comboBox12.Text = "Custom";
                comboBox13.Text = "Custom";
                comboBox16.Text = "Custom";
            }
            else if (comboBox9.Text == "Plane")
            {
                // Gets all plane xml files and names
                string[] planeFiles = Directory.GetFiles("samplexmls/planes").Select(file => Path.GetFileName(file)).ToArray();
                string[] planeNames = new string[planeFiles.Length];
                for (int i = 0; i < planeFiles.Length; i++)
                {
                    // Get <name> index from plane xml file
                    string[] nameLines = File.ReadAllLines("samplexmls/planes/" + planeFiles[i]);
                    for (int j = 0; j < nameLines.Length; j++)
                    {
                        if (nameLines[j].Contains("<name>"))
                        {
                            int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                            int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                            planeNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                            break;
                        }
                    }
                }

                // Adds planes to "vehicle type" comboBox
                comboBox10.Enabled = true;
                comboBox10.Items.Clear();
                comboBox10.Items.AddRange(planeNames);

                // Adds planes to "immunity" comboBox
                comboBox2.Enabled = true;
                comboBox2.Items.Clear();
                comboBox2.Text = comboBox2.Tag.ToString();
                comboBox2.Items.Add("0");
                comboBox2.Items.Add("Rhino");

                // Disables "extra flags" comboBox
                comboBox1.Enabled = false;
                comboBox1.Items.Clear();
                comboBox1.Text = comboBox1.Tag.ToString();

                // Disables "engine audio" comboBox
                comboBox3.Enabled = false;
                comboBox3.Items.Clear();

                // Disables "horn audio" comboBox
                comboBox4.Enabled = false;
                comboBox4.Items.Clear();

                // Disables "siren audio" comboBox
                comboBox5.Enabled = false;
                comboBox5.Items.Clear();

                // Disables "door audio" comboBox
                comboBox6.Enabled = false;
                comboBox6.Items.Clear();

                // Disables "default" comboBox
                comboBox12.Enabled = false;
                comboBox12.Items.Clear();

                // Disables "bike data", "boat data" and "flying data" comboBox
                comboBox14.Enabled = false;
                comboBox15.Enabled = false;
                comboBox16.Enabled = false;
                comboBox14.Items.Clear();
                comboBox15.Items.Clear();
                comboBox16.Items.Clear();

                // Disables "boat engine audio" and "helicopter data" comboBox
                comboBox7.Enabled = false;
                comboBox8.Enabled = false;
                comboBox7.Items.Clear();
                comboBox8.Items.Clear();

                // Disables "carcols", "default", "handling", "bike handling", "boat/plane handling" and "helicopter/plane handling" textBox
                textBox3.Enabled = false;
                textBox5.Enabled = false;
                textBox8.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                textBox3.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox8.Text = string.Empty;
                textBox10.Text = string.Empty;
                textBox11.Text = string.Empty;
                textBox12.Text = string.Empty;

                // Enables "carcols.DAT" and "Save XML" button
                button1.Enabled = true;
                button2.Enabled = true;
            }
        }

        private void comboBox10_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enables "Paste carcols, default and handling lines" checkBox if "vehicle category" comboBox value is not empty
            if (comboBox10.Text != string.Empty)
            {
                checkBox1.Enabled = true;
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Enabled = false;
                checkBox1.Checked = false;
            }

            // Determines which settings are available depending on "vehicle category" comboBox
            if (comboBox10.Text == "RC Baron")
            {
                // Adds RC Baron to "default" comboBox
                comboBox12.Enabled = true;
                comboBox12.Items.Clear();
                comboBox12.Items.Add("Custom");
                comboBox12.Items.Add("RC Baron");

                // Adds RC Baron to "flying data" comboBox
                comboBox16.Enabled = true;
                comboBox16.Items.Clear();
                comboBox16.Items.Add("Custom");
                comboBox16.Items.Add("RC Baron");

                // Disables "bike data", "boat data", "boat engine audio" and "helicopter data" comboBox
                comboBox14.Enabled = false;
                comboBox15.Enabled = false;
                comboBox7.Enabled = false;
                comboBox8.Enabled = false;
                comboBox14.Items.Clear();
                comboBox15.Items.Clear();
                comboBox7.Items.Clear();
                comboBox8.Items.Clear();

                // Disables "carcols", "default", "handling", "bike handling", "boat/plane handling" and "helicopter/plane handling" textBox
                textBox3.Enabled = false;
                textBox5.Enabled = false;
                textBox8.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                textBox3.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox8.Text = string.Empty;
                textBox10.Text = string.Empty;
                textBox11.Text = string.Empty;
                textBox12.Text = string.Empty;

                // Enables "extra flags" comboBox
                comboBox1.Enabled = true;
                comboBox1.Items.Clear();
                comboBox1.Text = comboBox1.Tag.ToString();
                comboBox1.Items.Add("RC Baron");
                comboBox1.Text = "RC Baron";

                // Enables "engine audio" comboBox
                comboBox3.Enabled = true;
                comboBox3.Items.Clear();
                comboBox3.Items.Add("RC Baron");
                comboBox3.Text = "RC Baron";

                // Enables "horn audio" comboBox
                comboBox4.Enabled = true;
                comboBox4.Items.Clear();
                comboBox4.Items.Add("RC Baron");
                comboBox4.Text = "RC Baron";

                // Enables "siren audio" comboBox
                comboBox5.Enabled = true;
                comboBox5.Items.Clear();
                comboBox5.Items.Add("RC Baron");
                comboBox5.Text = "RC Baron";

                // Enables "door audio" comboBox
                comboBox6.Enabled = true;
                comboBox6.Items.Clear();
                comboBox6.Items.Add("RC Baron");
                comboBox6.Text = "RC Baron";

                // Sets "carcols.DAT line", "default.IDE line", "handling.CFG line" and "flying data line" comboBox value to "Custom"
                comboBox11.ResetText();
                comboBox13.ResetText();
                comboBox11.SelectedIndex = -1;
                comboBox13.SelectedIndex = -1;
                comboBox11.Text = "Custom";
                comboBox12.Text = "Custom";
                comboBox13.Text = "Custom";
                comboBox16.Text = "Custom";
            }
            else if (comboBox10.Text == "Skimmer")
            {
                // Adds Skimmer to "default" comboBox
                comboBox12.Enabled = true;
                comboBox12.Items.Clear();
                comboBox12.Items.Add("Custom");
                comboBox12.Items.Add("Skimmer");

                // Adds Skimmer to "boat data" comboBox
                comboBox15.Enabled = true;
                comboBox15.Items.Clear();
                comboBox15.Items.Add("Custom");
                comboBox15.Items.Add("Skimmer");

                // Adds Skimmer to "flying data" comboBox
                comboBox16.Enabled = true;
                comboBox16.Items.Clear();
                comboBox16.Items.Add("Custom");
                comboBox16.Items.Add("Skimmer");

                // Disables "bike data" and "helicopter data" comboBox
                comboBox14.Enabled = false;
                comboBox8.Enabled = false;
                comboBox14.Items.Clear();
                comboBox8.Items.Clear();

                // Disables "carcols", "default", "handling", "bike handling", "boat/plane handling" and "helicopter/plane handling" textBox
                textBox3.Enabled = false;
                textBox5.Enabled = false;
                textBox8.Enabled = false;
                textBox10.Enabled = false;
                textBox11.Enabled = false;
                textBox12.Enabled = false;
                textBox3.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox8.Text = string.Empty;
                textBox10.Text = string.Empty;
                textBox11.Text = string.Empty;
                textBox12.Text = string.Empty;

                // Enables "extra flags" comboBox
                comboBox1.Enabled = true;
                comboBox1.Items.Clear();
                comboBox1.Text = comboBox1.Tag.ToString();
                comboBox1.Items.Add("0000");

                // Enables "engine audio" comboBox
                comboBox3.Enabled = true;
                comboBox3.Items.Clear();
                comboBox3.Items.Add("Skimmer");
                comboBox3.Text = "Skimmer";

                // Enables "horn audio" comboBox
                comboBox4.Enabled = true;
                comboBox4.Items.Clear();
                comboBox4.Items.Add("Skimmer");
                comboBox4.Text = "Skimmer";

                // Enables "siren audio" comboBox
                comboBox5.Enabled = true;
                comboBox5.Items.Clear();
                comboBox5.Items.Add("Skimmer");
                comboBox5.Text = "Skimmer";

                // Enables "door audio" comboBox
                comboBox6.Enabled = true;
                comboBox6.Items.Clear();
                comboBox6.Items.Add("Skimmer");
                comboBox6.Text = "Skimmer";

                // Enables "boat engine audio" comboBox
                comboBox7.Enabled = true;
                comboBox7.Items.Clear();
                comboBox7.Items.Add("Skimmer");
                comboBox7.Text = "Skimmer";

                // Sets "carcols.DAT line", "default.IDE line", "handling.CFG line", "boat data line" and "flying data line" comboBox value to "Custom"
                comboBox11.ResetText();
                comboBox13.ResetText();
                comboBox11.SelectedIndex = -1;
                comboBox13.SelectedIndex = -1;
                comboBox11.Text = "Custom";
                comboBox12.Text = "Custom";
                comboBox13.Text = "Custom";
                comboBox15.Text = "Custom";
                comboBox16.Text = "Custom";
            }
            else if (comboBox10.Text == "Hunter")
            {
                comboBox8.Text = "Hunter";
            }
            else if (comboBox10.Text == "Sea Sparrow")
            {
                comboBox8.Text = "Sea Sparrow";
            }
            else if (comboBox10.Text == "Regular Helicopter")
            {
                comboBox8.ResetText();
                comboBox8.SelectedIndex = -1;
            }
            else if (comboBox10.Text == "RC Bandit")
            {
                // Enables "extra flags" comboBox
                comboBox1.Enabled = true;
                comboBox1.Items.Clear();
                comboBox1.Text = comboBox1.Tag.ToString();
                comboBox1.Items.Add("RC Bandit");
                comboBox1.Text = "RC Bandit";

                // Enables "engine audio" comboBox
                comboBox3.Enabled = true;
                comboBox3.Items.Clear();
                comboBox3.Items.Add("RC Bandit");
                comboBox3.Text = "RC Bandit";

                // Enables "horn audio" comboBox
                comboBox4.Enabled = true;
                comboBox4.Items.Clear();
                comboBox4.Items.Add("RC Bandit");
                comboBox4.Text = "RC Bandit";

                // Enables "siren audio" comboBox
                comboBox5.Enabled = true;
                comboBox5.Items.Clear();
                comboBox5.Items.Add("RC Bandit");
                comboBox5.Text = "RC Bandit";

                // Enables "door audio" comboBox
                comboBox6.Enabled = true;
                comboBox6.Items.Clear();
                comboBox6.Items.Add("RC Bandit");
                comboBox6.Text = "RC Bandit";
            }
            else if (comboBox9.Text == "Car" && comboBox10.Text != "RC Bandit" && comboBox10.Text != string.Empty)
            {
                // Gets all car xml files and names
                string[] carFiles = Directory.GetFiles("samplexmls/carsNoRC").Select(file => Path.GetFileName(file)).ToArray();
                string[] carNames = new string[carFiles.Length];
                for (int i = 0; i < carFiles.Length; i++)
                {
                    // Get <name> index from car xml file
                    string[] nameLines = File.ReadAllLines("samplexmls/carsNoRC/" + carFiles[i]);
                    for (int j = 0; j < nameLines.Length; j++)
                    {
                        if (nameLines[j].Contains("<name>"))
                        {
                            int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                            int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                            carNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                            if (carFiles[i].CompareTo("vicechee.xml") == 0)
                            {
                                carNames[i] = "Vice Cheetah";
                            }
                            else if (carFiles[i].CompareTo("hotrina.xml") == 0)
                            {
                                carNames[i] = "Hotring Racer (hotrina)";
                            }
                            else if (carFiles[i].CompareTo("hotrinb.xml") == 0)
                            {
                                carNames[i] = "Hotring Racer (hotrinb)";
                            }
                            else if (carFiles[i].CompareTo("hotring.xml") == 0)
                            {
                                carNames[i] = "Hotring Racer (hotring)";
                            }
                            else if (carFiles[i].CompareTo("bloodra.xml") == 0)
                            {
                                carNames[i] = "Bloodring Banger (bloodra)";
                            }
                            else if (carFiles[i].CompareTo("bloodrb.xml") == 0)
                            {
                                carNames[i] = "Bloodring Banger (bloodrb)";
                            }
                            break;
                        }
                    }
                }

                // Gets all car with extra flags xml files and names
                string[] carExtraFlagsFiles = Directory.GetFiles("samplexmls/carExtraflagsNoRC").Select(file => Path.GetFileName(file)).ToArray();
                string[] carExtraFlagsNames = new string[carExtraFlagsFiles.Length];
                for (int i = 0; i < carExtraFlagsFiles.Length; i++)
                {
                    // Get <name> index from car with extra flags xml file
                    string[] nameLines = File.ReadAllLines("samplexmls/carExtraflagsNoRC/" + carExtraFlagsFiles[i]);
                    for (int j = 0; j < nameLines.Length; j++)
                    {
                        if (nameLines[j].Contains("<name>"))
                        {
                            int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                            int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                            carExtraFlagsNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                            if (carExtraFlagsNames[i].CompareTo("Cheetah") == 0)
                            {
                                carExtraFlagsNames[i] = "Vice Cheetah";
                            }
                            break;
                        }
                    }
                }

                // Adds cars to "extra flags" comboBox
                comboBox1.Enabled = true;
                comboBox1.Items.Clear();
                comboBox1.Text = comboBox1.Tag.ToString();
                comboBox1.Items.Add("0000");
                comboBox1.Items.AddRange(carExtraFlagsNames);
                if (comboBox10.Text == "Ambulance")
                {
                    comboBox1.Text = "Ambulance";
                }
                else if (comboBox10.Text == "Cabbie")
                {
                    comboBox1.Text = "Cabbie";
                }
                else if (comboBox10.Text == "Coach")
                {
                    comboBox1.Text = "Coach";
                }
                else if (comboBox10.Text == "Enforcer")
                {
                    comboBox1.Text = "Enforcer";
                }
                else if (comboBox10.Text == "FBI Rancher")
                {
                    comboBox1.Text = "FBI Rancher";
                }
                else if (comboBox10.Text == "Firetruck")
                {
                    comboBox1.Text = "Firetruck";
                }
                else if (comboBox10.Text == "Kaufman Cab")
                {
                    comboBox1.Text = "Kaufman Cab";
                }
                else if (comboBox10.Text == "Police")
                {
                    comboBox1.Text = "Police";
                }
                else if (comboBox10.Text == "Taxi")
                {
                    comboBox1.Text = "Taxi";
                }
                else if (comboBox10.Text == "Vice Cheetah")
                {
                    comboBox1.Text = "Vice Cheetah";
                }
                else if (comboBox10.Text == "Zebra Cab")
                {
                    comboBox1.Text = "Zebra Cab";
                }

                // Adds cars to "engine audio" comboBox
                comboBox3.Enabled = true;
                comboBox3.Items.Clear();
                comboBox3.Items.AddRange(carNames);

                // Adds cars to "horn audio" comboBox
                comboBox4.Enabled = true;
                comboBox4.Items.Clear();
                comboBox4.Items.AddRange(carNames);

                // Adds cars to "siren audio" comboBox
                comboBox5.Enabled = true;
                comboBox5.Items.Clear();
                comboBox5.Items.AddRange(carNames);
                if (comboBox10.Text == "Ambulance")
                {
                    comboBox5.Text = "Ambulance";
                }
                else if (comboBox10.Text == "Enforcer")
                {
                    comboBox5.Text = "Enforcer";
                }
                else if (comboBox10.Text == "FBI Rancher")
                {
                    comboBox5.Text = "FBI Rancher";
                }
                else if (comboBox10.Text == "Firetruck")
                {
                    comboBox5.Text = "Firetruck";
                }
                else if (comboBox10.Text == "Police")
                {
                    comboBox5.Text = "Police";
                }
                else if (comboBox10.Text == "Vice Cheetah")
                {
                    comboBox5.Text = "Vice Cheetah";
                }

                // Adds cars to "door audio" comboBox
                comboBox6.Enabled = true;
                comboBox6.Items.Clear();
                comboBox6.Items.AddRange(carNames);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                VisitLink();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open link that was clicked.");
            }  
        }

        private void comboBox15_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enables "boat/plane handling" textBox if "boat data" comboBox value is set to "Custom"
            if (comboBox15.Text == "Custom")
            {
                textBox11.Enabled = true;
                textBox11.Text = string.Empty;
            }
            else if (comboBox15.Text != string.Empty && comboBox15.Text != "Custom" && checkBox1.Checked == true)
            {
                string fileName = FindSampleXML(comboBox15.Text).Split(new[] { ' ', '\t', '.', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                string[] defaultFileLines = File.ReadAllLines("samplexmls/originalFiles/default.ide");
                string[] handlingFileLines = File.ReadAllLines("samplexmls/originalFiles/handling.cfg");
                string defaultLine = "";
                string boatDataLine = "";
                int handlingLinesCount = 0;
                for (int i = 0; i < defaultFileLines.Length; i++)
                {
                    if (Regex.IsMatch(defaultFileLines[i], @"(\s+)" + fileName.ToUpper() + "," + @"(\s+)"))
                    {
                        defaultLine = defaultFileLines[i];
                        break;
                    }
                }
                string boatDataLineName = defaultLine.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[4];
                for (int i = 0; i < handlingFileLines.Length; i++)
                {
                    if (Regex.IsMatch(handlingFileLines[i], @"^(\s+)?" + boatDataLineName.ToUpper() + @"(\s+)"))
                    {
                        handlingLinesCount++;
                        if (handlingLinesCount == 2)
                        {
                            boatDataLine = handlingFileLines[i];
                            break;
                        }
                    }
                }
                while (Char.IsNumber(boatDataLine.First()) == false)
                {
                    boatDataLine = boatDataLine.Remove(0, 1);
                }
                textBox11.Enabled = true;
                textBox11.Text = boatDataLine.Trim();
            }
            else if (comboBox15.Text != string.Empty && comboBox15.Text != "Custom" && checkBox1.Checked == false)
            {
                textBox11.Enabled = false;
                textBox11.Text = string.Empty;
            }
        }

        private void comboBox14_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enables "bike handling" textBox if "bike data" comboBox value is set to "Custom"
            if (comboBox14.Text == "Custom")
            {
                textBox10.Enabled = true;
                textBox10.Text = string.Empty;
            }
            else if (comboBox14.Text != string.Empty && comboBox14.Text != "Custom" && checkBox1.Checked == true)
            {
                string fileName = FindSampleXML(comboBox14.Text).Split(new[] { ' ', '\t', '.', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                string[] defaultFileLines = File.ReadAllLines("samplexmls/originalFiles/default.ide");
                string[] handlingFileLines = File.ReadAllLines("samplexmls/originalFiles/handling.cfg");
                string defaultLine = "";
                string bikeDataLine = "";
                int handlingLinesCount = 0;
                for (int i = 0; i < defaultFileLines.Length; i++)
                {
                    if (Regex.IsMatch(defaultFileLines[i], @"(\s+)" + fileName.ToUpper() + "," + @"(\s+)"))
                    {
                        defaultLine = defaultFileLines[i];
                        break;
                    }
                }
                string bikeDataLineName = defaultLine.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[4];
                for (int i = 0; i < handlingFileLines.Length; i++)
                {
                    if (Regex.IsMatch(handlingFileLines[i], @"^(\s+)?" + bikeDataLineName.ToUpper() + @"(\s+)"))
                    {
                        handlingLinesCount++;
                        if (handlingLinesCount == 2)
                        {
                            bikeDataLine = handlingFileLines[i];
                            break;
                        }
                    }
                }
                while (Char.IsNumber(bikeDataLine.First()) == false)
                {
                    bikeDataLine = bikeDataLine.Remove(0, 1);
                }
                textBox10.Enabled = true;
                textBox10.Text = bikeDataLine.Trim();
            }
            else if (comboBox14.Text != string.Empty && comboBox14.Text != "Custom" && checkBox1.Checked == false)
            {
                textBox10.Enabled = false;
                textBox10.Text = string.Empty;
            }
        }

        private void comboBox16_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enables "helicopter/plane handling" textBox if "flying data" comboBox value is set to "Custom"
            if (comboBox16.Text == "Custom")
            {
                textBox12.Enabled = true;
                textBox12.Text = string.Empty;
            }
            else if (comboBox16.Text != string.Empty && comboBox16.Text != "Custom" && checkBox1.Checked == true)
            {
                string fileName = FindSampleXML(comboBox16.Text).Split(new[] { ' ', '\t', '.', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                string[] defaultFileLines = File.ReadAllLines("samplexmls/originalFiles/default.ide");
                string[] handlingFileLines = File.ReadAllLines("samplexmls/originalFiles/handling.cfg");
                string defaultLine = "";
                string flyingDataLine = "";
                int handlingLinesCount = 0;
                for (int i = 0; i < defaultFileLines.Length; i++)
                {
                    if (Regex.IsMatch(defaultFileLines[i], @"(\s+)" + fileName.ToUpper() + "," + @"(\s+)"))
                    {
                        defaultLine = defaultFileLines[i];
                        break;
                    }
                }
                string flyingDataLineName = defaultLine.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[4];
                for (int i = 0; i < handlingFileLines.Length; i++)
                {
                    if (Regex.IsMatch(handlingFileLines[i], @"^(\s+)?" + flyingDataLineName.ToUpper() + @"(\s+)"))
                    {
                        handlingLinesCount++;
                        if (handlingLinesCount == 2 && comboBox16.Text != "Skimmer")
                        {
                            flyingDataLine = handlingFileLines[i];
                            break;
                        }
                        else if (handlingLinesCount == 3 && comboBox16.Text == "Skimmer")
                        {
                            flyingDataLine = handlingFileLines[i];
                            break;
                        }
                    }
                }
                while (Char.IsNumber(flyingDataLine.First()) == false)
                {
                    flyingDataLine = flyingDataLine.Remove(0, 1);
                }
                textBox12.Enabled = true;
                textBox12.Text = flyingDataLine.Trim();
            }
            else if (comboBox16.Text != string.Empty && comboBox16.Text != "Custom" && checkBox1.Checked == false)
            {
                textBox12.Enabled = false;
                textBox12.Text = string.Empty;
            }
        }

        private void comboBox11_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enables "carcols" textBox if "carcols" comboBox value is set to "Custom"
            if (comboBox11.Text == "Custom")
            {
                textBox3.Enabled = true;
                textBox3.Text = string.Empty;
            }
            else if (comboBox11.Text != string.Empty && comboBox11.Text != "Custom" && checkBox1.Checked == true)
            {
                string carcolsLine = "";
                string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox11.Text));
                for (int i = 0; i < fileLines.Length; i++)
                {
                    if (fileLines[i].Contains("<carcol>"))
                    {
                        int carcolIndex = i;
                        while (fileLines[carcolIndex].Contains("<carcol>"))
                        {
                            if (fileLines[carcolIndex].Any(char.IsDigit))
                            {
                                carcolsLine += fileLines[carcolIndex].Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1] + ", ";
                            }
                            carcolIndex++;
                        }
                        break;
                    }
                }
                while (Char.IsNumber(carcolsLine.Last()) == false)
                {
                    carcolsLine = carcolsLine.Remove(carcolsLine.Length - 1);
                }
                textBox3.Enabled = true;
                textBox3.Text = carcolsLine.Trim();
            }
            else if (comboBox11.Text != string.Empty && comboBox11.Text != "Custom" && checkBox1.Checked == false)
            {
                textBox3.Enabled = false;
                textBox3.Text = string.Empty;
            }
        }

        private void comboBox12_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enables "default" textBox if "default" comboBox value is set to "Custom"
            if (comboBox12.Text == "Custom")
            {
                textBox5.Enabled = true;
                textBox5.Text = string.Empty;
            }
            else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == true)
            {
                string fileName = FindSampleXML(comboBox12.Text).Split(new[] { ' ', '\t', '.', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                string[] defaultFileLines = File.ReadAllLines("samplexmls/originalFiles/default.ide");
                string defaultLine = "";
                for (int i = 0; i < defaultFileLines.Length; i++)
                {
                    if (Regex.IsMatch(defaultFileLines[i], @"(\s+)" + fileName.ToUpper() + "," + @"(\s+)"))
                    {
                        defaultLine = defaultFileLines[i];
                        break;
                    }
                }
                textBox5.Enabled = true;
                textBox5.Text = defaultLine.Trim();
            }
            else if (comboBox12.Text != string.Empty && comboBox12.Text != "Custom" && checkBox1.Checked == false)
            {
                textBox5.Enabled = false;
                textBox5.Text = string.Empty;
            }
        }

        private void comboBox13_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Enables "handling" textBox if "handling" comboBox value is set to "Custom"
            if (comboBox13.Text == "Custom")
            {
                textBox8.Enabled = true;
                textBox8.Text = string.Empty;
            }
            else if (comboBox13.Text != string.Empty && comboBox13.Text != "Custom" && checkBox1.Checked == true)
            {
                string fileName = FindSampleXML(comboBox13.Text).Split(new[] { ' ', '\t', '.', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                string[] defaultFileLines = File.ReadAllLines("samplexmls/originalFiles/default.ide");
                string[] handlingFileLines = File.ReadAllLines("samplexmls/originalFiles/handling.cfg");
                string defaultLine = "";
                string handlingLine = "";
                for (int i = 0; i < defaultFileLines.Length; i++)
                {
                    if (Regex.IsMatch(defaultFileLines[i], @"(\s+)" + fileName.ToUpper() + "," + @"(\s+)"))
                    {
                        defaultLine = defaultFileLines[i];
                        break;
                    }
                }
                string handlingLineName = defaultLine.Split(new[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[4];
                for (int i = 0; i < handlingFileLines.Length; i++)
                {
                    if (Regex.IsMatch(handlingFileLines[i], @"^(\s+)?" + handlingLineName.ToUpper() + @"(\s+)"))
                    {
                        handlingLine = handlingFileLines[i];
                        break;
                    }
                }
                while (Char.IsNumber(handlingLine.First()) == false)
                {
                    handlingLine = handlingLine.Remove(0, 1);
                }
                textBox8.Enabled = true;
                textBox8.Text = handlingLine.Trim();
            }
            else if (comboBox13.Text != string.Empty && comboBox13.Text != "Custom" && checkBox1.Checked == false)
            {
                textBox8.Enabled = false;
                textBox8.Text = string.Empty;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Resets "error" textBox
            textBox13.Text = textBox13.Tag.ToString();
            textBox13.ForeColor = Color.Red;
            textBox13.Visible = false;

            // SaveFileDialog
            SaveFileDialog savefile = new SaveFileDialog();

            // Sets filters - this can be done in properties as well
            savefile.Filter = "xml files (*.xml)|*.xml";

            // Gets all car with extra flags xml files and names
            string[] carExtraFlagsFiles = Directory.GetFiles("samplexmls/extraflags").Select(file => Path.GetFileName(file)).ToArray();
            string[] carExtraFlagsNames = new string[carExtraFlagsFiles.Length];
            for (int i = 0; i < carExtraFlagsFiles.Length; i++)
            {
                // Get <name> index from car with extra flags xml file
                string[] nameLines = File.ReadAllLines("samplexmls/extraflags/" + carExtraFlagsFiles[i]);
                for (int j = 0; j < nameLines.Length; j++)
                {
                    if (nameLines[j].Contains("<name>"))
                    {
                        int indexOfNameStart = nameLines[j].IndexOf('>') + 1;
                        int indexOfNameEnd = nameLines[j].IndexOf('/') - 2;
                        carExtraFlagsNames[i] = nameLines[j].Substring(indexOfNameStart, indexOfNameEnd - indexOfNameStart + 1);
                        if (carExtraFlagsNames[i].CompareTo("Cheetah") == 0)
                        {
                            carExtraFlagsNames[i] = "Vice Cheetah";
                        }
                        break;
                    }
                }
            }

            // Some vehicle parameters
            string vehicleName = textBox9.Text.Trim();
            string extraflags = "0000";
            if (comboBox1.Text.Trim() != string.Empty)
            {
                if (carExtraFlagsNames.Contains(comboBox1.Text.Trim()))
                {
                    extraflags = FindLineInSampleXML(FindSampleXML(comboBox1.Text.Trim()), "<extraflags>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                }
                else if (comboBox1.Text.Trim() != "0000")
                {
                    extraflags = comboBox1.Text.Trim();
                }
            }
            string immunity = "0";
            if (comboBox2.Text.Trim() == "Rhino")
            {
                immunity = FindLineInSampleXML(FindSampleXML(comboBox2.Text.Trim()), "<immunity>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
            else if (comboBox2.Text.Trim() != "0" && comboBox2.Text.Trim() != string.Empty)
            {
                immunity = comboBox2.Text.Trim();
            }
            bool isAmbulan = false;
            bool isBaggage = false;
            bool isCabbie = false;
            bool isCoach = false;
            bool isEnforcr = false;
            bool isFbiranc = false;
            bool isFiretrk = false;
            bool isKaufman = false;
            bool isPolicar = false;
            bool isRcbandt = false;
            bool isTaxi = false;
            bool isVicechee = false;
            bool isZebra = false;
            if (comboBox10.Text == "Ambulance")
            {
                isAmbulan = true;
            }
            else if (comboBox10.Text == "Baggage Handler")
            {
                isBaggage = true;
            }
            else if (comboBox10.Text == "Cabbie")
            {
                isCabbie = true;
            }
            else if (comboBox10.Text == "Coach")
            {
                isCoach = true;
            }
            else if (comboBox10.Text == "Enforcer")
            {
                isEnforcr = true;
            }
            else if (comboBox10.Text == "FBI Rancher")
            {
                isFbiranc = true;
            }
            else if (comboBox10.Text == "Firetruck")
            {
                isFiretrk = true;
            }
            else if (comboBox10.Text == "Kaufman Cab")
            {
                isKaufman = true;
            }
            else if (comboBox10.Text == "Police")
            {
                isPolicar = true;
            }
            else if (comboBox10.Text == "RC Bandit")
            {
                isRcbandt = true;
            }
            else if (comboBox10.Text == "Taxi")
            {
                isTaxi = true;
            }
            else if (comboBox10.Text == "Vice Cheetah")
            {
                isVicechee = true;
            }
            else if (comboBox10.Text == "Zebra Cab")
            {
                isZebra = true;
            }

            // Vehicle audio
            string enginefarsample = "";
            string enginenearsample = "";
            string hornsample = "";
            string hornfreq = "";
            string sirensample = "";
            string sirenfreq = "";
            string doorsounds = "";
            if (comboBox3.Text != string.Empty && comboBox4.Text != string.Empty && comboBox5.Text != string.Empty && comboBox6.Text != string.Empty)
            {
                enginefarsample = FindLineInSampleXML(FindSampleXML(comboBox3.Text), "<enginefarsample>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                enginenearsample = FindLineInSampleXML(FindSampleXML(comboBox3.Text), "<enginenearsample>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                hornsample = FindLineInSampleXML(FindSampleXML(comboBox4.Text), "<hornsample>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                hornfreq = FindLineInSampleXML(FindSampleXML(comboBox4.Text), "<hornfreq>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                sirensample = FindLineInSampleXML(FindSampleXML(comboBox5.Text), "<sirensample>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                sirenfreq = FindLineInSampleXML(FindSampleXML(comboBox5.Text), "<sirenfreq>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                doorsounds = FindLineInSampleXML(FindSampleXML(comboBox6.Text), "<doorsounds>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
            }

            // Some bike parameters
            bool isPizzabo = false;
            if (comboBox10.Text == "Pizza Boy")
            {
                isPizzabo = true;
            }

            // Some heli parameters (helidata, isHunter, isSeaSpar)
            // helidata: helitype, weapons, rotorradius, rotordammult
            string helitype = "";
            string weapons = "";
            string rotorradius = "";
            string rotordammult = "";
            if (comboBox8.Text != string.Empty)
            {
                helitype = FindLineInSampleXML(FindSampleXML(comboBox8.Text), "<helitype>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                weapons = FindLineInSampleXML(FindSampleXML(comboBox8.Text), "<weapons>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                rotorradius = FindLineInSampleXML(FindSampleXML(comboBox8.Text), "<rotorradius>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                rotordammult = FindLineInSampleXML(FindSampleXML(comboBox8.Text), "<rotordammult>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
            bool isHunter = false;
            bool isSeaSpar = false;
            if (comboBox10.Text == "Hunter")
            {
                isHunter = true;
            }
            else if (comboBox10.Text == "Sea Sparrow")
            {
                isSeaSpar = true;
            }

            // Some boat parameters (boatengine)
            string type = "";
            string basevolume = "";
            string basefrequency = "";
            string volumeincrease = "";
            string frequencyincrease = "";
            if (comboBox7.Text != string.Empty)
            {
                string[] fileLines = File.ReadAllLines(FindSampleXML(comboBox7.Text));
                int typeCount = 0;
                for (int i = 0; i < fileLines.Length; i++)
                {
                    if (fileLines[i].Contains("<type>"))
                    {
                        typeCount++;
                        if (typeCount == 2)
                        {
                            type = fileLines[i].Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                            break;
                        }
                    }
                }
                basevolume = FindLineInSampleXML(FindSampleXML(comboBox7.Text), "<basevolume>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                basefrequency = FindLineInSampleXML(FindSampleXML(comboBox7.Text), "<basefrequency>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                volumeincrease = FindLineInSampleXML(FindSampleXML(comboBox7.Text), "<volumeincrease>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
                frequencyincrease = FindLineInSampleXML(FindSampleXML(comboBox7.Text), "<frequencyincrease>").Split(new[] { ' ', '\t', '<', '>', '/' }, StringSplitOptions.RemoveEmptyEntries)[1];
            }

            // Some plane parameters
            bool isRcbaron = false;
            bool isSkimmer = false;
            if (comboBox10.Text == "RC Baron")
            {
                isRcbaron = true;
            }
            else if (comboBox10.Text == "Skimmer")
            {
                isSkimmer = true;
            }

            // Checks "vehicle type" comboBox value and if everything is valid
            if (comboBox9.Text == "Car")
            {
                if (EverythingIsValidForCar())
                {
                    // Makes "error" textBox invisible
                    textBox13.Text = textBox13.Tag.ToString();
                    textBox13.Visible = false;

                    // Fixes carcols line in "carcols.DAT line" textBox if needed
                    FixCarcolsLine();

                    // Builds car XML file
                    BuildCarXML(isAmbulan, isBaggage, isCabbie, isCoach, isEnforcr, isFbiranc, isFiretrk, isKaufman, isPolicar, isRcbandt, isTaxi, isVicechee, isZebra, vehicleName, extraflags, immunity, enginefarsample, enginenearsample, hornsample, hornfreq, sirensample, sirenfreq, doorsounds);

                    // Sets a default file name
                    savefile.FileName = "car_XML.xml";

                    // Saves XML file
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(savefile.FileName))
                        {
                            // Gets all lines from "carXML.xml"
                            string[] fileLines = File.ReadAllLines("samplexmls/temp/carXML.xml");

                            for (int i = 0; i < fileLines.Length; i++)
                            {
                                sw.WriteLine(fileLines[i]);
                            }
                        }
                    }
                }
                else
                {
                    // Makes "error" textBox visible
                    textBox13.Visible = true;
                }
            }
            else if (comboBox9.Text == "Bike")
            {
                if (EverythingIsValidForBike())
                {
                    // Makes "error" textBox invisible
                    textBox13.Text = textBox13.Tag.ToString();
                    textBox13.Visible = false;

                    // Fixes carcols line in "carcols.DAT line" textBox if needed
                    FixCarcolsLine();

                    // Builds bike XML file
                    BuildBikeXML(isPizzabo, vehicleName, extraflags, immunity, enginefarsample, enginenearsample, hornsample, hornfreq, sirensample, sirenfreq, doorsounds);

                    // Sets a default file name
                    savefile.FileName = "bike_XML.xml";

                    // Saves XML file
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(savefile.FileName))
                        {
                            // Gets all lines from "bikeXML.xml"
                            string[] fileLines = File.ReadAllLines("samplexmls/temp/bikeXML.xml");

                            for (int i = 0; i < fileLines.Length; i++)
                            {
                                sw.WriteLine(fileLines[i]);
                            }
                        }
                    }
                }
                else
                {
                    // Makes "error" textBox visible
                    textBox13.Visible = true;
                }
            }
            else if (comboBox9.Text == "Plane")
            {
                if (EverythingIsValidForPlane())
                {
                    // Makes "error" textBox invisible
                    textBox13.Text = textBox13.Tag.ToString();
                    textBox13.Visible = false;

                    // Fixes carcols line in "carcols.DAT line" textBox if needed
                    FixCarcolsLine();

                    // Builds plane XML file
                    BuildPlaneXML(isRcbaron, isSkimmer, vehicleName, extraflags, immunity, enginefarsample, enginenearsample, hornsample, hornfreq, sirensample, sirenfreq, doorsounds);

                    // Sets a default file name
                    savefile.FileName = "plane_XML.xml";

                    // Saves XML file
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(savefile.FileName))
                        {
                            // Gets all lines from "planeXML.xml"
                            string[] fileLines = File.ReadAllLines("samplexmls/temp/planeXML.xml");

                            for (int i = 0; i < fileLines.Length; i++)
                            {
                                sw.WriteLine(fileLines[i]);
                            }
                        }
                    }
                }
                else
                {
                    // Makes "error" textBox visible
                    textBox13.Visible = true;
                }
            }
            else if (comboBox9.Text == "Helicopter")
            {
                if (EverythingIsValidForHelicopter())
                {
                    // Makes "error" textBox invisible
                    textBox13.Text = textBox13.Tag.ToString();
                    textBox13.Visible = false;

                    // Fixes carcols line in "carcols.DAT line" textBox if needed
                    FixCarcolsLine();

                    // Builds heli XML file
                    BuildHeliXML(isHunter, isSeaSpar, vehicleName, extraflags, immunity, enginefarsample, enginenearsample, hornsample, hornfreq, sirensample, sirenfreq, doorsounds, helitype, weapons, rotorradius, rotordammult);

                    // Sets a default file name
                    savefile.FileName = "heli_XML.xml";

                    // Saves XML file
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(savefile.FileName))
                        {
                            // Gets all lines from "heliXML.xml"
                            string[] fileLines = File.ReadAllLines("samplexmls/temp/heliXML.xml");

                            for (int i = 0; i < fileLines.Length; i++)
                            {
                                sw.WriteLine(fileLines[i]);
                            }
                        }
                    }
                }
                else
                {
                    // Makes "error" textBox visible
                    textBox13.Visible = true;
                }
            }
            else if (comboBox9.Text == "Boat")
            {
                if (EverythingIsValidForBoat())
                {
                    // Makes "error" textBox invisible
                    textBox13.Text = textBox13.Tag.ToString();
                    textBox13.Visible = false;

                    // Fixes carcols line in "carcols.DAT line" textBox if needed
                    FixCarcolsLine();

                    // Builds boat XML file
                    BuildBoatXML(vehicleName, extraflags, immunity, enginefarsample, enginenearsample, hornsample, hornfreq, sirensample, sirenfreq, doorsounds, type, basevolume, basefrequency, volumeincrease, frequencyincrease);

                    // Sets a default file name
                    savefile.FileName = "boat_XML.xml";

                    // Saves XML file
                    if (savefile.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamWriter sw = new StreamWriter(savefile.FileName))
                        {
                            // Gets all lines from "boatXML.xml"
                            string[] fileLines = File.ReadAllLines("samplexmls/temp/boatXML.xml");

                            for (int i = 0; i < fileLines.Length; i++)
                            {
                                sw.WriteLine(fileLines[i]);
                            }
                        }
                    }
                }
                else
                {
                    // Makes "error" textBox visible
                    textBox13.Visible = true;
                }
            }
        }

        private void XML_Builder_Form_FormClosing(Object sender, FormClosingEventArgs e)
        {
            // Deletes all files
            if (File.Exists("samplexmls/temp/bikeXML.xml"))
            {
                File.Delete("samplexmls/temp/bikeXML.xml");
            }
            if (File.Exists("samplexmls/temp/boatXML.xml"))
            {
                File.Delete("samplexmls/temp/boatXML.xml");
            }
            if (File.Exists("samplexmls/temp/carcols.txt"))
            {
                File.Delete("samplexmls/temp/carcols.txt");
            }
            if (File.Exists("samplexmls/temp/carcolsLine.txt"))
            {
                File.Delete("samplexmls/temp/carcolsLine.txt");
            }
            if (File.Exists("samplexmls/temp/carcolsOriginal.txt"))
            {
                File.Delete("samplexmls/temp/carcolsOriginal.txt");
            }
            if (File.Exists("samplexmls/temp/carXML.xml"))
            {
                File.Delete("samplexmls/temp/carXML.xml");
            }
            if (File.Exists("samplexmls/temp/heliXML.xml"))
            {
                File.Delete("samplexmls/temp/heliXML.xml");
            }
            if (File.Exists("samplexmls/temp/planeXML.xml"))
            {
                File.Delete("samplexmls/temp/planeXML.xml");
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (this.Size.Height - 111 > 808)
            {
                textBox13.Location = new Point(textBox13.Location.X, this.Size.Height - 111);
                int height = textBox13.Location.Y - 748;
                textBox13.Location = new Point(textBox13.Location.X, 808);
                textBox13.Height = height;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (comboBox9.Text == "Bike")
                {
                    textBox3.Enabled = true;
                    textBox5.Enabled = true;
                    textBox8.Enabled = true;
                    textBox10.Enabled = true;
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Text = "";
                    }
                    if (comboBox14.Text != "Custom")
                    {
                        textBox10.Text = "";
                    }
                }
                else if (comboBox9.Text == "Boat")
                {
                    textBox3.Enabled = true;
                    textBox5.Enabled = true;
                    textBox8.Enabled = true;
                    textBox11.Enabled = true;
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Text = "";
                    }
                    if (comboBox15.Text != "Custom")
                    {
                        textBox11.Text = "";
                    }
                }
                else if (comboBox9.Text == "Car")
                {
                    textBox3.Enabled = true;
                    textBox5.Enabled = true;
                    textBox8.Enabled = true;
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Text = "";
                    }
                }
                else if (comboBox9.Text == "Helicopter" || comboBox10.Text == "RC Baron")
                {
                    textBox3.Enabled = true;
                    textBox5.Enabled = true;
                    textBox8.Enabled = true;
                    textBox12.Enabled = true;
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Text = "";
                    }
                    if (comboBox16.Text != "Custom")
                    {
                        textBox12.Text = "";
                    }
                }
                else if (comboBox10.Text == "Skimmer")
                {
                    textBox3.Enabled = true;
                    textBox5.Enabled = true;
                    textBox8.Enabled = true;
                    textBox11.Enabled = true;
                    textBox12.Enabled = true;
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Text = "";
                    }
                    if (comboBox15.Text != "Custom")
                    {
                        textBox11.Text = "";
                    }
                    if (comboBox16.Text != "Custom")
                    {
                        textBox12.Text = "";
                    }
                }
            }
            else
            {
                if (comboBox9.Text == "Bike")
                {
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Enabled = false;
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Enabled = false;
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Enabled = false;
                        textBox8.Text = "";
                    }
                    if (comboBox14.Text != "Custom")
                    {
                        textBox10.Enabled = false;
                        textBox10.Text = "";
                    }
                }
                else if (comboBox9.Text == "Boat")
                {
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Enabled = false;
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Enabled = false;
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Enabled = false;
                        textBox8.Text = "";
                    }
                    if (comboBox15.Text != "Custom")
                    {
                        textBox11.Enabled = false;
                        textBox11.Text = "";
                    }
                }
                else if (comboBox9.Text == "Car")
                {
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Enabled = false;
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Enabled = false;
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Enabled = false;
                        textBox8.Text = "";
                    }
                }
                else if (comboBox9.Text == "Helicopter" || comboBox10.Text == "RC Baron")
                {
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Enabled = false;
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Enabled = false;
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Enabled = false;
                        textBox8.Text = "";
                    }
                    if (comboBox16.Text != "Custom")
                    {
                        textBox12.Enabled = false;
                        textBox12.Text = "";
                    }
                }
                else if (comboBox10.Text == "Skimmer")
                {
                    if (comboBox11.Text != "Custom")
                    {
                        textBox3.Enabled = false;
                        textBox3.Text = "";
                    }
                    if (comboBox12.Text != "Custom")
                    {
                        textBox5.Enabled = false;
                        textBox5.Text = "";
                    }
                    if (comboBox13.Text != "Custom")
                    {
                        textBox8.Enabled = false;
                        textBox8.Text = "";
                    }
                    if (comboBox15.Text != "Custom")
                    {
                        textBox11.Enabled = false;
                        textBox11.Text = "";
                    }
                    if (comboBox16.Text != "Custom")
                    {
                        textBox12.Enabled = false;
                        textBox12.Text = "";
                    }
                }
            }
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
