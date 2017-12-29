using System.ComponentModel;

namespace MVVMLight
{
	public class ViewModel : INotifyPropertyChanged
	{

		#region PropertyChanged

		public event PropertyChangedEventHandler PropertyChanged;

		protected bool SetPropertyValue<T>(ref T field, T value, string propertyName) {
			if (field == null || !field.Equals(value)) {
				field = value;
				Notify(propertyName);
				return true;
			}
			return false;
		}

		protected void Notify(string propertyName) {
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		#endregion

	}
}
