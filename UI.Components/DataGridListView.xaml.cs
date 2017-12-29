using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace UI.Components
{
	public partial class DataGridListView : UserControl
	{

		List<object> defaultItemSource = new List<object>();

		public static readonly DependencyProperty ItemsSourceProperty = 
			DependencyProperty.Register("ItemsSource", typeof(ICollectionView), typeof(DataGridListView),
			new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

		public static readonly DependencyProperty ListViewItemRowHeightProperty = 
			DependencyProperty.Register("ListViewItemRowHeight", typeof(string), typeof(DataGridListView), new PropertyMetadata(""));

		public static readonly DependencyProperty SelectedItemProperty = 
			DependencyProperty.Register("SelectedItem", typeof(object), typeof(DataGridListView), new PropertyMetadata(""));

		public static readonly DependencyProperty VisibleItemsProperty = 
			DependencyProperty.Register("VisibleItems", typeof(IList), typeof(DataGridListView),
			new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

		GridView _GridView = new GridView();

		public GridViewColumnCollection Columns {
			get {
				return _GridView.Columns;
			}
			set {
				_GridView.Columns.Clear();
				foreach (GridViewColumn col in value) {
					_GridView.Columns.Add(col);
				}

			}
		}

		List<object> _ListItems = new List<object>();
		ICollectionView _GridListViewItemSource { get; set; }

		public IEnumerable ItemsSource {
			get {
				return GetValue(ItemsSourceProperty) as ICollectionView;
			}
			set {
				SetValue(ItemsSourceProperty, value);
			}
		}

		public string ListViewItemRowHeight {
			get {
				return (string)GetValue(ListViewItemRowHeightProperty);
			}
			set {
				SetValue(ListViewItemRowHeightProperty, value);
			}
		}

		public object SelectedItem {
			get {
				return GetValue(SelectedItemProperty);
			}
			set {
				SetValue(SelectedItemProperty, value);
			}
		}

		public IList VisibleItems {
			get {
				return GetValue(VisibleItemsProperty) as IList;
			}
			set {
				SetValue(VisibleItemsProperty, value);
			}
		}

		public DataGridListView() {
			InitializeComponent();

			ListViewItemRowHeight = "32";

			GridListView.View = _GridView;

			_GridListViewItemSource = CollectionViewSource.GetDefaultView(defaultItemSource);
			GridListView.ItemsSource = _GridListViewItemSource;

			GridVisibleItems.Text = "0";
			GridTotalItems.Text = defaultItemSource.Count.ToString();

			_GridListViewItemSource.Filter = (item) => {
				return Filter(item);
			};
		}

		private bool Filter(object item) {
			string trimmedMatchvalue = GridSearchText.Text.Trim();
			bool match = string.IsNullOrEmpty(trimmedMatchvalue);

			foreach (var attribute in item.GetType().GetFields()) {
				if (attribute.GetValue(item) == null)
					continue;

				match = match || 
					attribute.GetValue(item).ToString().ToLowerInvariant()
						.Contains(trimmedMatchvalue.ToLowerInvariant());
			}
			foreach (var property in item.GetType().GetProperties()) {
				if (property.GetValue(item, new object[] { }) == null)
					continue;

				match = match  
					|| (property.CanRead 
						&& property.GetValue(item, new object[] { })
									.ToString()
									.ToLowerInvariant()
									.Contains(trimmedMatchvalue.ToLowerInvariant()));
			}

			return match;
		}

		private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) {
			DataGridListView control = sender as DataGridListView;
			if (control != null)
				control.OnItemsSourceChanged((IEnumerable)e.OldValue, (IEnumerable)e.NewValue);
		}

		private void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue) {
			// Remove handler for oldValue.CollectionChanged
			INotifyCollectionChanged oldValueINotifyCollectionChanged = oldValue as INotifyCollectionChanged;

			if (null != oldValueINotifyCollectionChanged) {
				oldValueINotifyCollectionChanged.CollectionChanged -= new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
			}
			// Add handler for newValue.CollectionChanged (if possible)
			INotifyCollectionChanged newValueINotifyCollectionChanged = newValue as INotifyCollectionChanged;
			if (null != newValueINotifyCollectionChanged) {
				newValueINotifyCollectionChanged.CollectionChanged += new NotifyCollectionChangedEventHandler(newValueINotifyCollectionChanged_CollectionChanged);
			}

		}
		
		void newValueINotifyCollectionChanged_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			//Do your stuff here.
			defaultItemSource.Clear();
			foreach (var item in sender as IEnumerable) {
				defaultItemSource.Add(item);
			}
			RefreshCollection();

		}

		private void RefreshCollection() {
			
			GridTotalItems.Text = defaultItemSource.Count.ToString();

			IList collection = defaultItemSource.Where(item => Filter(item)).ToList();
			GridVisibleItems.Text = collection.Count.ToString();

			VisibleItems = collection;
			_GridListViewItemSource.Refresh();
		}

		private void OnGridSearchTextChanged(object sender, TextChangedEventArgs e) {
			RefreshCollection();
		}

	}
}
