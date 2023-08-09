using Calculator_pyj.ViewModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Calculator.ViewModel
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        #region [상수]
        private string _result;

        #endregion

        public string Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public SelectedOperator selectedOperator;

        public enum SelectedOperator
        {
            None,
            Addiction,
            Substraction,
            Multiplication,
            Division
        }

        public ICommand NumberCommand { get; }
        public ICommand AcCommand { get; }
        public ICommand PlusMinusCommand { get; }
        public ICommand PercentCommand { get; }
        public ICommand OperatorCommand { get; }
        public ICommand EqualCommand { get; }
        public ICommand DotCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public CalculatorViewModel()
        {
            NumberCommand = new RelayCommand<string>(executeNumberCommand);
            AcCommand = new RelayCommand<string>(executeAcCommand);
            PlusMinusCommand = new RelayCommand<string>(executePlusMinusCommand);
            PercentCommand = new RelayCommand<string>(executePercentCommand);
            OperatorCommand = new RelayCommand<string>(executeOperatorCommand);
            EqualCommand = new RelayCommand<string>(executeEqualCommand);
            DotCommand = new RelayCommand<string>(executeDotCommand);
        }

        // Command methods here
        private void executeNumberCommand(object parameter)
        {
            if (parameter is string number)
            {
                if (Result == "0")
                {
                    Result = number;
                }
                else
                {
                    Result += number;
                }
            }
        }


        private void executeAcCommand(object parameter)
        {
            Result = "0";
        }

        private void executePlusMinusCommand(object parameter)
        {
            if (double.TryParse(Result, out double numericResult))
            {
                numericResult *= -1;
                Result = numericResult.ToString();
            }

        }

        private void executePercentCommand(object parameter)
        {
            if (double.TryParse(Result, out double numericResult))
            {
                numericResult *= 0.01;
                Result = numericResult.ToString();
            }
        }

        private void executeOperatorCommand(object parameter)
        {

        }



        private void executeEqualCommand(object parameter)
        {

        }

        private void executeDotCommand(object parameter)
        {
            if (!Result.Contains("."))
            {
                Result += ".";
            }
        }
    }
}