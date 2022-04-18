using System;

namespace ViewModel
{
    public class LamdaCommand : Command
    {

        private readonly Action<object> _Execute;

        private readonly Func<object, bool> _CanExecute;

        public LamdaCommand(Action<object> Execute, Func<object, bool>? CanExecute = null) {

            _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));

            _CanExecute = CanExecute ?? throw new ArgumentNullException(nameof(CanExecute)); ;


        }

        public override bool CanExecute(object? parameter) => _CanExecute.Invoke(parameter!);

        public override void Execute(object? parameter) => _Execute(parameter!);
        
    }
}
