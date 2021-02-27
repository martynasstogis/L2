using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace L2
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Agency1 file
        /// </summary>
        string CFd1;
        /// <summary>
        /// Agency2 file
        /// </summary>
        string CFd2;
        /// <summary>
        /// AgencyExtra file
        /// </summary>
        string CFd3;
        /// <summary>
        /// Result file
        /// </summary>
        string CFr;
        /// <summary>
        /// instruction file
        /// </summary>
        string CFi = "..\\..\\Instruction.txt";
        /// <summary>
        /// assignment file
        /// </summary>
        string CFa = "..\\..\\Assignment.txt";
        private List<Worker> Agency1;
        private List<Worker> Agency2;
        private List<Worker> Agency3;
        private List<Worker> AgencyExtra;
        string agencyName1;
        string agencyName2;
        string agencyNameExtra;
        /// <summary>
        /// required experience
        /// </summary>
        int exp;
        /// <summary>
        /// maximum wage
        /// </summary>
        int wage;
        public Form1()
        {
            InitializeComponent();
            requirements.Enabled = false;
            newList.Enabled = false;
            remove.Enabled = false;
            wage2.Enabled = false;
            experience.Enabled = false;
            wage1.Enabled = false;
            add.Enabled = false;
            if (File.Exists(CFr)) File.Delete(CFr);
        }
        private void InputToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt|" +
                                     "All files (*.*)|*.*";
            saveFileDialog1.Title = "Pasirinkite rezultatų failą";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                CFr = saveFileDialog1.FileName;
                if (File.Exists(CFr)) File.Delete(CFr);
            }
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|" +
                                     "All files (*.*)|*.*";
            openFileDialog1.Title = "Pasirinkite pirmosios biržos duomenų failą";
            result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                CFd1 = openFileDialog1.FileName;
                Input(CFd1, out Agency1, out agencyName1);
                OutputAgency(CFr, agencyName1, Agency1);
            }
            OpenFileDialog openFileDialog2 = new OpenFileDialog();
            openFileDialog2.Filter = "txt files (*.txt)|*.txt|" +
                                     "All files (*.*)|*.*";
            openFileDialog2.Title = "Pasirinkite antrosios biržos duomenų failą";
            result = openFileDialog2.ShowDialog();
            if (result == DialogResult.OK)
            {
                CFd2 = openFileDialog2.FileName;
                Input(CFd2, out Agency2, out agencyName2);
                OutputAgency(CFr, agencyName2, Agency2);
            }
            output.Text = File.ReadAllText(CFr, Encoding.UTF8);
            experience.Enabled = true;
            wage1.Enabled = true;
            requirements.Enabled = true;
        }
        private void requirements_Click(object sender, EventArgs e)
        {
            exp = int.Parse(experience.Text);
            wage = int.Parse(wage1.Text);
            OutputText(CFr, "Reikalavimai:\nMinimalus stažas: " + exp +
                       " m.\tMaksimali alga: " + wage + " eur.\n");
            OutputText(CFr, BetterAgency(Agency1, Agency2, agencyName1, 
                       agencyName2, exp, wage));
            output.Text = File.ReadAllText(CFr, Encoding.UTF8);
            requirements.Enabled = false;
            experience.Enabled = false;
            wage1.Enabled = false;
            newList.Enabled = true;
        }

        private void newList_Click(object sender, EventArgs e)
        {
            Agency3 = new List<Worker>();
            NewAgency(ref Agency3, Agency1, exp, wage);
            NewAgency(ref Agency3, Agency2, exp, wage);
            Agency3.Sort();
            OutputAgency(CFr, "Tinkami darbuotojai:" , Agency3);
            output.Text = File.ReadAllText(CFr, Encoding.UTF8);
            newList.Enabled = false;
            remove.Enabled = true;
            wage2.Enabled = true;
            add.Enabled = true;
        }

        private void remove_Click(object sender, EventArgs e)
        {
            wage = int.Parse(wage2.Text);
            RemoveFromAgency(Agency3, wage);
            OutputText(CFr, "Reikalavimai:\nMinimalus stažas: " + exp +
                       " m.\tMaksimali alga: " + wage + " eur.\n");
            OutputAgency(CFr, "Tinkami darbuotojai:", Agency3);
            output.Text = File.ReadAllText(CFr, Encoding.UTF8);
        }
        private void Add_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|" +
                                     "All files (*.*)|*.*";
            openFileDialog1.Title = "Pasirinkite pirmosios biržos duomenų failą";
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                CFd3 = openFileDialog1.FileName;
                Input(CFd3, out AgencyExtra, out agencyNameExtra);
                OutputAgency(CFr, string.Format("Papildoma darbo birža: " +
                                                agencyNameExtra), AgencyExtra);
                
                AddToList(ref Agency3, AgencyExtra, exp, wage);
                OutputAgency(CFr, "Papildytas tinkamų darbuotojų sąrašas:", 
                             Agency3);
                output.Text = File.ReadAllText(CFr, Encoding.UTF8);
            }
        }
        private void AssignmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(File.ReadAllText(CFa, Encoding.UTF8), "Užduotis");
        }

        private void InstructionsToolStripMenuItem_Click(object sender, 
                                                         EventArgs e)
        {
            MessageBox.Show(File.ReadAllText(CFi, Encoding.UTF8), "Instrukcija");
        }
        private void close_Click(object sender, EventArgs e)
        {
            OutputCSV(Agency3);
            Close();
        }
        /// <summary>
        /// inputs data about an agency from a file
        /// </summary>
        /// <param name="fn">name of file</param>
        /// <returns></returns>
        static void Input(string fn, out List<Worker> EmploymentAgency, 
                          out string agencyName)
        {
            EmploymentAgency = new List<Worker>();
            using (StreamReader reader = new StreamReader(fn, Encoding.UTF8))
            {
                string line;
                agencyName = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(';');
                    string name = parts[0];
                    string prof = parts[1];
                    int exp = int.Parse(parts[2]);
                    int hoursD = int.Parse(parts[3]);
                    int hoursM = int.Parse(parts[4]);
                    int pay = int.Parse(parts[5]);
                    Worker A = new Worker(name, prof, exp, hoursD, hoursM, pay);
                    EmploymentAgency.Add(A);
                }
            }
        }
        /// <summary>
        /// outputs data from the Agency into a file
        /// </summary>
        /// <param name="fn">output file name</param>
        /// <param name="header">text above the table</param>
        /// <param name="EmploymentAgency">Agency</param>
        static void OutputAgency(string fn, string header,
                                 List<Worker> EmploymentAgency)
        {
            using (var fr = new StreamWriter(File.Open(fn, FileMode.Append),
                                             Encoding.UTF8))
            {
                fr.WriteLine(header);
                if (EmploymentAgency.Count > 0)
                {
                    string line = "|-------------------|-----------------|---" +
                                  "-------|---|-----|----------|";
                    fr.WriteLine(string.Format(
                                 ' ' + new string('_', 69) + '\n' +
                                 "|{0,-19}|{1,-17}|{2,10}|{3,3}|{4,5}|{5,10}|",
                                 "Vardas", "Profesija", "Stažas, m.", "h/d",
                                 "h/mėn", "Alga, eur."));
                    for (int i = 0; i < EmploymentAgency.Count; i++)
                    {
                        fr.WriteLine(line);
                        fr.WriteLine(EmploymentAgency[i].ToString());
                    }
                    fr.WriteLine("|___________________|_________________|____" +
                                  "______|___|_____|_______" +
                                  "___|\n\n");
                }
                else fr.WriteLine("Sąraše darbuotojų nėra.\n");
            }
        }
        /// <summary>
        /// outputs data from the Agency into a .csv file
        /// </summary>
        /// <param name="Agency"></param>
        static void OutputCSV(List<Worker> Agency)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "csv files (*.csv)|*.csv|" +
                                     "All files (*.*)|*.*";
            saveFileDialog1.Title = "Pasirinkite rezultatų failą";
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string fileCSV = saveFileDialog1.FileName;
                if (File.Exists(fileCSV)) File.Delete(fileCSV);
                using (var fr = new StreamWriter(File.Open(fileCSV, 
                                                 FileMode.Append), Encoding.UTF8))
                {
                    if (Agency.Count > 0)
                    {
                        fr.WriteLine(string.Format(
                                     "{0},{1},{2},{3},{4},{5}",
                                     "Vardas", "Profesija", "Stažas; m.", "h/d",
                                     "h/mėn", "Alga; eur."));
                        for (int i = 0; i < Agency.Count; i++)
                        {
                            fr.WriteLine(Agency[i].ToStringCSV());
                        }
                        //pakeitimas
                    }
                    else fr.WriteLine("Sąraše darbuotojų nėra.\n");
                }
            }
            
        }

        /// <summary>
        /// outputs a single string
        /// </summary>
        /// <param name="fn">output</param>
        /// <param name="text">string that will be output</param>
        static void OutputText(string fn, string text)
        {
            using (var fr = new StreamWriter(File.Open(fn, FileMode.Append),
                                             Encoding.UTF8))
            {
                fr.WriteLine(text);
            }
        }
        /// <summary>
        /// finds which agency contains more applicable employees
        /// </summary>
        /// <param name="Agency1"></param>
        /// <param name="Agency2"></param>
        /// <param name="exp">required experience of worker</param>
        /// <param name="wage">maximum wage of worker</param>
        /// <returns></returns>
        static string BetterAgency(List<Worker> Agency1, List<Worker> Agency2, 
                                   string agencyName1, string agencyName2,
                                   int exp, int wage)
        {
            if (ApplicableEmployees(Agency1, exp, wage) != 0 && 
                ApplicableEmployees(Agency2, exp, wage) != 0)
            {

                if (ApplicableEmployees(Agency1, exp, wage) >
                    ApplicableEmployees(Agency2, exp, wage))
                    return "Tinkamiausia birža: " + agencyName1 + '\n';
                if (ApplicableEmployees(Agency1, exp, wage) <
                    ApplicableEmployees(Agency2, exp, wage))
                    return "Tinkamiausia birža: " + agencyName2 + '\n';
                return "Biržos " + agencyName1 + " ir " + agencyName2 +
                       " turi vienodą skaičių tinkamų darbuotojų\n";
            }
            else return "Biržose tinkamų darbuotojų nėra.\n";
        }
        /// <summary>
        /// counts the number of applicable employees an agency has
        /// </summary>
        /// <param name="Agency1"></param>
        /// <param name="exp">minimum experience</param>
        /// <param name="wage">maximum requested pay</param>
        /// <returns>returns the number of applicable employees</returns>
        static int ApplicableEmployees(List<Worker> Agency1, int exp,
                                       int wage)
        {
            int employees; //number of applicable employees
            employees = Agency1.Count(item => item.Experience >= exp &&
                                      item.RequestedPay <= wage);
            return employees;
        }
        /// <summary>
        /// adds all applicable employees from Agency1 to Agency3
        /// </summary>
        /// <param name="Agency3"></param>
        /// <param name="Agency1"></param>
        /// <param name="exp">minimum required experience</param>
        /// <param name="wage">maximum requested pay</param>
        static void NewAgency(ref List<Worker> Agency3, List<Worker> Agency1, 
                              int exp, int wage)
        {
            for(int i = 0; i < Agency1.Count; i++)
            {
                if (Agency1[i].Experience >= exp &&
                    Agency1[i].RequestedPay <= wage)
                    Agency3.Add(Agency1[i]);
            }
        }
        /// <summary>
        /// removes workers who request a higher wage than newWage from Agency
        /// </summary>
        /// <param name="Agency"></param>
        /// <param name="newWage"></param>
        static void RemoveFromAgency(List<Worker> Agency, int newWage)
        {
            if (Agency.Count != 0)
            {
                for (int i = 0; i < Agency.Count; i++)
                {
                    if (Agency[i].RequestedPay > newWage)
                        Agency.RemoveAt(i);
                }
                if (Agency[0].RequestedPay > newWage)
                    Agency.RemoveAt(0);
            }
        }
        /// <summary>
        /// adds applicable employees from AgencyExtra to Agency3
        /// </summary>
        /// <param name="Agency3"></param>
        /// <param name="AgencyExtra"></param>
        /// <param name="exp"></param>
        /// <param name="wage"></param>
        static void AddToList(ref List<Worker> Agency3, List<Worker> AgencyExtra,
                              int exp, int wage)
        {
            for (int i = 0; i < AgencyExtra.Count; i++)
            {
                if (AgencyExtra[i].Experience >= exp &&
                    AgencyExtra[i].RequestedPay <= wage)
                {
                    if(Agency3.Count == 0)
                    {
                        Agency3.Add(AgencyExtra[i]);
                    }
                    else if(Agency3.Count == 1)
                    {
                        if (AgencyExtra[i] < Agency3[0])
                            
                            Agency3.Insert(0, AgencyExtra[i]);
                        else
                            Agency3.Add(AgencyExtra[i]);
                    }
                    else for (int j = 1; j < Agency3.Count; j++)
                    {
                        if (AgencyExtra[i].Exists(Agency3[j]) ||
                            AgencyExtra[i].Exists(Agency3[j - 1]))
                            break;
                        if (j == (AgencyExtra.Count - 1))
                        {
                            Agency3.Add(AgencyExtra[i]);
                        }
                        else
                        {
                            if ((AgencyExtra[i] < Agency3[j] && AgencyExtra[i] >
                                Agency3[j - 1]) || (AgencyExtra[i] < Agency3[j] &&
                                AgencyExtra[i].Equals(Agency3[j - 1])))
                            {
                                Agency3.Insert(j, AgencyExtra[i]);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
