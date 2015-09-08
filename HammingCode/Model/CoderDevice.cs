using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


/* <copyright file=CoderDevice company="GromovProd">
   Copyright (c) 2014 All Rights Reserved
   </copyright>
   <author>Gromov Daniil</author>
   <date>11/20/2014 4:28:41 PM</date>
   <summary></summary>*/

namespace HammingCode.Model
{
    public class CoderDevice : INotifyPropertyChanged
    {
        public bool isInitialized = false;

        private bool[] inputBool = new bool[7] { false, false, false, false, false, false, false };

        public bool[] InputBool
        {
            get { return inputBool; }
            set { inputBool = value; OnPropertyChanged(); }
        }

        public bool CurrentInputBool
        {
            get { return inputBool[Tick]; }
        }

        private bool[] outputBool = new bool[7] { false, false, false, false, false, false, false };

        public bool[] OutputBool
        {
            get { return outputBool; }
            set { outputBool = value; OnPropertyChanged(); }
        }

        public String OutputCode
        {
            get 
            {
                var temp = String.Empty;
                for (int i = 0; i < Tick; i++)
                {
                    if (OutputBool[i] == false) temp += "0";
                    else temp += "1";
                }

                return temp;
            }

        }

        private bool[] registerBool = new bool[3] { false, false, false };

        public bool[] RegisterBool
        {
            get { return registerBool; }
            set
            {
                registerBool = value;
                OnPropertyChanged();
            }

        }

        private int tick = 0;

        public int Tick
        {
            get { return tick; }
            set { tick = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// Позиция 1 - тру 2-фолс
        /// </summary>
        private bool key = true;

        public bool Key
        {
            get { return key; }
            set { key = value; OnPropertyChanged(); }
        }

        public CoderDevice()
        {
            this.key = true;
        }

        public void Clear()
        {
            isInitialized = false;
            InputBool = new bool[7] { false, false, false, false, false, false, false };
            OutputBool = new bool[7] { false, false, false, false, false, false, false };
            RegisterBool = new bool[3] { false, false, false };
            Tick = 0;
            Key = true;

            OnPropertyChanged("CurrentInputBool");
            OnPropertyChanged("OutputCode");
        }

        public void Initialize(bool[] input)
        {
            this.isInitialized = true;
            //копируем входные данные. старшие первые
            input.CopyTo(this.InputBool, 0);
        }

        public void MakeTick()
        {
            if (tick >= 4) Key = false;

            if (tick == 7)
            {
                MessageBox.Show("Код найден: " + OutputCode, "Сообщение");
                return;
            }

            bool temp = inputBool[tick] ^ registerBool[2];

            if (key)
            {
                outputBool[tick] = inputBool[tick];
                registerBool[2] = registerBool[1] ^ temp;
                registerBool[1] = registerBool[0];
                registerBool[0] = temp;
            }
            else
            {
                temp = false;
                outputBool[tick] = registerBool[2];
                registerBool[2] = registerBool[1] ^ temp;
                registerBool[1] = registerBool[0];
                registerBool[0] = temp;
            }

            OnPropertyChanged("RegisterBool");
            OnPropertyChanged("OutputBool");
            OnPropertyChanged("CurrentInputBool");

            Tick++;

            OnPropertyChanged("OutputCode");
          
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
