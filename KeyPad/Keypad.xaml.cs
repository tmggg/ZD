﻿

/*
 * Copyright (c) 2008, Andrzej Rusztowicz (ekus.net)
* All rights reserved.

* Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.

* Neither the name of ekus.net nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
/*
 * Added by Michele Cattafesta (mesta-automation.com) 29/2/2011
 * The code has been totally rewritten to create a control that can be modified more easy even without knowing the MVVM pattern.
 * If you need to check the original source code you can download it here: http://wosk.codeplex.com/
 */
using System;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;

namespace KeyPad
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class Keypad : Window,INotifyPropertyChanged
    {
        #region Public Properties

        private string _result;
        public string Result
        {
            get { return _result; }
            private set { _result = value; this.OnPropertyChanged("Result"); }
        }

        private double _min;

        public double Min
        {
            get { return _min; }
            set { _min = value; this.OnPropertyChanged("Min"); }
        }

        private double _max;

        public double Max
        {
            get { return _max; }
            set { _max = value; this.OnPropertyChanged("Max"); }
        }

        private string  _varName;

        public string VarName
        {
            get { return _varName; }
            set { _varName = value; this.OnPropertyChanged("VarName"); }
        }

        private string _range;

        public string Range
        {
            get 
            {
                if (Min == Max)
                    return string.Empty;
                return string.Format("最小值: {0},最大值: {1}",Min,Max); 
            }
            set { _range = value; this.OnPropertyChanged("Range"); }
        }



        #endregion

        public Keypad( Window w,string varName = null,double min = 0.0, double max = 0.0,double oldValue = 0.0)
        {
            InitializeComponent();
            this.Owner = w;
            this.DataContext = this;
            this.Min = min;
            this.Max = max;
            this.VarName = varName?? "参数设置";
            this.MouseLeftButtonDown += Keypad_MouseLeftButtonDown;
            Result = oldValue.ToString();
            Range = "";
        }

        private void Keypad_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public Keypad(TextBox owner, Window wndOwner)
        {
            InitializeComponent();
            this.Owner = wndOwner;
            this.DataContext = this;
            Result = "";
        }  
        


        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            switch (button.CommandParameter.ToString()) 
            {
                case "ESC":
                    this.DialogResult = false;
                    break;

                case "RETURN":
                    this.DialogResult = true;
                    break;

                case "BACK":
                    if (Result.Length > 0)
                        Result = Result.Remove(Result.Length - 1);
                    if (Result.Length == 0)
                        Result = "0";
                    break;
                default:
                    if(button.CommandParameter.ToString() == "DECIMAL")
                    {
                        if (Result.Contains("."))
                            return;
                    }
                    if(Result.Length == 1 && Result.EndsWith("0"))
                    {
                        Result = "";
                    }
                    Result += button.Content.ToString();
                    break;
            }   
        }    

        #region INotifyPropertyChanged members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        #endregion

        
       
    }
}
