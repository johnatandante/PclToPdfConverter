using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using MVVMLight;
using PclToPdf.Config;
using PclToPdf.DataItem;
using PclToPdf.Interfaces;
using PclToPdf.Model;
using Utility.Log;

namespace PclToPdf.ViewModel
{
	public class PclToPdfViewModel : MVVMLight.ViewModel
	{

		static Array EnumList =Enum.GetValues(typeof(FilePclFinder.Volume));

		public string[] Volumes {
			get {
				List<string> list = new List<string>() { string.Empty };

				foreach (FilePclFinder.Volume vol in EnumList)
					list.Add(vol.ToString());

				return list.ToArray();

			}
		}

		static SortByRecord[] _SortList = new SortByRecord[] { 
			new SortByRecord(){ Field = "PdfFileName", SortMode = ListSortDirection.Ascending },
			new SortByRecord(){ Field = "PdfFileName", SortMode = ListSortDirection.Descending },
			new SortByRecord(){ Field = "Pages", SortMode = ListSortDirection.Ascending },
			new SortByRecord(){ Field = "Pages", SortMode = ListSortDirection.Descending },
		};

		public SortByRecord[] SortList {
			get {
				return _SortList;
			}
		}

		private SortByRecord _SortItemMode = null;
		public SortByRecord SortItemMode {
			get { return _SortItemMode; }
			set {
				SetPropertyValue<SortByRecord>(ref _SortItemMode, value, "SortItemMode");

				if (_SortItemMode == null)
					return;
				FileList.SortDescriptions.Clear();
				FileList.SortDescriptions.Add(new SortDescription(SortItemMode.Field, SortItemMode.SortMode));

				FileList.Refresh();
			}
		}

		public IList VisibleItems { get; set; }

		private List<IPclToPdfFileInfo> _FileList = new List<IPclToPdfFileInfo>();
		public ICollectionView FileList { get; set; }

		private List<IFilePclInfo> _FilePclList = new List<IFilePclInfo>();
		public ICollectionView FilePclList { get; set; }

		private string _TestoDiProva = string.Empty;
		public string TestoDiProva {
			get { 
				return _TestoDiProva; 
			}
			set {
				SetPropertyValue<string>(ref _TestoDiProva, value, "TestoDiProva");
			}
		}

		private string _VolumeSelected = string.Empty;
		public string VolumeSelected {
			get { return _VolumeSelected; }
			set {
				SetPropertyValue<string>(ref _VolumeSelected, value, "VolumeSelected");
				FilePclList.Refresh();
			}
		}

		private bool _ShowOnlyPdfToDo = false;
		public bool ShowOnlyPdfToDo { 
			get { return _ShowOnlyPdfToDo; } 
			set { 
				SetPropertyValue<bool>(ref _ShowOnlyPdfToDo, value, "ShowOnlyPdfToDo");
				FileList.Refresh();
			} 
		}

		private bool _ShowOnlyPdfWithPages = false;
		public bool ShowOnlyPdfWithPages {
			get { return _ShowOnlyPdfWithPages; }
			set {
				SetPropertyValue<bool>(ref _ShowOnlyPdfWithPages, value, "ShowOnlyPdfWithPages");
				FileList.Refresh();
			}
		}

		private bool _ShowOnlyPclToDo = true;
		public bool ShowOnlyPclToDo {
			get { return _ShowOnlyPclToDo; } 
			set {
				SetPropertyValue<bool>(ref _ShowOnlyPclToDo, value, "ShowOnlyPclToDo");
				SelectToDosOnly();
				FilePclList.Refresh();
			} 
		}

		private bool _ShowOnlyPclWithPdf = true;
		public bool ShowOnlyPclWithPdf {
			get { return _ShowOnlyPclToDo; }
			set {
				SetPropertyValue<bool>(ref _ShowOnlyPclWithPdf, value, "ShowOnlyPclWithPdf");
				FilePclList.Refresh();
			}
		}

		private string _LogMessages = string.Empty;
		public string LogMessages { get { return _LogMessages; } set { SetPropertyValue<string>(ref _LogMessages, value, "LogMessages"); } }

		private int _ProgressValue = 0;
		public int ProgressValue { get { return _ProgressValue; } set { SetPropertyValue<int>(ref _ProgressValue, value, "ProgressValue"); } }
		
		private string _LastModified = string.Empty;
		public string LastModified { get { return _LastModified; } set { SetPropertyValue<string>(ref _LastModified, value, "LastModified"); } }

		private string _PclDone = string.Empty;
		public string PclDone { get { return _PclDone; } set { SetPropertyValue<string>(ref _PclDone, value, "PclDone"); } }

		private string _PclToDo = string.Empty;
		public string PclToDo { get { return _PclToDo; } set { SetPropertyValue<string>(ref _PclToDo, value, "PclToDo"); } }

		public string PdfDir { get; set; }

		public string SourcePclDir { get; set; }
		
		public IFilePclInfo CurrentPclFileItem { get; set; }

		// project
		public ICommand NewProjectCommand { get; private set; }

		public ICommand LoadProjectCommand { get; private set; }

		public ICommand SaveProjectCommand { get; private set; }

		// List
		public ICommand LoadFilePclListCommand { get; private set; }

		public ICommand AccodaPdfDaGenerareCommand { get; private set; }

		public ICommand GetPdfCommand { get; private set; }

		public ICommand ElaboraPdfCommand { get; private set; }

		public ICommand EsportaPdfCommand { get; private set; }

		public ICommand EsportaPclCommand { get; private set; }

		public ICommand ToggleSelectedCommand { get; private set; }

		public ICommand ClearSelectedCommand { get; private set; }

		public ICommand ClearPdfListCommand { get; private set; }

		public ICommand CopyPdfListCommand { get; private set; }
		
		FilePclToPdfReader reader = new FilePclToPdfReader();
		PclToPdfDbReader dbReader = new PclToPdfDbReader();

		ConfigManager config = new ConfigManager();

		public PclToPdfViewModel() {

			this.NewProjectCommand = new CustomCommand(() => {
				config.New(PdfDir);
				_FilePclList.Clear();
			});

			this.LoadProjectCommand = new CustomCommand(() => {
				LoadFromConfig();

			});

			this.SaveProjectCommand = new CustomCommand(() => {
				config.Sync(_FilePclList);
				config.Save(PdfDir);

			});

			this.LoadFilePclListCommand = new CustomCommand(() => {
				LoadFilePclList();
			});

			this.ElaboraPdfCommand = new CustomCommand(() => {
				Elabora();
			});

			this.GetPdfCommand = new RelayCommand<IPclToPdfFileInfo>((item) => {
				ViewPdf(item);
			});

			this.AccodaPdfDaGenerareCommand = new CustomCommand(() => {
				AccodaPdfDaGenerare();
			});

			this.ToggleSelectedCommand = new CustomCommand(() => {
				ToggleToDoSelected();
			});

			this.ClearSelectedCommand = new CustomCommand(() => {
				ClearSelected();
			});

			this.ClearPdfListCommand = new CustomCommand(() => {
				_FileList.Clear();
				FileList.Refresh();
			});

			this.EsportaPdfCommand = new CustomCommand(() => {
				EsportaPdfList();
			});

			this.EsportaPclCommand = new CustomCommand(() => {
				EsportaPclList();
			});
			
			SourcePclDir = System.Environment.CurrentDirectory;
			PdfDir = System.Environment.CurrentDirectory;

			// Iscrizione eventi

			config.AnythigHappened += OnAnythigHappened;
			config.OnSaved += OnConfigSaved;

			reader.ReadLine += ReaderReadLine;

			dbReader.AnythigHappened += OnAnythigHappened;

			LoadSettings();

			FilePclList = CollectionViewSource.GetDefaultView(_FilePclList);
			FilePclList.Filter = (item) => {
				return (!_ShowOnlyPclWithPdf || (item as IFilePclInfo).Quanti > 0)
					&&  FiltroComboBox(item);
			};

			FilePclList.SortDescriptions.Add(new SortDescription("Quanti", ListSortDirection.Descending));

			FileList = CollectionViewSource.GetDefaultView(_FileList);
			FileList.Filter = (item) => {
				return (!_ShowOnlyPdfToDo || !(item as IPclToPdfFileInfo).Done)
					&& (!_ShowOnlyPdfWithPages || ((item as IPclToPdfFileInfo).Pages > 0));
			};

		}

		private void EsportaPdfList() {
			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += (sender, e) => {
				try {
					PclToPdfFileWriter.Export(_FileList.ToArray(), PdfDir);
				} catch (Exception exc) {
					Log("Errore nel salvataggio: " + exc.Message);

				}
			};

			worker.RunWorkerCompleted += (sender, e) => {
				Log("File esportazione disponibile in " + PdfDir);

			};

			worker.RunWorkerAsync();

		}
		
		private void EsportaPclList() {
			BackgroundWorker worker = new BackgroundWorker();
			worker.DoWork += (sender, e) => {
				try {
					FilePclFinder.ExportFromConfig(config, PdfDir);
				}catch(Exception exc){
					Log("Errore nel salvataggio: " + exc.Message);

				}
			};

			worker.RunWorkerCompleted += (sender, e) => {
				Log("File esportazione disponibile in " + PdfDir);

			};

			worker.RunWorkerAsync();

		}

		private bool FiltroComboBox(object item) {
			return string.IsNullOrEmpty(VolumeSelected) ||
					(item as IFilePclInfo).Volume
											.ToLowerInvariant()
											.Equals(VolumeSelected.ToLowerInvariant());
		}

		private void SelectToDosOnly() {
			if (!ShowOnlyPclToDo)
				return;

			foreach (IFilePclInfo item in _FilePclList.Where(i => FiltroComboBox(i)))
				item.ToDo = !item.Done && item.Quanti > 0;

			FileList.Refresh();
		}

		private void ClearSelected() {
			foreach (IFilePclInfo item in _FilePclList.Where(i => i.ToDo))
				item.ToDo = false;

			VolumeSelected = string.Empty;

		}

		private void ToggleToDoSelected() {
			foreach (IFilePclInfo item in _FilePclList.Where(i => VisibleItems.Contains(i) && !i.Done && FiltroComboBox(i)))
				item.ToDo = !item.ToDo;

			FilePclList.Refresh();
			FileList.Refresh();
		}

		private void LoadFromConfig() {
			_FilePclList.Clear();
			_FileList.Clear();
			config.Read(PdfDir);

			FilePclFinder finder = new FilePclFinder(PdfDir);
			_FilePclList.AddRange(finder.ReadFromConfig(config));

			PclToDo = _FilePclList.Count(f => !f.Done).ToString();
			PclDone = _FilePclList.Count(f => f.Done).ToString();

			FilePclList.Refresh();
			FileList.Refresh();
		}

		private void LoadSettings() {
			if (config.Exists(PdfDir)) {
				config.Read(PdfDir);
				LastModified = config.LastModified.ToShortDateString() + " " + config.LastModified.ToShortTimeString();
			}

		}

		private void AccodaPdfDaGenerare() {
			List<IFilePclInfo> toDos = _FilePclList.Where(item => item.ToDo).ToList();
			List<object> volumes = toDos
				.Select(item => Enum.Parse(typeof(FilePclFinder.Volume), item.Volume, ignoreCase: true))
				.Distinct()
				.ToList();
			
			ProgressValue = 0;
			int count = 0;
			// faccio un thread per volume
			foreach (FilePclFinder.Volume vol in volumes) {
				BackgroundWorker worker = new BackgroundWorker();
				worker.DoWork += (sender, e) => {
					IFilePclInfo[] itemsToPick = toDos.Where(pcl => pcl.Volume == vol.ToString()
																&& !_FileList.Any(pdf => pdf.SourceName.ToLowerInvariant()
																						.Equals(pcl.Filename.ToLowerInvariant())))
															.ToArray();

					List<IPclToPdfFileInfo> list = new List<IPclToPdfFileInfo>(dbReader.ReadAll(itemsToPick));
					dbReader.CheckPclFatto(toDos, list, PdfDir);

					_FileList.AddRange(list);
					worker.ReportProgress((int)(++count * 100 / volumes.Count));

				};

				worker.WorkerReportsProgress = true;

				worker.ProgressChanged += (sender, e) => {
					SetProgress(e);

					FileList.Refresh();
				};

				worker.RunWorkerCompleted += (sender, e) => {
					FileList.Refresh();
					FilePclList.Refresh();
				};

				worker.RunWorkerAsync();
			}

		}

		private void SetProgress(ProgressChangedEventArgs e) {
			if (e.ProgressPercentage > ProgressValue) {
				ProgressValue = e.ProgressPercentage;
			}
		}

		public void LoadFilePclList() {
			ProgressValue = 0;
			_FilePclList.Clear();
			_FileList.Clear();
						
			FilePclFinder finder = new FilePclFinder(PdfDir);

			int count = 0;
			foreach (FilePclFinder.Volume vol in EnumList) {

				BackgroundWorker bw = new BackgroundWorker();

				bw.DoWork += (sender, eargs) => {

					List<IFilePclInfo> items = finder.GetAllFileAfpFromDb(vol);

					//foreach (IFilePclInfo f in items)
					//	f.Done = config.AcceptAfpFile(f);

					_FilePclList.AddRange(items);
				};

				bw.RunWorkerCompleted += (sender, e) => {
					ProgressValue = (int)(++count * 100 / EnumList.LongLength);
					FilePclList.Refresh();

				};

				bw.RunWorkerAsync();

			}


			FileList.Refresh();
		}

		private void ReaderReadLine(object sender, EventArgs e) {
			ProgressValue = (int)(reader.BytesRead * 100 / reader.TotalBytes);
		}

		public void ViewPdf(IPclToPdfFileInfo item) {
			if (item == null || string.IsNullOrEmpty(item.PdfFileName)) {
				Log("No item to show");
				return;
			}

			Log("Pdf to open: " + item.PdfFileName + ".");
			if (!item.Done)
				MessageBox.Show("Errore nel reperimento della posizione " + item.PdfFileName, "Errore", MessageBoxButton.OK);
			else {
				FilePclToPdfReader.Open(item, PdfDir);
			}

		}

		string errorMessage = string.Empty;

		public void Elabora() {
			try {
				errorMessage = string.Empty;
				HashSet<string> StampeErrate = new HashSet<string>();
				IPclToPdfFileInfo[] toDoList = _FileList.Where(f => !f.Done && f.Pages > 0)
										.ToArray();

				var groups = toDoList
					.GroupBy( k => k.VirtualFolder)
					.ToDictionary( key => key, value => value.ToArray());

				foreach (var group in groups) {
					DoWorkOnGroup(StampeErrate, group.Value, toDoList.LongLength);
				}

			} catch (Exception e) {
				Log(e.Message);
			} finally {
				FileList.Refresh();
			}

		}

		private void DoWorkOnGroup(HashSet<string> StampeErrate, IPclToPdfFileInfo[] list, long countAll) {

			ProgressValue = 0;

			PclToPdfFileWriter writer = new PclToPdfFileWriter(PdfDir, SourcePclDir);
			writer.AnythigHappened += OnAnythigHappened;

			BackgroundWorker bw = new BackgroundWorker();

			bw.DoWork += (sender, eargs) => {

				Log("Processing items " + list.LongLength);
				long count = 0;

				foreach (IPclToPdfFileInfo item in list) {
					bw.ReportProgress((int)(++count * 100 / countAll));
					try {

						writer.Process(item);

						if (item.Done)
							Log("File pdf disponibile: " + item.PdfFileName);
						else if (!StampeErrate.Contains(item.SourceName))
							StampeErrate.Add(item.SourceName);
						else
							continue;

					} catch (Exception e) {
						Log("File non elaborato " + item.PdfFileName + " " + e.Message);
					}

				}

			};

			bw.WorkerReportsProgress = true;

			bw.ProgressChanged +=
				(sender, e) => {
					if (e is ProgressChangedEventArgs) {
						SetProgress(e);
						FileList.Refresh();
					}

				};

			bw.RunWorkerCompleted += (sender, e) => {
				var groupPclFile = list
					.ToArray()
					.GroupBy(k => k.SourceName)
					.ToDictionary(key => key, value => value.ToArray());


				foreach (var sourcekey in groupPclFile.Keys) {
					IFilePclInfo sourceItem = _FilePclList.Single(f => f.Filename == sourcekey.Key);
					sourceItem.Done = FilePclToPdfReader.CheckAreDone(groupPclFile[sourcekey], PdfDir);

				}

				FileList.Refresh();

				if (StampeErrate.Any()) {
					StringBuilder sb = new StringBuilder();
					foreach (string s in StampeErrate) {
						sb.AppendLine(s);
					}
					Log("File pcl non elaborati: " + Environment.NewLine + sb.ToString());
					MessageBox.Show("L'operazione ha avuto qualche errore", "Errore", MessageBoxButton.OK);
				} else {
					MessageBox.Show("Operazione conclusa senza errori", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
					Log("Operazione conclusa senza errori");

					System.Diagnostics.Process.Start(PdfDir);
				}

				ProgressValue = 0;
			};

			bw.RunWorkerAsync();

		}

		private void OnConfigSaved(object sender, EventArgs e) {
			LastModified = config.LastModified.ToShortDateString() + " " + config.LastModified.ToShortTimeString();

		}

		private void OnAnythigHappened(object sender, EventArgs e) {
			if (!(e is AnythingHappenedEventArgs))
				return;

			AnythingHappenedEventArgs args = e as AnythingHappenedEventArgs;
			Log(args.Message, args.When);

		}

		static object lockLog = new object() { };

		private void Log(string message, DateTime? now = null) {
			if (!now.HasValue)
				now = DateTime.Now;

			lock (lockLog) {
				LogMessages = string.Format("{0} {1} - {2}{3}{4}", now.Value.ToShortDateString(), now.Value.ToShortTimeString(), message, Environment.NewLine, LogMessages);
			}

		}

	}
}
