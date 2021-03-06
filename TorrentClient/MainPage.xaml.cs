﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using NetTorrent;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Diagnostics;
using System.Text;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TorrentClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".torrent");
            StorageFile torrentFile = await picker.PickSingleFileAsync();

            if (torrentFile != null)
            {
                //For converting the torrentfile to string
                byte[] torrentByte;
                using (Stream stream = await torrentFile.OpenStreamForReadAsync())
                {
                    using (var memorystream = new MemoryStream())
                    {
                        stream.CopyTo(memorystream);
                        torrentByte = memorystream.ToArray();
                    }
                }
                Bencode meta = new Bencode();
                string metaString = Encoding.ASCII.GetString(torrentByte);

                dynamic metaValues = meta.DeserializeBencode(metaString);

                Torrent myTorrent = new Torrent(metaValues);
                foreach (Torrent.fileStruct item in myTorrent.Files)
                {
                    Debug.WriteLine((item.Length/1024)+"KB\t"+item.Path.OriginalString);
                }
            }
        }
    }
}
