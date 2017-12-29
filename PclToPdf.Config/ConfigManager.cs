using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using PclToPdf.Interfaces;
using Utility.Log;

namespace PclToPdf.Config
{
	public class ConfigManager
	{
		public const string DefaultProjectName = ".pclToPdf.prj";

		public event EventHandler AnythigHappened;

		public event EventHandler OnSaved;

		ProjectConfig _Config = new ProjectConfig();

		public void New(string workingDir) {
			_Config = new ProjectConfig();
			Save(workingDir);

		}

		public void Read(string workingDir) {
			string filename = Path.Combine(workingDir, DefaultProjectName);
			try {

				DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ProjectConfig));
				string json = File.ReadAllText(filename);

				using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json))) {
					_Config = jsonSerializer.ReadObject(stream) as ProjectConfig;
				}

				NotifyAnythigHappened("Project loaded: " + filename);

			} catch {
				// NotFound
				NotifyAnythigHappened("Can't load " + filename);
			}

		}

		public void Save(string workingDir) {
			string filename = Path.Combine(workingDir, DefaultProjectName);
			try {
				DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ProjectConfig));
				_Config.LastModified = DateTime.Now.Ticks;

				BackgroundWorker w = new BackgroundWorker();

				w.DoWork += (sender, e) => {
					using (StreamWriter writer = new StreamWriter(filename)) {
						jsonSerializer.WriteObject(writer.BaseStream, _Config);
						writer.Flush();

					}
				};
				
				w.RunWorkerCompleted += (sender, e) => {

					if (OnSaved != null) {
						OnSaved(this, new EventArgs());
					}

					NotifyAnythigHappened("Project saved: " + filename);
				};

				w.RunWorkerAsync();

			} catch {
				// NotFound
				NotifyAnythigHappened("Can't save " + filename);
			}

		}

		public bool Exists(string workingDir) {
			return Directory.GetFiles(workingDir, DefaultProjectName, SearchOption.TopDirectoryOnly).Any();

		}

		private void NotifyAnythigHappened(string message) {
			if (AnythigHappened != null)
				AnythigHappened(this, new AnythingHappenedEventArgs(message));
		}

		public DateTime LastModified {
			get {
				return new DateTime(_Config.LastModified);
			}
		}

		static object lockConfig = new object() { };

		public bool AcceptAfpFile(IFilePclInfo fileAfp, bool forceAdd = false) {
			lock (lockConfig) {
				if (forceAdd || !_Config.Pcls.Any(f => f.Filename == fileAfp.Filename && f.Volume == fileAfp.Volume)) {
					FilePclConfig item = new FilePclConfig() {
						Volume = fileAfp.Volume,
						Done = fileAfp.Done,
						Filename = fileAfp.Filename,
						Quanti = fileAfp.Quanti,
					};

					_Config.Pcls.Add(item);
				}

				return _Config.Pcls.Single(f => f.Filename == fileAfp.Filename && f.Volume == fileAfp.Volume).Done;
			}

		}

		public IEnumerable<IFilePclInfo> GetAllPcls() {
			List<IFilePclInfo> list = new List<IFilePclInfo>();
			foreach (var item in _Config.Pcls) {
				list.Add(item);
			}

			return list;
		}

		public void Sync(List<IFilePclInfo> _FilePclList) {

			NotifyAnythigHappened("Sync starts at: " + DateTime.Now.ToLongTimeString());

			NotifyAnythigHappened("Accepting new files starts at... " + DateTime.Now.ToLongTimeString());

			var hashTablePcls = _Config.Pcls
				.ToArray()
				.ToDictionary((k => k.Filename), (v => v));
			uint count = 0;
			foreach (IFilePclInfo item in _FilePclList) {
				if (!hashTablePcls.ContainsKey(item.Filename)) {
					AcceptAfpFile(item, true);
					count++;
				}
			}

			NotifyAnythigHappened("Added " + count + "new files");
			
			NotifyAnythigHappened("Accepting new files ends starts at... " + DateTime.Now.ToLongTimeString());

			var hashTable = _FilePclList
				.ToArray()
				.ToDictionary((k => k.Filename), (v => v));
			foreach (FilePclConfig item in _Config.Pcls.ToArray()) {
				// match			
				if (hashTable.ContainsKey(item.Filename)) {
					IFilePclInfo fItem = hashTable[item.Filename];
					item.Quanti = fItem.Quanti;
					item.Done = fItem.Done;

				} else {
					// Not Match to remove
					_Config.Pcls.Remove(item);					
				}

			}

			

			NotifyAnythigHappened("Sync ends at: " + DateTime.Now.ToLongTimeString());

		}
	}
}
