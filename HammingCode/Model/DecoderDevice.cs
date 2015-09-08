using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


/* <copyright file=DecoderDevice company="GromovProd">
   Copyright (c) 2014 All Rights Reserved
   </copyright>
   <author>Gromov Daniil</author>
   <date>11/21/2014 1:57:27 PM</date>
   <summary></summary>*/

namespace HammingCode.Model
{
    public enum DecoderState
    {
        Fill = 0,
        HasntError = 1,
        FindError = 2,
        EditError = 3
    }
    public class DecoderDevice : INotifyPropertyChanged
    {
        private bool[] inputBool = new bool[7] { false, false, false, false, false, false, false };

        public bool[] InputBool
        {
            get { return inputBool; }
            set { inputBool = value; OnPropertyChanged(); }
        }

        private bool[] outputBool = new bool[4] { false, false, false, false };
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
                for (int i = OutputBool.Length - 1; i >= 0; i--)
                {
                    if (OutputBool[i] == false) temp += "0";
                    else temp += "1";
                }

                return temp;
            }

        }

        private bool[] registerBool = new bool[7] { false, false, false, false, false, false, false };

        public bool[] RegisterBool
        {
            get { return registerBool; }
            set { registerBool = value; OnPropertyChanged(); }
        }

        private bool[] pilotBool = new bool[3] { false, false, false };

        public bool[] PilotBool
        {
            get { return pilotBool; }
            set { pilotBool = value; OnPropertyChanged(); }
        }

        private int summator = 0;

        public int Summator
        {
            get { return summator; }
            set { summator = value; OnPropertyChanged(); }
        }

        private int tick = 0;

        public int Tick
        {
            get { return tick; }
            set { tick = value; OnPropertyChanged(); }
        }

        private int errorTick = 0;

        public int ErrorTick
        {
            get { return errorTick; }
            set { errorTick = value; }
        }

        public bool CurrentInputBool
        {
            get
            {
                if (Tick < 7)
                {
                    return inputBool[Tick];
                }
                else return false;
            }
        }

        private bool key = false;

        public bool Key
        {
            get { return key; }
            set { key = value; OnPropertyChanged(); }
        }

        private DecoderState state = DecoderState.Fill;

        public DecoderState State
        {
            get { return state; }
            set { state = value; }
        }

        private bool isInitialized = false;

        public bool IsInitialized
        {
            get { return isInitialized; }
            set { isInitialized = value; }
        }

        private bool isError = false;

        private bool isEnd = false;

        public bool IsEnd
        {
            get { return isEnd; }
            set { isEnd = value; }
        }

        public DecoderDevice()
        {

        }

        public void Initialize(bool[] input)
        {
            IsInitialized = true;
            input.CopyTo(InputBool, 0);
            State = DecoderState.Fill;
        }

        public void Clear()
        {
            isInitialized = false;
            InputBool = new bool[7] { false, false, false, false, false, false, false };
            OutputBool = new bool[4] { false, false, false, false };
            PilotBool = new bool[4] { false, false, false, false };
            RegisterBool = new bool[7] { false, false, false, false, false, false, false };
            Tick = 0;
            Key = false;
            ErrorTick = 0;
            IsEnd = false;
            State = DecoderState.Fill;
            Summator = 0;
            isError = false;

            OnPropertyChanged("CurrentInputBool");
            OnPropertyChanged("OutputCode");
        }

        public void MakeTick()
        {
            //tick ==7 закончили вычисление синдрома. Дальше крутить по кругу пока T1 ==0 а другие равны 0.
            //когда код синдрома меньше 2, то ключ во 2е положение
            //? потом еще 7 тактов и то что было в  буферах надо поместить в основные ресистры
            //

            if (IsEnd)
            {
                MessageBox.Show("Код найден: " + OutputCode, "Сообщение");
                return;
            }

            switch (State)
            {
                case DecoderState.Fill:
                    {
                        MakeSimpleTick(false);

                        if (Tick == 6)
                        {
                            if (Summator > 1)
                            {
                                State = DecoderState.FindError;
                            }
                            else
                            {
                                State = DecoderState.HasntError;
                            }
                        }

                        break;
                    }
                case DecoderState.HasntError:
                    {

                        for (int i = OutputBool.Length - 1; i > 0; i--)
                        {
                            OutputBool[i] = OutputBool[i - 1];
                        }
                        OutputBool[0] = RegisterBool[6];

                        MakeSimpleTick(false);

                        if (Tick == 10) IsEnd = true;
                        break;
                    }
                case DecoderState.FindError:
                    {
                        MakeSimpleTick(false);

                        if (Summator < 2)
                        {
                            Key = true;
                            State = DecoderState.EditError;
                            ErrorTick = Tick;
                            isError = true;
                        }
                        break;
                    }

                case DecoderState.EditError:
                    {
                        MakeSimpleTick(true);

                        for (int i = OutputBool.Length - 1; i > 0; i--)
                        {
                            OutputBool[i] = OutputBool[i - 1];
                        }
                        OutputBool[0] = RegisterBool[6];

                        if (Tick == 16)
                        {
                            IsEnd = true;
                        }

                        break;
                    }
            }



            Tick++;

            OnPropertyChanged("RegisterBool");
            OnPropertyChanged("PilotBool");
            OnPropertyChanged("OutputBool");
            OnPropertyChanged("CurrentInputBool");
            OnPropertyChanged("OutputCode");

            if (isError)
            {
                isError = false;
                var N = Tick - 7 + 1;
                MessageBox.Show("Ошибка в " + N.ToString() + " символе.", "Сообщение");
            }

            if (IsEnd)
            {
                MessageBox.Show("Код найден: " + OutputCode, "Сообщение");
            }

        }

        private void MakeSimpleTick(bool key)
        {
            var lastRegisterBool6 = RegisterBool[6];
            var lastPilotBool2 = PilotBool[2];

            RegisterBool[6] = RegisterBool[5];
            RegisterBool[5] = RegisterBool[4];
            RegisterBool[4] = RegisterBool[3];
            if (!key)
            {
                RegisterBool[3] = RegisterBool[2] ^ false;
            }
            else
            {
                RegisterBool[3] = RegisterBool[2] ^ PilotBool[2];
            }
            RegisterBool[2] = RegisterBool[1];
            RegisterBool[1] = RegisterBool[0];

            if (Tick < 7)
            {
                RegisterBool[0] = lastRegisterBool6 ^ InputBool[tick];
            }
            else
            {
                RegisterBool[0] = lastRegisterBool6 ^ false;
            }
            if (!key)
            {
                PilotBool[2] = PilotBool[1] ^ lastPilotBool2;
            }
            else
            {
                PilotBool[2] = PilotBool[1] ^ false;
            }
            PilotBool[1] = PilotBool[0];
            if (Tick < 7)
            {
                PilotBool[0] = InputBool[tick] ^ lastPilotBool2;
            }
            else
            {
                if (!key)
                {
                    PilotBool[0] = false ^ lastPilotBool2;
                }
                else
                {
                    PilotBool[0] = false ^ false;
                }
            }

            Summator = 0;

            for (int i = 0; i < PilotBool.Length; i++)
            {
                if (PilotBool[i] == true) Summator++;
            }
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
