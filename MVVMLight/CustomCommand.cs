using System;
using System.Windows.Input;

namespace MVVMLight
{

	public abstract class CommandEventHandler : ICommand {

		public virtual bool CanExecute(object parameter) {
			return true;
		}

		public abstract void Execute(object parameter);

		protected event EventHandler CanExecuteChangedInternal;

		public event EventHandler CanExecuteChanged {
			add {
				CommandManager.RequerySuggested += value;
				this.CanExecuteChangedInternal += value;
			}

			remove {
				CommandManager.RequerySuggested -= value;
				this.CanExecuteChangedInternal -= value;
			}
		}


		public void OnCanExecuteChanged() {
			EventHandler handler = this.CanExecuteChangedInternal;
			if (handler != null) {
				handler.Invoke(this, EventArgs.Empty);
			}
		}

	}

	public class CustomCommand : CommandEventHandler
	{

		private Action actionToExecute;
		private Predicate<object> actionToCanExecute;

		public CustomCommand(Action action, Predicate<object> actionToCan = null) {
			actionToExecute = action;
			actionToCanExecute = actionToCan;

		}

		#region ICommand Members

		public override void Execute(object parameter) {
			if (actionToExecute == null)
				return;

			actionToExecute.Invoke();

		}

		#endregion
	}

	public class RelayCommand<T> : CommandEventHandler
	{
		private Action<T> execute;

		private Predicate<T> canExecute;
		
		public RelayCommand(Action<T> execute)
			: this(execute, DefaultCanExecute) {
		}

		public RelayCommand(Action<T> execute, Predicate<T> canExecute) {
			if (execute == null) {
				throw new ArgumentNullException("execute");
			}

			if (canExecute == null) {
				throw new ArgumentNullException("canExecute");
			}

			this.execute = execute;
			this.canExecute = canExecute;
		}

		public override bool CanExecute(object parameter) {
			return this.canExecute != null && this.canExecute((T)parameter);
		}

		public override void Execute(object parameter) {
			this.execute((T)parameter);
		}

		public void Destroy() {
			this.canExecute = _ => false;
			this.execute = _ => { return; };
		}

		protected static bool DefaultCanExecute(T parameter) {
			return true;
		}
	}

}
